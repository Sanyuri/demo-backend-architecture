using System.Security.Claims;
using DemoBackendArchitecture.Application.Common.Interfaces;
using NetCore.AutoRegisterDi;

namespace DemoBackendArchitecture.Application.Services;

public class CurrentUserService(ICookieService cookieService, ITokenService tokenService) : ICurrentUserService
{
    private readonly ICookieService _cookieService = cookieService;
    private readonly ITokenService _tokenService = tokenService;
    public string GetCurrentUserEmail()
    {
        //Get the current user from cookie 
        var jwtCookie = _cookieService.Get();
        //Validate the token
        var token = _tokenService.ValidateToken(jwtCookie);
        //Get the user id from the token
        var userEmail = token.Claims.First(x => x.Type == ClaimTypes.Email).Value;
        return userEmail;
        
    }
}