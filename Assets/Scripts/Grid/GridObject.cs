using UnityEngine;

/// <summary>
/// Entity that references a Cell / Grid in the 'Grid System'.
/// It contains GameObjects, and Items in a Cell, and also
///...a Mathematical Position (x, z) in the Struct called: GridPosition
/// </summary>
public class GridObject
{
    #region Attributes

    
    [Tooltip("System that created this Object")]
    private GridSystem _gridSystem;
    
    [Tooltip("Position this Object belongs to...")]
    private GridPosition _gridPosition;

    
    #endregion Attributes


    #region Constructors

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        _gridSystem = gridSystem;
        _gridPosition = gridPosition;
    }

    #endregion Constructors

    
    #region Methods
    
    
    /// <summary>
    /// Override to return the values: x , z.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return _gridPosition.ToString();
    }
    
    #endregion Methods
}
