using AutoMapper;
using DemoBackendArchitecture.Application.DTOs;
using DemoBackendArchitecture.Application.Interfaces;
using DemoBackendArchitecture.Domain.Entities;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = DemoBackendArchitecture.API.Models.AuthenticationRequest.LoginRequest;
using RegisterRequest = DemoBackendArchitecture.API.Models.AuthenticationRequest.RegisterRequest;
using TenantInfo = DemoBackendArchitecture.Infrastructure.Helpers.MultiTenancy.TenantInfo;

namespace DemoBackendArchitecture.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController(IUserService userService, IMapper mapper, IBackgroundJobService backgroundJobService, IMultiTenantStore<TenantInfo> tenantStore) : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // backgroundJobService.EnqueueJob(() => Console.WriteLine("Hello from the background job"));
        // backgroundJobService.RecurringJob(() => Console.WriteLine("Hello World"), "* * * * *", TimeZoneInfo.Local);

        var tenantList = tenantStore.GetAllAsync();
        Console.WriteLine(tenantList);
        
        //Validate the request
        if (!ModelState.IsValid)
        {
            return Unauthorized("Invalid request");
        }
        //Map the request to a DTO
        var userDto = mapper.Map<UserDto>(request);
        //Authenticate the user
        var token = userService.Authenticate(userDto);
        if (token == null)
        {
            return Unauthorized("Invalid email or password");
        }
        return Ok(token);
    }
    
    [HttpPost("register")]
    [ProducesResponseType<string>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        //Validate the request
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid request");
        }
        //Map the request to a DTO
        var userDto = mapper.Map<UserDto>(request);
        //Call the register method of the userService
        var createdUser =userService.Register(userDto);
        return Created("User registered successfully", createdUser);
    }
}