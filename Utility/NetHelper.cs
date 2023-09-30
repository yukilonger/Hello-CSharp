using System.Text;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System.Collections.Specialized;
using RestSharp;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Utility
{
    public static class NetHelper
    {
        public static string[] SHZones = new string[] { "山东", "江苏", "上海", "浙江", "安徽", "福建", "江西", "河南", "湖北", "重庆", "四川", "西藏", "陕西" };
        public static string[] BJZones = new string[] { "北京", "天津", "河北", "山西", "内蒙古", "辽宁", "吉林", "黑龙江", "宁夏", "青海", "甘肃", "新疆" };
        public static string[] GDZones = new string[] { "广东", "广西", "海南" , "湖南", "云南", "贵州", "香港", "澳门", "台湾" };

        public static string GetLocation()
        {
            try
            {
                var html = GetWebRequest($"https://{DateTime.Now.Year}.ip138.com", Encoding.UTF8);
                int idx = html.IndexOf("来自");
                string city = html.Substring(idx + 3, 30);
                return Regex.Replace(city, @"[^\u4e00-\u9fa5]", "");
            }
            catch(Exception ex)
            {
                return "";
            }
        }

        public static string GetLocalIP()
        {
            try
            {
                string hostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(hostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        return IpEntry.AddressList[i].ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static long Ping(string ip)
        {
            //构造Ping实例  
            Ping pingSender = new Ping();
            //Ping 选项设置  
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            //测试数据  
            string data = "";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            //设置超时时间  
            int timeout = 120;
            //调用同步 send 方法发送数据,将返回结果保存至PingReply实例  
            PingReply reply = pingSender.Send(ip, timeout, buffer, options);
            if (reply.Status == IPStatus.Success)
            {
                return reply.RoundtripTime;
                //("往返时间：" + reply.RoundtripTime);
                //("生存时间（TTL）：" + reply.Options.Ttl);
                //("是否控制数据包的分段：" + reply.Options.DontFragment);
                //("缓冲区大小：" + reply.Buffer.Length);
            }
            else
                return -1;
        }

        public static Dictionary<string, long> Ping(List<string> addressList, Func<string,bool> outputFun = null)
        {
            //构造Ping实例  
            Ping pingSender = new Ping();
            //Ping 选项设置  
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            //测试数据  
            string data = "";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            //设置超时时间  
            int timeout = 2;
            Dictionary<string, long> rDict = new Dictionary<string, long>();
            foreach (var address in addressList)
            {
                if (!rDict.ContainsKey(address))
                {
                    var ip = address.Split(':')[0];
                    //调用同步 send 方法发送数据,将返回结果保存至PingReply实例  
                    PingReply reply = pingSender.Send(ip, timeout, buffer, options);
                    if (reply.Status == IPStatus.Success)
                    {
                        rDict.Add(address, reply.RoundtripTime);
                        if(outputFun!=null)
                        {
                            outputFun(string.Format("{0} {1}ms", address.PadRight(30, ' '), reply.RoundtripTime));
                        }
                    }
                    else
                    {
                        rDict.Add(address, -1);
                        if (outputFun != null)
                        {
                            outputFun(string.Format("{0} {1}ms", address.PadRight(30, ' '), -1));
                        }
                    }
                }
            }
            return rDict;
        }

        public static void SetHeaderValue(WebHeaderCollection header, string name, string value)
        {
            var property = typeof(WebHeaderCollection).GetProperty("InnerCollection",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (property != null)
            {
                var collection = property.GetValue(header, null) as NameValueCollection;
                collection[name] = value;
            }
        }

        public static string GetHtmlFromGet(string urlString,Encoding encode,Dictionary<string, string> headers)
        {
            //定义局部变量
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebRespones = null;
            Stream stream = null;
            string htmlString = string.Empty;

            //请求页面
            try
            {
                httpWebRequest = WebRequest.Create(urlString) as HttpWebRequest;
                if (headers != null && headers.Count > 0)
                {
                    foreach (var dict in headers)
                    {
                        SetHeaderValue(httpWebRequest.Headers, dict.Key, dict.Value);
                    }
                }
            }
            //处理异常
            catch (Exception ex)
            {
                throw new Exception("建立页面请求时发生错误！", ex);
            }
            httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; Maxthon 2.0)";
            //获取服务器的返回信息
            try
            {
                httpWebRespones = (HttpWebResponse)httpWebRequest.GetResponse();
                stream = httpWebRespones.GetResponseStream();
            }
            //处理异常
            catch (Exception ex)
            {
                throw new Exception("接受服务器返回页面时发生错误！", ex);
            }
            StreamReader streamReader = new StreamReader(stream, encode);
            //读取返回页面
            try
            {
                htmlString = streamReader.ReadToEnd();
            }
            //处理异常
            catch (Exception ex)
            {
                throw new Exception("读取页面数据时发生错误！", ex);
            }
            //释放资源返回结果
            streamReader.Close();
            stream.Close();
            return htmlString;
        }

        /// <summary>
        /// Get数据接口
        /// </summary>
        /// <param name="getUrl">接口地址</param>
        /// <returns></returns>
        public static string GetWebRequest(string getUrl)
        {
            string responseContent = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getUrl);
            request.ContentType = "application/json";
            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //在这里对接收到的页面内容进行处理
            using (Stream resStream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(resStream, Encoding.UTF8))
                {
                    responseContent = reader.ReadToEnd().ToString();
                }
            }
            return responseContent;
        }
        public static string GetWebRequest(string getUrl, Encoding encoding)
        {
            string responseContent = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getUrl);
            request.ContentType = "application/html";
            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //在这里对接收到的页面内容进行处理
            using (Stream resStream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(resStream, encoding))
                {
                    responseContent = reader.ReadToEnd().ToString();
                }
            }
            return responseContent;
        }
        /// <summary>
        /// Post数据接口
        /// </summary>
        /// <param name="postUrl">接口地址</param>
        /// <param name="paramData">提交json数据</param>
        /// <param name="dataEncode">编码方式(Encoding.UTF8)</param>
        /// <returns></returns>
        public static string PostWebRequest(string postUrl, string paramData, Encoding dataEncode, Dictionary<string, string> headers)
        {
            string responseContent = string.Empty;
            try
            {
                byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                if (headers != null && headers.Count > 0)
                {
                    foreach (var dict in headers)
                    {
                        SetHeaderValue(webReq.Headers, dict.Key, dict.Value);
                    }
                }
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";
                webReq.ContentLength = byteArray.Length;
                using (Stream reqStream = webReq.GetRequestStream())
                {
                    reqStream.Write(byteArray, 0, byteArray.Length);//写入参数
                    //reqStream.Close();
                }
                using (HttpWebResponse response = (HttpWebResponse)webReq.GetResponse())
                {
                    //在这里对接收到的页面内容进行处理
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), dataEncode))
                    {
                        responseContent = sr.ReadToEnd().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return responseContent;
        }

        #region RestApi
        public static string RestDelete(string host, string route, object bodyData, Dictionary<string, string> inHeaderDict)
        {
            return RestPost(host, route, bodyData, inHeaderDict,"DELETE");
        }
        public static string RestPut(string host, string route, object bodyData, Dictionary<string, string> inHeaderDict)
        {
            return RestPost(host, route, bodyData, inHeaderDict, "PUT");
        }
        public static string RestPost(string host, string route, object bodyData, Dictionary<string, string> inHeaderDict, string method = "")
        {
            Method _method = Method.Post;
            switch (method.ToUpper())
            {
                case "PUT":
                    _method = Method.Put;
                    break;
                case "DELETE":
                    _method = Method.Delete;
                    break;
                case "PATCH":
                    _method = Method.Patch;
                    break;
            }
            var client = new RestSharp.RestClient(host);
            var requestPost = new RestRequest(route, _method);
            var json = JsonConvert.SerializeObject(bodyData);
            requestPost.AddParameter("application/json", json, ParameterType.RequestBody);
            if (inHeaderDict != null)
            {
                foreach (var dict in inHeaderDict)
                {
                    if (dict.Key.ToLower() == "cookie")
                    {
                        foreach (var cookie in dict.Value.Split(';'))
                        {
                            var values = cookie.Split('=');
                            requestPost.AddCookie(values[0], values[1], "", "");
                            break;
                        }
                    }
                    else
                        requestPost.AddHeader(dict.Key, dict.Value);
                }

            }
            var responsePost = client.Execute(requestPost);
            return responsePost.Content;
        }
        public static string RestPost(string host,string route,object bodyData, Dictionary<string, string> inHeaderDict, out Dictionary<string, object> outHeaderDict, string method="")
        {
            Method _method = Method.Post;
            switch(method.ToUpper())
            {
                case "PUT":
                    _method = Method.Put;
                    break;
                case "DELETE":
                    _method = Method.Delete;
                    break;
                case "PATCH":
                    _method = Method.Patch;
                    break;
            }
            outHeaderDict = new Dictionary<string, object>();
            var client = new RestSharp.RestClient(host);
            var requestPost = new RestRequest(route, _method);
            var json = JsonConvert.SerializeObject(bodyData);
            requestPost.AddParameter("application/json", json, ParameterType.RequestBody);
            if (inHeaderDict != null)
            {
                foreach (var dict in inHeaderDict)
                {
                    if (dict.Key.ToLower()=="cookie")
                    {
                        foreach (var cookie in dict.Value.Split(';'))
                        {
                            var values = cookie.Split('=');
                            requestPost.AddCookie(values[0], values[1], "", "");
                            break;
                        }
                    }
                    else
                        requestPost.AddHeader(dict.Key, dict.Value);
                }
                
            }
            var responsePost = client.Execute(requestPost);
            foreach(var header in responsePost.Headers)
            {
                outHeaderDict.Add(header.Name.ToLower(), header.Value);
            }
            return responsePost.Content;
        }

        public static string RestGet(string host,string route, Dictionary<string, string> headerDict = null, Dictionary<string,string> paramDict=null)
        {
            var client = new RestSharp.RestClient(host);
            var requestGet = new RestRequest(route, Method.Get);
            if(paramDict!=null)
            {
                foreach (var dict in paramDict)
                {
                    requestGet.AddUrlSegment(dict.Key, dict.Value);
                }
            }
            if (headerDict != null)
            {
                foreach (var dict in headerDict)
                {
                    if (dict.Key.ToLower() == "cookie")
                    {
                        foreach (var cookie in dict.Value.Split(';'))
                        {
                            var values = cookie.Split('=');
                            requestGet.AddCookie(values[0], values[1], "", "");
                            break;
                        }
                    }
                    else
                        requestGet.AddHeader(dict.Key, dict.Value);
                }
            }
            var response = client.Execute(requestGet);
            return response.Content;
        }
        #endregion
    }
}
