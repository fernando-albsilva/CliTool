using CliTool.Core;
using CliTool.Modules.Commands;
using CliTool.Modules.Exemple;
using CliTool.Modules.Hash;
using CliTool.Modules.Project;
using CliTool.Modules.Text;
using CliTool.Modules.Time;
using CliTool.Services;

var modules = new List<BaseModule>
            {
                new ToolInfoModule(),
                new ProjectModuleModule(),
                new TextModule(),
                new HashModule(),
                new TimeModule(),
                new ConfigurationModule(),
            };

#if DEBUG
modules.Add(new ExempleModule());
#endif

while (true)
{
    ConsoleService.WriteModuleName("Menu Principal");

    for (int i = 0; i < modules.Count; i++)
    {
        ConsoleService.WriteLine($"{i + 1} - {modules[i].Name}");
    }

    ConsoleService.WriteLine("0 - Sair");

    Console.WriteLine();
    ConsoleService.Write("Escolha um módulo: ");
    
    var input = Console.ReadLine();

    if (input == "0")
        break;

    if (int.TryParse(input, out int choice) && choice >= 1 && choice <= modules.Count)
    {
        var selectedModule = modules[choice - 1];
        selectedModule.RunMenu();
    }
    else
    {
        ConsoleService.WriteError("Opção inválida! Pressione qualquer tecla para tentar novamente...");
        Console.ReadKey();
        Console.Clear();
    }
}

Console.WriteLine();
Console.WriteLine("Saindo...");