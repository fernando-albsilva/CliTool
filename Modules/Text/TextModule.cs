using CliTool.Core;
using CliTool.Services;

namespace CliTool.Modules.Text
{
    public class TextModule : BaseModule
    {
        public TextModule() : base(CreateMenu())
        {

        }

        private static Menu CreateMenu()
        {
            return new Menu
            {
                Name = "Módulo de Texto",
                Options = new List<Option>
                {
                    new() { OrderText = "1", DisplayText = "Exibir texto", Execute = ShowText },
                    new() { OrderText = "2", DisplayText = "Editar texto", Execute = EditText }
                }
            };
        }

        private static void ShowText()
        {
            ConsoleService.WriteLine("Mostrando texto...");
        }

        private static void EditText()
        {
            Console.WriteLine("Editando texto...");
        }

       
    }
}
