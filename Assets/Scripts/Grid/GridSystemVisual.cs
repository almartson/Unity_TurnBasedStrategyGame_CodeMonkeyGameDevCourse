using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Singleton for handling the Visual Prefabs-Representations of the available Cells of the Grid that the Player can TakeAction in to, in this Turn.
/// </summary>
public class GridSystemVisual : MonoBehaviour
{
    #region Attributes

    #region Singleton Utils
    
    [Tooltip("Singleton Pattern's Main Key: Instance of this very Class")] 
    public static GridSystemVisual Instance { get; private set; }
    
    #endregion Singleton Utils
    
    
    [Tooltip("(Prefab) Visual representation of the Grid Positions where the Unit can move, in the current Turn")]
    [SerializeField]
    private Transform _gridSystemVisualSinglePrefab;

    /// <summary>
    /// (Array / 2x2 Matrix... of GridSystemVisualSingle, a Prefab): Array with ALL the: Visual Representations (the Prefabs...) called by the same names, for generating Visual cues about the Cells/Squares,'Grid System' where the Player can move in to.
    /// </summary>
    private GridSystemVisualSingle[,] _gridSystemVisualSingleArray;

    #region Obstacles for Shooting (Experimental)

    [Tooltip("Obstacles Label-LayerMask in the AIM or 'Shooting Path'")]
    [SerializeField]
    private LayerMask _obstaclesLayerMask;
    //
    /// <summary>
    /// Getter and Setter Property for Field: _obstaclesLayerMask
    /// </summary>
    public LayerMask ObstaclesLayerMask { get => _obstaclesLayerMask; private set => _obstaclesLayerMask = value; }


    #endregion Obstacles for Shooting (Experimental)
    
    #region Colors: Grid Cells

    #region Color and Material VARIABLES / FIELDS

    [Tooltip("List of Materials / Colors associated to the Numbered: (Grid Positions...): Ground Squares of the Game Board.")]
    [SerializeField]
    private List<GridVisualTypeMaterial> _gridVisualTypeMaterialList;
    
    #endregion Color and Material VARIABLES / FIELDS
    
    #region Color and Material Types
    
    /// <summary>
    /// Type of Colors used for the Grid Position Visuals / GUI on the ground, for the squares (i.e.: each Color is meant to represent a Possible ACTION that is currently selected).
    /// </summary>
    public enum GridVisualColorType { White, Green, Blue, Red, RedSoft, Yellow, }
    
    /// <summary>
    /// (In bigger Projects we would use: Whether this on ANOTHER FILE.cs... OR we would define a SCRIPTABLE OBJECT to have these data - Colors and Materials, which are CONSTANTS in the Game...) <br /> <br />
    /// Materials used for the Grid Position Visuals / GUI on the ground, for the squares (i.e.: each Color is meant to represent a Possible ACTION that is currently selected). <br /> <br />
    /// NOTE: To Serialize a STRUCT in the Inspector, [Serializable]  must be used.
    /// </summary>
    [Tooltip("(In bigger Projects we would use: Whether this on ANOTHER FILE.cs... OR we would define a SCRIPTABLE OBJECT to have these data - Colors and Materials, which are CONSTANTS in the Game...)./n/n Materials used for the Grid Position Visuals / GUI on the ground, for the squares (i.e.: each Color is meant to represent a Possible ACTION that is currently selected). /n/n NOTE: To Serialize a STRUCT in the Inspector, [Serializable]  must be used.")]
    [Serializable]
    public struct GridVisualTypeMaterial
    {
        
        [Tooltip("Grid Visual Type of COLOR:")]
        public GridVisualColorType gridVisualColorType;

        /// <summary>
        /// Material
        /// </summary>
        public Material material;

    }// End struct GridVisualTypeMaterial

    #endregion Color and Material Types
    

