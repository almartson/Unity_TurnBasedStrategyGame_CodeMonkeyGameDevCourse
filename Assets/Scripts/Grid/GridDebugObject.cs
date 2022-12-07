using TMPro;
using UnityEngine;

/// <summary>
/// Class for (Visually) generating a Cell / Grid, only for Testing and Debugging matters.
/// </summary>
public class GridDebugObject : MonoBehaviour
{

    [SerializeField] private TextMeshPro _textMeshPro;
    
    public GridObject gridObject;


    #region Unity Methods

    // private void Start()
    // {
    //     _textMeshPro.text = gridObject.ToString();
    // }

    
    /// <summary>
    /// TODO: Fix: This Codemonkey's implementation is NOT Performant.
    /// </summary>
    private void Update()
    {
        
        // TODO: Fix: This Codemonkey's implementation is NOT Performant.
        //
        _textMeshPro.text = gridObject.ToString();
    }

    #endregion Unity Methods
    
    
    #region Custom Methods
    
    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }

    #endregion Custom Methods
}
