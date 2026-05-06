namespace SpaceMission.Core
{
    
    
    
    public class PathResult
    {
        public bool Success { get; }
        public int TotalCost { get; }

        
        
        
        
        public IReadOnlyList<(int row, int col)> Path { get; }

        public static PathResult Failed() => new(false, -1, Array.Empty<(int, int)>());
        public static PathResult Succeeded(int cost, IReadOnlyList<(int, int)> path)
            => new(true, cost, path);

        private PathResult(bool success, int cost, IReadOnlyList<(int, int)> path)
        {
            Success   = success;
            TotalCost = cost;
            Path      = path;
        }
    }

    
    
    
    public class Astronaut
    {
        public string Id { get; }         
        public Cell   StartCell { get; }
        public PathResult? Result { get; private set; }

        public Astronaut(Cell startCell)
        {
            StartCell = startCell ?? throw new ArgumentNullException(nameof(startCell));
            Id        = startCell.Symbol;
        }

        public void SetResult(PathResult result)
        {
            Result = result ?? throw new ArgumentNullException(nameof(result));
        }

        public override string ToString() => Id;
    }
}
