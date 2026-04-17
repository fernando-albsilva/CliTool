using CliTool.Core;
using CliTool.Services;
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
                        TerminalService.RunCommandInNewTerminal(command, cmdWorkingDir);
                    }
                    else
                    {
                        ConsoleService.WriteLine($"  [{commandIndex}] {command.Command}", ConsoleColor.Gray);
                        var success = TerminalService.RunCommandSequential(command, cmdWorkingDir);
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
    }
}
