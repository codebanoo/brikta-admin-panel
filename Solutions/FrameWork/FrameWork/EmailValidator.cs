using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace FrameWork
{
    public static class EmailValidator
    {
        public static bool IsValid(string Email)
        {
            string expression = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|" + @"0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z]" + @"[a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";

            Match match = Regex.Match(Email, expression, RegexOptions.IgnoreCase);
            if (match.Success)
                return true;
            return false;
        }

        public static bool VerifyEmail(string Email)
        {
            try
            {
                string host = Email.Split('@')[1];
                string server = "";
                switch (host.ToLower())
                {
                    case "gmail.com": server = "gmail-smtp-in.l.google.com";
                        break;
                    case "yahoo.com": server = "mta-v1.mail.vip.ne1.yahoo.com";
                        break;
                    default: server = host;
                        break;
                }
                TcpClient tClient = new TcpClient(server, 25);
                string CRLF = "\r\n";
                byte[] dataBuffer;
                string ResponseString;
                NetworkStream netStream = tClient.GetStream();
                StreamReader reader = new StreamReader(netStream);
                ResponseString = reader.ReadLine();
                /* Perform HELO to SMTP Server and get Response */
                dataBuffer = BytesFromString("HELO KirtanHere" + CRLF);
                netStream.Write(dataBuffer, 0, dataBuffer.Length);
                ResponseString = reader.ReadLine();
                dataBuffer = BytesFromString("MAIL FROM:<YourGmailIDhere@gmail.com>" + CRLF);
                netStream.Write(dataBuffer, 0, dataBuffer.Length);
                ResponseString = reader.ReadLine();
                /* Read Response of the RCPT TO Message to know from google if it exist or not */
                dataBuffer = BytesFromString("RCPT TO:<" + Email.Trim() + ">" + CRLF);
                netStream.Write(dataBuffer, 0, dataBuffer.Length);
                ResponseString = reader.ReadLine();
                if (GetResponseCode(ResponseString) == 550)
                {
                    return false;
                }
                /* QUITE CONNECTION */
                dataBuffer = BytesFromString("QUITE" + CRLF);
                netStream.Write(dataBuffer, 0, dataBuffer.Length);
                tClient.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #region Required Methods

        private static byte[] BytesFromString(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        private static int GetResponseCode(string ResponseString)
        {
            try
            {
                return int.Parse(ResponseString.Substring(0, 3));
            }
            catch
            {
                return 0;
            }
        }

        #endregion
    }
}