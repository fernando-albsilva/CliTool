using CliTool.Core;
using CliTool.Services;
using System.Diagnostics;
using System.Text;

namespace CliTool.Modules.Project
{
    public class ModuleProjectLauncher : BaseModule
    {
        private static readonly JsonService _jsonService = new();
        private static List<ProjectArg> _projects = new();

        public ModuleProjectLauncher()
        {
            LoadProjects();
            SetMenu(CreateMenu());
        }

        private static void LoadProjects()
        {
            _projects = _jsonService.ReadJsonFile<List<ProjectArg>>(AppContext.BaseDirectory, nameof(ModuleProjectLauncher)) ?? new List<ProjectArg>();

            if (_projects.Count == 0)
            {
                ConsoleService.WriteWarning($"Nenhum projeto encontrado no arquivo {nameof(ModuleProjectLauncher)}.json.");
            }
        }

        private static Menu CreateMenu()
        {
            var options = new List<Option>();

            if (_projects.Count > 0)
            {
                var singleProjects = _projects.Where(project => project.GrouId is null);
                var setProjects = _projects.Where(project => project.GrouId is not null).GroupBy(project => project.GrouId);

                int order = 1;
                
                foreach (var project in singleProjects)
                {
                    options.Add(new Option
                    {
                        OrderText = order.ToString(),
                        DisplayText = $"{project.Label} ({project.Ide})",
                        Execute = () => OpenProject(project)
                    });
                    order++;
                }

                foreach (var set in setProjects)
                {

                    var displayText = new StringBuilder();
                    displayText.AppendLine("Conjunto:");

                    foreach (var project in set) {
                        displayText.AppendLine($"    * {project.Label} ({project.Ide})");
                    }

                    options.Add(new Option
                    {
                        OrderText = order.ToString(),
                        DisplayText = displayText.ToString(),
                        Execute = () => OpenSetProject(set)
                    });

                    order++;
                }
            }

            return new Menu
            {
                Name = "Projetos",
                Options = options
            };
        }

        private static void OpenSetProject(IGrouping<int?, ProjectArg> set)
        {
            foreach (var project in set)
            {
                OpenProject(project);
            }
        }

        private static void OpenProject(ProjectArg project)
        {
            if (!Directory.Exists(project.DirectoryPath))
            {
                ConsoleService.WriteError($"Diretório não encontrado: {project.DirectoryPath}");
                return;
            }

            try
            {
                if (project.Ide.Equals("visualstudio", StringComparison.OrdinalIgnoreCase))
                {
                    var solution = Directory.GetFiles(project.DirectoryPath, "*.sln").FirstOrDefault();
                    if (solution == null)
                    {
                        ConsoleService.WriteError("Nenhum arquivo .sln encontrado no diretório.");
                        return;
                    }

                    var psi = new ProcessStartInfo
                    {
                        FileName = "devenv.exe",   
                        Arguments = $"\"{solution}\"", 
                        WorkingDirectory = project.DirectoryPath,
                        UseShellExecute = true
                    };

                    Process.Start(psi);
                    ConsoleService.WriteResult($"Abrindo {solution} no Visual Studio...");
                }
                else if (project.Ide.Equals("vscode", StringComparison.OrdinalIgnoreCase))
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = "code",               
                        Arguments = ".",                 
                        WorkingDirectory = project.DirectoryPath,
                        UseShellExecute = true           
                    };

                    Process.Start(psi);
                    ConsoleService.WriteResult($"Abrindo {project.Label} no VS Code...");
                }
                else
                {
                    ConsoleService.WriteWarning($"IDE não reconhecida: {project.Ide}");
                }
            }
            catch (Exception ex)
            {
                ConsoleService.WriteError($"Erro ao abrir o projeto: {ex.Message}");
            }
        }
    }
}
