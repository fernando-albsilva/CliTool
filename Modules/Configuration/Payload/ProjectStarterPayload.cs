using CliTool.Modules.ProjectStarter;

namespace CliTool.Modules.Configuration.Payload
{
    public static class ProjectStarterPayload
    {
        public static List<StarterArg> Projects => new List<StarterArg>
        {
            new StarterArg
            {
                Label = "API Backend",
                DirectoryPath = "/home/user/projetos/api-backend",
                Commands = new List<string> { "dotnet restore", "dotnet run" },
                Terminal = null,
                GroupId = null,
                GroupName = null
            },
            new StarterArg
            {
                Label = "Frontend React",
                DirectoryPath = "/home/user/projetos/frontend-react",
                Commands = new List<string> { "npm install", "npm run dev" },
                Terminal = null,
                GroupId = null,
                GroupName = null
            },
            new StarterArg
            {
                Label = "API do Sistema",
                DirectoryPath = "/home/user/projetos/sistema/api",
                Commands = new List<string> { "dotnet watch run" },
                Terminal = null,
                GroupId = 1,
                GroupName = "Sistema Completo"
            },
            new StarterArg
            {
                Label = "Frontend do Sistema",
                DirectoryPath = "/home/user/projetos/sistema/frontend",
                Commands = new List<string> { "npm run dev" },
                Terminal = null,
                GroupId = 1,
                GroupName = "Sistema Completo"
            }
        };
    }
}
