using Microsoft.AspNetCore.Mvc;
using WebShell.Api.Models;
using WebShell.Api.Services;
using Microsoft.EntityFrameworkCore;
using WebShell.Api.Data;

namespace WebShell.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommandsController : ControllerBase
{
    private readonly CommandRunner _commandRunner;
    private readonly AppDbContext _dbContext;
    private readonly ILogger<CommandsController> _logger;
    
    public CommandsController(CommandRunner commandRunner, AppDbContext dbContext,
        ILogger<CommandsController> logger)
    {
        _commandRunner = commandRunner;
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<CommandResult>> Run(CommandRequest request)
    {
        var result = await _commandRunner.RunAsync(request.Command);

        try
        {
            _dbContext.CommandHistories.Add(new CommandHistory
            {
                Command = request.Command,
                ExecutedAt = DateTime.UtcNow
            });
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Не удалось сохранить комманду");
        }
            
        return Ok(result);
    }

    [HttpGet("history")]
    public async Task<ActionResult<List<string>>> GetHistory()
    {
        var commands = await _dbContext.CommandHistories
            .OrderBy(h => h.ExecutedAt)
            .Select(h => h.Command)
            .ToListAsync();
        
        return Ok(commands);
    }
}