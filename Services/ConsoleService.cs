namespace CliTool.Services
{
    public class ConsoleService
    {
        public static void WriteModuleName(string name)
        {
            WriteLine($"=== {name.ToUpper()} ===\n\n", ConsoleColorEnum.Yellow);
        }

        public static void WriteLine(string text, ConsoleColorEnum color = ConsoleColorEnum.White)
        {
            // Salva a cor atual do console
            var previousColor = Console.ForegroundColor;

            // Converte enum para ConsoleColor
            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color.ToString());

            Console.WriteLine(text);

            // Restaura a cor original
            Console.ForegroundColor = previousColor;
        }

        public static void Write(string text, ConsoleColorEnum color = ConsoleColorEnum.White)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color.ToString());
            Console.Write(text);
            Console.ForegroundColor = previousColor;
        }
    }

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
