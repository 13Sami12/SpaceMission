using System;
using System.Linq;
using SpaceMission.Core;
using Xunit;

namespace SpaceMission.Tests;

public class SpaceMissionTests
{
    [Fact]
    public void Run_ReturnsSummaryWithSuccessfulPath()
    {
        var cells = new Cell[2, 3]
        {
            { new Cell("S1", 0, 0), new Cell("O", 0, 1), new Cell("F", 0, 2) },
            { new Cell("O", 1, 0), new Cell("O", 1, 1), new Cell("O", 1, 2) }
        };
        var grid = new Grid(cells);
        var mission = new SpaceMission.Core.SpaceMission(grid, new AStarPathfinder());

        mission.Run();
        string summary = mission.GetSummary();

        Assert.Contains("Astronaut S1", summary);
        Assert.Contains("Shortest path", summary);
    }
}
