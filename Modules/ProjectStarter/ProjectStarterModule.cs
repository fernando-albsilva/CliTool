using CliTool.Core;
using CliTool.Services;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace CliTool.Modules.ProjectStarter
{
    public class ProjectStarterModule : BaseModule
    {
        private static readonly JsonService _jsonService = new();
        private static List<StarterArg> _projects = new();

        public ProjectStarterModule()
        {
            LoadProjects();
            SetMenu(CreateMenu());
        }

        private static void LoadProjects()
        {
            _projects = _jsonService.ReadJsonFile<List<StarterArg>>(AppContext.BaseDirectory, nameof(ProjectStarterModule)) ?? new List<StarterArg>();

            if (_projects.Count == 0)
            {
                ConsoleService.WriteWarning($"Nenhum projeto encontrado no arquivo {nameof(ProjectStarterModule)}.json.");
            }
        }

        private static Menu CreateMenu()
        {
            var options = new List<Option>();

            if (_projects.Count > 0)
            {
                var singleProjects = _projects.Where(project => project.GroupId is null);
                var setProjects = _projects.Where(project => project.GroupId is not null).GroupBy(project => project.GroupId);

                int order = 1;

                foreach (var project in singleProjects)
                {
                    options.Add(new Option
                    {
                        OrderText = order.ToString(),
                        DisplayText = $"{project.Label} ({project.DirectoryPath})",
                        Execute = () => StartProject(project)
                    });
                    order++;
                }

                foreach (var set in setProjects)
                {
                    var groupName = set.FirstOrDefault()?.GroupName ?? $"Grupo {set.Key}";
                    var displayText = new StringBuilder();
                    displayText.AppendLine($"Conjunto: {groupName}");

                    foreach (var project in set)
                    {
                        displayText.AppendLine($"    * {project.Label}");
                    }

                    options.Add(new Option
                    {
                        OrderText = order.ToString(),
                        DisplayText = displayText.ToString(),
                        Execute = () => StartSetProject(set)
                    });

                    order++;
                }
            }

            return new Menu
            {
                Name = "Iniciar Projeto",
                Options = options
            };
        }

        private static void StartSetProject(IGrouping<int?, StarterArg> set)
        {
            ConsoleService.WriteInfo($"Iniciando conjunto de projetos...");
            foreach (var project in set)
            {
                StartProject(project);
                Thread.Sleep(500); // Pequeno delay entre cada terminal
            }
            ConsoleService.WriteSuccess($"Todos os {set.Count()} projetos foram iniciados em terminais separados.");
        }

        private static void StartProject(StarterArg project)
        {
            if (!Directory.Exists(project.DirectoryPath))
            {
                ConsoleService.WriteError($"Diretório não encontrado: {project.DirectoryPath}");
                return;
            }

            if (project.Commands == null || project.Commands.Count == 0)
            {
                ConsoleService.WriteWarning($"Nenhum comando definido para o projeto: {project.Label}");
                return;
            }

            try
            {
                OpenTerminalWithCommands(project);
                ConsoleService.WriteSuccess($"Projeto '{project.Label}' iniciado em novo terminal.");
            }
            catch (Exception ex)
            {
                ConsoleService.WriteError($"Erro ao iniciar o projeto '{project.Label}': {ex.Message}");
            }
        }

        private static void OpenTerminalWithCommands(StarterArg project)
        {
            // Concatena os comandos com ; (Linux/Mac) ou && (Windows)
            var commandSeparator = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? " && " : "; ";
            var commands = string.Join(commandSeparator, project.Commands);

            // Adiciona cd para o diretório do projeto como primeiro comando
            var fullCommand = $"cd \"{project.DirectoryPath}\"{commandSeparator}{commands}";

            ProcessStartInfo psi;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Windows: usa o terminal especificado ou cmd por padrão
                var terminal = project.Terminal?.ToLowerInvariant() ?? "cmd";
                
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
                // macOS: usa Terminal.app ou iTerm
                var terminal = project.Terminal?.ToLowerInvariant() ?? "terminal";
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
                // Linux: tenta detectar o terminal disponível ou usa o especificado
                var terminal = project.Terminal?.ToLowerInvariant() ?? DetectLinuxTerminal();

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
            // Lista de terminais comuns em ordem de preferência
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
                    // Ignora erros e tenta o próximo
                }
            }

            return "xterm"; // Fallback
        }
    }
}
