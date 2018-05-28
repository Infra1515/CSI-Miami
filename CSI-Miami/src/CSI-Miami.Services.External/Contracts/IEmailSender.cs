using System.Threading.Tasks;

namespace CSI_Miami.Services.External
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
