using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemplateApi.Application.Users.Commands.CreateUser;
using TemplateApi.Application.Users.Commands.InactivateUser;
using TemplateApi.Application.Users.Commands.UpdateUser;

namespace TemplateApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserCommand command,
        CancellationToken ct)
    {
        var id = await mediator.Send(command, ct);
        return CreatedAtAction("GetById", new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateUserCommand command,
        CancellationToken ct)
    {
        await mediator.Send(command with { Id = id }, ct);
        return NoContent();
    }

    [HttpPatch("{id:guid}/inactivate")]
    public async Task<IActionResult> Inactivate(Guid id, CancellationToken ct)
    {
        await mediator.Send(new InactivateUserCommand(id), ct);
        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        // Placeholder para CreatedAtAction funcionar. 
        // Normalmente haveria uma Query aqui, mas o requisito não pediu explicitamente o Get.
        return Ok(new { id });
    }
}
