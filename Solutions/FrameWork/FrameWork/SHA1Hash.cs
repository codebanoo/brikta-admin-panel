using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace FrameWork
{
    public static class SHA1Hash
    {
        public static string GetSHAHash(string input = "")
        {
            try
            {
                HashAlgorithm algorithm = SHA1.Create();
                byte[] butes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in butes)
                    sb.Append(b.ToString("X2"));

                return sb.ToString();
            }
            catch (Exception exc)
            {
                return "error";
            }
        }

        public static string GetSHAHashWithKey(string key, string input = "")
        {
            try
            {
                byte[] hashKey = Encoding.UTF8.GetBytes(key);
                string result = "";

                HMACSHA1 myhmacsha1 = new HMACSHA1(hashKey);
                byte[] byteArray = Encoding.UTF8.GetBytes(input);
                MemoryStream stream = new MemoryStream(byteArray);
                result =  myhmacsha1.ComputeHash(stream).Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s);
                return result;
            }
            catch (Exception exc)
            {
                return "error";
            }
        }
    }
}