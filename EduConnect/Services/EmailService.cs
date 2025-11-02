using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace EduConnect.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _cfg;
        public EmailService(IConfiguration cfg) => _cfg = cfg;

        public async Task SendAsync(string toEmail, string subject, string htmlBody, CancellationToken ct = default)
        {
            var mail = _cfg.GetSection("Mail");
            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress(mail["FromName"], mail["FromEmail"]));
            msg.To.Add(MailboxAddress.Parse(toEmail));
            msg.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = htmlBody };
            msg.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            var host = mail["SmtpHost"];
            var port = int.Parse(mail["SmtpPort"]!);
            var useStartTls = bool.Parse(mail["UseStartTls"] ?? "true");

            await smtp.ConnectAsync(host, port, useStartTls ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto, ct);
            await smtp.AuthenticateAsync(mail["Username"], mail["Password"], ct);
            await smtp.SendAsync(msg, ct);
            await smtp.DisconnectAsync(true, ct);
        }
    }
}
