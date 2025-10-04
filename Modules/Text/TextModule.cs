using CliTool.Core;
using CliTool.Services;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace CliTool.Modules.Text
{
    public class TextModule : BaseModule
    {
        public TextModule()
        {
            SetMenu(CreateMenu());
        }

        private static Menu CreateMenu()
        {
            return new Menu
            {
                Name = "Módulo de Texto",
                Options = new List<Option>
                {
                    new() { OrderText = "1", DisplayText = "Contador de caracteres", Execute = CharCounter },
                    new() { OrderText = "2", DisplayText = "Contador de palavras", Execute = WordCounter },
                    new() { OrderText = "3", DisplayText = "Inverter texto", Execute = ReverseText },
                    new() { OrderText = "4", DisplayText = "Transformar em maiúsculas/minúsculas", Execute = ChangeCase },
                    new() { OrderText = "5", DisplayText = "Remover espaços extras", Execute = TrimSpaces },
                    new() { OrderText = "6", DisplayText = "Contar ocorrências de uma palavra", Execute = WordOccurrences },
                    new() { OrderText = "7", DisplayText = "Converter para camelCase / PascalCase / snake_case / kebab-case", Execute = ConvertCaseStyle },
                    new() { OrderText = "8", DisplayText = "Extrair apenas números", Execute = ExtractNumbers }
                }
            };
        }

        private static string ReadText(string label = "Texto: ")
        {
            ConsoleService.Write(label);
            return Console.ReadLine() ?? string.Empty;
        }

        private static void CharCounter()
        {
            string text = ReadText();
            ConsoleService.WriteResult(text.Length.ToString());
        }

        private static void WordCounter()
        {
            string text = ReadText();
            int count = Regex.Matches(text.Trim(), @"\b\S+\b").Count;
            ConsoleService.WriteResult(count.ToString());
        }

        private static void ReverseText()
        {
            string text = ReadText();
            string reversed = new string(text.Reverse().ToArray());
            ConsoleService.WriteResult(reversed);
        }

        private static void ChangeCase()
        {
            string text = ReadText();
            ConsoleService.WriteLine("1 - Maiúsculas | 2 - Minúsculas");
            string? opt = Console.ReadLine();
            string result = opt == "1" ? text.ToUpper() : text.ToLower();
            ConsoleService.WriteResult(result);
        }

        private static void TrimSpaces()
        {
            string text = ReadText();
            string normalized = Regex.Replace(text.Trim(), @"\s+", " ");
            ConsoleService.WriteResult(normalized);
        }

        private static void WordOccurrences()
        {
            string text = ReadText();
            string word = ReadText("Palavra: ");
            int count = Regex.Matches(text, $@"\b{Regex.Escape(word)}\b", RegexOptions.IgnoreCase).Count;
            ConsoleService.WriteResult(count.ToString());
        }

        private static void ConvertCaseStyle()
        {
            string text = ReadText();
            var words = Regex.Split(text.Trim(), @"\s+|_|-|(?=[A-Z])")
                .Where(w => !string.IsNullOrWhiteSpace(w))
                .Select(w => w.ToLower())
                .ToArray();

            string camel = words[0] + string.Concat(words.Skip(1).Select(w => char.ToUpper(w[0]) + w[1..]));
            string pascal = string.Concat(words.Select(w => char.ToUpper(w[0]) + w[1..]));
            string snake = string.Join("_", words);
            string kebab = string.Join("-", words);

            ConsoleService.WriteResult(
                $"camelCase: {camel}\nPascalCase: {pascal}\nsnake_case: {snake}\nkebab-case: {kebab}"
            );
        }

        private static void ExtractNumbers()
        {
            string text = ReadText();
            string result = string.Concat(text.Where(char.IsDigit));
            ConsoleService.WriteResult(result);
        }
    }
}
