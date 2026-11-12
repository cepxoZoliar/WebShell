using System.ComponentModel.DataAnnotations;

namespace WebShell.Api.Models;

public record CommandRequest([Required] string Command);