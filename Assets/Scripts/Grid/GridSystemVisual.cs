using System;
using Unity.Mathematics;
using UnityEngine;


public class GridSystemVisual : MonoBehaviour
{

    #region Attributes

    [Tooltip("(Prefab) Visual representation of the Grid Positions where the Unit can move, in the current Turn")]
    [SerializeField]
    private Transform _gridSystemVisualSinglePrefab;
    


    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start()
    {
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
                Instantiate(_gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition),
                    Quaternion.identity);
                
            }//End for 2
        }//End for 1
    }// End Start


    /// <summary>
    /// Update is called once per frame
    /// </summary>


    #endregion Unity Methods
    

    #region My Custom Methods





    #endregion My Custom Methods

}
