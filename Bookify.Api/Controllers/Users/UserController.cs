﻿using Asp.Versioning;
using Bookify.Application.User.GetLoggedIn;
using Bookify.Application.User.Login;
using Bookify.Application.User.Registration;
using Bookify.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Users;

[ApiController]
[Route("api/v{version:apiVersion}/users")]
[ApiVersion(ApiVersions.V1)]
public class UserController : Controller
{
    private readonly ISender _mediator;

    public UserController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    //[Authorize(Roles = Roles.Registered)]
    [HasPermission(Permissions.UsersRead)]
    [HttpGet("me")]
    public async Task<IActionResult> GetLoggedInUser(CancellationToken cancellationToken)
    {
        var query = new GetLoggedInUserQuery();
        
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result.Value);
    }
    
    [HasPermission(Permissions.UsersRead)]
    [ApiVersion(ApiVersions.V2)]
    [HttpGet("me")]
    public Task<IActionResult> GetLoggedInUserV2(CancellationToken cancellationToken)
    {
        return GetLoggedInUser(cancellationToken);
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterRequest request, CancellationToken cancellationToken)
    {
        var command = new UserRegisterCommand(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password);

        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LogIn(
        LogInUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LogInUserCommand(request.Email, request.Password);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return Unauthorized(result.Error);
        }

        return Ok(result.Value);
    }
}