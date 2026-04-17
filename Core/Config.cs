namespace CliTool.Core
{
    public static class Config
    {
        public static string ConfigDirectoryName { get; } = "cli-tool-config";
        public static string ConfigDirectoryPath { get; } = Path.Combine(AppContext.BaseDirectory, ConfigDirectoryName);
    }
}
