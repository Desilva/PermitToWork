using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Utilities
{
    public class SendEmail
    {
        public string Send(List<String> to, string message, string subject)
        {
            System.Net.Mail.MailMessage email = new System.Net.Mail.MailMessage();
            foreach (string t in to)
            {
                if (t != null)
                {
                    email.To.Add(t); //recipient
                }
            }
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
                smtp.EnableSsl = true;
                try
                {
                    smtp.Send(email);
                }
                catch (SmtpException e)
                {

                }
                finally
                {
                    smtp.Dispose();
                }
            }
            return "200";
        }

    }
}
