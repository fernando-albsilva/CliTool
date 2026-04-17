namespace CliTool.Modules.Git;

public class GitModuleArg
{
    public required string MenuDescription { get; set; }   
    public List<GitModuleRepositoryArg> Repositories { get; set; }  = new List<GitModuleRepositoryArg>(); 
    public List<string> DefaultBranches { get; set; } = new List<string>();   
}

public class GitModuleRepositoryArg
{
    public required string Name { get; set; }
    public required string Path { get; set; }
    public List<string> SpecicBranches { get; set; } = new List<string>();   
}