using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FrameWork
{
    public static class MailBeeManagement
    {
        public static Exception Send(string SenderEmail, string to, string DisplayName, string subject, string body)
        {
            try
            {
                //MailBee.SmtpMail.Smtp.LicenseKey = "MN600-533234A832252A2B958D949B675B-0A3F";
                //MailBee.SmtpMail.Smtp mailer = new MailBee.SmtpMail.Smtp();
                //mailer.Message.EncodeAllHeaders(Encoding.UTF8, MailBee.Mime.HeaderEncodingOptions.Base64);

                //mailer.DnsServers.Autodetect();
                //mailer.To = new MailBee.Mime.EmailAddressCollection(to);
                //mailer.From.Email = to;
                //mailer.From.DisplayName = SenderEmail;

                //mailer.Subject = subject;
                //mailer.BodyHtmlText = body;

                //mailer.Message.Charset = "utf-8";
                //mailer.Charset = "utf-8";

                //mailer.Send();
            }
            catch (Exception exc)
            {
                return exc;
            }
            return null;
        }
    }
}