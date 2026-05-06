namespace SpaceMission.Core
{
    
    
    
    
    
    
    
    public class SpaceMission
    {
        private readonly Grid        _grid;
        private readonly IPathfinder _pathfinder;

        public SpaceMission(Grid grid, IPathfinder pathfinder)
        {
            _grid       = grid       ?? throw new ArgumentNullException(nameof(grid));
            _pathfinder = pathfinder ?? throw new ArgumentNullException(nameof(pathfinder));
        }

        public void Run()
        {
            Cell? station = _grid.FindStation();
            if (station == null)
            {
                Console.WriteLine("❌ No Space Station (F) found on the map!");
                return;
            }

            
            var astronauts = _grid
                .FindAstronauts()
                .Select(cell => new Astronaut(cell))
                .ToList();

            if (astronauts.Count == 0)
            {
                Console.WriteLine("❌ No astronauts found on the map!");
                return;
            }

            
            foreach (var astronaut in astronauts)
            {
                var result = _pathfinder.FindPath(_grid, astronaut.StartCell, station);
                astronaut.SetResult(result);
            }

            
            var sorted = astronauts
                .OrderBy(a => a.Result!.Success ? 0 : -1)   
                .ThenBy(a => a.Result!.Success ? a.Result.TotalCost : int.MaxValue)
                .ToList();

            
            var failures  = sorted.Where(a => !a.Result!.Success).ToList();
            var successes = sorted.Where(a =>  a.Result!.Success)
                                  .OrderBy(a => a.Result!.TotalCost)
                                  .ToList();

            
            foreach (var a in failures)
                Console.WriteLine($"Mission failed — Astronaut {a.Id} lost in space!");

            if (failures.Count > 0 && successes.Count > 0)
                Console.WriteLine();

            
            for (int i = 0; i < successes.Count; i++)
            {
                var a = successes[i];
                Console.WriteLine($"Astronaut {a.Id} - Shortest path: {a.Result!.TotalCost} steps");
                _grid.PrintWithPath(a.Result.Path);
                if (i < successes.Count - 1) Console.WriteLine();
            }
        }
    }
}
