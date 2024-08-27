using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DemoBackendArchitecture.Application.Common.Model.Email;

public class MailRequest
{
    [Required]
    public string ToEmail { get; set; } = null!;
    [Required]
    public string Subject { get; set; } = null!;
    [Required]
    public string Body { get; set; } = null!;
    public List<IFormFile>? Attachments { get; set; }
}