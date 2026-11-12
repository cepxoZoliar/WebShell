using Microsoft.AspNetCore.Mvc;
using WebShell.Api.Models;
using WebShell.Api.Services;

namespace WebShell.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommandsController : ControllerBase
{
    private readonly CommandRunner _commandRunner;
    
    public CommandsController(CommandRunner commandRunner)
    {
        _commandRunner = commandRunner;
    }

    [HttpPost]
    public async Task<ActionResult<CommandResult>> Run(CommandRequest request)
    {
        var result = await _commandRunner.RunAsync(request.Command);
        return Ok(result);
    }
}