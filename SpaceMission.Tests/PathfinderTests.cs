using System;
using System.Linq;
using SpaceMission.Core;
using Xunit;

namespace SpaceMission.Tests;

public class PathfinderTests
{
    private static Grid BuildSimpleGrid()
    {
        var cells = new Cell[3, 3]
        {
            { new Cell("S1", 0, 0), new Cell("O", 0, 1), new Cell("F", 0, 2) },
            { new Cell("O", 1, 0), new Cell("D", 1, 1), new Cell("O", 1, 2) },
            { new Cell("O", 2, 0), new Cell("O", 2, 1), new Cell("O", 2, 2) }
        };
        return new Grid(cells);
    }

    [Fact]
    public void DijkstraAndAStar_ReturnSameCostAndPath()
    {
        var grid = BuildSimpleGrid();
        var start = grid.FindAstronauts().First();
        var goal = grid.FindStation()!;

        var dijkstra = new DijkstraPathfinder().FindPath(grid, start, goal);
        var astar = new AStarPathfinder().FindPath(grid, start, goal);

        Assert.True(dijkstra.Success);
        Assert.True(astar.Success);
        Assert.Equal(dijkstra.TotalCost, astar.TotalCost);
        Assert.Equal(dijkstra.Path, astar.Path);
    }

    [Fact]
    public void AStar_ReturnsFailed_ForBlockedGoal()
    {
        var cells = new Cell[2, 2]
        {
            { new Cell("S1", 0, 0), new Cell("X", 0, 1) },
            { new Cell("X", 1, 0), new Cell("F", 1, 1) }
        };
        var grid = new Grid(cells);
        var result = new AStarPathfinder().FindPath(grid, grid.FindAstronauts().First(), grid.FindStation()!);

        Assert.False(result.Success);
        Assert.Empty(result.Path);
    }
}
