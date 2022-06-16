using Donatech.Core.Model;
using Donatech.Core.ServiceProviders.Interfaces;
using Donatech.Core.Utils;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Donatech.Core.ServiceProviders
{
    public class MailServiceProvider: IMailServiceProvider
    {
        private readonly static string cPrefix = "MailServiceProvider";
        private readonly ILogger _logger;
        private readonly MailSettings _mailSettings;

        public MailServiceProvider(ILogger<MailServiceProvider> logger,
            IOptions<MailSettings> mailSettings)
        {
            _logger = logger;
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(MailRequestDto mailRequest)
        {
            string mPrefix = "[SendEmailAsync(MailRequestDto mailRequest)]";

            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
                email.Subject = mailRequest.Subject;
                var builder = new BodyBuilder();
                if (mailRequest.Attachments != null)
                {
                    byte[] fileBytes;
                    foreach (var file in mailRequest.Attachments)
                    {
                        if (file.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);
                                fileBytes = ms.ToArray();
                            }
                            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                        }
                    }
                }
                builder.HtmlBody = mailRequest.Body;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
            catch(Exception ex)
            {
                // En caso de obtener una excepción inesperada, guardamos el valor en el logger
                _logger.AddCustomLog(cPrefix,
                        mPrefix,
                        "Ha ocurrido un error inesperado",
                        ex);
            }
        }
    }
}
