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
            Write("[INFO]: ", ConsoleColorEnum.Blue);
            WriteLine(text);
        }

        /// <summary>
        /// Escreve uma mensagem de aviso no console com o prefixo "[WARN]:" em amarelo.
        /// </summary>
        /// <param name="text">Texto da mensagem a ser exibida.</param>
        public static void WriteWarning(string text)
        {
            Write("[WARN]: ", ConsoleColorEnum.Yellow);
            WriteLine(text);
        }

        /// <summary>
        /// Escreve uma mensagem de erro no console com o prefixo "[ERROR]:" em vermelho.
        /// </summary>
        /// <param name="text">Texto da mensagem a ser exibida.</param>
        public static void WriteError(string text)
        {
            Write("[ERROR]: ", ConsoleColorEnum.Red);
            WriteLine(text);
        }

        /// <summary>
        /// Escreve uma mensagem de sucesso no console com o prefixo "[SUCCESS]:" em verde.
        /// </summary>
        /// <param name="text">Texto da mensagem a ser exibida.</param>
        public static void WriteSuccess(string text)
        {
            Write("[SUCCESS]: ", ConsoleColorEnum.Green);
            WriteLine(text);
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

            WriteLine($"┌{layoutLine}┐", ConsoleColorEnum.Yellow);
            WriteLine($"│{moduleTitle}│", ConsoleColorEnum.Yellow);
            WriteLine($"└{layoutLine}┘", ConsoleColorEnum.Yellow);

            Console.WriteLine();
        }

        /// <summary>
        /// Escreve uma linha de texto colorida no console e move o cursor para a próxima linha.
        /// </summary>
        /// <param name="text">Texto a ser exibido.</param>
        /// <param name="color">Cor do texto. Padrão: branco.</param>
        public static void WriteLine(string text, ConsoleColorEnum color = ConsoleColorEnum.White)
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
        public static void Write(string text, ConsoleColorEnum color = ConsoleColorEnum.White)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color.ToString());
            Console.Write(text);
            Console.ForegroundColor = previousColor;
        }
    }

    /// <summary>
    /// Enum que representa as cores suportadas pelo console.
    /// Utilizado para definir a cor do texto ao exibir mensagens.
    /// </summary>
    public enum ConsoleColorEnum
    {
        Black,
        Blue,
        Cyan,
        DarkBlue,
        DarkCyan,
        DarkGray,
        DarkGreen,
        DarkMagenta,
        DarkRed,
        DarkYellow,
        Gray,
        Green,
        Magenta,
        Red,
        White,
        Yellow
    }
}
