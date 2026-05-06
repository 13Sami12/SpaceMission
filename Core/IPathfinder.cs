namespace SpaceMission.Core
{
    
    
    
    
    
    public interface IPathfinder
    {
        
        
        
        
        PathResult FindPath(Grid grid, Cell start, Cell goal);
    }
}
