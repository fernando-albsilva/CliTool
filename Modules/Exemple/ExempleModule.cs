using CliTool.Core;
using CliTool.Services;

namespace CliTool.Modules.Exemple
{
    public class ExempleModule : BaseModule
    {
        public ExempleModule()
        {
            SetMenu(CreateMenu());
        }

        private static Menu CreateMenu()
        {
            return new Menu
            {
                Name = "Módulo de Exemplo",
                Options = new List<Option>
                {
                    new() { OrderText = "1", DisplayText = "Ação 1", Execute = Action1 },
                    new() { OrderText = "2", DisplayText = "Ação 2", Execute = Action2 },
                    new() { OrderText = "3", DisplayText = "Ação 3", Execute = Action3 }
                }
            };
        }

        private static void Action1()
        {
            ConsoleService.WriteResult("Executando opção 1...");
        }

        private static void Action2()
        {
            ConsoleService.WriteResult("Executando opção 2...");
        }

        private static void Action3()
        {
            ConsoleService.WriteResult("Executando opção 3...");
        }
    }
}
