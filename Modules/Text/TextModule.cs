using CliTool.Core;
using CliTool.Services;

namespace CliTool.Modules.Text
{
    public class TextModule : BaseModule
    {
        public TextModule()
        {
            SetMenu(CreateMenu());
        }

        private static Menu CreateMenu()
        {
            return new Menu
            {
                Name = "Módulo de Texto",
                Options = new List<Option>
                {
                    new() { OrderText = "2", DisplayText = "Contador de caracter", Execute = CharCounter }
                }
            };
        }

        private static void CharCounter()
        {
            ConsoleService.WriteLine("Texto: ");
            string text = Console.ReadLine() ?? string.Empty;
            ConsoleService.WriteResult(text.Length.ToString());
        }
    }
}
