using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace myclinic_back.Utilities.Service
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void Send(string toEmail, string messageText)
        {
            var sender = _config["Email:Sender"];
            var password = _config["Email:Password"];
            var smtp = _config["Email:SmtpServer"];
            var port = int.Parse(_config["Email:Port"]);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Email Logger", sender));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = "CRUD Operation Performed";

            message.Body = new TextPart("plain")
            {
                Text = messageText
            };

            using var client = new SmtpClient();
            client.Connect(smtp, port, SecureSocketOptions.StartTls);
            client.Authenticate(sender, password);
            client.Send(message);
            client.Disconnect(true);
        }
    }
}
