using CliTool.Core;
using CliTool.Modules.CommandHelper;
using CliTool.Services;

namespace CliTool.Modules.Commands
{
    public class ModuleCommandHelper : BaseModule
    {
        private static readonly JsonService _jsonService = new();
        private static List<ToolArg> _tools = new();

        public ModuleCommandHelper()
        {
            LoadToolsFromJson();
            SetMenu(CreateMenu());
        }

        private static Menu CreateMenu()
        {
            var options = new List<Option>();
            int counter = 1;

            foreach (var tool in _tools)
            {
                string displayText = $"{tool.Tool} ({tool.Commands.Count} comandos)";
                options.Add(new Option
                {
                    OrderText = counter.ToString(),
                    DisplayText = displayText,
                    Execute = () => ListCommands(tool)
                });
                counter++;
            }

            return new Menu
            {
                Name = "Listagem de Comandos",
                Options = options
            };
        }

        private static void LoadToolsFromJson()
        {
            _tools = _jsonService.ReadJsonFile<List<ToolArg>>(AppContext.BaseDirectory, nameof(ModuleCommandHelper)) ?? new List<ToolArg>();
           
            if (_tools.Count == 0) 
            { 
                ConsoleService.WriteWarning($"Nenhum ferramenta encontrado no arquivo {nameof(ModuleCommandHelper)}.json.");
            }
        }

        private static void ListCommands(ToolArg tool)
        {

            foreach (var cmd in tool.Commands)
            {
                ConsoleService.WriteCommand(cmd.Name);
                ConsoleService.WriteLine(cmd.Description, ConsoleColor.DarkGray);
                Console.WriteLine();
            }
        }
    }
}
