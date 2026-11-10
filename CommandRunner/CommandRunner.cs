using System.Diagnostics;

public record CommandResult(string Output, string Error, int ExitCode);

public class CommandRunner
{
    public async Task<CommandResult> RunAsync(string command)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "/bin/bash",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        startInfo.ArgumentList.Add("-c");
        startInfo.ArgumentList.Add(command);

        Process? process;
        try
        {
            process = Process.Start(startInfo);
        }
        catch (System.ComponentModel.Win32Exception ex)
        {
            //Console.WriteLine($"Не удалось запустить комманду: {ex.Message}");
            return new CommandResult("",
                $"Не удалось запустить комманду: {ex.Message}", -1);
        }

        if (process == null)
        {
            //Console.WriteLine("Could not start process");
            return new CommandResult("",
                "Не удалось запустить процесс", -1);
        }

        using (process)
        {
            var outputTask = process.StandardOutput.ReadToEndAsync();
            var errorTask = process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            var output = await outputTask;
            var error = await errorTask;
            
            return new CommandResult(output, error, process.ExitCode);
        }
    }
}