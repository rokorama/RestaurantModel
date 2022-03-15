using System;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace RestaurantModel
{
    static public class EmailSendingService
    {
        static public void SendEmail(string recipientAddress, string messageBody)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(SettingConstants.MailClientUsername));
            email.To.Add(MailboxAddress.Parse(recipientAddress));
            email.Subject = $"Your receipt from {SettingConstants.RestaurantName}";
            email.Body = new TextPart(TextFormat.Html) { Text = messageBody };

            //add error handling throughout

            // send email
            using var smtp = new SmtpClient();
            try
            {
                smtp.Connect(SettingConstants.MailServer, SettingConstants.MailServerPort, SecureSocketOptions.StartTls);
                smtp.Authenticate(SettingConstants.MailClientUsername, SettingConstants.MailClientPassword);
                smtp.Send(email);
                smtp.Disconnect(true);

                Console.WriteLine($"\nEmail successfully sent to {recipientAddress}\n");
                System.Threading.Thread.Sleep(1000);
            }
            catch // TODO - specify exceptions
            {
                Console.WriteLine("Something went wrong with sending the email!");
            }
        }
    }
}
