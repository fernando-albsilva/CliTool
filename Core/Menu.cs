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
        public string OrderText { get; set; }   // ex: "1"
        public string DisplayText { get; set; } // ex: "Listar usuários"
        public Action Execute { get; set; }
    }
}
