using System;
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

    private void Start()
    {
        _textMeshPro.text = gridObject.ToString();
    }

    // private void Update()
    // {
    //     _textMeshPro.text = gridObject.ToString();
    // }

    #endregion Unity Methods
    
    
    #region Custom Methods
    
    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }

    #endregion Custom Methods
}
