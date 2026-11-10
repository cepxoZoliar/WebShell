
var runner = new CommandRunner();
var result = await runner.RunAsync("echo hello");


Console.WriteLine("===STDOUT===");
Console.WriteLine(result.Output);
Console.WriteLine("===STDERR===");
Console.WriteLine(result.Error);
Console.WriteLine($"Длина вывода: {result.Output.Length}");
Console.WriteLine($"Код завершен {result.ExitCode}");
