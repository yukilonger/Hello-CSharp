using System.Net.Mail;

namespace Utility
{
    static public class EmailHelper
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="toAccount">接收账户(多个以','分割)</param>
        public static void Send(string title,string content,string toAccount)
        {
            string fromAccount = "x.com";
            string userName = "x.com";
            string password = "x";

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromAccount);
            // 发给多个人
            toAccount.Split(',').ToList().ForEach(s => mail.To.Add(s));
            mail.Subject = title;
            mail.IsBodyHtml = true;
            mail.Body = content;
            SmtpClient smtp = new SmtpClient("smtp.exmail.qq.com");
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(userName, password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(mail);
        }

    }
}
