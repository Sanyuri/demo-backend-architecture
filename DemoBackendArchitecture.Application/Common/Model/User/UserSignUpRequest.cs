namespace DemoBackendArchitecture.Application.Common.Model.User;

public class UserSignUpRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}