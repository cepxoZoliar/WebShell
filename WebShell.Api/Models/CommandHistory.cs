namespace WebShell.Api.Models;

public class CommandHistory
{
    public int Id { get; set; }
    public string Command { get; set; } = string.Empty;
    public DateTime ExecutedAt { get; set; }
}