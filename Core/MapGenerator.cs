namespace SpaceMission.Core
{
    
    
    
    
    
    
    
    public static class MapGenerator
    {
        private const int MaxAttempts = 50;

        public static Grid GenerateRandom()
        {
            Console.WriteLine("── Random Map Generator ──");
            int rows = InputParser.ReadInt("Rows (2-20): ", 2, 20);
            int cols = InputParser.ReadInt("Columns (2-20): ", 2, 20);

            int maxAsteroids = (rows * cols) / 3;   
            int asteroidCount = InputParser.ReadInt(
                $"Number of asteroids (0-{maxAsteroids}): ", 0, maxAsteroids);

            Console.WriteLine("Include Space Debris 'D'? (y/n): ");
            bool includeDebris = Console.ReadLine()?.Trim().ToLower() == "y";

            int astronautCount = InputParser.ReadInt("Number of astronauts (1-3): ", 1, 3);

            var rng = new Random();

            for (int attempt = 0; attempt < MaxAttempts; attempt++)
            {
                var grid = TryGenerate(rng, rows, cols, asteroidCount,
                                       includeDebris, astronautCount);
                if (grid != null)
                    return grid;
            }

            throw new InvalidOperationException(
                "Could not generate a solvable map after many attempts. " +
                "Try fewer asteroids or a larger map.");
        }

        private static Grid? TryGenerate(
            Random rng, int rows, int cols,
            int asteroidCount, bool includeDebris, int astronautCount)
        {
            int total = rows * cols;

            
            int required = astronautCount + 1;   
            if (required + asteroidCount > total)
                return null;

            
            var positions = Enumerable.Range(0, total).OrderBy(_ => rng.Next()).ToList();

            var cells = new Cell[rows, cols];

            int idx = 0;

            
            string[] astronautIds = { "S1", "S2", "S3" };
            for (int i = 0; i < astronautCount; i++)
            {
                int pos = positions[idx++];
                cells[pos / cols, pos % cols] = new Cell(astronautIds[i], pos / cols, pos % cols);
            }

            
            {
                int pos = positions[idx++];
                cells[pos / cols, pos % cols] = new Cell(Cell.Station, pos / cols, pos % cols);
            }

            
            for (int i = 0; i < asteroidCount; i++)
            {
                int pos = positions[idx++];
                cells[pos / cols, pos % cols] = new Cell(Cell.Asteroid, pos / cols, pos % cols);
            }

            
            for (; idx < total; idx++)
            {
                int pos = positions[idx];
                int r = pos / cols, c = pos % cols;
                if (cells[r, c] == null!)
                {
                    string sym = (includeDebris && rng.Next(5) == 0) ? Cell.Debris : Cell.Open;
                    cells[r, c] = new Cell(sym, r, c);
                }
            }

            var grid = new Grid(cells);

            
            var pathfinder = new DijkstraPathfinder();
            var astronauts = grid.FindAstronauts().ToList();
            var station = grid.FindStation()!;

            foreach (var astronaut in astronauts)
            {
                var result = pathfinder.FindPath(grid, astronaut, station);
                if (!result.Success)
                    return null;
            }

            return grid;
        }
    }
}
