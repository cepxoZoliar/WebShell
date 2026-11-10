
var runner = new CommandRunner();
var result = await runner.RunAsync("exit 5");

Console.WriteLine("===STDOUT===");
Console.WriteLine(result.Output);
Console.WriteLine("===STDERR===");
Console.WriteLine(result.Error);
Console.WriteLine($"Код завершен {result.ExitCode}");
