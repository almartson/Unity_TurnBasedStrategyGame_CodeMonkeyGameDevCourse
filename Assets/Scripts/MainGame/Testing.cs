using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Testing : MonoBehaviour
{

    #region Attributes

    [SerializeField] private Unit _unit;

    
    
    #region Show the PATHFINDING ALGORITHM
    
    #endregion Show the PATHFINDING ALGORITHM
    
    
    #endregion Attributes
    

    // /// <summary>
    // /// Start is called before the first frame update
    // /// </summary>
    // void Awake()
    // {  }

    
    private void Update()
    {
        // If the user presses "T" then Validate all Possible Moves to the ADJACENT SQUARES / GRID CELLS:
        //
        // Show the PATHFINDING ALGORITHM
        //
        if (Input.GetKeyDown(KeyCode.T))
        {
            
            
            #region Show the PATHFINDING ALGORITHM

            ShowBestPathFromSelectedUnitToMousePosition();

            #endregion Show the PATHFINDING ALGORITHM

        }//End if (Input.GetKeyDown(KeyCode.T))
        
    }//End Update

    
    #region Old Update Code Snippets
    
    // private void Update()
    // {
    //     // If the user presses "T" then Validate all Possible Moves to the ADJACENT SQUARES / GRID CELLS.
    //     //
    //     if (Input.GetKeyDown(KeyCode.T))
    //     {
    //
    //         // Hide all Grid Positions Prefab's Visual cues..:
    //         //
    //         GridSystemVisual.Instance.HideAllGridPositions();
    //         
    //         // ShowAndSetMaterial only the VALID Cells of the Grid, to move in:
    //         //
    //         GridSystemVisual.Instance.ShowGridPositionList(_unit.GetMoveAction().GetValidActionGridPositionList(), GridSystemVisual.GridVisualColorType.White);
    //
    //     }//End if
    //     
    // }//End Update
    
    #endregion Old Update Code Snippets

    
    #region My Custom Methods

    #region Show the PATHFINDING ALGORITHM
    
    /// <summary>
    /// Shows the 'Best Path' from the selected Unit, to the current Mouse Position (a GridPosition on the Map). 
    /// </summary>
    public void ShowBestPathFromSelectedUnitToMousePosition()
    {
        // Show the PATHFINDING ALGORITHM
        //
        GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
        
        // Call the PATHFINDING ALGORITHM
        // 1- Initialize: Start Node
        //
        GridPosition startGridPosition;
        //
        if (TurnSystem.Instance.IsPlayerTurn)
        {
            startGridPosition = UnitActionSystem.Instance.GetSelectedUnit().GetGridPosition();
        }
        else
        {
            startGridPosition = UnitManager.Instance.GetEnemyUnitList().First().GetGridPosition();
        }//
        //
        if ( startGridPosition == null )
        {
            startGridPosition = new GridPosition(0, 0);
        }//
        //
        // 2- Call the PATHFINDING ALGORITHM
        //
        List<GridPosition> gridPositionList = Pathfinding.Instance.FindPath(startGridPosition, mouseGridPosition, out int pathLength);

        // Visual (Cue) Feedback:  Draw Lines on the ground
        //..representing the BEST PATH
        //
        if (gridPositionList != null)
        {
        
            int gridPositionListCount = gridPositionList.Count;
            //
            // Draw Lines on the ground
            //
            for (int i = 0; i < gridPositionListCount - 1; i++)
            {
                // Draw Lines
                //
                Debug.DrawLine(
                    LevelGrid.Instance.GetWorldPosition(gridPositionList[i]),
                    LevelGrid.Instance.GetWorldPosition(gridPositionList[i + 1]),
                    Color.white,
                    10.0f
                );
            
            }//End for
            
        }//End if (gridPositionList != null)

    }//End ShowPathFromSelectedUnitToMousePosition
                
    #endregion Show the PATHFINDING ALGORITHM

    #endregion My Custom Methods
    
}
