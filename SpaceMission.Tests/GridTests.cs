using System;
using System.Linq;
using SpaceMission.Core;
using Xunit;

namespace SpaceMission.Tests;

public class GridTests
{
    [Fact]
    public void FindAstronautsAndStation_ReturnsCorrectCells()
    {
        var cells = new Cell[2, 3]
        {
            { new Cell("S1", 0, 0), new Cell("O", 0, 1), new Cell("F", 0, 2) },
            { new Cell("D", 1, 0), new Cell("X", 1, 1), new Cell("O", 1, 2) }
        };

        var grid = new Grid(cells);
        Assert.Single(grid.FindAstronauts());
        Assert.Equal("S1", grid.FindAstronauts().First().Symbol);
        Assert.Equal("F", grid.FindStation()!.Symbol);
    }

    [Fact]
    public void GetNeighbours_OnlyReturnsPassableCells()
    {
        var cells = new Cell[3, 3];
        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 3; c++)
                cells[r, c] = new Cell("O", r, c);

        cells[1, 1] = new Cell("O", 1, 1);
        cells[0, 1] = new Cell("X", 0, 1);

        var grid = new Grid(cells);
        var neighbours = grid.GetNeighbours(1, 1).ToList();

        Assert.DoesNotContain(neighbours, cell => cell.Symbol == "X");
        Assert.Equal(3, neighbours.Count);
    }
}
