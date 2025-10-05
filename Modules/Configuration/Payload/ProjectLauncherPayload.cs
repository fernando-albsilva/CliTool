namespace CliTool.Modules.Configuration.Payload
{
    public static class ProjectLauncherPayload
    {
        public static List<ProjectArg> Projects => new List<ProjectArg>
        {
            new ProjectArg
            {
                Label = "easy-restaurant-api",
                DirectoryPath = @"C:\Users\ferna\Repositorios\Easy-Restaurant-Api",
                Ide = "vscode"
            },
            new ProjectArg
            {
                Label = "web-app",
                DirectoryPath = @"C:\Users\ferna\Repositorios\ezyapp-web",
                Ide = "visualstudio"
            },
             new ProjectArg
            {
                Label = "easy-restaurant-api",
                DirectoryPath = @"C:\Users\ferna\Repositorios\Easy-Restaurant-Api",
                Ide = "visualstudio",
                GrouId = 1
            },
            new ProjectArg
            {
                Label = "web-app",
                DirectoryPath = @"C:\Users\ferna\Repositorios\ezyapp-web",
                Ide = "vscode",
                GrouId = 1
            },
        };
    }
}
