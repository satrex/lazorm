using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace LazormBoilerplate
{
	public static class CommandExecutor
	{
        public static Task<int> Bash(this string cmd, ILogger? logger)
        {
            var source = new TaskCompletionSource<int>();
            var escapedArgs = cmd.Replace("\"", "\\\"");
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                },
                EnableRaisingEvents = true
            };
            process.Exited += (sender, args) =>
            {
                Console.WriteLine(process.StandardError.ReadToEnd());
                Console.WriteLine(process.StandardOutput.ReadToEnd());
                if (process.ExitCode == 0)
                {
                    source.SetResult(0);
                }
                else
                {
                    source.SetException(new Exception($"Command `{cmd}` failed with exit code `{process.ExitCode}`"));
                }

                process.Dispose();
            };

            try
            {
                process.Start();
            }
            catch (Exception e)
            {
                logger?.LogError(e, "Command {} failed", cmd);
                source.SetException(e);
            }

            return source.Task;
        }
    }
}

