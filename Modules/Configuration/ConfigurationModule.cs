
using CliTool.Modules.Commands;
using CliTool.Modules.Configuration.Payload;
using CliTool.Modules.OracleExemple;
using CliTool.Modules.Project;
using CliTool.Modules.Time;
using CliTool.Services;

namespace CliTool.Core
{
    public class ConfigurationModule : BaseModule
    {
        private JsonService JsonService { get; } = new JsonService();
        public ConfigurationModule() 
        {
            var createdFile = RunDefaultConfiguration();   
            SetMenu(CreateMenu());
        }

        private Menu CreateMenu()
        {
            return new Menu
            {
                Name = "Configuração",
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

            if (resetConfiguration || !CheckForJsonFile<ProjectModuleModule>())
            {
                configurationArgs.ModulesConfig.Add(new ModuleConfig
                {
                    Name = nameof(ProjectModuleModule),
                    JsonFileName = nameof(ProjectModuleModule),
                    InitialData = ProjectLauncherPayload.Projects,
                });
            }

            if (resetConfiguration || !CheckForJsonFile<ToolInfoModule>())
            {
                configurationArgs.ModulesConfig.Add(new ModuleConfig
                {
                    Name = nameof(ToolInfoModule),
                    JsonFileName = nameof(ToolInfoModule),
                    InitialData = ToolInfoPayload.Tools

                });
            }

            if (resetConfiguration || !CheckForJsonFile<TimeModule>())
            {
                configurationArgs.ModulesConfig.Add(new ModuleConfig
                {
                    Name = nameof(TimeModule),
                    JsonFileName = nameof(TimeModule),
                    InitialData = new List<TimeMark>()
                });
            }

            if (resetConfiguration || !CheckForJsonFile<OracleSnippetModule>())
            {
                configurationArgs.ModulesConfig.Add(new ModuleConfig
                {
                    Name = nameof(OracleSnippetModule),
                    JsonFileName = nameof(OracleSnippetModule),
                    InitialData = OracleSnippetPayload.Snippets
                });
            }

            foreach (var moduleConfig in configurationArgs.ModulesConfig)
            {
                createFiles = true;
                JsonService.CreateJsonFile(
                    configurationArgs.JsonModulesFilesDirectoryPath,
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
            var fullPath = JsonService.CreateFullPath(AppContext.BaseDirectory, typeof(T).Name);
            return JsonService.ExistJsonFile(fullPath);
        }

        public class ConfigurationArgs
        {
            public string JsonModulesFilesDirectoryPath { get; set; } = AppContext.BaseDirectory;
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
