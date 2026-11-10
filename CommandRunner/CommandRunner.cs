using System.Diagnostics;
using System.Text;

public record CommandResult(string Output, string Error, int ExitCode);

public class CommandRunner
{
    private const int MaxOutputChars = 100_000;

    private static async Task<string> ReadLimitedAsync(
        StreamReader reader, CancellationToken token)
    {
        var sb = new StringBuilder();
        var buffer = new char[4096];
        int total = 0;
        int read;

        while ((read = await reader.ReadAsync(buffer.AsMemory(), token)) > 0)
        {
            if (sb.Length < MaxOutputChars)
            {
                sb.Append(buffer, 0, Math.Min(read, MaxOutputChars - sb.Length));
            }
            total += read;
        }

        if (total > MaxOutputChars)
        {
            sb.Append("\n[вывод обрезан]");
        }
        
        return sb.ToString();
    }
    
    public async Task<CommandResult> RunAsync(string command)
    {
        if (string.IsNullOrWhiteSpace(command))
        {
            return new CommandResult("", "Команда пуста", -1);
        }
        
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
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            string output;
            string error;
            try
            {
                var outputTask = ReadLimitedAsync(process.StandardOutput, cts.Token);
                var errorTask = ReadLimitedAsync(process.StandardError, cts.Token);
                await process.WaitForExitAsync(cts.Token);

                output = await outputTask;
                error = await errorTask;
            }
            catch (OperationCanceledException)
            {
                process.Kill(entireProcessTree: true);
                return new CommandResult("", "Превышено время ожидания", -1);
            }

            return new CommandResult(output, error, process.ExitCode);
        }
    }
}