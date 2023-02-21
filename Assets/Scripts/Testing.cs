using UnityEngine;

public class Testing : MonoBehaviour
{
    #region Attributes

    [SerializeField] private Unit _unit;
    
    
    #endregion Attributes
    

    // /// <summary>
    // /// Start is called before the first frame update
    // /// </summary>
    // void Awake()
    // {  }


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
