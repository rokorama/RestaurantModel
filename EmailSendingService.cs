using System;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace RestaurantModel
{
    static public class EmailSendingService
    {
        static public void SendEmail(string recipientAddress)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(SettingConstants.MailClientUsername));
            email.To.Add(MailboxAddress.Parse(recipientAddress));
            email.Subject = "Rokorama Fake Goods Inc. order receipt";
            email.Body = new TextPart(TextFormat.Html) { Text = "<h1>Example HTML Message Body</h1>\n<p>Oh shit it's working</p>" };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(SettingConstants.MailServer, SettingConstants.MailServerPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(SettingConstants.MailClientUsername, SettingConstants.MailClientPassword);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
