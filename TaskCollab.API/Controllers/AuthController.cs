using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskCollab.Application.Features.Auth.Commands.Login;

namespace TaskCollab.API.Controllers;

public class AuthController : ApiControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LoginResponse>> Login(LoginCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }
}