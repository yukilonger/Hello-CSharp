using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Utility
{
    /// <summary>
    /// 加/解密
    /// </summary>
    public class Encryption
    {
        /// <summary>
        /// DES数据加密
        /// </summary>
        /// <param name="targetValue">目标值</param> 
        /// <returns>加密值</returns>
        public static string Encrypt(string targetValue, string key)
        {
            try
            {
                if (string.IsNullOrEmpty(targetValue) || string.IsNullOrWhiteSpace(key))
                {
                    return string.Empty;
                }
                key = key.ToUpper();

                var returnValue = new StringBuilder();
                var des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.Default.GetBytes(targetValue);
                // 通过两次哈希密码设置对称算法的初始化向量   
                des.Key = ASCIIEncoding.ASCII.GetBytes(MD5(key).Substring(0, 8));
                // 通过两次哈希密码设置算法的机密密钥   
                des.IV = ASCIIEncoding.ASCII.GetBytes(MD5(key).Substring(0, 8));
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                foreach (byte b in ms.ToArray())
                {
                    returnValue.AppendFormat("{0:X2}", b);
                }
                return returnValue.ToString();
            }
            catch (Exception ex)
            {
                return targetValue;
            }
        }

        public static string Decrypt(string targetValue, string key)
        {
            try
            {
                if (string.IsNullOrEmpty(targetValue) || string.IsNullOrWhiteSpace(key))
                {
                    return string.Empty;
                }
                key = key.ToUpper();
                // 定义DES加密对象
                var des = new DESCryptoServiceProvider();
                int len = targetValue.Length / 2;
                var inputByteArray = new byte[len];
                int x, i;
                for (x = 0; x < len; x++)
                {
                    i = Convert.ToInt32(targetValue.Substring(x * 2, 2), 16);
                    inputByteArray[x] = (byte)i;
                }
                // 通过两次哈希密码设置对称算法的初始化向量   
                des.Key = ASCIIEncoding.ASCII.GetBytes(MD5(key).Substring(0, 8));
                // 通过两次哈希密码设置算法的机密密钥   
                des.IV = ASCIIEncoding.ASCII.GetBytes(MD5(key).Substring(0, 8));
                // 定义内存流
                var ms = new MemoryStream();
                // 定义加密流
                var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Encoding.Default.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                return targetValue;
            }
        }
        public static string MD5(string str)
        {
            //微软md5方法参考return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "md5");
            byte[] b = Encoding.Default.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("X").PadLeft(2, '0');
            return ret;
        }
    }

    public class RTool
    {
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="destStr">目标字符串</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        public string Encrypt(string destStr, string publicKey)
        {
            publicKey = publicKey.Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Trim();
            byte[] data = Encoding.UTF8.GetBytes(destStr);
            RSACryptoServiceProvider rsa = DecodePemPublicKey(publicKey);
            SHA1 sh = new SHA1CryptoServiceProvider();
            byte[] result = rsa.Encrypt(data, false);
            return Convert.ToBase64String(result);
        }
        private RSACryptoServiceProvider DecodePemPublicKey(string pemstr)
        {
            byte[] pkcs8publickkey;
            pkcs8publickkey = Convert.FromBase64String(pemstr);
            if (pkcs8publickkey != null)
            {
                RSACryptoServiceProvider rsa = DecodeRSAPublicKey(pkcs8publickkey);
                return rsa;
            }
            else
                return null;
        }

        private RSACryptoServiceProvider DecodeRSAPublicKey(byte[] publickey)
        {
            // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"    
            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];
            // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------    
            MemoryStream mem = new MemoryStream(publickey);
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading    
            byte bt = 0;
            ushort twobytes = 0;

            try
            {

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)    
                    binr.ReadByte();    //advance 1 byte    
                else if (twobytes == 0x8230)
                    binr.ReadInt16();   //advance 2 bytes    
                else
                    return null;

                seq = binr.ReadBytes(15);       //read the Sequence OID    
                if (!CompareBytearrays(seq, SeqOID))    //make sure Sequence for OID is correct    
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8103) //data read as little endian order (actual data order for Bit String is 03 81)    
                    binr.ReadByte();    //advance 1 byte    
                else if (twobytes == 0x8203)
                    binr.ReadInt16();   //advance 2 bytes    
                else
                    return null;

                bt = binr.ReadByte();
                if (bt != 0x00)     //expect null byte next    
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)    
                    binr.ReadByte();    //advance 1 byte    
                else if (twobytes == 0x8230)
                    binr.ReadInt16();   //advance 2 bytes    
                else
                    return null;

                twobytes = binr.ReadUInt16();
                byte lowbyte = 0x00;
                byte highbyte = 0x00;

                if (twobytes == 0x8102) //data read as little endian order (actual data order for Integer is 02 81)    
                    lowbyte = binr.ReadByte();  // read next bytes which is bytes in modulus    
                else if (twobytes == 0x8202)
                {
                    highbyte = binr.ReadByte(); //advance 2 bytes    
                    lowbyte = binr.ReadByte();
                }
                else
                    return null;
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };   //reverse byte order since asn.1 key uses big endian order    
                int modsize = BitConverter.ToInt32(modint, 0);

                byte firstbyte = binr.ReadByte();
                binr.BaseStream.Seek(-1, SeekOrigin.Current);

                if (firstbyte == 0x00)
                {   //if first byte (highest order) of modulus is zero, don't include it    
                    binr.ReadByte();    //skip this null byte    
                    modsize -= 1;   //reduce modulus buffer size by 1    
                }

                byte[] modulus = binr.ReadBytes(modsize);   //read the modulus bytes    

                if (binr.ReadByte() != 0x02)            //expect an Integer for the exponent data    
                    return null;
                int expbytes = (int)binr.ReadByte();        // should only need one byte for actual exponent data (for all useful values)    
                byte[] exponent = binr.ReadBytes(expbytes);

                // ------- create RSACryptoServiceProvider instance and initialize with public key -----    
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSAParameters RSAKeyInfo = new RSAParameters();
                RSAKeyInfo.Modulus = modulus;
                RSAKeyInfo.Exponent = exponent;
                RSA.ImportParameters(RSAKeyInfo);
                return RSA;
            }
            catch (Exception)
            {
                return null;
            }

            finally { binr.Close(); }

        }

        private bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }

        #region X509
        string cerPath = "";
        string pfxPath = "";
        public void SetCerPath(string path)
        {
            cerPath = path;
        }
        public void SetPfxPath(string path)
        {
            pfxPath = path;
        }

        //public static void RsaTest()
        //{
        //    try
        //    {
        //        //Create a UnicodeEncoder to convert between byte array and string.
        //        UnicodeEncoding ByteConverter = new UnicodeEncoding();


        //        //Create byte arrays to hold original, encrypted, and decrypted data.
        //        byte[] dataToEncrypt = ByteConverter.GetBytes("Data to Encrypt");
        //        byte[] encryptedData;
        //        byte[] decryptedData;

        //        X509Certificate2 pubcrt = new X509Certificate2(AppDomain.CurrentDomain.BaseDirectory + "bfkey.cer");
        //        RSACryptoServiceProvider pubkey = (RSACryptoServiceProvider)pubcrt.PublicKey.Key;
        //        X509Certificate2 prvcrt = new X509Certificate2(AppDomain.CurrentDomain.BaseDirectory + "bfkey.pfx", "123456789", X509KeyStorageFlags.Exportable);
        //        RSACryptoServiceProvider prvkey = (RSACryptoServiceProvider)prvcrt.PrivateKey;

        //        encryptedData = RSAEncrypt(dataToEncrypt, pubkey.ExportParameters(false), false);
        //        Console.WriteLine("Encrypted plaintext: {0}", Convert.ToBase64String(encryptedData));



        //        decryptedData = RSADecrypt(encryptedData, prvkey.ExportParameters(true), false);


        //        Console.WriteLine("Decrypted plaintext: {0}", ByteConverter.GetString(decryptedData));


        //        //加密长内容
        //        String data = @"RSA 是常用的非对称加密算法。最近使用时却出现了“不正确的长度”的异常，研究发现是由于待加密的数据超长所致。
