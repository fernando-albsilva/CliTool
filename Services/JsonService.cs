using System.Text.Json;

namespace CliTool.Services
{
    public class JsonService
    {
        private readonly JsonSerializerOptions _options;

        public JsonService()
        {
            _options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
        }

        /// <summary>
        /// Cria um novo arquivo JSON no caminho especificado.
        /// Se já existir, sobrescreve.
        /// </summary>
        public string CreateJsonFile(string filePath, string FileName, object data)
        {
            var json = JsonSerializer.Serialize(data, _options);
            var fullPath = CreateFullPath(filePath, FileName);
            ConsoleService.WriteLine($"Escrevendo no arquivo: {fullPath}", ConsoleColorEnum.Blue);
            File.WriteAllText(fullPath, json);
            return fullPath;
        }


        /// <summary>
        /// Lê um arquivo JSON e desserializa para um objeto.
        /// </summary>
        public T? ReadJsonFile<T>(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Arquivo não encontrado: {filePath}");

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(json, _options);
        }

        /// <summary>
        /// Escreve (sobrescreve) um objeto em um arquivo JSON existente.
        /// Se não existir, cria.
        /// </summary>
        public void WriteJsonFile<T>(string filePath, T data)
        {
            var json = JsonSerializer.Serialize(data, _options);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Verifica se um arquivo JSON existe.
        /// </summary>
        public bool ExistJsonFile(string filePath)
        {
            return File.Exists(filePath);
        }


        private static string CreateFullPath(string filePath, string fileName)
        {
            return string.Concat(filePath, fileName);
        }


    }
}
