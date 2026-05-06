namespace SpaceMission.Core
{
    
    
    
    
    public static class InputParser
    {
        public static Grid ReadGridFromConsole()
        {
            int rows = ReadInt("Map rows: ", 2, 100);
            int cols = ReadInt("Map columns: ", 2, 100);

            Console.WriteLine("Cosmic map (enter each row):");

            var cells = new Cell[rows, cols];
            var validSymbols = new HashSet<string>
                { "O", "X", "F", "D", "S1", "S2", "S3" };

            for (int r = 0; r < rows; r++)
            {
                string[]? tokens = null;
                while (tokens == null)
                {
                    Console.Write($"  Row {r + 1}: ");
                    string? line = Console.ReadLine();

                    if (line == null)
                        throw new InvalidOperationException("Unexpected end of input.");

                    tokens = line.Trim()
                                 .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    if (tokens.Length != cols)
                    {
                        Console.WriteLine($"  ⚠ Expected {cols} symbols, got {tokens.Length}. Try again.");
                        tokens = null;
                        continue;
                    }

                    
                    bool valid = true;
                    for (int c = 0; c < cols; c++)
                    {
                        string sym = tokens[c].ToUpper();
                        tokens[c] = sym;
                        if (!validSymbols.Contains(sym))
                        {
                            Console.WriteLine($"  ⚠ Unknown symbol '{tokens[c]}'. Valid: O X F D S1 S2 S3. Try again.");
                            valid = false;
                            break;
                        }
                    }
                    if (!valid) tokens = null;
                }

                for (int c = 0; c < cols; c++)
                    cells[r, c] = new Cell(tokens[c], r, c);
            }

            ValidateMap(cells, rows, cols);
            return new Grid(cells);
        }

        private static void ValidateMap(Cell[,] cells, int rows, int cols)
        {
            int stationCount = 0;
            var astronautIds = new HashSet<string>();

            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                {
                    if (cells[r, c].Symbol == Cell.Station)
                        stationCount++;

                    if (cells[r, c].IsAstronaut)
                    {
                        if (!astronautIds.Add(cells[r, c].Symbol))
                            throw new InvalidDataException($"Duplicate astronaut '{cells[r, c].Symbol}' found.");
                    }
                }

            if (stationCount != 1)
                throw new InvalidDataException("Map must contain exactly one Space Station (F).");

            if (astronautIds.Count == 0)
                throw new InvalidDataException("Map must contain at least one astronaut (S1, S2 or S3).");

            if (astronautIds.Count > 3)
                throw new InvalidDataException("Map may contain at most three astronauts (S1, S2, S3).");
        }

        
        public static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (int.TryParse(input, out int value) && value >= min && value <= max)
                    return value;
                Console.WriteLine($"  ⚠ Please enter an integer between {min} and {max}.");
            }
        }
    }
}