　　      //              .NET Framework 中提供的 RSA 算法规定：
　　      //              待加密的字节数不能超过密钥的长度值除以 8 再减去 11（即：RSACryptoServiceProvider.KeySize / 8 - 11），而加密后得到密文的字节数，正好是密钥的长度值除以 8（即：RSACryptoServiceProvider.KeySize / 8）。
　　      //              所以，如果要加密较长的数据，则可以采用分段加解密的方式，实现方式如下：";
        //        string encrypt = Encrypt(data, pubcrt);
        //        Console.WriteLine("Encrypted plaintext: {0}", encrypt);
        //        string decrypt = Decrypt(encrypt, prvcrt);
        //        Console.WriteLine("Decrypted plaintext: {0}", decrypt);

        //        prvkey.Clear();
        //        pubkey.Clear();
        //        Console.Read();
        //    }
        //    catch (ArgumentNullException)
        //    {
        //        //Catch this exception in case the encryption did
        //        //not succeed.
        //        Console.WriteLine("Encryption failed.");
        //    }
        //}


        byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {


                    //Import the RSA Key information. This only needs
                    //toinclude the public key information.
                    RSA.ImportParameters(RSAKeyInfo);


                    //Encrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.  
                    encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
                }
                return encryptedData;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);


                return null;
            }
        }


        byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Import the RSA Key information. This needs
                    //to include the private key information.
                    RSA.ImportParameters(RSAKeyInfo);


                    //Decrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.  
                    decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
                }
                return decryptedData;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());


                return null;
            }


        }


        public string Encrypt(string plaintext)
        {
            if (File.Exists(cerPath))
            {
                X509Certificate2 pubcrt = new X509Certificate2(cerPath);
                using (RSACryptoServiceProvider RSACryptography = pubcrt.PublicKey.Key as RSACryptoServiceProvider)
                {
                    Byte[] PlaintextData = Encoding.UTF8.GetBytes(plaintext);
                    int MaxBlockSize = RSACryptography.KeySize / 8 - 11;    //加密块最大长度限制
                    if (PlaintextData.Length <= MaxBlockSize)
                        return Convert.ToBase64String(RSACryptography.Encrypt(PlaintextData, false));
                    using (MemoryStream PlaiStream = new MemoryStream(PlaintextData))
                    using (MemoryStream CrypStream = new MemoryStream())
                    {
                        Byte[] Buffer = new Byte[MaxBlockSize];
                        int BlockSize = PlaiStream.Read(Buffer, 0, MaxBlockSize);
                        while (BlockSize > 0)
                        {
                            Byte[] ToEncrypt = new Byte[BlockSize];
                            Array.Copy(Buffer, 0, ToEncrypt, 0, BlockSize);
                            Byte[] Cryptograph = RSACryptography.Encrypt(ToEncrypt, false);
                            CrypStream.Write(Cryptograph, 0, Cryptograph.Length);
                            BlockSize = PlaiStream.Read(Buffer, 0, MaxBlockSize);
                        }
                        return Convert.ToBase64String(CrypStream.ToArray(), Base64FormattingOptions.None);
                    }
                }
            }
            else return "";
        }


        public string Decrypt(string ciphertext, string passwd)
        {
            X509Certificate2 prvcrt = new X509Certificate2(pfxPath, passwd, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
            using (RSACryptoServiceProvider RSACryptography = prvcrt.PrivateKey as RSACryptoServiceProvider)
            {
                Byte[] CiphertextData = Convert.FromBase64String(ciphertext);
                int MaxBlockSize = RSACryptography.KeySize / 8;    //解密块最大长度限制
                if (CiphertextData.Length <= MaxBlockSize)
                    return Encoding.UTF8.GetString(RSACryptography.Decrypt(CiphertextData, false));
                using (MemoryStream CrypStream = new MemoryStream(CiphertextData))
                using (MemoryStream PlaiStream = new MemoryStream())
                {
                    Byte[] Buffer = new Byte[MaxBlockSize];
                    int BlockSize = CrypStream.Read(Buffer, 0, MaxBlockSize);
                    while (BlockSize > 0)
                    {
                        Byte[] ToDecrypt = new Byte[BlockSize];
                        Array.Copy(Buffer, 0, ToDecrypt, 0, BlockSize);
                        Byte[] Plaintext = RSACryptography.Decrypt(ToDecrypt, false);
                        PlaiStream.Write(Plaintext, 0, Plaintext.Length);
                        BlockSize = CrypStream.Read(Buffer, 0, MaxBlockSize);
                    }
                    return Encoding.UTF8.GetString(PlaiStream.ToArray());
                }
            }
        }

        #endregion
    }

    public class SecurityEncDecrypt
    {
        #region 
        private static byte[] Keys = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
        /// <summary> 
        /// DES加密字符串 
        /// </summary> 
        /// <param name="encryptString">待加密的字符串</param> 
        /// <param name="encryptKey">加密密钥,要求为16位</param> 
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns> 

        public static string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 16));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                var DCSP = Aes.Create();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception("数据库密码加密异常" + ex.Message);
            }

        }

        /// <summary> 
        /// DES解密字符串 
        /// </summary> 
        /// <param name="decryptString">待解密的字符串</param> 
        /// <param name="decryptKey">解密密钥,要求为16位,和加密密钥相同</param> 
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns> 

        public static string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 16));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                var DCSP = Aes.Create();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                Byte[] inputByteArrays = new byte[inputByteArray.Length];
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        #endregion
    }
}