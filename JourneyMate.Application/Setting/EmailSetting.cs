using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace JourneyMate.Application.Setting
{
    public class EmailSetting
    {
        private MailSetting _options;

        public EmailSetting(IOptions<MailSetting> options)
        {
            _options = options.Value;
        }
        public void SendEmail(Email email)
        {
            var mail = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_options.Email),
                Subject = email.Subject,
            };
            mail.To.Add(MailboxAddress.Parse(email.To));
            var body = new BodyBuilder();
            body.HtmlBody = email.Body;
            mail.Body = body.ToMessageBody();

            mail.From.Add(new MailboxAddress(_options.DispalyName, _options.Email));

            using var smtp = new SmtpClient();
            smtp.Connect(_options.Host, _options.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Email, _options.Password);
            smtp.Send(mail);
            smtp.Disconnect(true);

        }
    }
}
