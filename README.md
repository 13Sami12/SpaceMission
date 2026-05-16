# SpaceMission

This is a C# console project that simulates a mission to find the shortest path from astronauts to a space station on a cosmic map.

## What it does

- Generates a random map or takes manual input from the user
- Supports both `Dijkstra` and `A*` pathfinding algorithms
- Displays mission output with enhanced console colors for readability
- Supports optional email notifications through SMTP
- Includes unit tests covering parsing, map generation, pathfinding, and email service

## Map symbols

- `O` - open cell
- `X` - asteroid, impassable
- `D` - space debris, higher traversal cost
- `F` - space station
- `S1`, `S2`, `S3` - astronaut starting positions

## How to run

1. Open the project folder in VS Code.
2. Open the terminal.
3. Run:

```powershell
cd C:\Project\SpaceMission
dotnet run
```

4. Choose mode:
   - `1` - manual map input
   - `2` - generate random map
   - `3` - exit

5. Follow the prompts to select the pathfinding algorithm, enter map data, and optionally send an email report.

If you want to try it quickly automatically, use:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File run_demo.ps1
```

## Example manual input

When choosing mode `1`, the program asks for map dimensions and then row by row:

```
3
3
S1 O F
O D O
X O O
```

## New features added

- `AStarPathfinder` with heuristic-driven search
- `EmailService` using SMTP to send mission summaries
- `ConsoleEx` for colored output and better console interactions
- `SpaceMission.Tests` project with xUnit tests

## How it works

- `InputParser` reads and validates console input
- `MapGenerator` creates playable maps and ensures there is one station and at least one astronaut
- `DijkstraPathfinder` and `AStarPathfinder` compute shortest paths
- `SpaceMission` summarizes mission results and prints them on screen

## Testing

Run the unit tests from the `SpaceMission.Tests` project:

```powershell
dotnet test .\SpaceMission.Tests\SpaceMission.Tests.csproj --no-restore
```

## Technologies used

- .NET 10
- C#
- xUnit for unit testing

## Notes

- Ensure .NET SDK is installed to run with `dotnet run`.
- If SMTP is used, configure valid server settings before sending email reports.
