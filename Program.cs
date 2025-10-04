using CliTool.Core;
using CliTool.Modules.Text;
using CliTool.Services;

var modules = new List<BaseModule>
            {
                new TextModule(),
                new ConfigurationModule(),
            };

while (true)
{
    Console.Clear();
    ConsoleService.WriteModuleName("Menu Principal");

    for (int i = 0; i < modules.Count; i++)
    {
        ConsoleService.WriteLine($"{i + 1} - {modules[i].Name}");
    }
    ConsoleService.WriteLine("0 - Sair");

    ConsoleService.Write("\nEscolha um módulo: ");
    var input = Console.ReadLine();
    Console.Clear();

    if (input == "0")
        break;

    if (int.TryParse(input, out int choice) && choice >= 1 && choice <= modules.Count)
    {
        var selectedModule = modules[choice - 1];
        selectedModule.RunMenu();
    }
    else
    {
        ConsoleService.WriteLine("\nOpção inválida! Pressione qualquer tecla para tentar novamente...");
        Console.ReadKey();
    }
}

Console.WriteLine("\nSaindo...");