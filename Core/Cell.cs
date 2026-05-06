namespace SpaceMission.Core
{
    
    
    
    
    public class Cell
    {
        
        public const string Open     = "O";
        public const string Asteroid = "X";
        public const string Debris   = "D";
        public const string Station  = "F";
        public const string Path     = "*";

        public string Symbol { get; }
        public int Row { get; }
        public int Col { get; }

        
        
        
        
        
        
        public int MoveCost => Symbol switch
        {
            Asteroid => int.MaxValue,   
            Debris   => 2,              
            _        => 1               
        };

        public bool IsPassable => Symbol != Asteroid;

        
        public bool IsAstronaut => Symbol.Length == 2
                                   && Symbol[0] == 'S'
                                   && Symbol[1] >= '1'
                                   && Symbol[1] <= '3';

        public Cell(string symbol, int row, int col)
        {
            Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
            Row    = row;
            Col    = col;
        }

        public override string ToString() => Symbol;
    }
}
