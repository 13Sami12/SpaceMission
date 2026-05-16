using SpaceMission.Core;

namespace SpaceMission
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║        🚀 SPACE MISSION CONTROL       ║");
            Console.WriteLine("╚══════════════════════════════════════╝");
            Console.WriteLine();

            string choice = "";
            if (args.Length > 0 && (args[0] == "1" || args[0] == "2"))
            {
                choice = args[0];
            }
            else
            {
                bool running = true;
                while (running)
                {
                    Console.WriteLine("Select mode:");
                    Console.WriteLine("  [1] Enter map manually");
                    Console.WriteLine("  [2] Generate random map");
                    Console.WriteLine("  [3] Exit");
                    Console.Write("Choice: ");

                    choice = Console.ReadLine()?.Trim() ?? "";
                    Console.WriteLine();

                    switch (choice)
                    {
                        case "1":
                        case "2":
                            running = false;
                            break;
                        case "3":
                            Console.WriteLine("Mission Control signing off. Goodbye! 👋");
                            return;
                        default:
                            Console.WriteLine("⚠ Invalid choice. Please try again.\n");
                            break;
                    }
                }
            }

            switch (choice)
            {
                case "1":
                    RunManualMode();
                    break;
                case "2":
                    RunRandomMode();
                    break;
            }
        }

        static void RunManualMode()
        {
            try
            {
                Grid grid = InputParser.ReadGridFromConsole();
                ExecuteMission(grid);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error reading map: {ex.Message}\n");
            }
        }

        static void RunRandomMode()
        {
            try
            {
                Grid grid = MapGenerator.GenerateRandom();
                Console.WriteLine("Generated map:");
                grid.Print();
                Console.WriteLine();
                ExecuteMission(grid);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error generating map: {ex.Message}\n");
            }
        }

        static void ExecuteMission(Grid grid)
        {
            IPathfinder pathfinder = SelectPathfinder();
            var mission = new Core.SpaceMission(grid, pathfinder);
            mission.Run();
            Console.WriteLine();

            if (AskYesNo("Send mission summary by email? (y/n): "))
            {
                var settings = PromptEmailSettings();
                try
                {
                    EmailService.SendMissionSummary(settings, mission.GetSummary());
                    ConsoleEx.WriteLine("✔ Email sent successfully.", ConsoleColor.Green);
                }
                catch (Exception ex)
                {
                    ConsoleEx.WriteLine($"❌ Email failed: {ex.Message}", ConsoleColor.Red);
                }
            }
        }

        static IPathfinder SelectPathfinder()
        {
            ConsoleEx.WriteHeader("Choose pathfinding algorithm:");
            Console.WriteLine("  [1] Dijkstra (safe for small maps)");
            Console.WriteLine("  [2] A* (recommended for larger maps)");
            Console.Write("Choice (default 2): ");

            string choice = Console.ReadLine()?.Trim() ?? "";
            Console.WriteLine();
            return choice == "1" ? new DijkstraPathfinder() : new AStarPathfinder();
        }

        static bool AskYesNo(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? answer = Console.ReadLine()?.Trim().ToLower();
                if (string.IsNullOrEmpty(answer) || answer == "n" || answer == "no")
                    return false;
                if (answer == "y" || answer == "yes")
                    return true;

                Console.WriteLine("Please answer 'y' or 'n'.");
            }
        }

        static string PromptText(string prompt, string? defaultValue = null)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                return defaultValue ?? string.Empty;
            return input.Trim();
        }

        static EmailSettings PromptEmailSettings()
        {
            ConsoleEx.WriteHeader("SMTP Email Configuration:");
            string host = PromptText("SMTP host (e.g. smtp.gmail.com): ", "smtp.gmail.com");
            int port = int.TryParse(PromptText("SMTP port (587): ", "587"), out int parsed) ? parsed : 587;
            bool enableSsl = AskYesNo("Enable SSL? (y/n, default y): ");
            string sender = PromptText("From email: ");
            string recipient = PromptText("To email: ");
            string username = PromptText("SMTP username: ");
            string password = PromptText("SMTP password: ");
            Console.WriteLine();

            return new EmailSettings(host, port, sender, recipient, username, password, enableSsl);
        }
    }
}
