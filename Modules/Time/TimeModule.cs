using CliTool.Core;
using CliTool.Services;

namespace CliTool.Modules.Time
{
    public class ModuleTime : BaseModule
    {
        private static readonly JsonService _jsonService = new JsonService();
        public ModuleTime()
        {
            SetMenu(CreateMenu());
        }

        private static Menu CreateMenu()
        {
            return new Menu
            {
                Name = "Tempo",
                Options = new List<Option>
                {
                    new Option { OrderText = "1", DisplayText = "Marcar horário", Execute = AddMark },
                    new Option { OrderText = "2", DisplayText = "Listar horários", Execute = ListMarks },
                    new Option { OrderText = "3", DisplayText = "Iniciar cronômetro/alarme", Execute = StartTimer }
                }
            };
        }

        private static void StartTimer()
        {
            ConsoleService.WriteLine("Digite o tempo em minutos:");
            if (!int.TryParse(Console.ReadLine(), out int minutes) || minutes <= 0)
            {
                ConsoleService.WriteError("Entrada inválida. Informe um número maior que zero.");
                return;
            }

            int totalSeconds = minutes * 60;

            ConsoleService.WriteLine($"Cronômetro iniciado por {minutes} minuto(s).");

            for (int i = 0; i < totalSeconds; i++)
            {
                int remainingMinutes = (totalSeconds - i) / 60;
                int remainingSeconds = (totalSeconds - i) % 60;

                Console.Write($"\rTempo restante: {remainingMinutes:D2}:{remainingSeconds:D2}");
                Thread.Sleep(1000);
            }

            Console.WriteLine();
            ConsoleService.WriteResult($"⏰ Alarme! {minutes} minuto(s) se passaram.");
            Console.Beep();
            Thread.Sleep(1000);
            Console.Beep();
            Thread.Sleep(1000);
            Console.Beep();
        }

        private static void AddMark()
        {
            var now = DateTime.Now;
            ConsoleService.WriteInfo($"Data atual: {now:yyyy-MM-dd HH:mm:ss}");

            ConsoleService.WriteLine("Informe o tempo gasto ou pressione Enter para deixar em branco:");
            var time = Console.ReadLine() ?? "";

            ConsoleService.WriteLine("Informe uma descrição ou pressione Enter para deixar em branco:");
            var description = Console.ReadLine() ?? "";

            var mark = new TimeMark
            {
                Date = now,
                Time = time,
                Description = description
            };

           
            var marks = _jsonService.ReadJsonFile<List<TimeMark>>(AppContext.BaseDirectory, nameof(ModuleTime)) ?? new List<TimeMark>();
            marks.Add(mark);

            var orderMarks = marks.OrderByDescending(x => x.Date);

            _jsonService.CreateJsonFile(AppContext.BaseDirectory, nameof(ModuleTime), marks);
        }

        private static void ListMarks()
        {
            var marks = _jsonService.ReadJsonFile<List<TimeMark>>(AppContext.BaseDirectory, nameof(ModuleTime));

            if (marks == null || !marks.Any())
            {
                ConsoleService.WriteWarning("Nenhuma marcação encontrada.");
                return;
            }

            var grouped = marks
         .OrderByDescending(x => x.Date)
         .GroupBy(x => x.Date.Date);

            ConsoleService.WriteLine("Marcações Registradas", ConsoleColor.DarkYellow);

            foreach (var dayGroup in grouped)
            {
                ConsoleService.WriteLine(new string('-', 40), ConsoleColor.DarkYellow);
                ConsoleService.WriteLine($"\nDia: {dayGroup.Key:yyyy-MM-dd}", ConsoleColor.Yellow);
                Console.WriteLine();

                foreach (var mark in dayGroup.OrderBy(x => x.Date))
                {
                    ConsoleService.WriteLine($"Tempo gasto: {mark.Time}", ConsoleColor.DarkCyan);
                    if (!string.IsNullOrWhiteSpace(mark.Description))
                        ConsoleService.WriteLine($"Descrição: {mark.Description}", ConsoleColor.DarkGray);

                    Console.WriteLine();
                }

              
            }
        }
    }
}
