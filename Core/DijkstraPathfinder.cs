namespace SpaceMission.Core
{
    
    
    
    
    
    
    
    
    
    
    
    
    public class DijkstraPathfinder : IPathfinder
    {
        public PathResult FindPath(Grid grid, Cell start, Cell goal)
        {
            int rows = grid.Rows;
            int cols = grid.Cols;

            
            var dist = new int[rows, cols];
            
            var prev = new (int r, int c)?[rows, cols];

            
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    dist[r, c] = int.MaxValue;

            dist[start.Row, start.Col] = 0;

            
            
            var pq = new PriorityQueue<(int r, int c, int cost), int>();
            pq.Enqueue((start.Row, start.Col, 0), 0);

            while (pq.Count > 0)
            {
                var (cr, cc, currentDist) = pq.Dequeue();

                
                if (currentDist > dist[cr, cc])
                    continue;

                
                if (cr == goal.Row && cc == goal.Col)
                    break;

                foreach (var neighbour in grid.GetNeighbours(cr, cc))
                {
                    
                    int newDist = currentDist + neighbour.MoveCost;

                    if (newDist < dist[neighbour.Row, neighbour.Col])
                    {
                        dist[neighbour.Row, neighbour.Col] = newDist;
                        prev[neighbour.Row, neighbour.Col] = (cr, cc);
                        pq.Enqueue((neighbour.Row, neighbour.Col, newDist), newDist);
                    }
                }
            }

            
            if (dist[goal.Row, goal.Col] == int.MaxValue)
                return PathResult.Failed();

            
            var path = ReconstructPath(prev, start, goal);
            return PathResult.Succeeded(dist[goal.Row, goal.Col], path);
        }

        private static List<(int, int)> ReconstructPath(
            (int r, int c)?[,] prev,
            Cell start,
            Cell goal)
        {
            var path = new List<(int, int)>();
            (int r, int c)? current = (goal.Row, goal.Col);

            while (current.HasValue)
            {
                path.Add(current.Value);
                current = prev[current.Value.r, current.Value.c];
            }

            path.Reverse();   
            return path;
        }
    }
}
