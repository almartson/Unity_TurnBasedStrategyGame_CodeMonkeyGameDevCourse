/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/

using System;
using System.Collections.Generic;
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

    #region Singleton Pattern's
    
    [Tooltip("Singleton Pattern's Main Key: Instance of this very Class")] 
    public static Pathfinding Instance { get; private set; }

    #endregion Singleton Pattern's

    
    #region Constants
    
    // Todo: To Move these Constants to:  a Scriptable Object for these Type of CONSTANTS (GridPosition, Game Grid Board's...etc)
    /// <summary>
    /// Math position of a Movement to another 'GridPosition':  Forwards
    /// ...(Cache: for Performance reasons)
    /// </summary>
    public static readonly GridPosition _A_FORWARDS_GRID_POSITION = new GridPosition(0, 1);
    /// <summary>
    /// Math position of a Movement to another 'GridPosition':  Backwards
    /// ...(Cache: for Performance reasons)
    /// </summary>
    public static readonly GridPosition _A_BACKWARDS_GRID_POSITION = new GridPosition(0, -1);
    /// <summary>
    /// Math position of a Movement to another 'GridPosition':  Rightwards
    /// ...(Cache: for Performance reasons)
    /// </summary>
    public static readonly GridPosition _A_RIGHTWARDS_GRID_POSITION = new GridPosition(1, 0);
    /// <summary>
    /// Math position of a Movement to another 'GridPosition':  Leftwards
    /// ...(Cache: for Performance reasons)
    /// </summary>
    public static readonly GridPosition _A_LEFTWARDS_GRID_POSITION = new GridPosition(-1, 0);

    #endregion Constants

    #region COST Constants, for Computing G, H, F

    /// <summary>
    /// (G, H, F) Cost of Walking from a NODE to -> the NEXT in a STRAIGHT Path, in a  <br /> <br />
    /// 'Straight/Narrow Line'  (Horizontal or Vertical... not Diagonal).
    /// </summary>
    private const int _MOVE_STRAIGHT_COST = 10;

    /// <summary>
    /// (G, H, F) Cost of Walking from a NODE to -> the NEXT in a DIAGONAL Path, in a <br /> <br />
    /// 'Diagonal' Line  (JUST Diagonals... NOT Horizontals NOR Vertical lines).
    /// </summary>
    private const int _MOVE_DIAGONAL_COST = 14;
    
    #endregion COST Constants, for Computing G, H, F
    

    #region GridSystem, Game Board

    /// <summary>
    /// The System of:  NODES (Pathfinding), the Grid Cells, the 'GAME BOARD' itself. <br /> <br />
    ///
    /// From a Logical point of view. <br /> <br />
    /// 
    /// Contains: Path Nodes (the Logical Squares/Cells) that contain inside: "GridPositions" (Structs: the Mathematical Positions and Data: (x, y, z)).
    /// </summary>
    private GridSystem<PathNode> _gridSystem;
    
    
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

    #endregion GridSystem, Game Board
    
    
    #region Obstacles for Pathfinding

    [Tooltip("Obstacles Label-LayerMask for Pathfinding")]
    [SerializeField]
    private LayerMask _obstaclesLayerMask;
    //
    /// <summary>
    /// Getter and Setter Property for Field: _obstaclesLayerMask
    /// </summary>
    public LayerMask ObstaclesLayerMask { get => _obstaclesLayerMask; private set => _obstaclesLayerMask = value; }

    #region Raycast
    
    /// <summary>
    /// Raycast Hit (past) Summary / info.
    /// </summary>
    private RaycastHit[] _raycastHitInfo;

    [Tooltip("Raycast Vertical Offset Distance, from the Ground Level:  To shoot the RAYCAST from this level y=something almost zero,... so we do not need to activate:  In the Unity Editor, in the Settings -> Physics TAB ...-> the Option: 'Queries MAY HIT BACKFACES' = TRUE.")]
    [SerializeField]
    private float _raycastVerticalOffsetDistance = 0.2f;

    #endregion Raycast

    #endregion Obstacles for Pathfinding
    
    
    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>
    private void Awake()
    {

        #region Singleton Pattern's
        
        // Singleton Pattern, protocol:
        //
        // Validation: There MUST be ONLY ONE Instance of this Class (i.e.: ONE Object):
        //
        if (Instance != null)
        {
            Debug.LogError("There's more than one '" + GetType().Name + "'!. GameObject: ---> " + transform + "  - " + Instance);
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
        
        #endregion Singleton Pattern's
        
        #region Utils

        // Raycast info:
        //
        _raycastHitInfo = new RaycastHit[7];
        
        #endregion Utils

    }// End Awake


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>



    /// <summary>
    /// Update is called once per frame
    /// </summary>


    #endregion Unity Methods


    #region My Custom Methods

    #region Setup, Initialization Methods

    
    /// <summary>
    /// Creates and Initializes the "Node System" (using the Class <code>PathNode</code>, a List of those... which is equivalent to a List of GridObject's), for A* Pathfinding.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="cellSize"></param>
    public void Setup(int width, int height, float cellSize)
    {
        // 0- Setup the Game Board (Grid) Dimensions:
        //
        _width = width;
        _height = height;
        _cellSize = cellSize;


        // 1- Create the "GridSystem",  for (A.I.) A* Pathfinding:
        // ...with  "Path Nodes"
        //
        _gridSystem = new GridSystem<PathNode>(_width, _height, _cellSize,
            (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));


        // 3- (Debugging Purposes :) Create the GameObject that will hold a Visual Representation of the Grid System: for  'A* Pathfinding'. Calling the Constructor:
        //
        //_gridSystem.CreateDebugObjects(_gridDebugObjectPrefab);

        
        #region Initialize:  Walkable NodePaths  (and Obstacles)
        
        // 4- Setup:  OBSTACLES  &  NON-WALKABLE NODES
        // .. (for Pathfinding)
        //
        UpdateWalkableAndNonWalkableNodes();

        #endregion Initialize:  Walkable NodePaths  (and Obstacles)
        
    }// End Setup
    

    /// <summary>
    /// Setup:  OBSTACLES  and  NON-WALKABLE NODES  of  Type:<code>NodePath</code>
    /// ... (for Pathfinding).
    /// </summary>
    private void UpdateWalkableAndNonWalkableNodes()
    {
        // Auxiliary variables:
        // GridPosition
        //
        GridPosition gridPosition = new GridPosition(0, 0);
        //
        // WorldPosition
        //
        Vector3 worldPositionAtTheFloorLevel;
        //
        // Raycast Offset Distance, from the Ground Level:  To shoot the RAYCAST from this level y=something,...
        //.. so we do not need to activate  In the Unity Editor, in the Settings -> Physics TAB ...-> the Option: 'Queries MAY HIT BACKFACES' = TRUE.
        //
        // We use this:   _raycastVerticalOffsetDistance;
        //
        // Raycast Offset Distance:  the distance the Ray will Travel (from the ground Level, upwards)
        //
        float raycastTravelUpwardsDistance = UnitActionSystem.Instance.GetSelectedUnit().ShoulderHeightForUnitCharacter;

        // Cycle through all possible GridPositions
        //
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                // Set this  GridPosition
                //
                gridPosition.SetXZ(x, z);

                // Set this  worldPositionAtTheFloorLevel, using 'gridPosition'
                //
                worldPositionAtTheFloorLevel = LevelGrid.Instance.GetWorldPosition(gridPosition);


                #region Raycast: Optimized Code - v-2.0

                // Shoot a Raycast from BELOW the Floor-Ground Level (y=-raycastOffsetDistance) on THAT specific 'GridPosition'... UPWARDS (Vector3.up) 1 ONE Meter (mtr) to find the OBSTACLE.
                // NOT NECESSARY, OPTIONAL:  NOTE:  IMPORTANT:  In the Unity Editor, in the Settings -> Physics TAB ...-> set the Option: 'Queries MAY HIT BACKFACES' = TRUE.
                //
                if (Physics.RaycastNonAlloc(worldPositionAtTheFloorLevel + Vector3.down * _raycastVerticalOffsetDistance,
                        Vector3.up, _raycastHitInfo, raycastTravelUpwardsDistance, _obstaclesLayerMask) > 0)
                {

                    // This 'Grid Position'  is   blocked   by obstacle
                    //
                    GetNode(x, z).SetIsWalkable(false);

                } //End if ( Physics.RaycastNonAlloc

                #endregion Raycast: Optimized Code - v-2.0

            } //End for (int z = 0; z < _height; z++)

        } //End for (int x = 0; x < _width; x++)

    }// End UpdateWalkableAndNonWalkableNodes
    

    #endregion  Setup, Initialization Methods


    /// <summary>
    /// (Optimized by AlMartson, v-2.0) <br /> <br />
    ///
    /// Main Function for calculating the Best (optimal) Path.
    /// </summary>
    /// <param name="startGridPosition"></param>
    /// <param name="endGridPosition"></param>
    /// <param name="pathLength">It is the F COST of the 'End Node' (a.k.a.: end/Destination: 'NodePath'): it is also the TOTAL COST OF THE PATH.</param>
    /// <returns>The BEST PATH, as a List of 'GridPosition'(s)</returns>
    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLength)
    {
        #region 0- Initialize Node(s), and Open and Closed Nodes (Lists)

        // PATHS
        // List of PathNode(s):   Open to visit (yet)
        //
        List<PathNode> openList = new List<PathNode>();
        //
        // List of PathNode(s):   Closed, they were already visited  (already computed / calculated there its G, H and F)
        //
        List<PathNode> closedList = new List<PathNode>();

        // NODES
        // Initial / Start NODE:
        //
        PathNode startNode = _gridSystem.GetGridObject(startGridPosition);
        //
        // End NODE:
        //
        PathNode endNode = _gridSystem.GetGridObject(endGridPosition);
        //
        // Add the "Start Node" to the List:
        //
        openList.Add(startNode);
        
        // Initialize G, H, and F Costs
        // Reset them
        //..in all the PathNode(s)  (GridPositions)
        // Lists Lenght
        //
        int gridSystemWidth = _gridSystem.GetWidth();
        int gridSystemHeight = _gridSystem.GetHeight();
        //
        // Cycle - Loop through every 'GridPosition'
        // Horizontal Values
        //
        for (int x = 0; x < gridSystemWidth; x++)
        {
            // Vertical / Forward  Values
            // 
            for (int z = 0; z < gridSystemHeight; z++)
            {
                // Get a   CURRENT   GridPosition  &   PathNode
                // CURRENT   GridPosition
                //
                GridPosition gridPosition = new GridPosition(x, z);
                //
                // CURRENT   PathNode
                //
                PathNode pathNode = _gridSystem.GetGridObject(gridPosition);

                
                #region Initialize  (G, H, F)  Costs

                // Initialize  (G, H, F)  Costs
                // G Cost   ->   Infinite
                // (Walking Cost from:  START -> to -> CURRENT Node)
                //
                pathNode.SetGCost( int.MaxValue );

                // H Cost   ->   Zero
                // ('Heuristic' Walking Cost from:  CURRENT Node -> to -> END Node ...
                // ..assuming that THERE ARE NO WALLS or obstacles: Simplification / Idealization:
                // * Let's assume it to be: the RAW linear-SHORTEST DISTANCE (in Squares / GridPositions)
                // CURRENT Node -> to -> END Node):    Initialization = Zero
                //
                pathNode.SetHCost(0);

                // F Cost   ->   Infinite   (Infinite + 0)
                //
                pathNode.CalculateFCost();
                
                // Reset / Initialize to NULL:
                // The NODE Path   (_cameFromPathNode)
                //
                pathNode.ResetCameFromPathNode();

                #endregion Initialize  (G, H, F)  Costs

            }//End for (int z = 0; x < gridSystemHeight ... 
        }//End for (int x = 0; x < gridSystemWidth ...
        
        #endregion 0- Initialize Node(s), and Open and Closed Nodes (Lists)

        
        #region 1- Start searching for a PATH

        // 1- Start searching for a PATH
        //   1.1- Set up the START Node:
        
        #region 1.1- Set up the START Node   /  Initialize  (G, H, F)  Costs
        
        //   1.1- Set up the START Node:
        //     Calculate the COST
        
        // G Cost
        //     (Walking Cost from:  START -> to -> CURRENT Node):  ZERO (initial Node)
        //
        startNode.SetGCost(0);

        // H Cost
        //     ('Heuristic' Walking Cost from:  CURRENT Node -> to -> END Node ...
        // ..assuming that THERE ARE NO WALLS or obstacles: Simplification / Idealization:
        // * Let's assume it to be: the RAW linear-SHORTEST DISTANCE (in Squares / GridPositions)
        // CURRENT Node -> to -> END Node):    DISTANCE between 2 points
        //
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));

        // F Cost   ->   Infinite   (H Distance(ThisNode, EndNode) + 0)
        //
        startNode.CalculateFCost();

        #endregion 1.1- Set up the START Node   /  Initialize  (G, H, F)  Costs
        
        #endregion 1- Start searching for a PATH
        
        
        #region 2- Continue in a  CURRENT NODE  (searching for a PATH)

        // 2- Continue in a  CURRENT NODE  (searching for a PATH)
        // WHILE  (there are NODES yet to visit...)  DO
        //
        while (openList.Count > 0)
        {

            // 1- Get the "Next" NODE in the List that:
            //     Has the LOWEST F-COST  in the group
            // (...so we can prioritize... and end the Algorithm quickly).
            //
            PathNode currentNode = GetLowestFCostPathNode(openList);

            
            // 2- Validations:
            //   .1- CURRENT Node   is our   FINAL Node?
            //
            if (currentNode == endNode)
            {

                // Reached FINAL NODE
                // Get the TOTAL COST (a.k.a.: F Cost) to this PATH
                //
                pathLength = endNode.GetFCost();
                //
                // Return the Path  (but reverse it... we want to start from START NODE to -> Ending Node)
                //
                return CalculatePath(endNode);

            }//End if (currentNode == endNode)


            // 3- Update NODES LISTS:
            //..we visited the  CURRENT NODE
            //
            openList.Remove(currentNode);
            closedList.Add(currentNode);


            #region 4- Search through all (Current Node's):  NEIGHBORS

            // 4- Search through all (Current Node's):  NEIGHBORS
            //
            List<PathNode> neighbourList = GetNeighbourList(currentNode);
            //
            int neighbourListCount = neighbourList.Count;
            //
            // For - Loop  search  for... my:   NEIGHBORS
            //
            for (int i = 0; i < neighbourListCount; i++)
            {

                // Get the / a particular  neighbourNode  to work with:
                //
                PathNode neighbourNode = neighbourList[i];
                
                
                // 4.1- Check / Validate:
                //... Is this NEIGHBOUR Node in the 'Closed List' ???   (i.e.: It is already computed...)  ->  Skip it
                //
                if (closedList.Contains(neighbourNode))
                {
                    continue;
                    
                }//End if (closedList.Contains


                // 4.2- Check / Validate:
                //   Is it Not WALKABLE ??   (an Obstacle !)
                //   Do NOT Walk (in that Node):
                //
                if (!neighbourNode.IsWalkable())
                {
                    // Add to the "already visited & checked"  List:
                    //
                    closedList.Add(neighbourNode);

                    // Skip this iteration, go and check: another neighbour
                    //
                    continue;

                }//End if (!neighbourNode.IsWalkable


                #region 4.2- Calculate the G, H, F COSTS of the NEIGHBOUR NODE:
                
                // 4.2- Calculate the G, H, F COSTS of the NEIGHBOUR NODE:
                //
                int tentativeGCostOfNeighbour = currentNode.GetGCost() +
                                                CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition()); 
                

                // Based on "the smallest"   G COST  as Criteria:
                //
                if (tentativeGCostOfNeighbour < neighbourNode.GetGCost())
                {

                    // We found the BETTER PATH:
                    //...to go FROM CURRENT NODE -> to -> NEIGHBOUR NODE
                    //
                    neighbourNode.SetCameFromPathNode(currentNode);
                    //
                    // Set G Cost
                    //
                    neighbourNode.SetGCost(tentativeGCostOfNeighbour);
                    //
                    // Set H Cost
                    //
                    neighbourNode.SetHCost( CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition) );
                    //
                    // Calculate F Cost
                    //
                    neighbourNode.CalculateFCost();
                    

                    // Update the OPEN LIST
                    // (because: we already visited this NEIGHBOUR NODE):
                    //
                    openList.Add(neighbourNode);

                }//End if (tentativeGCostOfNeighbour < neighbourNode.GetGCost())

                #endregion 4.2- Calculate the G, H, F COSTS of the NEIGHBOUR NODE:


            }//End For loop (4- Search through all (Current Node's):  NEIGHBORS)
            
            #endregion 4- Search through all (Current Node's):  NEIGHBORS

        }//End while (openList.Count > 0)
        
        
        #endregion 2- Continue in a  CURRENT NODE  (searching for a PATH)
        
        // If it reached this point, then there is no possible Path... it is a NULL Path:
        // No Path found
        // F Cost  (i.e.: Total Cost of the Path) = 0
        //
        pathLength = 0;
        //
        return null;

    }//End FindPath

    
    #region Deprecated - Obsolete Methods  (of this Class)
    
    /// <summary>
    /// (Deprecated for performance reasons)  Main Function for calculating the Best (optimal) Path.
    /// </summary>
    /// <param name="startGridPosition"></param>
    /// <param name="endGridPosition"></param>
    /// <param name="pathLength">It is the F COST of the 'End Node' (a.k.a.: end/Destination: 'NodePath'): it is also the TOTAL COST OF THE PATH.</param>
    /// <param name="useTentativeFCostOrGCostAsCriteriaInTheEnd">* TRUE: Use 'F Cost' as a CRITERIA in the end...<br /> <br /> * FALSE: Use 'G Cost'. NOTE: CodeMonkey used it in the video.</param>
    /// <returns>The BEST PATH, as a List of 'GridPosition'(s)</returns>
    [Obsolete("This method is deprecated. Use: 'FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLength, bool useTentativeFCostOrGCostAsCriteriaInTheEnd = false)' instead", true)]
    public List<GridPosition> DeprecatedFindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLength, bool useTentativeFCostOrGCostAsCriteriaInTheEnd = false)
    {
        #region 0- Initialize Node(s), and Open and Closed Nodes (Lists)

        // PATHS
        // List of PathNode(s):   Open to visit (yet)
        //
        List<PathNode> openList = new List<PathNode>();
        //
        // List of PathNode(s):   Closed, they were already visited  (already computed / calculated there its G, H and F)
        //
        List<PathNode> closedList = new List<PathNode>();

        // NODES
        // Initial / Start NODE:
        //
        PathNode startNode = _gridSystem.GetGridObject(startGridPosition);
        //
        // End NODE:
        //
        PathNode endNode = _gridSystem.GetGridObject(endGridPosition);
        //
        // Add the "Start Node" to the List:
        //
        openList.Add(startNode);
        
        // Initialize G, H, and F Costs
        // Reset them
        //..in all the PathNode(s)  (GridPositions)
        // Lists Lenght
        //
        int gridSystemWidth = _gridSystem.GetWidth();
        int gridSystemHeight = _gridSystem.GetHeight();
        //
        // Cycle - Loop through every 'GridPosition'
        // Horizontal Values
        //
        for (int x = 0; x < gridSystemWidth; x++)
        {
            // Vertical / Forward  Values
            // 
            for (int z = 0; z < gridSystemHeight; z++)
            {
                // Get a   CURRENT   GridPosition  &   PathNode
                // CURRENT   GridPosition
                //
                GridPosition gridPosition = new GridPosition(x, z);
                //
                // CURRENT   PathNode
                //
                PathNode pathNode = _gridSystem.GetGridObject(gridPosition);

                
                #region Initialize  (G, H, F)  Costs

                // Initialize  (G, H, F)  Costs
                // G Cost   ->   Infinite
                // (Walking Cost from:  START -> to -> CURRENT Node)
                //
                pathNode.SetGCost( int.MaxValue );

                // H Cost   ->   Zero
                // ('Heuristic' Walking Cost from:  CURRENT Node -> to -> END Node ...
                // ..assuming that THERE ARE NO WALLS or obstacles: Simplification / Idealization:
                // * Let's assume it to be: the RAW linear-SHORTEST DISTANCE (in Squares / GridPositions)
                // CURRENT Node -> to -> END Node):    Initialization = Zero
                //
                pathNode.SetHCost(0);

                // F Cost   ->   Infinite   (Infinite + 0)
                //
                pathNode.CalculateFCost();
                
                // Reset / Initialize to NULL:
                // The NODE Path   (_cameFromPathNode)
                //
                pathNode.ResetCameFromPathNode();

                #endregion Initialize  (G, H, F)  Costs

            }//End for (int z = 0; x < gridSystemHeight ... 
        }//End for (int x = 0; x < gridSystemWidth ...
        
        #endregion 0- Initialize Node(s), and Open and Closed Nodes (Lists)

        
        #region 1- Start searching for a PATH

        // 1- Start searching for a PATH
        //   1.1- Set up the START Node:
        
        #region 1.1- Set up the START Node   /  Initialize  (G, H, F)  Costs
        
        //   1.1- Set up the START Node:
        //     Calculate the COST
        
        // G Cost
        //     (Walking Cost from:  START -> to -> CURRENT Node):  ZERO (initial Node)
        //
        startNode.SetGCost(0);

        // H Cost
        //     ('Heuristic' Walking Cost from:  CURRENT Node -> to -> END Node ...
        // ..assuming that THERE ARE NO WALLS or obstacles: Simplification / Idealization:
        // * Let's assume it to be: the RAW linear-SHORTEST DISTANCE (in Squares / GridPositions)
        // CURRENT Node -> to -> END Node):    DISTANCE between 2 points
        //
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));

        // F Cost   ->   Infinite   (H Distance(ThisNode, EndNode) + 0)
        //
        startNode.CalculateFCost();

        #endregion 1.1- Set up the START Node   /  Initialize  (G, H, F)  Costs
        
        #endregion 1- Start searching for a PATH
        
        
        #region 2- Continue in a  CURRENT NODE  (searching for a PATH)

        // 2- Continue in a  CURRENT NODE  (searching for a PATH)
        // WHILE  (there are NODES yet to visit...)  DO
        //
        // Performance-wise Version 2.0-: for (; openList.Count > 0 ;)
        //
        while (openList.Count > 0)
        {

            // 1- Get the "Next" NODE in the List that:
            //     Has the LOWEST F-COST  in the group
            // (...so we can prioritize... and end the Algorithm quickly).
            //
            PathNode currentNode = GetLowestFCostPathNode(openList);

            
            // 2- Validations:
            //   .1- CURRENT Node   is our   FINAL Node?
            //
            if (currentNode == endNode)
            {

                // Reached FINAL NODE
                // Get the TOTAL COST (a.k.a.: F Cost) to this PATH
                //
                pathLength = endNode.GetFCost();
                //
                // Return the Path  (but reverse it... we want to start from START NODE to -> Ending Node)
                //
                return CalculatePath(endNode);

            }//End if (currentNode == endNode)


            // 3- Update NODES LISTS:
            //..we visited the  CURRENT NODE
            //
            openList.Remove(currentNode);
            closedList.Add(currentNode);


            #region 4- Search through all (Current Node's):  NEIGHBORS

            // 4- Search through all (Current Node's):  NEIGHBORS
            //
            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                
                // 4.1- Check / Validate:
                //... Is this NEIGHBOUR Node in the 'Closed List' ???   (i.e.: It is already computed...)  ->  Skip it
                //
                if (closedList.Contains(neighbourNode))
                {
                    continue;
                    
                }//End if (closedList.Contains


                // 4.2- Check / Validate:
                //   Is it Not WALKABLE ??   (an Obstacle !)
                //   Do NOT Walk (in that Node):
                //
                if (!neighbourNode.IsWalkable())
                {
                    // Add to the "already visited & checked"  List:
                    //
                    closedList.Add(neighbourNode);

                    // Skip this iteration, go and check: another neighbour
                    //
                    continue;

                }//End if (!neighbourNode.IsWalkable


                #region 4.2- Calculate the G, H, F COSTS of the NEIGHBOUR NODE:
                
                // 4.2- Calculate the G, H, F COSTS of the NEIGHBOUR NODE:
                //
                int tentativeGCostOfNeighbour = currentNode.GetGCost() +
                                                CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition()); 
                
                #region Using F Cost as Criterion - Remove

                // H Cost   (Tentative)
                //
                int tentativeHCostOfNeighbour = CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition);
                //
                // F Cost   (Tentative)
                //
                int tentativeFCostOfNeighbour = tentativeGCostOfNeighbour + tentativeHCostOfNeighbour; 
                
                #endregion Using F Cost as Criterion - Remove
                
                
                // CRITERIA: ...based on:  useTentativeFCostOrGCostAsCriteriaInTheEnd
                //
                if (useTentativeFCostOrGCostAsCriteriaInTheEnd)
                {
                    
                    // Based on   F COST  as Criteria:
                    //
                    if (tentativeFCostOfNeighbour < neighbourNode.GetFCost())
                    {

                        // We found the BETTER PATH:
                        //...to go FROM CURRENT NODE -> to -> NEIGHBOUR NODE
                        //
                        neighbourNode.SetCameFromPathNode(currentNode);
                        //
                        // Set G Cost
                        //
                        neighbourNode.SetGCost(tentativeGCostOfNeighbour);
                        //
                        // Set H Cost
                        //
                        neighbourNode.SetHCost( CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition) );
                        //
                        // Calculate F Cost
                        //
                        neighbourNode.CalculateFCost();
                        

                        // Update the OPEN LIST
                        // (because: we already visited this NEIGHBOUR NODE):
                        //
                        openList.Add(neighbourNode);

                    }//End if (tentativeFCostOfNeighbour < neighbourNode.GetFCost())

                }//End if (useTentativeFCostOrGCostAsCriteriaInTheEnd)
                else
                {
                    // Based on   G COST  as Criteria:   (correct one, according to CodeMonkey)
                    //
                    if (tentativeGCostOfNeighbour < neighbourNode.GetGCost())
                    {

                        // We found the BETTER PATH:
                        //...to go FROM CURRENT NODE -> to -> NEIGHBOUR NODE
                        //
                        neighbourNode.SetCameFromPathNode(currentNode);
                        //
                        // Set G Cost
                        //
                        neighbourNode.SetGCost(tentativeGCostOfNeighbour);
                        //
                        // Set H Cost
                        //
                        neighbourNode.SetHCost( CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition) );
                        //
                        // Calculate F Cost
                        //
                        neighbourNode.CalculateFCost();
                        

                        // Update the OPEN LIST
                        // (because: we already visited this NEIGHBOUR NODE):
                        //
                        openList.Add(neighbourNode);

                    }//End if (tentativeGCostOfNeighbour < neighbourNode.GetGCost())
                    
                }//End else of if (useTentativeFCostOrGCostAsCriteriaInTheEnd)
                
                #endregion 4.2- Calculate the G, H, F COSTS of the NEIGHBOUR NODE:


            }//End For loop (4- Search through all (Current Node's):  NEIGHBORS)
            
            #endregion 4- Search through all (Current Node's):  NEIGHBORS

        }//End while (openList.Count > 0)
        
        
        #endregion 2- Continue in a  CURRENT NODE  (searching for a PATH)
        
        // If it reached this point, then there is no possible Path... it is a NULL Path:
        // No Path found
        // F Cost  (i.e.: Total Cost of the Path) = 0
        //
        pathLength = 0;
        //
        return null;

    }// End DeprecatedFindPath

    
        
    /// <summary>
    /// (Deprecated for performance reasons)  Rebuilds the BEST PATH... <br /> <br />
    /// 
    /// Calculated from a series of 'PathNode' that are linked through an internal Attribute (Field) of the PathNode Class:  '_cameFromPathNode'. <br /> <br />
    /// NOTE: The list is built in REVERSE ORDER (END NODE -> ... -> START NODE), then that is fixed inside this function, so we get a NORMAL List in the end (START NODE -> ... -> END NODE).
    /// </summary>
    /// <param name="endNode"></param>
    /// <returns></returns>
    [Obsolete("This method is deprecated. Use: 'CalculatePath(PathNode endNode)' instead", true)]
    private List<GridPosition> DeprecatedCalculatePath(PathNode endNode)
    {
        // List to "almost" return:   Path Nodes, the BEST PATH, found by using PATHFINDING:
        //
        List<PathNode> pathNodeList = new List<PathNode>();
        
        // 1- Add the "endNode"   (starting in REVERSE MODE: from the END ..-> to -> the Beginning)
        //
        pathNodeList.Add(endNode);
        
        // "Current Node":  Moving BACKWARDS  (END to -> START)  in the NODES:
        //
        PathNode currentNode = endNode;
        
        
        #region 2- Rebuilding the Path of Nodes: Cycling BACKWARDS
        
        // Cycling BACKWARDS to rebuild the PATH of NODES... until the 'Start Node':
        //
        while ( currentNode.GetCameFromPathNode() != null )
        {

            // Means: There (still) are NODES on that PATH:
            // Add the  CURRENT NODE  to the List
            //
            pathNodeList.Add( currentNode.GetCameFromPathNode() );
            
            // Update the (auxiliary variable of)  CURRENT NODE  -> to the -> NEXT
            // (...rather: PREVIOUS, because we are going BACKWARDS, remember..?)
            //
            currentNode = currentNode.GetCameFromPathNode();

        }//End while ( currentNode.GetCameFromPathNode() != null )
        
        #endregion 2- Rebuilding the Path of Nodes: Cycling BACKWARDS

        
        #region 3- Reverting the ORDER of the Nodes  (we got an INVERSE PATH)
        
        // 3.1- INVERT the ORDER of the List<>
        //
        pathNodeList.Reverse();

        #endregion 3- Reverting the ORDER of the Nodes  (we got an INVERSE PATH)


        #region 4- Cast the type List "PathNode"   to:  List "GridPosition"
        
        // List to return finally
        //
        List<GridPosition> gridPositionList = new List<GridPosition>();
        

        // 4- Cast the type List <"PathNode">   to:  List <"GridPosition">
        //
        foreach (PathNode pathNode in pathNodeList)
        {

            // Get the "GridPosition"  represented by that (..Path..) NODE:
            //..build the List with all the GridPositions, in a correct (forward) order:
            //
            gridPositionList.Add(pathNode.GetGridPosition());
            
        }//End foreach (PathNode pathNode in pathNodeList)

        #endregion 4- Cast the type List "PathNode"   to:  List "GridPosition"
        

        // Return the List of "GridPositions":   The BEST PATH.
        //
        return gridPositionList;
        
    }// End DeprecatedCalculatePath
    
    
    #endregion Deprecated - Obsolete Methods  (of this Class)
    
    
    #region Search Nodes - Operations

    
    /// <summary>
    /// (Optimized by AlMartson, v-2.0)   Rebuilds the BEST PATH... <br /> <br />
    /// 
    /// Calculated from a series of 'PathNode' that are linked through an internal Attribute (Field) of the PathNode Class:  '_cameFromPathNode'. <br /> <br />
    /// NOTE: The list is built in REVERSE ORDER (END NODE -> ... -> START NODE), then that is fixed inside this function, so we get a NORMAL List in the end (START NODE -> ... -> END NODE).
    /// </summary>
    /// <param name="endNode"></param>
    /// <returns></returns>
    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        // List to "almost" return:   Path Nodes, the BEST PATH, found by using PATHFINDING:
        //
        List<PathNode> pathNodeList = new List<PathNode>();
        
        // 1- Add the "endNode"   (starting in REVERSE MODE: from the END ..-> to -> the Beginning)
        //
        pathNodeList.Add(endNode);
        
        // "Current Node":  Moving BACKWARDS  (END to -> START)  in the NODES:
        //
        PathNode currentNode = endNode;
        
        
        #region 2- Rebuilding the Path of Nodes: Cycling BACKWARDS
        
        // Cycling BACKWARDS to rebuild the PATH of NODES... until the 'Start Node':
        //
        while ( currentNode.GetCameFromPathNode() != null )
        {

            // Means: There (still) are NODES on that PATH:
            // Add the  CURRENT NODE  to the List
            //
            pathNodeList.Add( currentNode.GetCameFromPathNode() );
            
            // Update the (auxiliary variable of)  CURRENT NODE  -> to the -> NEXT
            // (...rather: PREVIOUS, because we are going BACKWARDS, remember..?)
            //
            currentNode = currentNode.GetCameFromPathNode();

        }//End while ( currentNode.GetCameFromPathNode() != null )
        
        #endregion 2- Rebuilding the Path of Nodes: Cycling BACKWARDS

        
        #region 3- Reverting the ORDER of the Nodes  (we got an INVERSE PATH)
        
        // 3.1- INVERT the ORDER of the List<>
        //
        pathNodeList.Reverse();

        #endregion 3- Reverting the ORDER of the Nodes  (we got an INVERSE PATH)


        #region 4- Cast / Convert   the type List "PathNode"   to:  List "GridPosition"
        
        // List to return finally:  initialize
        //
        List<GridPosition> gridPositionList = new List<GridPosition>();
        

        // 4- Cast the type List <"PathNode">   to:  List <"GridPosition">
        // Length of the Node's List:
        //
        int pathNodeListLength = pathNodeList.Count;
        //
        for (int i = 0; i < pathNodeListLength; i++)
        {

            // Get the "GridPosition"  represented by that (..Path..) NODE:
            //..build the List with all the GridPositions, in a correct (forward) order:
            //
            gridPositionList.Add(pathNodeList[i].GetGridPosition());
            
        }//End for

        #endregion 4- Cast the type List "PathNode"   to:  List "GridPosition"
        

        // Return the List of "GridPositions":   The BEST PATH.
        //
        return gridPositionList;
        
    }// End CalculatePath
    
    
    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        
        // Initialize the List to return:
        //
        List<PathNode> neighbourList = new List<PathNode>();

        // Get GridPosition  associated with that current Node:
        //
        GridPosition gridPosition = currentNode.GetGridPosition();
        
        
        #region Check ALL POSSIBLE NEIGHBOURS - Accessable locations of the Game Board
        
        // Todo:  Create Here access to STAIRS - STAIRWAY, to other LEVELS, and some other kind of Access to PORTALS, and ANY WALKABLE location of the Map in the 'Game Board' (i.e.: Level Grid and Grid System).
        // We Create here all possible Movements, based on the connections / Linkds THIS CURRENT NODE may have to any other part of MAP, such as: Portals, Stairways to other Levels (heights above, below, etc), etc.  (i.e.: Level Grid and Grid System):
        //
        List<GridPosition> neighbourPositionList = new List<GridPosition>()
        {
            // gridPosition + new GridPosition( 0,  1), // N
            // gridPosition + new GridPosition( 1,  1), // NE
            // gridPosition + new GridPosition( 1,  0), // E
            // gridPosition + new GridPosition( 1, -1), // SE
            // gridPosition + new GridPosition( 0, -1), // S
            // gridPosition + new GridPosition(-1, -1), // SW
            // gridPosition + new GridPosition(-1,  0), // W
            // gridPosition + new GridPosition(-1,  1), // NW
            //
            gridPosition + _A_FORWARDS_GRID_POSITION, // N
            gridPosition + _A_FORWARDS_GRID_POSITION + _A_RIGHTWARDS_GRID_POSITION, // NE
            gridPosition + _A_RIGHTWARDS_GRID_POSITION, // E
            gridPosition + _A_BACKWARDS_GRID_POSITION + _A_RIGHTWARDS_GRID_POSITION, // SE
            gridPosition + _A_BACKWARDS_GRID_POSITION, // S
            gridPosition + _A_BACKWARDS_GRID_POSITION + _A_LEFTWARDS_GRID_POSITION, // SW
            gridPosition + _A_LEFTWARDS_GRID_POSITION, // W
            gridPosition + _A_FORWARDS_GRID_POSITION + _A_LEFTWARDS_GRID_POSITION, // NW
        };

        // Validate the new GridPosition(s):
        //
        int neighbourPositionListCount = neighbourPositionList.Count;
        //
        // Non-Performant version:   foreach (GridPosition neighbourPosition in neighbourPositionList)
        // Performant (For) version:
        //
        for (int i = 0; i < neighbourPositionListCount; i++ ) 
        {
            // Initialize an Item of the List:
            //
            GridPosition neighbourPosition = neighbourPositionList[i];
            
            // Validate the new  'neighbourPosition'
            //
            if (_gridSystem.IsValidGridPosition(neighbourPosition))
            {
                neighbourList.Add(GetNode(neighbourPosition));
            }
            
        }//End for (int i = 0;...
        
        #endregion Check ALL POSSIBLE NEIGHBOURS - Accessable locations of the Game Board
        
        // Return Neighbour List
        //
        return neighbourList;

    }// End GetNeighbourList


    /// <summary>
    /// Gets a PathNode, <br /> ...given a 'GridPosition' (i.e.: Grid created with: (x, y=0, z) Position) on the Map, - Game Board, GridSystem, LevelGrid - 
    /// </summary>
    /// <returns></returns>
    private PathNode GetNode(GridPosition gridPosition)
    {
        // Get the Node (PathNode from:  gridPosition(x, z) )
        //
        return _gridSystem.GetGridObject(gridPosition);

    }// End GetNode
    
    
    /// <summary>
    /// Gets a PathNode, <br /> ...given a (x, z) (i.e.: (x, y=0, z) Position) on the Map, - Game Board, GridSystem, LevelGrid - 
    /// </summary>
    /// <returns></returns>
    private PathNode GetNode(int x, int z)
    {
        // Get the Node (PathNode from:  (x, z) )
        //
        return _gridSystem.GetGridObject(new GridPosition(x, z));

    }// End GetNode
    
    #endregion Search Nodes - Operations

    
    #region PathFinding Mathematical Calculations:  G, H, F

    /// <summary>
    /// Calculates the Distance between 2 GridPositions: A , B. <br /> <br />
    /// It is used for calculating 'H', the Heuristic Cost of 'Walking from the CURRENT NODE to -> END Node'...
    /// ...of the Pathfinding Algorithm <br /> <br />
    /// NOTE: <br />
    /// 1- H (distance A to B) in Pathfinding assumes there are no walls, no obstacles. <br />
    /// <param name="gridPositionA"></param>
    /// <param name="gridPositionB"></param>
    /// </summary>
    public int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        // Calculate the 'Distance Vector2':
        // gridPositionB -> to -> gridPositionA
        //
        GridPosition gridPositionDistance = gridPositionA - gridPositionB;
        //
        // Calculate the DISTANCE   (i.e.: the Norm / Module  of the Vector)
        //
        // Not necessary:  int totalDistance = Mathf.Abs(gridPositionDistance.x) + Mathf.Abs(gridPositionDistance.z);
        //
        // Get each Distance separately:  x  and  z
        //
        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        //
        // Get the Difference in Longitude: x vs z
        //
        int remainingDistanceXvsZ = Mathf.Abs(xDistance - zDistance);
        
        // Return the DISTANCE
        // (using the Minimum (MIN (xDistance, zDistance) ) number of DIAGONALS... +
        // ... + Maximum number of STRAIGHT Grid / Cells)
        //
        return ( _MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) ) + ( _MOVE_STRAIGHT_COST * remainingDistanceXvsZ );

    }// End CalculateDistance


    /// <summary>
    /// Calculates and Gets the 'PathNode' with the LOWEST value of 'F' Cost.
    /// </summary>
    /// <returns></returns>
    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        // For Loop - to find the NODE with:  The Lowest F Cost
        // List Lenght
        //
        int pathNodeListCount = pathNodeList.Count;
        
        // Return value:
        //
        PathNode lowestFCostPathNode = pathNodeList[0];
        
        // For Loop - to find the NODE
        //
        for (int i = 0; i < pathNodeListCount; i++)
        {
            // Get the Lowest 'F Cost' (NODE)
            //
            if (pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost())
            {
                // Save the Lowest F Cost (Node)
                //
                lowestFCostPathNode = pathNodeList[i];

            }//End if (pathNodeList[i].GetFCost() <...
            
        }//End for (int i = 0; i < pathNodeListCount; i++)
        

        // Return the PathNode  (NOTE: It could be 'null')
        //
        return lowestFCostPathNode;
        
    }//End GetLowestFCostPathNode

    #endregion PathFinding Mathematical Calculations:  G, H, F


    #region Obstacles for Pathfinding

    /// <summary>
    /// Is it suitable for a Mouse Click (on the Player's part...) or for to be used a 'Destination' for the ENEMY A.I.??...
    /// ...or NOT?
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public bool IsWalkableGridPosition(GridPosition gridPosition)
    {
        // Get the '_isWalkable' Boolean Flag  of the 'PathNode' related to the Input: gridPosition
        //
        return _gridSystem.GetGridObject(gridPosition).IsWalkable();

    }// End IsWalkableGridPosition
    
    
    /// <summary>
    /// Is there a at least ONE (1) possible Path to get there?? <br /> <br />
    /// ...from <code>startGridPosition</code> to <code>endGridPosition</code>
    /// </summary>
    /// <param name="startGridPosition"></param>
    /// <param name="endGridPosition"></param>
    /// <returns></returns>
    public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        // Execute the Pathfinding algorithm, and see if it returns  NULL
        //
        return (FindPath(startGridPosition, endGridPosition, out int pathLength) != null);

    }// End HasPath
    
    
    /// <summary>
    /// Gets the TOTAL COST OF THE PATH. <br /> <br />
    /// It is: 'F Cost'  of 'endGridPosition'.
    /// </summary>
    /// <param name="startGridPosition"></param>
    /// <param name="endGridPosition"></param>
    /// <returns></returns>
    public int GetPathLength(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        // Execute the Pathfinding algorithm, and return  'pathLength'
        //
        FindPath(startGridPosition, endGridPosition, out int pathLength);
        //
        return pathLength;

    }// End GetPathLength
    
    #endregion Obstacles for Pathfinding
    
    #endregion My Custom Methods

}// End Pathfinding
