namespace CliTool.Modules.ProjectStarter
{
    /// <summary>
    /// Representa um projeto que pode ser iniciado com comandos em terminal.
    /// </summary>
    public class StarterArg
    {
        /// <summary>
        /// Nome amigável exibido no menu.
        /// </summary>
        public string Label { get; set; } = "nome-do-projeto";

        /// <summary>
        /// Caminho completo do diretório do projeto.
        /// </summary>
        public string DirectoryPath { get; set; } = "/home/user/projetos/MeuProjeto";

        /// <summary>
        /// Lista de comandos a serem executados em sequência no terminal.
        /// </summary>
        public List<string> Commands { get; set; } = new() { "npm install", "npm run dev" };

        /// <summary>
        /// Terminal específico para executar os comandos (bash, powershell, cmd, etc).
        /// Se nulo, usa o terminal padrão do sistema.
        /// </summary>
        public string? Terminal { get; set; }

        /// <summary>
        /// Id usado para agrupar múltiplos projetos que devem ser iniciados juntos.
        /// Cada projeto do grupo será iniciado em um terminal separado.
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        /// Nome do grupo (exibido no menu quando há GroupId).
        /// </summary>
        public string? GroupName { get; set; }
    }
}
