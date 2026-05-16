namespace SpaceMission.Core
{
    
    
    
    
    public class Grid
    {
        private readonly Cell[,] _cells;

        public int Rows { get; }
        public int Cols { get; }

        
        private static readonly (int dr, int dc)[] Directions =
            { (-1, 0), (1, 0), (0, -1), (0, 1) };

        public Cell this[int row, int col] => _cells[row, col];

        public Grid(Cell[,] cells)
        {
            _cells = cells ?? throw new ArgumentNullException(nameof(cells));
            Rows   = cells.GetLength(0);
            Cols   = cells.GetLength(1);
        }

        
        public IEnumerable<Cell> GetNeighbours(int row, int col)
        {
            foreach (var (dr, dc) in Directions)
            {
                int nr = row + dr;
                int nc = col + dc;
                if (nr >= 0 && nr < Rows && nc >= 0 && nc < Cols)
                {
                    var cell = _cells[nr, nc];
                    if (cell.IsPassable)
                        yield return cell;
                }
            }
        }

        
        
        
        public IEnumerable<Cell> FindAstronauts()
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Cols; c++)
                    if (_cells[r, c].IsAstronaut)
                        yield return _cells[r, c];
        }

        
        public Cell? FindStation()
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Cols; c++)
                    if (_cells[r, c].Symbol == Cell.Station)
                        return _cells[r, c];
            return null;
        }

        
        
        
        
        public void PrintWithPath(IReadOnlyList<(int row, int col)> path)
        {
            var pathSet = new HashSet<(int, int)>(path.Skip(1).SkipLast(1));

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    string symbol = pathSet.Contains((r, c))
                        ? Cell.Path
                        : _cells[r, c].Symbol;

                    ConsoleEx.WriteSymbol(symbol);
                }
                Console.WriteLine();
            }
        }

        public void Print()
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                    ConsoleEx.WriteSymbol(_cells[r, c].Symbol);
                Console.WriteLine();
            }
        }

        public string RenderTextWithPath(IReadOnlyList<(int row, int col)> path)
        {
            var pathSet = new HashSet<(int, int)>(path.Skip(1).SkipLast(1));
            var builder = new System.Text.StringBuilder();

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    string symbol = pathSet.Contains((r, c))
                        ? Cell.Path
                        : _cells[r, c].Symbol;
                    builder.Append(symbol.PadRight(3));
                }
                builder.AppendLine();
            }

            return builder.ToString();
        }

        public string RenderText()
        {
            var builder = new System.Text.StringBuilder();
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                    builder.Append(_cells[r, c].Symbol.PadRight(3));
                builder.AppendLine();
            }
            return builder.ToString();
        }
    }
}
