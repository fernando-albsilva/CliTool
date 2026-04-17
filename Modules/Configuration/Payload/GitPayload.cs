using CliTool.Modules.Git;

namespace CliTool.Modules.Configuration.Payload
{
    public static class GitPayload
    {
        
        public static List<GitModuleArg> Args => new List<GitModuleArg>
        {
           new GitModuleArg
           {
               MenuDescription = "Atualizar Branches",
               Repositories = new List<GitModuleRepositoryArg>
               {
                    new GitModuleRepositoryArg
                    {
                        Name = "Projeto_site_2",
                        Path = "C:\\Users\\fernando\\Repositorios\\Projeto_site_2"
                    }
               },
               DefaultBranches = new List<string>
               {
                   "Master",
                   "branch_a",
                   "Branch_b"
               }
           }  
        };
            
    }
}
