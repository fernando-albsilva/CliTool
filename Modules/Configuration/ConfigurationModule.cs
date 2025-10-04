
using CliTool.Modules.Configuration;
using CliTool.Modules.Horario;
using CliTool.Services;

namespace CliTool.Core
{
    public class ConfigurationModule : BaseModule
    {
        public ConfigurationArgs ConfigurationArgs { get; set; }
        private JsonService JsonService { get; } = new JsonService();
        public ConfigurationModule() 
        {
            SetMenu(CreateMenu());
        }

        private Menu CreateMenu()
        {
            return new Menu
            {
                Name = "Módulo de Configuração",
                Options = new List<Option>
                {
                    new() { OrderText = "1", DisplayText = "Executar configuração padrão", Execute = () => RunDefaultConfiguration() }
                }
            };
        }

        private void RunDefaultConfiguration()
        {
            var config = new ConfigurationArgs
            {
                ModulesConfig = new List<ModuleConfig> {
                    new ModuleConfig {
                        Name = nameof(HorarioModule),
                        JsonFileName = nameof(HorarioModule),
                        InitialData = new List<HorarioArg>()
                    }
                }
            };

            var args = GetConfigurationArgs();

            if (args == null)
            {
                CreateConfigFile();
                args = JsonService.ReadJsonFile<ConfigurationArgs>(AppContext.BaseDirectory, nameof(ConfigurationModule));
            }

            if (args == null)
            {
                ConsoleService.WriteError("Não foi possível criar/recuperar o arquivo de configuração");
                throw new Exception("Não foi possível criar/recuperar o arquivo de configuração");
            }

            foreach (var moduleConfig in config.ModulesConfig)
            {
                JsonService.CreateJsonFile(
                    config.JsonModulesFilesDirectoryPath,
                    moduleConfig.JsonFileName,
                    moduleConfig.InitialData
                );
            }
        }

        private ConfigurationArgs? GetConfigurationArgs()
        {
            return JsonService.ReadJsonFile<ConfigurationArgs>(AppContext.BaseDirectory, nameof(ConfigurationModule));
        }

        private void CreateConfigFile()
        {
            JsonService.CreateJsonFile(AppContext.BaseDirectory, nameof(ConfigurationModule), new ConfigurationArgs());
        } 

    }
}
