using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using PermitToWork.Models;

namespace PermitToWork.Utilities
{
    public class SendEmail
    {
        private star_energy_ptwEntities db = new star_energy_ptwEntities();
        public string Send(List<String> to, string message, string subject)
        {
            System.Net.Mail.MailMessage email = new System.Net.Mail.MailMessage();
#if !DEBUG
            foreach (string t in to)
            {
                if (t != null)
                {
                    email.To.Add(t); //recipient
                }
            }
#else
            email.To.Add("septujamasoka@gmail.com");
#endif
            if (email.To.Count > 0)
            {
                email.Subject = subject;
                email.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["smtpuser"]); //from email
                email.IsBodyHtml = true;
                email.Body = message;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["smtp"]);  // you need an smtp server address to send emails
                smtp.UseDefaultCredentials = false;
                System.Net.NetworkCredential nc = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtpuser"], ConfigurationManager.AppSettings["smtppassword"]);
                smtp.Credentials = (System.Net.ICredentialsByHost)nc.GetCredential(ConfigurationManager.AppSettings["smtp"], Int32.Parse(ConfigurationManager.AppSettings["smtpport"]), "Basic");
                smtp.Port = Int32.Parse(ConfigurationManager.AppSettings["smtpport"]);
#if DEBUG
                smtp.EnableSsl = true;
#endif
                try
                {
                    smtp.Send(email);
                }
                catch (SmtpException e)
                {
                    email_error emails = new email_error();
                    emails.emails = String.Join(";", to.ToArray());
                    emails.content = message;
                    emails.subject = subject;
                    emails.exception = e.Message + e.StackTrace;
                    db.email_error.Add(emails);
                    db.SaveChanges();
                    
                }
                finally
                {
                    smtp.Dispose();
                }
            }
            return "200";
        }

        public void SendToNotificationCenter(List<int> id, string report, string remarks, string url)
        {
            WWUserService.UserServiceClient client = new WWUserService.UserServiceClient();

            WWUserService.ResponseModel response = client.CreateNotificationList(Base64.MD5Seal("starenergyww"), id.ToArray(), "PTW", report, remarks, url);

            client.Close();
        }
    }
}
