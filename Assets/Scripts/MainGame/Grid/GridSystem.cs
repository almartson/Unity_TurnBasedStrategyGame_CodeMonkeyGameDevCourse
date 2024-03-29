using System;
using UnityEngine;

/// <summary>
/// This is basically the GAME's BOARD (in logic terms). <br />
/// It contains all the (Square) Cells called "GridObject" (with the C# Generic Name Type: TGridObject) in a list-Array: <br /> <code>_gridObjectArray</code> <br />
/// 
/// Reference: Check this project's UML Class Diagram. <br />
/// </summary>
public class GridSystem<TGridObject>
{
    #region Attributes

    /// <summary>
    /// Number of Cells (horizontally), (of the Game Board).
    /// </summary>
    private int _width;
    
    /// <summary>
    /// Number of Cells (vertically), (of the Game Board).
    /// </summary>
    private int _height;
    
    /// <summary>
    /// Size of each Squared Cell (that compounds the Game Board).
    /// </summary>
    private float _cellSize;
    
    private static readonly Color _GRID_LINE_COLOR = Color.white;
    private const float _GRID_LINE_VISIBILITY_DURATION_IN_SECONDS = 1000.0f;
    private const float _HEIGHT_GRID_OFFSET = 0.2f;

    // Array that contains all the Cells/Grids (of the System)
    // It is the Game Board:

    [Tooltip("Array that contains all the Cells/Grids (of the System). \n * It is the Game Board.")]
    private TGridObject[,] _gridObjectArray;
    
    #endregion Attributes


    #region Constructors

    /// <summary>
    /// Main Constructor.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="cellSize"></param>
    /// <param name="createGridObject">Delegate, that works as a Constructor to create the Array of 'GridObject's  (in its GENERIC FORM: "TGridObject"s)</param>
    public GridSystem(int width, int height, float cellSize, /* We Pass a Delegate Here, to work as a Constructor for the "TGridObject's" Object: */ Func<GridSystem<TGridObject>, GridPosition, /* Return TYPE: */ TGridObject > createGridObject )
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;

        // Create the Array that will contain the GRID SYSTEM:
        //
        _gridObjectArray = new TGridObject[_width, _height];
        
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                // Visual Cue: Draw the Grid lines:
                //
                /////Original:   Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z) + Vector3.right * _HEIGHT_GRID_OFFSET, _GRID_LINE_COLOR, _GRID_LINE_VISIBILITY_DURATION_IN_SECONDS);
                
                
                // Visual Cue: Draw the Grid Numbers:
                //
                // Create the GridPosition 
                //
                GridPosition gridPosition = new GridPosition(x, z);
                //
                // Create the TGridObject (which will be in every cell/grid of the GridSystem)
                //
                // Old Implementation, without C# Generics, before implementing  "Pathfinding":   _gridObjectArray[x, z] = new TGridObject(this, gridPosition);
                //
                _gridObjectArray[x, z] = createGridObject(this, gridPosition);

            }//End for 2
        }//End for 1
        
    }//End Constructor

    #endregion Constructors

    
    #region Methods

    /// <summary>
    /// Converts from Grid Coordinates to Game World Coordinates. <br />
    /// TODO: Fix: Don't return a Vector3, use  an out Vector3 Input parameter and be a Procedure instead of a Function (for Performance)
    /// </summary>
    /// <param name="x">Horizontal Coordinate</param>
    /// <param name="z">Vertical/Forward Coordinate</param>
    /// <returns></returns>
    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * _cellSize;
    }


    /// <summary>
    /// Converts from (Game) World Coordinates to Grid Coordinates. <br />
    /// TODO: Fix: Don't return a 'new GridPosition', use an existing struct - Object: GridPosition... and set its parameters. (for Performance)
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x / _cellSize),
            Mathf.RoundToInt(worldPosition.z / _cellSize)
        );
    }
    
    
    /// <summary>
    /// Converts from (Game) World Coordinates to Grid Coordinates. <br />
    /// Note: this is an Optimized version 1.1
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public void GetGridPosition(ref GridPosition gridPosition, Vector3 worldPosition)
    {
        gridPosition.SetXZ(
            Mathf.RoundToInt(worldPosition.x / _cellSize),
            Mathf.RoundToInt(worldPosition.z / _cellSize)
        );
    }


    /// <summary>
    /// Visual representation for Debugging the Grid System and its Array.
    /// </summary>
    /// <param name="debugPrefab"></param>
    public void CreateDebugObjects(Transform debugPrefab)
    {
        // Create a parent GameObject for organizing the multiple GameObjects that will be instantiated later...
        //
        GameObject parentCell = new GameObject("Pathfinding_GridCells_ForDebugging_(x,y,z)Nodes_G,H,F_Pathfinding");
        
        
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                
                // Create a Mathematical Position (in the Grid System)
                //
                GridPosition gridPosition = new GridPosition(x, z);
                
                // Visual Cue:  Instantiate the Visual Prefab:  the GameObject that has
                //...(i.e.:...is associated with...) the Transform: 'debugPrefab'
                //
                Transform myGridCellTransformForDebug = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity, parentCell.transform);
                //
                // Get a Cell Object, for Debug
                //..(GridDebugObject contains a particular 'TGridObject'):
                //
                GridDebugObject gridDebugObject = myGridCellTransformForDebug.GetComponent<GridDebugObject>();
                //
                // Set the whole numbering of the Cells / Grid in the whole Grid System
                //..(NOTE: the best approach is to use a Method, and not to paste the code here):
                //
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));

                // Set & update the Numbers of each Cell/Grid: (x, y, z)
                //...this is done in the Start() Method of: 'GridDebugObject' 
                /////Mio: myGridCellTransformForDebug.GetComponentInChildren<TMP_Text>().text = $"{x}; {z}";

            }//End for 2
            
        }//End for 1
    }// End CreateDebugObjects


    /// <summary>
    /// Gets a particular: Cell / Grid... taken from the ARRAY <code>_gridObjectArray</code>
    /// </summary>
    /// <returns>Gets a particular: Cell / Grid: <code>TGridObject</code>. Taken from the ARRAY OF TGridObject <code>_gridObjectArray</code></returns>
    public TGridObject GetGridObject(GridPosition gridPosition)
    {
        return _gridObjectArray[gridPosition.x, gridPosition.z];
    }

    /// <summary>
    /// Gets the Board's (i.e.: Grid Objects of the GridSystem): Width.
    /// </summary>
    /// <returns></returns>
    public int GetWidth()
    {
        return _width;
    }
    
    /// <summary>
    /// Gets the Board's (i.e.: Grid Objects of the GridSystem): Height.
    /// </summary>
    /// <returns></returns>
    public int GetHeight()
    {
        return _height;
    }
    
    
    #region Validation of Movement towards any GridPositions
    
    /// <summary>
    /// Validates a GridPosition. Criteria: it should not be outside (off-limits) of the GridSystem itself. We do not allow negative numbers for any coordinate, for example:<code>(x, y, z) = (-1, 0, -1)</code>
    /// </summary>
    /// <returns>TRUE or FALSE, depending on whether the GridPosition is VALID or not (could be off-limits... outside the Grid System).</returns>
    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return (
            gridPosition.x >= 0 && gridPosition.z >= 0 &&
            gridPosition.x < _width && gridPosition.z < _height
        );
    }
    
    
    #endregion #region Validation of Movement towards any GridPositions
    
    
    #endregion Methods
}
