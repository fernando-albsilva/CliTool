namespace CliTool.Modules.CommandHelper
{
    public class ToolArg
    {
        public string Tool { get; set; } = string.Empty;
        public List<CommandInfo> Commands { get; set; } = new();
    }
}
