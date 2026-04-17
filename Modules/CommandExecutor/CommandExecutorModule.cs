using CliTool.Core;
using CliTool.Services;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace CliTool.Modules.CommandExecutor
{
    public class CommandExecutorModule : BaseModule
    {
        private static readonly JsonService _jsonService = new();
        private static List<CommandListArg> _commandLists = new();

        public CommandExecutorModule()
        {
            LoadCommandLists();
            SetMenu(CreateMenu());
        }

        private static void LoadCommandLists()
        {
            _commandLists = _jsonService.ReadJsonFile<List<CommandListArg>>(Config.ConfigDirectoryPath, nameof(CommandExecutorModule)) ?? new List<CommandListArg>();

            if (_commandLists.Count == 0)
            {
                ConsoleService.WriteWarning($"Nenhuma lista de comandos encontrada no arquivo {nameof(CommandExecutorModule)}.json.");
            }
        }

        private static Menu CreateMenu()
        {
            var options = new List<Option>();

            if (_commandLists.Count > 0)
            {
                var singleLists = _commandLists.Where(list => list.GroupId is null);
                var groupedLists = _commandLists.Where(list => list.GroupId is not null).GroupBy(list => list.GroupId);

                int order = 1;

                foreach (var commandList in singleLists)
                {
                    var displayText = new StringBuilder();
                    displayText.Append(commandList.Label);
                    if (!string.IsNullOrEmpty(commandList.Description))
                    {
                        displayText.Append($" - {commandList.Description}");
                    }

                    options.Add(new Option
                    {
                        OrderText = order.ToString(),
                        DisplayText = displayText.ToString(),
                        Execute = () => ExecuteCommandList(commandList)
                    });
                    order++;
                }

                foreach (var group in groupedLists)
                {
                    var groupName = group.FirstOrDefault()?.GroupName ?? $"Grupo {group.Key}";
                    var displayText = new StringBuilder();
                    displayText.AppendLine($"Conjunto: {groupName}");

                    foreach (var commandList in group)
                    {
                        displayText.AppendLine($"    * {commandList.Label}");
                    }

                    options.Add(new Option
                    {
                        OrderText = order.ToString(),
                        DisplayText = displayText.ToString(),
                        Execute = () => ExecuteCommandListGroup(group)
                    });

                    order++;
                }
            }

            return new Menu
            {
                Name = "Executor de Comandos",
                Options = options
            };
        }

        private static void ExecuteCommandListGroup(IGrouping<int?, CommandListArg> group)
        {
            ConsoleService.WriteInfo($"Executando grupo de listas de comandos...");
            foreach (var commandList in group)
            {
                ExecuteCommandList(commandList);
            }
            ConsoleService.WriteSuccess($"Todas as {group.Count()} listas foram executadas.");
        }

        private static void ExecuteCommandList(CommandListArg commandList)
        {
            ConsoleService.WriteInfo($"Executando: {commandList.Label}");
            
            if (commandList.Commands == null || commandList.Commands.Count == 0)
            {
                ConsoleService.WriteWarning($"Nenhum comando definido para: {commandList.Label}");
                return;
            }

            var workingDirectory = commandList.WorkingDirectory;
            if (!string.IsNullOrEmpty(workingDirectory) && !Directory.Exists(workingDirectory))
            {
                ConsoleService.WriteError($"Diretório de trabalho não encontrado: {workingDirectory}");
                return;
            }

            int commandIndex = 1;
            foreach (var command in commandList.Commands)
            {
                var cmdWorkingDir = command.WorkingDirectory ?? workingDirectory;

                if (!string.IsNullOrEmpty(cmdWorkingDir) && !Directory.Exists(cmdWorkingDir))
                {
                    ConsoleService.WriteError($"Diretório não encontrado para comando {commandIndex}: {cmdWorkingDir}");
                    commandIndex++;
                    continue;
                }

                try
                {
                    if (command.RunInParallel)
                    {
                        ConsoleService.WriteLine($"  [{commandIndex}] (Paralelo) {command.Command}", ConsoleColor.Cyan);
                        RunCommandInNewTerminal(command, cmdWorkingDir);
                    }
                    else
                    {
                        ConsoleService.WriteLine($"  [{commandIndex}] {command.Command}", ConsoleColor.Gray);
                        var success = RunCommandSequential(command, cmdWorkingDir);
                        if (!success)
                        {
                            ConsoleService.WriteError($"Comando falhou. Interrompendo execução da lista.");
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ConsoleService.WriteError($"Erro ao executar comando {commandIndex}: {ex.Message}");
                    return;
                }

                commandIndex++;
            }

            ConsoleService.WriteSuccess($"Lista '{commandList.Label}' executada com sucesso.");
        }

        private static bool RunCommandSequential(CommandItem command, string? workingDirectory)
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

        private static void RunCommandInNewTerminal(CommandItem command, string? workingDirectory)
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
