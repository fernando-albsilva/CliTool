using System.Runtime.InteropServices;
using System.Text;
using CliTool.Core;
using CliTool.Modules.CommandExecutor;
using CliTool.Services;

namespace CliTool.Modules.Git;

public class GitModule : BaseModule
{
    private static readonly JsonService JsonService = new();
    private static List<GitModuleArg> Args = new();
    
    public GitModule()
    {
        LoadArgs();
        SetMenu(CreateMenu());
    }

    private void LoadArgs()
    {
        Args = JsonService.ReadJsonFile<List<GitModuleArg>>(Config.ConfigDirectoryPath, nameof(GitModule)) ?? new List<GitModuleArg>();

        if (Args.Count == 0)
        {
            ConsoleService.WriteWarning($"Nenhuma lista de comandos encontrada no arquivo {nameof(GitModule)}.json.");
        }
    }

    private static Menu CreateMenu()
    {
        var options = new List<Option>();

        int order = 1;

        foreach (var arg in Args)
        {
            var displayText = new StringBuilder();
            displayText.Append(arg.MenuDescription);
            
            options.Add(new Option
            {
                OrderText = order.ToString(),
                DisplayText = displayText.ToString(),
                Execute = () => Run(arg)
            });

            order++;
        }

        return new Menu
        {
            Name = "Git",
            Options = options
        };
    }

    private static void Run(GitModuleArg arg)
    {

        foreach (var repository in arg.Repositories)
        {

            var branches = repository.SpecicBranches.Any() ? repository.SpecicBranches : arg.DefaultBranches;

            foreach (var branch in branches)
            {
                var commands = new List<string>
                {
                   GeneratePwdCommand(),
                   GenerateStashCommand(),
                   GenerateCheckoutCommand(branch),
                   GeneratePwdCommand(),
                   GenerateShowCurrentCommand(),
                   GenerateFetchCommand(),
                   GeneratePullCommand()
                };

                foreach (var command in commands)
                {
                    var executableCommand = new CommandItem
                    {
                        Command = command
                    };

                    try
                    {
                        ConsoleService.WriteLine($"$ {command}", ConsoleColor.Green);
                        var success = TerminalService.RunCommandSequential(executableCommand, repository.Path);
                        if (!success)
                        {
                            ConsoleService.WriteError($"Comando falhou. Interrompendo execução da lista.");
                            return;
                        }
                        ConsoleService.WriteLine();
                    }
                    catch (Exception ex)
                    {
                        ConsoleService.WriteError($"Erro ao executar comando <{command}>: {ex.Message}");
                        return;
                    }
                }
            }

        }

    }

    private static string GeneratePwdCommand()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "cd";
        }
        else
        {
            return "pwd";
        }
    }

    private static string GenerateStashCommand()
    {
        var dateTimeNow = DateTime.Now;
        return $"git stash save -m \"[cli-bot {DateTime.Now}]\"";
    }

    private static string GenerateCheckoutCommand(string branch)
    {
        return $"git checkout {branch}";
    }
  
    private static string GenerateShowCurrentCommand()
    {
         return "git branch --show-current";
    }

    private static string GenerateFetchCommand()
    {
        return "git fetch --all";
    }

     private static string GeneratePullCommand()
    {
        return "git pull";
    }
}