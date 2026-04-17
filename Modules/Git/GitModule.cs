using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using CliTool.Core;
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
                Execute = () => ExecuteCommandList(arg)
            });

            order++;
        }

        return new Menu
        {
            Name = "Git",
            Options = options
        };
    }

    private static List<string> CreteCommands(GitModuleRepositoryArg repository, List<string> defaultBranches)
    {
        var commands = new List<string>
        {
             GenerateChangeDirectoryCommand(repository.Path)
        };

        var branchsToGenerate = repository.SpecicBranches.Any() ? repository.SpecicBranches : defaultBranches;
        
        foreach (var branch in branchsToGenerate)
        {
            commands.Add(GenerateStashCommand());
            commands.Add(GenerateCheckoutCommand(branch));
            commands.Add(GenerateShowCurrentCommand());
            commands.Add(GenerateFetchCommand());
            commands.Add(GeneratePullCommand());   
        }

        return commands;   
    }

    private static string GenerateChangeDirectoryCommand(string path)
    {
        return $"cd {path}";
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

    private static void ExecuteCommandList(GitModuleArg arg)
    {
        var commands = new List<string>();

        foreach (var repository in arg.Repositories)
            commands.AddRange(CreteCommands(repository, arg.DefaultBranches));

        ConsoleService.WriteLine(string.Empty, ConsoleColor.White);

        Execute(commands);
    }

    private static bool Execute(List<string> commandList)
    {
        bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        string scriptPath;
        ProcessStartInfo psi;

        if (isWindows)
        {
            scriptPath = Path.Combine(Path.GetTempPath(), "clitool_script.bat");
            File.WriteAllLines(scriptPath, commandList);
            psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/C \"{scriptPath}\""
            };
        }
        else
        {
            scriptPath = Path.Combine(Path.GetTempPath(), "clitool_script.sh");
            File.WriteAllLines(scriptPath, commandList);
            psi = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = scriptPath
            };
        }

        psi.RedirectStandardOutput = true;
        psi.RedirectStandardError = true;
        psi.UseShellExecute = false;
        psi.CreateNoWindow = true;

        using var process = Process.Start(psi)!;

        // Lê stdout e stderr em paralelo para evitar deadlock
        var stdoutTask = Task.Run(() =>
        {
            string? line;
            while ((line = process.StandardOutput.ReadLine()) != null)
                ConsoleService.WriteLine(line, ConsoleColor.White);
        });

        var stderrTask = Task.Run(() =>
        {
            string? line;
            while ((line = process.StandardError.ReadLine()) != null)
                ConsoleService.WriteLine(line, ConsoleColor.Yellow);
        });

        process.WaitForExit();
        Task.WaitAll(stdoutTask, stderrTask);

        File.Delete(scriptPath);

        return process.ExitCode == 0;
    }
}