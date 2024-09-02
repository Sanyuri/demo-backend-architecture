using DemoBackendArchitecture.Application.Common.Interfaces;
using DemoBackendArchitecture.Application.Common.Model.User;
using Microsoft.AspNetCore.Mvc;

namespace DemoBackendArchitecture.API.Controllers;

[IgnoreAntiforgeryToken]
public class AuthController(IAuthService authService) : BaseController
{
    private readonly IAuthService _authService = authService;
    
    [HttpPost("login")]
    public async Task<IActionResult> SignIn([FromBody] UserSignInRequest request) 
        => Ok(await _authService.SignIn(request));
    
    [HttpPost("register")]
    public async Task<IActionResult> SignUp([FromBody] UserSignUpRequest request, CancellationToken token) 
        => Created("",await _authService.SignUp(request, token));
    
    [HttpDelete("logout")]
    public IActionResult Logout()
    {
        _authService.Logout();
        return Ok();
    }
    
    [HttpGet("refresh")]
    public async Task<IActionResult> RefreshToken() 
        => Ok(await _authService.RefreshToken());
}