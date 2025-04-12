using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskCollab.Application.Features.Tenants.Commands.CreateTenant;
using TaskCollab.Application.Features.Tenants.Queries.GetTenantById;

namespace TaskCollab.API.Controllers;

public class TenantsController : ApiControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateTenantCommand command)
    {
        var result = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.TenantId }, result);
    }
    
    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TenantDto>> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetTenantByIdQuery { Id = id });
        return Ok(result);
    }
}