using AuthService.Application.Auth.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(ISender sender) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("User Register")]
    public async Task<ActionResult> RegisterUser([FromBody] RegisterUserRequest request)
    {
        var command = new RegisterUserCommand(request);
        var result = await sender.Send(command);
        return Ok(result);
    }
}