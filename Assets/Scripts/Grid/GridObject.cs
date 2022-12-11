using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Entity that references "Logically" (in terms of Business Logic)
///...a Cell / Grid in the 'Grid System'.
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

    [Tooltip("Unit/Game Characters, this Object contains...")]
    private List<Unit> _unitList;

    
    #endregion Attributes


    #region Constructors

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        _gridSystem = gridSystem;
        _gridPosition = gridPosition;
        _unitList = new List<Unit>();
    }

    #endregion Constructors

    
    #region Methods
    
    
    /// <summary>
    /// Override to return the values: x , z.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        string unitString = "";

        int unitListLength = _unitList.Count;
        
        // Fill the String with the Data
        //
        for (int i = 0; i < unitListLength; i++)
        {
            unitString += _unitList[i] + "\n";
        }
        
        return _gridPosition.ToString() + "\n" + unitString;
    }


    /// <summary>
    /// Add a new Unit to the List.
    /// </summary>
    /// <param name="unit">This new unit must be correct, validated from the outside
    /// ...and passed to this Method as Input.</param>
    public void AddUnit(Unit unit)
    {
        _unitList.Add(unit);
    }

    /// <summary>
    /// Removes one Unit from the List.
    /// </summary>
    /// <param name="unit"></param>
    public void RemoveUnit(Unit unit)
    {
        _unitList.Remove(unit);
    }
    
    /// <summary>
    /// Get the List (of Units).
    /// </summary>
    /// <returns></returns>
    public List<Unit> GetUnitList()
    {
        return _unitList;
    }
    
    
    #region Validations for Move Actions on each GridObject

    /// <summary>
    /// Validation: Is this GridObject occupied by any Unit / Character?  
    /// </summary>
    /// <returns></returns>
    public bool HasAnyUnit()
    {
        return _unitList.Count > 0;
    }
    
    #endregion Validations for Move Actions on each GridObject
    
    #endregion Methods
}
