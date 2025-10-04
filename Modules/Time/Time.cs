using CliTool.Core;
using CliTool.Services;

namespace CliTool.Modules.Time
{
    public class TimeModule : BaseModule
    {
        public TimeModule()
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
                    new Option { OrderText = "1", DisplayText = "Iniciar cronômetro/alarme", Execute = StartTimer }
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
    }
}
