using CliTool.Core;
using CliTool.Services;

namespace CliTool.Modules.OracleExemple
{
    class OracleSnippetModule : BaseModule
    {
        private static readonly JsonService _jsonService = new();
        private static List<SnippetArg> _exemples = new();

        public OracleSnippetModule()
        {
            LoadExemplesFromJson();
            SetMenu(CreateMenu());
        }

        private static Menu CreateMenu()
        {
            var options = new List<Option>();
            int counter = 1;

            foreach (var exemple in _exemples)
            {
                string displayText = $"{exemple.Title}";
                
                options.Add(new Option
                {
                    OrderText = counter.ToString(),
                    DisplayText = displayText,
                    Execute = () => ShowExemple(exemple)
                });
                counter++;
            }

            return new Menu
            {
                Name = "Oracle Sql Snippets",
                Options = options
            };
        }

        private static void ShowExemple(SnippetArg exemple)
        {
            ConsoleService.WriteLine(new string('-', 40), ConsoleColor.DarkYellow);
            ConsoleService.WriteLine();
            ConsoleService.WriteLine($"{exemple.Title}", ConsoleColor.DarkGreen);
            ConsoleService.WriteLine();
            ConsoleService.WriteLine($"{exemple.Snippet}");
            //ConsoleService.WriteLine($"{exemple.Snippet}", ConsoleColor.DarkGray);
        }

        private static void LoadExemplesFromJson()
        {
            _exemples = _jsonService.ReadJsonFile<List<SnippetArg>>(AppContext.BaseDirectory, nameof(OracleSnippetModule)) ?? new List<SnippetArg>();

            if (_exemples.Count == 0)
            {
                ConsoleService.WriteWarning($"Nenhum exemplo encontrado no arquivo {nameof(OracleSnippetModule)}.json.");
            }
        }
    }
}
