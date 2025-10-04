using CliTool.Core;
using CliTool.Services;
using System.Diagnostics;

namespace CliTool.Modules.Project
{
    public class ProjectModuleModule : BaseModule
    {
        private const string ConfigPath = "./";
        private const string ConfigFile = "projects";
        private static readonly JsonService _jsonService = new();
        private static List<ProjectArg> _projects = new();

        public ProjectModuleModule()
        {
            LoadProjects();
            SetMenu(CreateMenu());
        }

        private static void LoadProjects()
        {
            _projects = _jsonService.ReadJsonFile<List<ProjectArg>>(AppContext.BaseDirectory, nameof(ProjectModuleModule)) ?? new List<ProjectArg>();

            if (_projects.Count == 0)
            {
                ConsoleService.WriteWarning($"Nenhum projeto encontrado no arquivo {nameof(ProjectModuleModule)}.json.");
            }
        }

        private static Menu CreateMenu()
        {
            var options = new List<Option>();

            if (_projects.Count > 0)
            {
                int order = 1;
                foreach (var project in _projects)
                {
                    options.Add(new Option
                    {
                        OrderText = order.ToString(),
                        DisplayText = $"{project.Label} ({project.Ide})",
                        Execute = () => OpenProject(project)
                    });
                    order++;
                }
            }

            return new Menu
            {
                Name = "Módulo de Projetos",
                Options = options
            };
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
