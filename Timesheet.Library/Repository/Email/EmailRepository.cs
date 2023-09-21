using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace Timesheet.Library.Repository.Email
{
    public class EmailRepository : IEmailRepository
    {
        public bool Send(string to, string subject, string body)
        {
            bool result = false;

            try
            {
                MailMessage message = new MailMessage(ConfigurationManager.AppSettings.Get("smtpFrom"), to, subject, body)
                {
                    IsBodyHtml = true
                };

                SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings.Get("smtpHost"), Int32.Parse(ConfigurationManager.AppSettings.Get("smtpPort"))) 
                { 
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings.Get("smtpUser"), ConfigurationManager.AppSettings.Get("smtpPassword"))
                    //,
                    //EnableSsl = true
                };                

                client.Send(message);

                result = true;
            }
            catch (Exception)
            {
 
            }

            return result;
        }
    }
}
