
using CliTool.Modules.CommandExecutor;
using CliTool.Modules.Commands;
using CliTool.Modules.Configuration.Payload;
using CliTool.Modules.Git;
using CliTool.Modules.Project;
using CliTool.Modules.ProjectStarter;
using CliTool.Services;

namespace CliTool.Core
{
    public class ModuleCliConfiguration : BaseModule
    {
        private JsonService JsonService { get; } = new JsonService();
        public ModuleCliConfiguration() 
        {
            var createdFile = RunDefaultConfiguration();   
            SetMenu(CreateMenu());
        }

        private Menu CreateMenu()
        {
            return new Menu
            {
                Name = "Configurar",
                Options = new List<Option>
                {
                    new() { OrderText = "1", DisplayText = "Resetar configurações", Execute = () => RunDefaultConfiguration(true) }
                }
            };
        }

        private bool RunDefaultConfiguration(bool resetConfiguration = false)
        {
            var configurationArgs = new ConfigurationArgs();
            var createFiles = false;

            if (!Directory.Exists(Config.ConfigDirectoryPath))
            {
                Directory.CreateDirectory(Config.ConfigDirectoryPath);
            }

            if (resetConfiguration || !CheckForJsonFile<ModuleProjectLauncher>())
            {
                configurationArgs.ModulesConfig.Add(new ModuleConfig
                {
                    Name = nameof(ModuleProjectLauncher),
                    JsonFileName = nameof(ModuleProjectLauncher),
                    InitialData = ProjectLauncherPayload.Projects,
                });
            }

            if (resetConfiguration || !CheckForJsonFile<ModuleCommandHelper>())
            {
                configurationArgs.ModulesConfig.Add(new ModuleConfig
                {
                    Name = nameof(ModuleCommandHelper),
                    JsonFileName = nameof(ModuleCommandHelper),
                    InitialData = ToolInfoPayload.Tools

                });
            }

            if (resetConfiguration || !CheckForJsonFile<ProjectStarterModule>())
            {
                configurationArgs.ModulesConfig.Add(new ModuleConfig
                {
                    Name = nameof(ProjectStarterModule),
                    JsonFileName = nameof(ProjectStarterModule),
                    InitialData = ProjectStarterPayload.Projects
                });
            }

            if (resetConfiguration || !CheckForJsonFile<CommandExecutorModule>())
            {
                configurationArgs.ModulesConfig.Add(new ModuleConfig
                {
                    Name = nameof(CommandExecutorModule),
                    JsonFileName = nameof(CommandExecutorModule),
                    InitialData = CommandExecutorPayload.CommandLists
                });
            }

            if (resetConfiguration || !CheckForJsonFile<GitModule>())
            {
                configurationArgs.ModulesConfig.Add(new ModuleConfig
                {
                    Name = nameof(GitModule),
                    JsonFileName = nameof(GitModule),
                    InitialData = GitPayload.Args,
                });
            }

            foreach (var moduleConfig in configurationArgs.ModulesConfig)
            {
                createFiles = true;
                JsonService.CreateJsonFile(
                    Config.ConfigDirectoryPath,
                    moduleConfig.JsonFileName,
                    moduleConfig.InitialData
                );
            }

            if (createFiles)
            {
                Console.Clear();
                ConsoleService.WriteWarning("Necessário reinicializar cli pois arquivos de configuração dos módulos foram criados");
                Environment.Exit(0);
            }

            return createFiles;
        }

        private bool CheckForJsonFile<T>()
        {
            var fullPath = JsonService.CreateFullPath(Config.ConfigDirectoryPath, typeof(T).Name);
            return JsonService.ExistJsonFile(fullPath);
        }

        public class ConfigurationArgs
        {
            public List<ModuleConfig> ModulesConfig { get; set; } = new List<ModuleConfig>();
        }

        public class ModuleConfig
        {
            public string Name { get; set; } = string.Empty;
            public string JsonFileName { get; set; } = string.Empty;
            public required object InitialData { get; set; }
        }
    }
}
