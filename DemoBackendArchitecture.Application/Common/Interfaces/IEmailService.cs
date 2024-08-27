using DemoBackendArchitecture.Application.Common.Model.Email;

namespace DemoBackendArchitecture.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendMailAsync(MailRequest request);
}