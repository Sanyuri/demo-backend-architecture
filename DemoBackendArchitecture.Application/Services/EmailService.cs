using DemoBackendArchitecture.Application.Interfaces;
using NetCore.AutoRegisterDi;

namespace DemoBackendArchitecture.Application.Services;

[RegisterAsSingleton]
public class EmailService : IEmailService
{
    public void SendEmail(string to, string subject, string body)
    {
        
    }
}