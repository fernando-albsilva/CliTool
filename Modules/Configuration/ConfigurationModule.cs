
using CliTool.Modules.Commands;
using CliTool.Modules.Configuration.Payload;
using CliTool.Modules.OracleExemple;
using CliTool.Modules.Project;
using CliTool.Modules.Time;
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

            if (resetConfiguration || !CheckForJsonFile<ModuleTime>())
            {
                configurationArgs.ModulesConfig.Add(new ModuleConfig
                {
                    Name = nameof(ModuleTime),
                    JsonFileName = nameof(ModuleTime),
                    InitialData = new List<TimeMark>()
                });
            }

            if (resetConfiguration || !CheckForJsonFile<ModuleOracleSnippet>())
            {
                configurationArgs.ModulesConfig.Add(new ModuleConfig
                {
                    Name = nameof(ModuleOracleSnippet),
                    JsonFileName = nameof(ModuleOracleSnippet),
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
