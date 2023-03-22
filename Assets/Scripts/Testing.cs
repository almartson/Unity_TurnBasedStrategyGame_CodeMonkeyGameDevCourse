using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{

    #region Attributes

    [SerializeField] private Unit _unit;

    
    [Tooltip("* TRUE: Use 'F Cost' as a CRITERIA in the end...\n\n * FALSE: Use 'G Cost' (may be an error, giving unexpected results...). NOTE: COdeMonkey used it in the video, (although I think he made a mistake...).")]
    [SerializeField]
    private bool _useTentativeFCostOrGCostAsCriteriaInTheEnd = false;

    
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

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            
            // Call the PATHFINDING ALGORITHM
            // 1- Initialize: Start Node
            //
            GridPosition startGridPosition = new GridPosition(0, 0);
            //
            // 2- Call the PATHFINDING ALGORITHM
            //
            List<GridPosition> gridPositionList = Pathfinding.Instance.FindPath(startGridPosition, mouseGridPosition, _useTentativeFCostOrGCostAsCriteriaInTheEnd);


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
            
        }//End if (Input.GetKeyDown(KeyCode.T))
        
    }//End Update

    
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
    
    
}
