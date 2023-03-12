using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Entity that references "Logically" (in terms of Business Logic)
///...a Cell / Grid in the "Game Board":  a.k.a.:   'Grid System'. <br />
/// It contains GameObjects, and Items in a Cell, and also
///...a Mathematical Position ((x, z), similar to Vector2 i.e. (x, y)...) in the Struct called: "GridPosition" <br />
/// Reference: Check this project's UML Class Diagram. <br />
/// </summary>
public class GridObject
{
    #region Attributes

    
    [Tooltip("System that created this Object. This System is the Game Board, with its Logic.")]
    private GridSystem<GridObject> _gridSystem;
    
    [Tooltip("Mathematical (x, z) Position this Object belongs to...")]
    private GridPosition _gridPosition;

    [Tooltip("Unit/Game Characters, this Object contains...")]
    private List<Unit> _unitList;

    
    #endregion Attributes


    #region Constructors

    /// <summary>
    /// Main Constructor
    /// </summary>
    /// <param name="gridSystem"></param>
    /// <param name="gridPosition"></param>
    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
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
    
    
    #region Validations for TakeAction Actions on each GridObject

    /// <summary>
    /// Validation: Is this GridObject occupied by any Unit / Character?  
    /// </summary>
    /// <returns></returns>
    public bool HasAnyUnit()
    {
        return _unitList.Count > 0;
    }
    
    #endregion Validations for TakeAction Actions on each GridObject
    
    
    /// <summary>
    /// Gets the first Unit that is in the (selected) Grid Object.
    /// Usually (99.99% of the cases...)  there will be only ONE Unit / Character there, because it is a RULE of the GAME. 
    /// </summary>
    /// <returns>null ... OR ... The first Unit of the List, that is in this SPOT / GRID / CELL</returns>
    public Unit GetFirstUnit()
    {
        if (HasAnyUnit())
        {
            // Return the FIRST element (i.e.: Unit) of the List<Unit>
            //
            return _unitList[0];
        }
        else
        {
            return null;
        }
    }//End GetFirstUnit

    #endregion Methods
}
