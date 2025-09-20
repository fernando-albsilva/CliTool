

using CliTool.Services;

namespace CliTool.Core
{
    public class BaseModule
    {
        public Menu? Menu { get; }

        public BaseModule(Menu menu)
        {
            Menu = menu;
        }

        public void RunMenu()
        {
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

                ConsoleService.Write("\nEscolha uma opção: ");
                var input = Console.ReadLine();

                if (input == "0")
                    break;

                var selectedOption = Menu.Options.FirstOrDefault(o => o.OrderText == input);
                if (selectedOption != null)
                {
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    try
                    {
                        ConsoleService.WriteLine($"\nExecutando: {selectedOption.DisplayText}\n", ConsoleColorEnum.Blue);
                        selectedOption.Execute.Invoke();
                        ConsoleService.WriteLine("Executado com sucesso.", ConsoleColorEnum.Green);
                    }
                    catch (Exception ex)
                    {
                        ConsoleService.WriteLine("Erro na execução.", ConsoleColorEnum.Red);
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
                    ConsoleService.WriteLine("\nOpção inválida! Pressione qualquer tecla para tentar novamente...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        protected bool HasMenu() =>  Menu != null;
    }
}
