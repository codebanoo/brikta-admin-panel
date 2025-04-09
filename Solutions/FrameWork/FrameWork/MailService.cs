using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace FrameWork
{
    public static class MailService
    {
        public static Exception Send(string from = "arashkazemi5@gmail.com",
            List<string> to = null,
            string body = "",
            bool isBodyHtml = true,
            string subject = "",
            int port = 587,
            bool enableSsl = true,
            string host = "smtp.gmail.com",
            SmtpDeliveryMethod deliveryMethod = SmtpDeliveryMethod.Network,
            string username = "arashkazemi5@gmail.com",
            string password = "godgodgod123")
        {
            try
            {
                if (string.IsNullOrEmpty(from))
                    from = "arashkazemi5@gmail.com";

                if (to == null)
                    to = new List<string>() { "arashkazemi5@gmail.com" };

                if (string.IsNullOrEmpty(body))
                    body = "";

                if (string.IsNullOrEmpty(host))
                    host = "smtp.gmail.com";

                SmtpClient LocalClient = new SmtpClient();
                MailMessage Mail = new MailMessage();
                Mail.From = new MailAddress(from);
                foreach (var receiver in to)
                {
                    Mail.To.Add(receiver);
                }
                Mail.Body = body;
                Mail.IsBodyHtml = isBodyHtml;
                Mail.Subject = subject;
                LocalClient.Port = port;
                LocalClient.EnableSsl = enableSsl;
                LocalClient.Host = host;
                LocalClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                LocalClient.Credentials = new NetworkCredential(username, password);
                LocalClient.Send(Mail);
            }
            catch (Exception exc)
            {
                return exc;
            }
            return null;
        }
    }
}
