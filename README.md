# SpaceMission

This is a simple C# project that simulates a mission to find the shortest path from astronauts to a space station.

## What it does

- Generates a random map or takes manual input from the user
- Uses Dijkstra's algorithm to find the cheapest path
- Works with different symbols for:
  - `O` - open cell
  - `X` - asteroid, impassable
  - `D` - space debris, more expensive cell
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

If you want to try it quickly automatically, use:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File run_demo.ps1
```

## Example for manual input

When choosing mode `1`, the program asks for map dimensions and then row by row:

```
3
3
S1 O F
O D O
X O O
```

## How it works

- `InputParser` reads the input and checks if it's valid.
- `MapGenerator` creates a random map with at least one astronaut and one station.
- `DijkstraPathfinder` finds the shortest path, considering different costs for moving.
- `SpaceMission` runs the search for all astronauts and displays the results.

## Technologies used

- .NET 10
- C#

## Notes

- The project is good for a small demo of algorithms and console input handling.
- Make sure you have .NET SDK installed to run with `dotnet run`.
