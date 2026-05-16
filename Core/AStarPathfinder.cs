namespace SpaceMission.Core
{
    using System;
    using System.Collections.Generic;

    public class AStarPathfinder : IPathfinder
    {
        public PathResult FindPath(Grid grid, Cell start, Cell goal)
        {
            int rows = grid.Rows;
            int cols = grid.Cols;

            var gScore = new int[rows, cols];
            var prev = new (int r, int c)?[rows, cols];

            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    gScore[r, c] = int.MaxValue;

            gScore[start.Row, start.Col] = 0;

            var openSet = new PriorityQueue<(int r, int c, int g), int>();
            openSet.Enqueue((start.Row, start.Col, 0), Heuristic(start, goal));

            while (openSet.Count > 0)
            {
                var (cr, cc, currentG) = openSet.Dequeue();

                if (currentG > gScore[cr, cc])
                    continue;

                if (cr == goal.Row && cc == goal.Col)
                    break;

                foreach (var neighbour in grid.GetNeighbours(cr, cc))
                {
                    int tentativeG = currentG + neighbour.MoveCost;
                    if (tentativeG < gScore[neighbour.Row, neighbour.Col])
                    {
                        gScore[neighbour.Row, neighbour.Col] = tentativeG;
                        prev[neighbour.Row, neighbour.Col] = (cr, cc);
                        int fScore = tentativeG + Heuristic(neighbour, goal);
                        openSet.Enqueue((neighbour.Row, neighbour.Col, tentativeG), fScore);
                    }
                }
            }

            if (gScore[goal.Row, goal.Col] == int.MaxValue)
                return PathResult.Failed();

            var path = ReconstructPath(prev, start, goal);
            return PathResult.Succeeded(gScore[goal.Row, goal.Col], path);
        }

        private static int Heuristic(Cell a, Cell b)
            => Math.Abs(a.Row - b.Row) + Math.Abs(a.Col - b.Col);

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
