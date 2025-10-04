namespace CliTool.Modules.Configuration
{
    public class ConfigurationArgs
    {
        public string JsonModulesFilesDirectoryPath { get; set; } = AppContext.BaseDirectory;
        public string RepositoriesDirectoryPath { get; set; } = "C:\\Repositorios";
        public List<ModuleConfig> ModulesConfig { get; set; } = new List<ModuleConfig>();
    }

    public class ModuleConfig
    {
        public string Name { get; set; } = string.Empty;
        public string JsonFileName { get; set; } = string.Empty;
        public object InitialData { get; set; }
    }
}
