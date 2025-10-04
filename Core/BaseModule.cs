

using CliTool.Services;

namespace CliTool.Core
{
    public class BaseModule
    {
        private Menu Menu { get; set; }

        public BaseModule() {}

        public string Name => Menu.Name;
        protected void SetMenu(Menu menu)
        {
            Menu = menu;
        }

        public void RunMenu()
        {
            Console.Clear();

            if (!HasMenu())
               throw new NotImplementedException("Menu não implementado");
           
            while (true)
            { 
                ConsoleService.WriteModuleName(Menu!.Name);

                foreach (var option in Menu.Options)
                {
                    ConsoleService.WriteLine($"{option.OrderText} - {option.DisplayText}");
                }

                ConsoleService.WriteLine("0 - Voltar");

                Console.WriteLine();

                ConsoleService.Write("Escolha uma opção: ");
                
                var input = Console.ReadLine();

                if (input == "0")
                {
                    Console.Clear();
                    break;
                }

                var selectedOption = Menu.Options.FirstOrDefault(o => o.OrderText == input);
                if (selectedOption != null)
                {
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    try
                    {
                        
                        Console.WriteLine();
                        ConsoleService.WriteInfo($"Executando: {selectedOption.DisplayText}");
                        Console.WriteLine();
                        selectedOption.Execute.Invoke();
                        Console.WriteLine();
                        ConsoleService.WriteSuccess("Executado com sucesso.");
                    }
                    catch (Exception ex)
                    {
                        ConsoleService.WriteError("Erro na execução.");
                        ConsoleService.WriteLine($"\nErro ao executar a opção: {ex.Message}");
                    }
                    finally
                    {
                        watch.Stop();
                        ConsoleService.WriteLine($"{watch.ElapsedMilliseconds}" + " ms\n", ConsoleColorEnum.DarkGray);
                        ConsoleService.WriteLine($"Pressione qualquer tecla para continuar...", ConsoleColorEnum.DarkYellow);
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
                else
                {
                    ConsoleService.WriteError("Opção inválida! Pressione qualquer tecla para tentar novamente...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        protected bool HasMenu() =>  Menu != null;
    }
}
