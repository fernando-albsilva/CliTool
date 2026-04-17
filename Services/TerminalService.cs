
using CliTool.Modules.CommandExecutor;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CliTool.Services
{
    public class TerminalService
    {
        public static bool RunCommandSequential(CommandItem command, string? workingDirectory)
        {
            ProcessStartInfo psi;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {command.Command}",
                    WorkingDirectory = workingDirectory ?? Environment.CurrentDirectory,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };
            }
            else
            {
                psi = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{command.Command}\"",
                    WorkingDirectory = workingDirectory ?? Environment.CurrentDirectory,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };
            }

            using var process = Process.Start(psi);
            if (process == null)
            {
                ConsoleService.WriteError("Falha ao iniciar processo.");
                return false;
            }

            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (!string.IsNullOrWhiteSpace(output))
            {
                ConsoleService.WriteLine(output, ConsoleColor.DarkGray);
            }

            if (!string.IsNullOrWhiteSpace(error))
            {
                ConsoleService.WriteLine(error, ConsoleColor.DarkRed);
            }

            return process.ExitCode == 0;
        }

        public static void RunCommandInNewTerminal(CommandItem command, string? workingDirectory)
        {
            var fullCommand = command.Command;

            if (!string.IsNullOrEmpty(workingDirectory))
            {
                var separator = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? " && " : "; ";
                fullCommand = $"cd \"{workingDirectory}\"{separator}{command.Command}";
            }

            ProcessStartInfo psi;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var terminal = command.Terminal?.ToLowerInvariant() ?? "cmd";

                psi = terminal switch
                {
                    "powershell" => new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-NoExit -Command \"{fullCommand}\"",
                        UseShellExecute = true,
                        CreateNoWindow = false
                    },
                    "pwsh" => new ProcessStartInfo
                    {
                        FileName = "pwsh.exe",
                        Arguments = $"-NoExit -Command \"{fullCommand}\"",
                        UseShellExecute = true,
                        CreateNoWindow = false
                    },
                    "wt" or "windows-terminal" => new ProcessStartInfo
                    {
                        FileName = "wt.exe",
                        Arguments = $"cmd /k \"{fullCommand}\"",
                        UseShellExecute = true,
                        CreateNoWindow = false
                    },
                    _ => new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/k \"{fullCommand}\"",
                        UseShellExecute = true,
                        CreateNoWindow = false
                    }
                };
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var terminal = command.Terminal?.ToLowerInvariant() ?? "terminal";
                var escapedCommand = fullCommand.Replace("\"", "\\\"");

                psi = terminal switch
                {
                    "iterm" => new ProcessStartInfo
                    {
                        FileName = "osascript",
                        Arguments = $"-e 'tell application \"iTerm\" to create window with default profile command \"{escapedCommand}\"'",
                        UseShellExecute = true
                    },
                    _ => new ProcessStartInfo
                    {
                        FileName = "osascript",
                        Arguments = $"-e 'tell application \"Terminal\" to do script \"{escapedCommand}\"'",
                        UseShellExecute = true
                    }
                };
            }
            else
            {
                var terminal = command.Terminal?.ToLowerInvariant() ?? DetectLinuxTerminal();

                psi = terminal switch
                {
                    "gnome-terminal" => new ProcessStartInfo
                    {
                        FileName = "gnome-terminal",
                        Arguments = $"-- bash -c \"{fullCommand}; exec bash\"",
                        UseShellExecute = true
                    },
                    "konsole" => new ProcessStartInfo
                    {
                        FileName = "konsole",
                        Arguments = $"-e bash -c \"{fullCommand}; exec bash\"",
                        UseShellExecute = true
                    },
                    "xfce4-terminal" => new ProcessStartInfo
                    {
                        FileName = "xfce4-terminal",
                        Arguments = $"-e \"bash -c '{fullCommand}; exec bash'\"",
                        UseShellExecute = true
                    },
                    "alacritty" => new ProcessStartInfo
                    {
                        FileName = "alacritty",
                        Arguments = $"-e bash -c \"{fullCommand}; exec bash\"",
                        UseShellExecute = true
                    },
                    "kitty" => new ProcessStartInfo
                    {
                        FileName = "kitty",
                        Arguments = $"bash -c \"{fullCommand}; exec bash\"",
                        UseShellExecute = true
                    },
                    "tilix" => new ProcessStartInfo
                    {
                        FileName = "tilix",
                        Arguments = $"-e \"bash -c '{fullCommand}; exec bash'\"",
                        UseShellExecute = true
                    },
                    "xterm" => new ProcessStartInfo
                    {
                        FileName = "xterm",
                        Arguments = $"-e \"bash -c '{fullCommand}; exec bash'\"",
                        UseShellExecute = true
                    },
                    _ => new ProcessStartInfo
                    {
                        FileName = "x-terminal-emulator",
                        Arguments = $"-e \"bash -c '{fullCommand}; exec bash'\"",
                        UseShellExecute = true
                    }
                };
            }

            Process.Start(psi);
        }

        private static string DetectLinuxTerminal()
        {
            var terminals = new[]
            {
                "gnome-terminal",
                "konsole",
                "xfce4-terminal",
                "alacritty",
                "kitty",
                "tilix",
                "xterm",
                "x-terminal-emulator"
            };

            foreach (var terminal in terminals)
            {
                try
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = "which",
                        Arguments = terminal,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using var process = Process.Start(psi);
                    process?.WaitForExit();

                    if (process?.ExitCode == 0)
                    {
                        return terminal;
                    }
                }
                catch
                {
                    // Ignora e tenta o próximo
                }
            }

            return "xterm";
        }
    }
}
