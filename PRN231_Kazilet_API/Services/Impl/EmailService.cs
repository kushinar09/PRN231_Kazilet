using MimeKit;

namespace PRN231_Kazilet_API.Services.Impl
{
    public class EmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _username = "hola.housing.1306@gmail.com";
        private readonly string _password = "vaap gqqy pvta qklp";
        public async Task SendEmailAsync(string toEmail, string tag, string subject, string htmlText)
        {
            //var filePath = Path.Combine(_env.WebRootPath, "template", fileName);
            //var htmlContent = await File.ReadAllTextAsync(filePath);
            // htmlContent.replace("{new@Password!Here}", newPwd);

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Kazilet", _username));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = !String.IsNullOrEmpty(tag) ? $"[{tag}] {subject}" : subject;

            var body = new TextPart("html")
            {
                Text = htmlText
            };

            email.Body = body;

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_username, _password);

                await client.SendAsync(email);
                await client.DisconnectAsync(true);
            }
        }
    }
}
