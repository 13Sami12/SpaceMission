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
            var mission = new Core.SpaceMission(grid, new DijkstraPathfinder());
            mission.Run();
            Console.WriteLine();
        }
    }
}
