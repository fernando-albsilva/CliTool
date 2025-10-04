using System.Text;

namespace CliTool.Services
{
    public class ConsoleService
    {
        public static void WriteInfo(string text)
        {
            Write("[INFO]: ", ConsoleColorEnum.Blue);
            WriteLine(text);
        }

        public static void WriteWarning(string text)
        {
            Write("[WARN]: ", ConsoleColorEnum.Yellow);
            WriteLine(text);
        }

        public static void WriteError(string text)
        {
            Write("[ERROR]: ", ConsoleColorEnum.Red);
            WriteLine(text);
        }

        public static void WriteSuccess(string text)
        {
            Write("[SUCCESS]: ", ConsoleColorEnum.Green);
            WriteLine(text);
        }

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

      

        public static void WriteLine(string text, ConsoleColorEnum color = ConsoleColorEnum.White)
        {
            var previousColor = Console.ForegroundColor;
            
            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color.ToString())
            Console.WriteLine(text);

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
