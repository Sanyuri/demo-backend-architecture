using AutoMapper;
using DemoBackendArchitecture.Application.Common.Exception;
using DemoBackendArchitecture.Application.Common.Interfaces;
using DemoBackendArchitecture.Application.Common.Model.User;
using DemoBackendArchitecture.Application.Common.Utilities;
using DemoBackendArchitecture.Domain.Constants;
using DemoBackendArchitecture.Domain.Entities;
using DemoBackendArchitecture.Domain.Interfaces;
using NetCore.AutoRegisterDi;

namespace DemoBackendArchitecture.Application.Services;

public class AuthService(
    IUserRepository userRepository,
    ITokenService tokenService,
    ICurrentUserService currentUserService,
    ICookieService cookieService,
    IMapper mapper) : IAuthService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITokenService _tokenService = tokenService;
    private readonly ICurrentUserService _currentUser = currentUserService;
    private readonly ICookieService _cookieService = cookieService;
    private readonly IMapper _mapper = mapper;
    public async Task<UserSignInResponse> SignIn(UserSignInRequest request)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email)
            ?? throw UserException.BadRequestException(UserErrorMessage.UserNotExist);
        //using Identity to verify password
        if (StringHelper.Verify(user, request.Password!))
        {
            throw UserException.BadRequestException(UserErrorMessage.PasswordIncorrect);
        }
        
        //generate token
        var token = _tokenService.GenerateToken(user);
        //set token to cookie
        _cookieService.Set(token);
        
        //map user to response
        var response = _mapper.Map<UserSignInResponse>(user);
        //set token to response
        response.Token = token;
        //set user role to response
        response.Role = user.Role.RoleName;
        //return response
        return response;
    }

    public async Task<UserSignUpResponse> SignUp(UserSignUpRequest request, CancellationToken token)
    {
        //check if email exist
        var isEmailExist = await _userRepository.AnyAsync(x => x.Email == request.Email);
        if (isEmailExist)
            throw UserException.UserAlreadyExistsException(request.Email);
        
        //map request to user
        var user = _mapper.Map<User>(request);
        //hash password
        user.Password = user.Password.Hash();
        //set user role
        user.Role = new Role { RoleName = RoleConstants.Admin };
        //add user to database
        await _userRepository.AddAsync(user, token);
        //Save changes
        await _userRepository.SaveChangeAsync(token);
        //map user to response
        var response = _mapper.Map<UserSignUpResponse>(user);
        //Set user role to response
        response.Role = user.Role.RoleName;
        //return response
        return response;
    }

    public void Logout()
    {
        try
        {
            //get token from cookie
            _ = _cookieService.Get();
            //delete token from cookie
            _cookieService.Delete();
        }
        catch { }
    }

    public async Task<string?> RefreshToken()
    {
        //get current user from cookie
        var user = await _userRepository.GetUserByEmailAsync(_currentUser.GetCurrentUserEmail());
        //generate token
        var accessToken = _tokenService.GenerateToken(user);
        //set token to cookie
        _cookieService.Set(accessToken);
        //return token
        return accessToken;
    }
}