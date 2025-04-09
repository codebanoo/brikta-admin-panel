using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;

namespace FrameWork
{
    public static class SmtpEmail
    {
        public static bool Send(string FromEmail, 
            string FromName, 
            string ToEmail, 
            string ReplyTo, 
            string Subject, 
            string[] Attachments,
            string EmailBody, 
            bool IsBodyHtml, 
            string EmailServer, 
            string LoginName, 
            string LoginPassword, 
            bool EnableSsl)
        {
            try
            {
                System.Net.Mail.MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(FromEmail, FromName);
                mailMessage.To.Add(new MailAddress(ToEmail));
                mailMessage.ReplyToList.Add(new MailAddress(ReplyTo));
                //mailMessage.ReplyTo = new MailAddress(replyTo);

                mailMessage.Subject = Subject;
                mailMessage.Body = EmailBody;
                mailMessage.IsBodyHtml = IsBodyHtml;
                if (Attachments != null && Attachments.Length > 0)
                    foreach (string attachment in Attachments)
                        mailMessage.Attachments.Add(new Attachment(attachment));

                SmtpClient smtpClient = new SmtpClient(EmailServer);

                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                NetworkCredential networkCredential = new NetworkCredential(LoginName, LoginPassword);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.EnableSsl = EnableSsl;
                smtpClient.Credentials = networkCredential;

                smtpClient.Send(mailMessage);

                mailMessage.Dispose();
                smtpClient = null;
                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }
    }
}