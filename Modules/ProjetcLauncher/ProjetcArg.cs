public class ProjectArg
{
    /// <summary>
    /// Nome amigável exibido no menu.
    /// </summary>
    public string Label { get; set; } = "nome-do-projeto";

    /// <summary>
    /// Caminho completo do diretório do projeto.
    /// </summary>
    public string DirectoryPath { get; set; } = "C:\\Projetos\\FrontendReac";

    /// <summary>
    /// IDE usada para abrir o projeto (vscode ou visualstudio).
    /// </summary>
    public string Ide { get; set; } = "vscode";
}
