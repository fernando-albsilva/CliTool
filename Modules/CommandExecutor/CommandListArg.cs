namespace CliTool.Modules.CommandExecutor
{
    /// <summary>
    /// Representa um comando individual a ser executado.
    /// </summary>
    public class CommandItem
    {
        /// <summary>
        /// Comando a ser executado.
        /// </summary>
        public string Command { get; set; } = string.Empty;

        /// <summary>
        /// Se true, executa em um terminal separado em paralelo.
        /// Se false, executa na sequência aguardando finalização.
        /// </summary>
        public bool RunInParallel { get; set; } = false;

        /// <summary>
        /// Terminal específico para executar (apenas quando RunInParallel = true).
        /// Se nulo, usa o terminal padrão do sistema.
        /// </summary>
        public string? Terminal { get; set; }

        /// <summary>
        /// Diretório de trabalho para o comando. Se nulo, usa o WorkingDirectory do CommandList.
        /// </summary>
        public string? WorkingDirectory { get; set; }
    }

    /// <summary>
    /// Representa uma lista de comandos a serem executados.
    /// </summary>
    public class CommandListArg
    {
        /// <summary>
        /// Nome amigável exibido no menu.
        /// </summary>
        public string Label { get; set; } = "Minha Lista de Comandos";

        /// <summary>
        /// Descrição opcional da lista de comandos.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Diretório de trabalho padrão para os comandos.
        /// </summary>
        public string? WorkingDirectory { get; set; }

        /// <summary>
        /// Lista de comandos a serem executados em ordem.
        /// </summary>
        public List<CommandItem> Commands { get; set; } = new();

        /// <summary>
        /// Id usado para agrupar múltiplas listas de comandos.
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        /// Nome do grupo (exibido no menu quando há GroupId).
        /// </summary>
        public string? GroupName { get; set; }
    }
}
