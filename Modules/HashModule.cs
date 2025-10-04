using CliTool.Core;
using CliTool.Services;
using System.Security.Cryptography;
using System.Text;

namespace CliTool.Modules.Hash
{
    public class HashModule : BaseModule
    {
        public HashModule()
        {
            SetMenu(CreateMenu());
        }

        private static Menu CreateMenu()
        {
            return new Menu
            {
                Name = "Hashes",
                Options = new List<Option>
                {
                    new Option { OrderText = "1", DisplayText = "MD5 de texto", Execute = HashMD5 },
                    new Option { OrderText = "2", DisplayText = "SHA256 de texto", Execute = HashSHA256 },
                    new Option { OrderText = "3", DisplayText = "SHA1 de texto", Execute = HashSHA1 },
                    new Option { OrderText = "4", DisplayText = "MD5 de arquivo", Execute = HashFileMD5 },
                    new Option { OrderText = "5", DisplayText = "SHA256 de arquivo", Execute = HashFileSHA256 },
                }
            };
        }

        private static string AskInput(string message)
        {
            ConsoleService.WriteLine(message);
            return Console.ReadLine() ?? string.Empty;
        }

        private static string ComputeHash(string input, HashAlgorithm algorithm)
        {
            byte[] data = Encoding.UTF8.GetBytes(input);
            byte[] hash = algorithm.ComputeHash(data);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        private static void HashMD5()
        {
            string text = AskInput("Digite o texto para gerar MD5:");
            string result = ComputeHash(text, MD5.Create());
            ConsoleService.WriteResult(result);
        }

        private static void HashSHA256()
        {
            string text = AskInput("Digite o texto para gerar SHA256:");
            string result = ComputeHash(text, SHA256.Create());
            ConsoleService.WriteResult(result);
        }

        private static void HashSHA1()
        {
            string text = AskInput("Digite o texto para gerar SHA1:");
            string result = ComputeHash(text, SHA1.Create());
            ConsoleService.WriteResult(result);
        }

        private static void HashFileMD5()
        {
            string path = AskInput("Digite o caminho do arquivo:");
            if (!File.Exists(path))
            {
                ConsoleService.WriteError("Arquivo não encontrado.");
                return;
            }

            using var stream = File.OpenRead(path);
            using var md5 = MD5.Create();
            string hash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
            ConsoleService.WriteResult(hash);
        }

        private static void HashFileSHA256()
        {
            string path = AskInput("Digite o caminho do arquivo:");
            if (!File.Exists(path))
            {
                ConsoleService.WriteError("Arquivo não encontrado.");
                return;
            }

            using var stream = File.OpenRead(path);
            using var sha256 = SHA256.Create();
            string hash = BitConverter.ToString(sha256.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
            ConsoleService.WriteResult(hash);
        }
    }
}
