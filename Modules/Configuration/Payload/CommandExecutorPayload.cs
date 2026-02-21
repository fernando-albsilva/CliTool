using CliTool.Modules.CommandExecutor;

namespace CliTool.Modules.Configuration.Payload
{
    public static class CommandExecutorPayload
    {
        public static List<CommandListArg> CommandLists => new List<CommandListArg>
        {
            new CommandListArg
            {
                Label = "Build e Deploy API",
                Description = "Compila e faz deploy da API",
                WorkingDirectory = "/home/user/projetos/api",
                Commands = new List<CommandItem>
                {
                    new CommandItem { Command = "dotnet restore", RunInParallel = false },
                    new CommandItem { Command = "dotnet build -c Release", RunInParallel = false },
                    new CommandItem { Command = "dotnet publish -c Release -o ./publish", RunInParallel = false }
                },
                GroupId = null,
                GroupName = null
            },
            new CommandListArg
            {
                Label = "Iniciar Ambiente de Dev",
                Description = "Inicia API e Frontend em terminais separados",
                WorkingDirectory = null,
                Commands = new List<CommandItem>
                {
                    new CommandItem 
                    { 
                        Command = "docker-compose up -d", 
                        RunInParallel = false, 
                        WorkingDirectory = "/home/user/projetos/infra" 
                    },
                    new CommandItem 
                    { 
                        Command = "dotnet watch run", 
                        RunInParallel = true, 
                        Terminal = "gnome-terminal",
                        WorkingDirectory = "/home/user/projetos/api" 
                    },
                    new CommandItem 
                    { 
                        Command = "npm run dev", 
                        RunInParallel = true, 
                        Terminal = "gnome-terminal",
                        WorkingDirectory = "/home/user/projetos/frontend" 
                    }
                },
                GroupId = null,
                GroupName = null
            }
        };
    }
}
