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


    #region Colors: Grid Cells

    #region Color and Material VARIABLES / FIELDS

    [Tooltip("List of Materials / Colors associated to the Numbered: (Grid Positions...): Ground Squares of the Game Board.")]
    [SerializeField]
    private List<GridVisualTypeMaterial> _gridVisualTypeMaterials;

    #endregion Color and Material VARIABLES / FIELDS
    
    
    #region Color and Material Types

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
    
    /// <summary>
    /// Type of Colors used for the Grid Position Visuals / GUI on the ground, for the squares (i.e.: each Color is meant to represent a Possible ACTION that is currently selected).
    /// </summary>
    public enum GridVisualColorType { White, Green, Blue, Red, Yellow, }
    
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
    /// Shows the Visual cues (GridPositions) passed in as Input.
    /// </summary>
    /// <param name="gridPositionList"></param>
    public void ShowGridPositionList(List<GridPosition> gridPositionList)
    {
        // Cycle through the items in the List of: <GridPosition> 
        //
        for (int i = 0; i < gridPositionList.Count; i++)
        {

            // Show the selected items (GridPositionsss) passed as Input: so they are VISIBLE, (in the Scene :)
            //
            _gridSystemVisualSingleArray[gridPositionList[i].x, gridPositionList[i].z].Show();

        }//End for 1
    }

    
    /// <summary>
    /// Updates (Re-renders) the Grid System Visual cues about: available Cells to 'Take Action' to (...or to move in to), in this turn.
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

        // 3- Final- Show only the VALID Cells of the Grid, to 'Take Action' to (...or to move in to),  (based on the Player's SELECTED Unit):
        //
        ShowGridPositionList( selectedAction.GetValidActionGridPositionList() );

    }//End UpdateGridVisual()

    
    #region Subscribing Events - Delegates

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

    #endregion Subscribing Events - Delegates

    
    #endregion My Custom Methods

}
