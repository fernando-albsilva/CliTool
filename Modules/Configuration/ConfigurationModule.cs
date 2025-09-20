
using CliTool.Modules.Configuration;
using CliTool.Modules.Horario;
using CliTool.Modules.Text;
using CliTool.Services;

namespace CliTool.Core
{
    public class ConfigurationModule : BaseModule
    {
        public ConfigurationArgs ConfigurationArgs { get; set; }
        public ConfigurationModule() : base(CreateMenu())
        {
            
        }

        private static Menu CreateMenu()
        {
            return new Menu
            {
                Name = "Módulo de Configuração",
                Options = new List<Option>
                {
                    new() { OrderText = "1", DisplayText = "Executar configuração padrão", Execute = RunDefaultConfiguration }
                }
            };
        }

        private static void RunDefaultConfiguration()
        {
            var config = new ConfigurationArgs
            {
                RootJsonFilesPath = AppContext.BaseDirectory,
                ModulesConfig = new List<ModuleConfig> {
                    new ModuleConfig {
                        Name = nameof(HorarioModule),
                        JsonFileName = $"{nameof(HorarioModule)}.json",
                        InitialData = new List<HorarioArg>()
                    }
                }
            };

            var jsonService = new JsonService();

            foreach (var moduleConfig in config.ModulesConfig)
            {
                jsonService.CreateJsonFile(
                    config.RootJsonFilesPath,
                    moduleConfig.JsonFileName,
                    moduleConfig.InitialData
                );
            }
        }
    }
}