    #endregion Colors: Grid Cells
    
    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>
    private void Awake()
    {
        // Singleton Pattern, protocol:
        //
        // Validation: There MUST be ONLY ONE Instance of this Class (i.e.: ONE Object):
        //
        if (Instance != null)
        {
            
            Debug.LogError("There's more than one 'GridSystemVisual'!. GameObject: ---> " + transform + "  - " + Instance);
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
    }

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start()
    {
        // Instantiate the ARRAY of: Prefabs with the Visual cue:
        //
        _gridSystemVisualSingleArray = new GridSystemVisualSingle[
            LevelGrid.Instance.GetWidth(),
            LevelGrid.Instance.GetHeight()
        ];
        
        
        // Create IN THE CURRENT SCENE: a parent GameObject for organizing the multiple GameObjects that will be instantiated later...
        //
        GameObject parentCell = new GameObject("GridCell_VisualOutlineCuesFor_AvailableToMoveIn");
        
        
        // Cycle through the Rows & Columns of the GridSystem (i.e.: Board)
        // Columns... limited by the _height
        //
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            // Rows... limited by the _width
            //
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                
                // Create new GridPosition:
                //
                GridPosition gridPosition = new GridPosition(x, z);
                
                // Instantiate the Visual Prefab, that I created beforehand for this UI Element representation of the Possible available cell to move in to.  
                //
                Transform gridSystemVisualSingleTransform = Instantiate(_gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity, parentCell.transform);
                
                // Save the GridSystemVisualSingle  Component of the Prefab in each ITERATION for working with it:
                //
                _gridSystemVisualSingleArray[x, z] =
                    gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();

            }//End for 2
        }//End for 1

        
        #region Subscribing Events - Delegates

        // Subscribe to the Event:   OnSelectedActionChanged
        //
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        
        // Subscribe to the Event:   OnSelectedActionChanged
        //
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;
        
        // Subscribe to the Event:   Unit.OnAnyUnitDied
        //
        Unit.OnAnyUnitDied += Unit_OnAnyUnitDied;
        
