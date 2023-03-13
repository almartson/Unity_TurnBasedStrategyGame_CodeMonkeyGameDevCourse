/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using UnityEngine;

/// <summary>
/// ("Pathfinding A.I. System") MANAGER that, being similar to "LevelGrid.cs": <br />
/// 1- Spawns the "Pathfinding GridSystem" (i.e.: the Game Board, for Pathfinding). <br />
/// 2- Modifies the "PathNodes" (i.e.: very similar to: "GridObjects") (i.e.: Cells / Grids in the "Game Board") that are in the "GridSystem" based on "GridPositions". <br />
/// 
/// Reference: Check this project's UML Class Diagram. <br />
/// </summary>
public class Pathfinding : MonoBehaviour
{

    #region Attributes

    [Tooltip("Visuals of Grid System, for Visual Debugging in the Unity Editor")]
    [SerializeField]
    private Transform _gridDebugObjectPrefab;


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


    #region GridSystem, Game Board

    /// <summary>
    /// The Grid Cells, the GAME BOARD.
    ///
    /// From a Logical point of view.
    /// 
    /// Contains: Path Nodes (the Logical Squares/Cells) that contain inside: "GridPositions" (Structs: the Mathematical Positions and Data: (x, y, z))
    /// </summary>
    private GridSystem<PathNode> _gridSystem;
    
    #endregion GridSystem, Game Board
    
    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>
    private void Awake()
    {

        // Create the "GridSystem",  for (A.I.) A* Pathfinding:
        // ...with  "Path Nodes"
        //
        _gridSystem = new GridSystem<PathNode>(LevelGrid.WIDTH_OF_GAME_BOARD_GRID_SYSTEM, LevelGrid.HEIGHT_OF_GAME_BOARD_GRID_SYSTEM, LevelGrid.CELL_SIZE_OF_GAME_BOARD_GRID_SYSTEM,
            (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));


        // Create the GameObject that will hold a Visual Representation of the Grid System. Calling the Constructor:
        //
        _gridSystem.CreateDebugObjects(_gridDebugObjectPrefab);

    }// End Awake


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>



    /// <summary>
    /// Update is called once per frame
    /// </summary>


    #endregion Unity Methods
    

    #region My Custom Methods





    #endregion My Custom Methods

}// End Pathfinding
