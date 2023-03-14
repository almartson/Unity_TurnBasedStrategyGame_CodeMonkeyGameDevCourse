using System;

/// <summary>
/// Entity that references "Logically" (in terms of Business Logic)
///...a (Pathfinding...) 'NODE'  ( "GridObject" ): <br /> <br />
/// Cell / Grid in the "Game Board" (i.e.: 'Grid System', but only for PATHFINDING). <br /> <br />
/// It contains GameObjects, and Items in a Cell, and also
///...a Mathematical Position ((x, z), similar to Vector2 i.e. (x, y)...) in the Struct called: "GridPosition" <br />
/// 
/// Reference: Check this project's UML Class Diagram. <br />
/// </summary>
public class PathNode
{

    #region Attributes

    /// <summary>
    /// Mathematical (x, z) Position this Object belongs to...
    /// </summary>
    private GridPosition _gridPosition;

    
    #region Pathfinding variables (for its computing)

    /// <summary>
    /// <code>G</code> <br />
    /// 
    /// Walking "Cost" from the Start Node.
    /// </summary>
    private int _gCost;

    /// <summary>
    /// <code>H</code> <br />
    /// 
    /// Heuristic "Cost" to reach the "End Node", (from the current "Node"), assuming there are no Walls or Obstacles (i.e.: it can be seen as the simplest or straight or narrow path towards: the "End Node").
    /// </summary>
    private int _hCost;

    /// <summary>
    /// <code>F = G + H</code> <br />
    /// 
    /// The Cost of the "Optimal Route". <br />
    /// We are trying to go through the PATH WHERE = F is the Lowest in each NEXT NODE... <br />
    /// 1. Select the **NEXT NODE** based on: <br />
    ///   1. **F** always has to be the **smallest option**. <br />
    /// 2. **G** always **Increases**  (in each "NEXT NODE"). <br />
    /// 3. **H** always **Decreases**  (in each "NEXT NODE"). <br />
    /// </summary>
    private int _fCost;
    
    /// <summary>
    /// After finishing calculating the "Best Route", in the end, we have to "go back" from the "End Node" to the "Start Node". <br />
    ///
    /// That's what this attribute is for. <br />
    /// </summary>
    private PathNode _cameFromPathNode;

    
    #endregion Pathfinding variables (for its computing)
    
    #endregion Attributes


    #region Constructors

    /// <summary>
    /// Main Constructor
    /// </summary>
    /// <param name="gridPosition"></param>
    public PathNode(GridPosition gridPosition)
    {
        // Set the  gridPosition
        //
        _gridPosition = gridPosition ;

    }// End Constructor

    #endregion Constructors


    #region My Custom Methods

    #region Misc - Utils

    /// <summary>
    /// Override to return the values: x , z.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        // Print out the  _gridPosition
        //
        return _gridPosition.ToString();
    }


    #endregion Misc - Utils

    
    #region Getters and Setters

    public int GetGCost() => _gCost;
    
    public int GetHCost() => _hCost;
    
    public int GetFCost() => _fCost;

    #endregion Getters and Setters


    #endregion My Custom Methods

}
