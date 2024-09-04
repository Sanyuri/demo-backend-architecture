using DemoBackendArchitecture.Application.Common.Interfaces;
using DemoBackendArchitecture.Application.Common.Model.Email;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DemoBackendArchitecture.API.Controllers;

[EnableCors("CorsPolicy")]
public class MailController(IEmailService emailService) : BaseController
{
    private readonly IEmailService _emailService = emailService;

    [HttpPost("send")]
    public async Task<IActionResult> SendMail([FromBody] MailRequest request)
    {
        await _emailService.SendMailAsync(request);
        return Ok("Mail sent successfully");
    }
}