        #endregion Subscribing Events - Delegates

        
        // Rendering the available positions, the Game Board, to move in to, in the next Turn. 
        //
        UpdateGridVisual();
        
    }// End Start


    /// <summary>
    /// Update is called once per frame
    /// </summary>

    #endregion Unity Methods


    #region My Custom Methods

    #region Getters and Setters
    
    /// <summary>
    /// Getter (it searches through the List...) for a specific GameObject of Type:  'GridVisualTypeMaterial', from the List of Struct: List of GridVisualTypeMaterial. 
    /// </summary>
    /// <returns>The Material, parting the Color Input.... from the Struct <code>List of GridVisualTypeMaterial</code></returns>
    private Material GetGridVisualTypeMaterial(GridVisualColorType gridVisualColorType)
    {
        #region CodeMonkey's initial implementation (using FOR EACH...)

        // // Search (for the Material that matches the Input: COLOR -> gridVisualColorType) through the List
        // //
        // foreach (GridVisualTypeMaterial gridVisualTypeMaterial in _gridVisualTypeMaterialList)
        // {
        //     // If you find a Material made with that COLOR, return it:
        //     //
        //     if (gridVisualTypeMaterial.gridVisualColorType == gridVisualColorType)
        //     {
        //         // Return the Material we were looking for:
        //         //
        //         return gridVisualTypeMaterial.material;
        //
        //     }// End if (gridVisualTypeMaterial.gridVisualColorType ==...
        //     
        // }//End foreach
        // //
        // // If you did not find a Material matching the Input COLOR,
        // // 1- Log / Inform of an Exception (it should NEVER happen):
        // //
        // Debug.LogError($"Could not find a defined 'GridVisualTypeMaterial' for an Input 'GridVisualColorType'!./nScript: {GetType().Name} /n/nGameObject: ---> {transform} - {Instance}./n/nReturning 'null'");
        // //
        // // 2- return null:
        // //
        // return null;

        #endregion CodeMonkey's initial implementation (using FOR EACH...)


        #region AlMartson's (optimized) implementation (using FOR...)

        // Search (for the Material that matches the Input: COLOR -> gridVisualColorType) through the List
        //
        // List Lenght:
        //
        int listLenght = _gridVisualTypeMaterialList.Count;
        //
        for (int i = 0; i < listLenght ; i++)
        {
            // Get a pointer (i.e.: a reference...) to: the Item from the List, to work with it:
            //
            GridVisualTypeMaterial gridVisualTypeMaterial = _gridVisualTypeMaterialList[i];
            
            // If you find a Material made with that COLOR, return it:
            //
            if (gridVisualTypeMaterial.gridVisualColorType == gridVisualColorType)
            {
                // Return the Material we were looking for:
                //
                return gridVisualTypeMaterial.material;

            }// End if (gridVisualTypeMaterial.gridVisualColorType ==...
            
        }//End for
        //
        // If you did not find a Material matching the Input COLOR,
        // 1- Log / Inform of an Exception (it should NEVER happen):
        //
        Debug.LogError($"Could not find a defined 'GridVisualTypeMaterial' for an Input 'GridVisualColorType' = {gridVisualColorType}!.\nScript: {GetType().Name} \n\nGameObject: ---> {transform} - {Instance}.\n\nReturning 'null'");
        //
        // 2- return null:
        //
        return null;

        #endregion AlMartson's (optimized) implementation (using FOR...)

    }//End GetGridVisualTypeMaterial
    
    #endregion Getters and Setters
    
    
    /// <summary>
    /// Hides all Visual cues (i.e.: the Visual Prefabs...) of the Grid System.
    /// </summary>
    public void HideAllGridPositions()
    {
        // Cycle through the Rows & Columns of the GridSystem (i.e.: Board).
        // Columns... limited by the _height
        //
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            // Rows... limited by the _width
            //
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                // Hide every Instanced Prefab, so it's INVISIBLE, (although it is in the Scene :)
                //
                _gridSystemVisualSingleArray[x, z].Hide();
                
            }//End for 2
        }//End for 1
    }
    
       
    /// <summary>
    /// Shows the Visual cues (GridPositions), for the RANGE of the SELECTED ACTION (e.g.: for ShootAction, the 'range' is the reach of the -shoot- AIM...), which must be passed in as Input.<br />
    /// It also sets the right Material, based on the Color of the ACTION that is selected (2nd Parameter:  gridVisualColorType).
    /// </summary>
    public void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualColorType gridVisualColorType)
    {
        // Auxiliary List to work with:
        //
        List<GridPosition> gridPositionList = new List<GridPosition>();
        
        // Cycle through the Rows & Columns of the GridSystem (i.e.: Board).
        // Columns... limited by the 'range' INPUT.
        //
        for (int x = -range; x <= range; x++)
        {
            // Rows... limited by the 'range' INPUT.
            //
            for (int z = -range; z <= range; z++)
            {
                
                // Create an Item: GridPosition CELL, and fill it in with data for the (COLUMN, ROW)
                //
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);
                
                // Validation:
                //
                // 1- "GridPosition" Must be inside the Grid System, not off-limits:
                //
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    // Not Valid: continue / SKIP: to the NEXT ITERATION.
                    continue;
                }

                #region Calculating the RANGE AREA

                #region Shape of th Area: Case of: Triangular shape

                // // Then: Calculating the RANGE AREA:  valid Squared/Cells:
                // // Shape of th Area: Case of: Circular shape made with square pixels  (using Pythagoras Theorem):
                // //
                // int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                //
                // // This is the Triangular Shape, for the RANGE
                // //
                // if (testDistance > range)
                // {
                //     continue;
                //
                // }//End if (testDistance > ...

                #endregion Shape of th Area: Case of: Triangular shape
                
                
                #region Shape of th Area: Case of: Circular shape made with square pixels  (using Pythagoras Theorem)
                
                // Then: Calculating the RANGE AREA:  valid Squared/Cells:
                // Shape of th Area: Case of: Circular shape made with square pixels  (using Pythagoras Theorem):
                //
                int squareTestDistance = (x * x) + (z * z);
                
                // This is the Circular Shape, for the RANGE (using Pythagoras):
                //
                if (squareTestDistance > ((range * range) + range + 0.25f))
                {
                    continue;
                
                }//End if (testDistance > ...
                
                #endregion Shape of th Area: Case of: Circular shape made with square pixels  (using Pythagoras Theorem)

                #endregion Calculating the RANGE AREA
                
                
                #region Experimental Validation:  Can not shoot behind WALLS or OBSTACLES

                // Validate: Can NOT shoot behind WALLS or OBSTACLES
                // TODO: put this Variable in a correct class, following the S.O.L.I.D. Principle:
                //
                float shoulderHeightForLineOfSight = UnitActionSystem.Instance.GetSelectedUnit().ShoulderHeightForUnitCharacter;
                //
                if (ValidateIsBlockedTheLineOfSightBetweenTwoGridPositions(gridPosition, testGridPosition, shoulderHeightForLineOfSight))
                {
                    continue;
                }

                #endregion Experimental Validation:  Can not shoot behind WALLS or OBSTACLES


                // Finally:  Add the Grid Position Cell to the List of VALID ones (within RANGE):
                //
                gridPositionList.Add(testGridPosition);
                
            }//End for 2
        }//End for 1
        
        // Print / Render on-screen the Grid / Cells AREA  (that's valid...)
        //
        ShowGridPositionList(gridPositionList, gridVisualColorType);

    }//End ShowGridPositionRange
    

    /// <summary>
    /// Validates: Is the Path between two (2) 'GridPosition'(s)  Blocked by an Obstacle. <br />
    /// Requires: That the Obstacle be set up with a LayerMask:   Obstacle. The Field Attribute for that: is this Class.
    /// </summary>
    /// <param name="fromGridPosition"></param>
    /// <param name="toTestGridPosition"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public bool ValidateIsBlockedTheLineOfSightBetweenTwoGridPositions(GridPosition fromGridPosition, GridPosition toTestGridPosition, float height)
    {
        // TODO:  Refactor and CLEAN this Code below, using Fields (Attributes) from a specific - Single Responsibility Class (that handles this kind of data, such as Unit for:  unitShoulderHeight, etc). According to the S.O.L.I.D. Principle.
        // TODO: Refactor this Physics.Raycast.. for a a non-alloc  Raycast option...
        //
        Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(fromGridPosition) + Vector3.up * height;
        Vector3 testWorldPosition = LevelGrid.Instance.GetWorldPosition(toTestGridPosition) + Vector3.up * height;

        Vector3 aimDir = (testWorldPosition - unitWorldPosition).normalized;

        if (Physics.Raycast(unitWorldPosition, aimDir, Vector3.Distance(unitWorldPosition, testWorldPosition),
                _obstaclesLayerMask))
        {
            // Line Of Sight   blocked   by obstacle
            //
            return true;
        }

        return false;
    }// End ValidateIsBlockedTheLineOfSightBetweenTwoGridPositions


    /// <summary>
    /// Shows the Visual cues (GridPositions) passed in as Input.<br />
    /// It also sets the right Material, based on the Color of the ACTION that is selected (2nd Parameter:  gridVisualColorType).
    /// </summary>
    /// <param name="gridPositionList"></param>
    /// <param name="gridVisualColorType"></param>
    public void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualColorType gridVisualColorType)
    {
        // Cycle through the items in the List of: <GridPosition>
        // Lenght:
        //
        int listLenght = gridPositionList.Count;
        //
        for (int i = 0; i < listLenght; i++)
        {

            // Show (And Set the Material of...) the selected items (i.e.: GridPositions) passed as Input: so they are VISIBLE, (in the Scene :)
            //
            _gridSystemVisualSingleArray[gridPositionList[i].x, gridPositionList[i].z].ShowAndSetMaterial(GetGridVisualTypeMaterial(gridVisualColorType));

        }//End for 1
    }


    /// <summary>
    /// Updates (Re-renders) the Grid System Visual cues about: available Cells to 'Take Action' to (...or to move in to), in this turn. <br />
    /// Also, the (Grid) COLORS are updated (set...) according to the (selected) ACTION's COLOR.
    /// </summary>
    private void UpdateGridVisual()
    {
        // Validate all Possible Moves to the ADJACENT SQUARES / GRID CELLS:
        //
        // 1- Hide all Grid Positions Prefab's Visual cues..:
        //
        HideAllGridPositions();

        // 2- Get the Player's currently: SELECTED ACTION
        //
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        //
        // Get the Player's Unit / Character:
        //
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        // 3- Select the COLOR (Material) of the Grid Cells, according to the ACTIONS that's currently selected:
        // Initialization:
        //
        GridVisualColorType gridVisualColorType;        // = GridVisualColorType.White;

        switch (selectedAction)
        {
            // Change the Material + Color, based on the selected ACTION:
            //
            default:
            case MoveAction moveAction:

                gridVisualColorType = GridVisualColorType.Green;
                break;

            case SpinAction spinAction:

                gridVisualColorType = GridVisualColorType.Yellow;      // GridVisualColorType.Blue;
                break;
            
            case ShootAction shootAction:

                // Target Grid Color
                //
                gridVisualColorType = GridVisualColorType.Red;
                //
                // Area Grid(s) Color
                //
                ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootDistance(), GridVisualColorType.RedSoft);
                break;

        }//End switch (selectedAction)
        
        // 3- Final- ShowAndSetMaterial only the VALID Cells of the Grid, to 'Take Action' to (...or to move in to),  (based on the Player's SELECTED Unit):
        //
        ShowGridPositionList( selectedAction.GetValidActionGridPositionList(), gridVisualColorType );

    }//End UpdateGridVisual()

    
    #region Subscribing to Events - Delegates

    /// <summary>
    /// Event that Updates the Grid Cells color on the ground, anytime a new Action is selected (that's the Trigger).
    /// (re-rendering the available positions, to move in to, in this Turn).
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        // Update the Grid System Visual (GUI) on the ground  (i.e.: the GAME BOARD):
        //..(re-rendering the available positions, to move in to, in this Turn):
        // Update the Grid Visual... 
        //
        UpdateGridVisual();
        
    }// End UnitActionSystem_OnSelectedActionChanged
    
    /// <summary>
    /// Event that Updates the Grid Cells color on the ground, whenever a player / Unit / Character moves... thus changing its 'Grid Position' (that's the Trigger).
    /// (re-rendering the available positions, the Game Board, to move in to, in the next Turn).
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs e)
    {
        // Update the Grid System Visual (GUI) on the ground  (i.e.: the GAME BOARD):
        //..(re-rendering the available positions, the Game Board, to move in to, in the next Turn). 
        //
        UpdateGridVisual();
        
    }// End LevelGrid_OnAnyUnitMovedGridPosition

    
    /// <summary>
    /// Event that Updates the Grid Cells color on the ground, whenever a Game Character / Unit:
    /// ..."DIES"... (ALL the GAME BOARD:  'Grid Position'(s) are RE-RENDERED).
    /// (re-rendering the Grid Position Cell's numbers and characters, the Game Board, to move in to, in the next Turn).
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Unit_OnAnyUnitDied(object sender, EventArgs e)
    {
        // Update the Grid System Visual (GUI) on the ground  (i.e.: the GAME BOARD):
        //..(re-rendering the available positions, the Game Board, to move in to, in the next Turn). 
        //
        UpdateGridVisual();

    }// End Unit_OnAnyUnitDied
    
    #endregion Subscribing to Events - Delegates

    
    #endregion My Custom Methods

}
