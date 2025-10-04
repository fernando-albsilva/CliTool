namespace CliTool.Core
{
    public class Menu
    {
        public string Name { get; set; } = string.Empty;
        public List<Option> Options { get; set; } = new();
        public Menu? FatherMenu { get; set; }
    }

    public class Option
    {
        public string OrderText { get; set; }  
        public string DisplayText { get; set; } 
        public Action Execute { get; set; }
    }
}
