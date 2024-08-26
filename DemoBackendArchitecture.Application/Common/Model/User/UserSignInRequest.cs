using System.ComponentModel.DataAnnotations;

namespace DemoBackendArchitecture.Application.Common.Model.User;

public class UserSignInRequest
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    public string? Password { get; set; }
}