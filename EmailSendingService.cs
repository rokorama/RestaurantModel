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

            //Send email
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
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"An error occured with the message that's supposed to be sent out ({e.GetType().Name})");
                System.Threading.Thread.Sleep(1000);
            }
            catch (MailKit.ServiceNotConnectedException e)
            {
                Console.WriteLine($"An error occured with connecting to the email server ({e.GetType().Name})");
                System.Threading.Thread.Sleep(1000);
            }
            catch (MailKit.ServiceNotAuthenticatedException e)
            {
                Console.WriteLine($"An error occured with authenticating with the email server ({e.GetType().Name})");
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
