using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using DemoBackendArchitecture.Application.DTOs;
using DemoBackendArchitecture.Application.Interfaces;
using DemoBackendArchitecture.Domain.Entities;
using DemoBackendArchitecture.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using NetCore.AutoRegisterDi;

namespace DemoBackendArchitecture.Application.Services;

[RegisterAsTransient]
public class UserService(IUserRepository userRepository, IConfiguration configuration,IPasswordHasher<User> passwordHasher, IMapper mapper, IRoleService roleService) : IUserService
{
    public string? Authenticate(UserDto userDto)
    {
        // Get the user from the database
        var user = userRepository.GetUserByEmail(userDto.Email);
        // Check if the user exists and the password is correct
        if (user == null || passwordHasher.VerifyHashedPassword(user, user.Password!, userDto.Password!) == PasswordVerificationResult.Failed)
        {
            return null;
        }
        // Retrieve JWT settings from configuration
        var jwtSettings = configuration.GetSection("Jwt");
        var key = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT_KEY environment variable is not set");
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expireMinutes = Convert.ToInt32(jwtSettings["ExpireMinutes"]);
        var algorithm = jwtSettings["Algorithm"];
        
        // Create security key and signing credentials
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var signingCredentials = new SigningCredentials(securityKey, algorithm);
        
        // Defne token descriptor
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Name!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, user.Role!.RoleName!)
            }),
            Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = signingCredentials
        };
        
        // Create token handler and generate token
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        // Return the token
        return tokenHandler.WriteToken(token);
    }

    public User Register(UserDto userDto)
    {
        //Mapping the userDto to a User entity
        var user = mapper.Map<User>(userDto);
        //Hash the password
        user.Password = passwordHasher.HashPassword(user, user.Password!);
        //Set role user to User
        user.RoleId = roleService.GetRoleByName("User")!.Id;
        //Call the Add method of the userRepository
        return userRepository.Add(user);
    }
}