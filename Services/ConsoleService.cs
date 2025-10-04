using System.Text;

namespace CliTool.Services
{
    public class ConsoleService
    {
        /// <summary>
        /// Escreve uma mensagem informativa no console com o prefixo "[INFO]:" em azul.
        /// </summary>
        /// <param name="text">Texto da mensagem a ser exibida.</param>
        public static void WriteInfo(string text)
        {
            Write("[Info]: ", ConsoleColor.Blue);
            WriteLine(text);
        }

        /// <summary>
        /// Escreve uma mensagem de aviso no console com o prefixo "[WARN]:" em amarelo.
        /// </summary>
        /// <param name="text">Texto da mensagem a ser exibida.</param>
        public static void WriteWarning(string text)
        {
            Write("[Warn]: ", ConsoleColor.Yellow);
            WriteLine(text);
        }

        /// <summary>
        /// Escreve uma mensagem de erro no console com o prefixo "[ERROR]:" em vermelho.
        /// </summary>
        /// <param name="text">Texto da mensagem a ser exibida.</param>
        public static void WriteError(string text)
        {
            Write("[Error]: ", ConsoleColor.Red);
            WriteLine(text);
        }

        /// <summary>
        /// Escreve uma mensagem de sucesso no console com o prefixo "[SUCCESS]:" em verde.
        /// </summary>
        /// <param name="text">Texto da mensagem a ser exibida.</param>
        public static void WriteSuccess(string text)
        {
            Write("[Success]: ", ConsoleColor.Green);
            WriteLine(text);
        }

        /// <summary>
        /// Exibe uma mensagem de resultado no console, precedida pelo rótulo "Resultado:" 
        /// em amarelo escuro. O texto informado é exibido logo em seguida na cor padrão do console.
        /// </summary>
        /// <param name="text">O texto a ser exibido após o rótulo "Resultado:". Caso não seja informado,  nada é exibido.</param>
        public static void WriteResult(string text = "")
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Resultado: ");
            Console.ForegroundColor = previousColor;
            Write(text);
        }

        /// <summary>
        /// Exibe um título de módulo formatado dentro de uma caixa com bordas no console.
        /// </summary>
        /// <param name="name">Nome do módulo que será exibido como título.</param>
        public static void WriteModuleName(string name)
        {
            var title = name.ToUpper().Trim();

            Console.WriteLine();

            var lineBuilder = new StringBuilder();

            for (int i = 0; i < name.Length + 10; i++)
            {
                lineBuilder.Append("─");
            }

            var titleAligment = "     ";

            var layoutLine = lineBuilder.ToString();
            var moduleTitle = $"{titleAligment}{title}{titleAligment}";

            WriteLine($"┌{layoutLine}┐", ConsoleColor.Yellow);
            WriteLine($"│{moduleTitle}│", ConsoleColor.Yellow);
            WriteLine($"└{layoutLine}┘", ConsoleColor.Yellow);

            Console.WriteLine();
        }

        /// <summary>
        /// Escreve uma linha de texto colorida no console e move o cursor para a próxima linha.
        /// </summary>
        /// <param name="text">Texto a ser exibido.</param>
        /// <param name="color">Cor do texto. Padrão: branco.</param>
        public static void WriteLine(string text = "", ConsoleColor color = ConsoleColor.White)
        {
            var previousColor = Console.ForegroundColor;

            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color.ToString());
            Console.WriteLine(text);

            Console.ForegroundColor = previousColor;
        }

        /// <summary>
        /// Escreve um texto colorido no console sem quebrar linha.
        /// </summary>
        /// <param name="text">Texto a ser exibido.</param>
        /// <param name="color">Cor do texto. Padrão: branco.</param>
        public static void Write(string text, ConsoleColor color = ConsoleColor.White)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color.ToString());
            Console.Write(text);
            Console.ForegroundColor = previousColor;
        }
    }
}
