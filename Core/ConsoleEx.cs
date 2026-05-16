namespace SpaceMission.Core
{
    using System;

    public static class ConsoleEx
    {
        public static void WriteSymbol(string symbol)
        {
            ConsoleColor color = symbol switch
            {
                Cell.Open => ConsoleColor.Gray,
                Cell.Asteroid => ConsoleColor.DarkGray,
                Cell.Debris => ConsoleColor.Yellow,
                Cell.Station => ConsoleColor.Cyan,
                Cell.Path => ConsoleColor.Magenta,
                _ when symbol.StartsWith("S") => ConsoleColor.Green,
                _ => ConsoleColor.White
            };

            Console.ForegroundColor = color;
            Console.Write(symbol.PadRight(3));
            Console.ResetColor();
        }

        public static void WriteLine(string text, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void WriteHeader(string text)
        {
            var previous = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(text);
            Console.ForegroundColor = previous;
        }
    }
}
