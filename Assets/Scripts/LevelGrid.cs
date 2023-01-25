/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/

using System.Collections.Generic;
using UnityEngine;


public class LevelGrid : MonoBehaviour
{

    #region Attributes

    
    [Tooltip("Singleton Pattern's Main Key: Instance of this very Class")] 
    public static LevelGrid Instance { get; private set; }
    
    
    [Tooltip("Visuals of Grid System, for Visual Debugging in the Unity Editor")]
    [SerializeField]
    private Transform _gridDebugObjectPrefab;
    
    
    /// <summary>
    /// The Grid Cells.
    ///
    /// From a Logical point of view.
    /// 
    /// Contains: GridObjects (the Logical Squares/Cells) + GridPositions (the Mathematical Positions and Data: (x, y, z))
    /// </summary>
    private GridSystem _gridSystem;
    
    
    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>
    private void Awake()
    {
        // 1- This class' SINGLETON PATTERN Initialization
        // Singleton Pattern, protocol:
        //
        // Validation: There MUST be ONLY ONE Instance of this Class (i.e.: ONE Object):
        //
        if (Instance != null)
        {
            
            Debug.LogError("There's more than one 'UnitActionSystem'!. GameObject: ---> " + transform + "  - " + Instance);
            //
            // Destroy, to be able to continue the Gameplay (i.e.: Recovery from the Error/Exception...)
            //
            Destroy(gameObject);
            return;
        }
        //
        // If everything went well, create / assign THIS Instance:
        //
        Instance = this;

        
        // 2- Grid System Initialization
        //
        _gridSystem = new GridSystem(10, 10, 2f);
        //
        // Create the GameObject that will hold a Visual Representation of the Grid System. Calling the Constructor:
        //
        _gridSystem.CreateDebugObjects(_gridDebugObjectPrefab);
    }


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>



    /// <summary>
    /// Update is called once per frame
    /// </summary>


    #endregion Unity Methods


    #region My Custom Methods

    /// <summary>
    /// Adds a Unit (to List<Unit> on GridObject Class) to a GridObject, on a certain Position / Location (GridPosition).
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <param name="unit"></param>
    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }
    
    /// <summary>
    /// Get Unit from a Grid Position
    /// </summary>
    /// <param name="gridPosition"></param>
    public List<Unit> GetListOfUnitsAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }

    /// <summary>
    /// Removes the Unit from a given Position (on the Grid...)
    /// </summary>
    /// <param name="gridPosition"></param>
    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit); 
    }

    /// <summary>
    /// Updates the Unit's GridPosition Data, when we have detected that
    /// ..it has previously changed position (i.e.: the Character has moved!)
    /// </summary>
    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        // 1- Clear previous (old) GridPosition
        //
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        
        // 2- Set Unit at the NEW GridPosition:
        //
        AddUnitAtGridPosition(toGridPosition, unit);
    }

    /// <summary>
    /// Gets a Grid Position.
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public GridPosition GetGridPosition(Vector3 worldPosition) => _gridSystem.GetGridPosition(worldPosition);
    //
    // CodeMonkey TIPS:   This is the same as:
    //
    // public GridPosition GetGridPosition(Vector3 worldPosition)
    // {
    //     return _gridSystem.GetGridPosition(worldPosition);
    // }

    /// <summary>
    /// Gets a (Game) World Position.
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public Vector3 GetWorldPosition(GridPosition gridPosition) => _gridSystem.GetWorldPosition(gridPosition);

    /// <summary>
    /// Getter for the GridSystem's exposed Getter for: _width
    /// </summary>
    /// <returns></returns>
    public int GetWidth() => _gridSystem.GetWidth();

    /// <summary>
    /// Getter for the GridSystem's exposed Getter for: _height
    /// </summary>
    /// <returns></returns>
    public int GetHeight() => _gridSystem.GetHeight();
    
    
    /// <summary>
    /// Gets a VALID Grid Position. Validity Criteria: it must be inside the Board, only positive numbers for the Coordinates are allowed.
    /// </summary>
    /// <param name="gridPosition">A test GridPosition struct, to check the validity of that (x, y=0, z) position.</param>
    /// <returns>True or False</returns>
    public bool IsValidGridPosition(GridPosition gridPosition) => _gridSystem.IsValidGridPosition(gridPosition);
    
    
    /// <summary>
    /// Gets a VALID Grid Position.
    /// Validity Criteria: it must NOT have any Unit or Obstacle... or any Object in it. It must be completely empty to be Valid.
    /// </summary>
    /// <param name="gridPosition">A test GridPosition struct, to check the validity of that (x, y=0, z) position.</param>
    /// <returns>True or False</returns>
    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    { 
        // Get a GridObject:
        //
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        //
        // Check whether it is occupied..:
        //
        return gridObject.HasAnyUnit();
    }

    
    /// <summary>
    /// Gets the First 'Unit' (i.e.: _unit[0] of the List of Units),  that is in the input 'Grid Position'.
    /// </summary>
    /// <param name="gridPosition">A GridPosition struct, to check whether there is an 'Unit' there, or not... in that mathematical (x, y=0, z) position.</param>
    /// <returns>True or False</returns>
    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    { 
        // Get a GridObject:
        //
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        //
        // Check whether it is occupied..:
        //
        return gridObject.GetFirstUnit();
    }

    
    #endregion My Custom Methods

}
