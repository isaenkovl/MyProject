using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MyPhoneBook.Manager
{
    public class MailSender
    {
        static bool mailSent = false;
        private String token = "Token";
        private MailMessage message;
        private int counter = 0;

        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            String token = (string)e.UserState;
            mailSent = true;
            message.Dispose();
        }

        public void SendMessage(string address, string subject, string body)
        { 
            string kassa = System.Environment.MachineName;

            var fromAddress = new MailAddress(Properties.Resources.EmailAddress, "MyPhoneBook");
            var toAddress = new MailAddress(address);
            string fromPassword = Properties.Resources.PasswordMail;


            var smtp = new SmtpClient
            {
                Host = Properties.Resources.MailServer,
                Port = Convert.ToInt32(Properties.Resources.MailPort),
                EnableSsl = true,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };


            message = new MailMessage(fromAddress, toAddress);
            message.Subject = subject;
            message.Body = body;
            message.To.Add(toAddress);
            
            try
            {
                smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                smtp.SendAsync(message, token);
                counter = 0;
            }
            catch (SmtpException e)
            {
                if (counter == 0)
                {
                    counter++;
                    SendMessage(address, subject, body);
                }
            }
        }
    }
}
