namespace CliTool.Modules.ToolInfo
{
    public class ToolArg
    {
        public string Tool { get; set; } = string.Empty;
        public List<CommandInfo> Commands { get; set; } = new();
    }
}
