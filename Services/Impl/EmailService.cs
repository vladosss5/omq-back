using AskAgainApi.Exceptions;
using MimeKit;
using MimeKit.Text;
using System.Text.RegularExpressions;

namespace AskAgainApi.Services.Impl
{
    public class EmailService : IEmailService
    {

        private readonly ILogger<EmailService> _logger;

        private readonly ISMTPService _smtp;

        public EmailService(ILogger<EmailService> logger, ISMTPService smtp)
        {
            _logger = logger;
            _smtp = smtp;
        }

        public async Task SendMessegeAsync(string address, string subject, string body)
        {
            string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";

            if (!Regex.IsMatch(address, pattern, RegexOptions.IgnoreCase))
                throw new HttpException("Email address is not valid.");

            var mail = MakeMessage(address, subject, body);

            try
            {
                await _smtp.SendEmailAsync(mail);
                _logger.LogInformation("Successfully sending");
            }
            catch (Exception e)
            {
                _logger.LogError(e.GetBaseException().Message);
            }

        }

        private static MimeMessage MakeMessage(string address, string subject, string body)
        {
            var mail = new MimeMessage();
            mail.To.Add(MailboxAddress.Parse(address));
            mail.Subject = subject;
            mail.Body = new TextPart(TextFormat.Html) { Text = body };

            return mail;
        }
    }
}
