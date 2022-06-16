using Donatech.Core.Model;

namespace Donatech.Core.ServiceProviders.Interfaces
{
    public interface IMailServiceProvider
    {
        Task SendEmailAsync(MailRequestDto mailRequest);
    }
}
