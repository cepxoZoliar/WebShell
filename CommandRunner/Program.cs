using System.Diagnostics;

var fileName = "/bin/bash";

var startInfo = new ProcessStartInfo
{
    FileName = fileName,
    RedirectStandardOutput = true,
    RedirectStandardError = true,
    UseShellExecute = false
};

startInfo.ArgumentList.Add("-c");
startInfo.ArgumentList.Add("ls -la | grep dll");

Process? process;
try
{
    process = Process.Start(startInfo);
}
catch (System.ComponentModel.Win32Exception ex)
{
    Console.WriteLine($"Не удалось запустить комманду: {ex.Message}");
    return;
}

if (process == null)
{
    Console.WriteLine("Could not start process");
    return;
}

using (process)
{
    string output = process.StandardOutput.ReadToEnd();
    string error = process.StandardError.ReadToEnd();
    process.WaitForExit();

    Console.WriteLine("===STDOUT===");
    Console.WriteLine(output);
    Console.WriteLine("===STDERR===");
    Console.WriteLine(error);
    Console.WriteLine($"Код завершен {process.ExitCode}");
}