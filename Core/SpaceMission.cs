namespace SpaceMission.Core
{
    
    
    
    
    
    
    
    public class SpaceMission
    {
        private readonly Grid        _grid;
        private readonly IPathfinder _pathfinder;
        private List<Astronaut>?     _astronauts;
        private string               _summary = string.Empty;

        public SpaceMission(Grid grid, IPathfinder pathfinder)
        {
            _grid       = grid       ?? throw new ArgumentNullException(nameof(grid));
            _pathfinder = pathfinder ?? throw new ArgumentNullException(nameof(pathfinder));
        }

        public string GetSummary() => _summary;

        public void Run()
        {
            Cell? station = _grid.FindStation();
            if (station == null)
            {
                ConsoleEx.WriteLine("❌ No Space Station (F) found on the map!", ConsoleColor.Red);
                _summary = "Mission failed: no space station found.";
                return;
            }

            _astronauts = _grid
                .FindAstronauts()
                .Select(cell => new Astronaut(cell))
                .ToList();

            if (_astronauts.Count == 0)
            {
                ConsoleEx.WriteLine("❌ No astronauts found on the map!", ConsoleColor.Red);
                _summary = "Mission failed: no astronauts found.";
                return;
            }

            foreach (var astronaut in _astronauts)
            {
                var result = _pathfinder.FindPath(_grid, astronaut.StartCell, station);
                astronaut.SetResult(result);
            }

            var sorted = _astronauts
                .OrderBy(a => a.Result!.Success ? 0 : -1)
                .ThenBy(a => a.Result!.Success ? a.Result.TotalCost : int.MaxValue)
                .ToList();

            var failures  = sorted.Where(a => !a.Result!.Success).ToList();
            var successes = sorted.Where(a =>  a.Result!.Success)
                                  .OrderBy(a => a.Result!.TotalCost)
                                  .ToList();

            var builder = new System.Text.StringBuilder();

            foreach (var a in failures)
            {
                ConsoleEx.WriteLine($"Mission failed — Astronaut {a.Id} lost in space!", ConsoleColor.Red);
                builder.AppendLine($"Mission failed — Astronaut {a.Id} lost in space!");
            }

            if (failures.Count > 0 && successes.Count > 0)
            {
                Console.WriteLine();
                builder.AppendLine();
            }

            for (int i = 0; i < successes.Count; i++)
            {
                var a = successes[i];
                string heading = $"Astronaut {a.Id} - Shortest path: {a.Result!.TotalCost} steps";
                ConsoleEx.WriteLine(heading, ConsoleColor.Green);
                builder.AppendLine(heading);
                builder.AppendLine(_grid.RenderTextWithPath(a.Result.Path));
                if (i < successes.Count - 1)
                {
                    Console.WriteLine();
                    builder.AppendLine();
                }
            }

            if (successes.Count == 0 && failures.Count > 0)
            {
                builder.AppendLine("No successful missions.");
            }

            _summary = builder.ToString();
        }
    }
}
