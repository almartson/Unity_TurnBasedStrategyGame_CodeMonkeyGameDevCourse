using TMPro;
using UnityEngine;

/// <summary>
/// Class for (Visually) generating a Cell / Grid, only for Testing and Debugging matters.
/// </summary>
public class GridDebugObject : MonoBehaviour
{

    [SerializeField] private TextMeshPro _textMeshPro;
    
    /// <summary>
    /// Generic C# Object, able to be casted into 'GridObject'.
    /// </summary>
    private object _gridObject;


    #region Unity Methods

    // private void Start()
    // {
    //     _textMeshPro.text = _gridObject.ToString();
    // }

    
    /// <summary>
    /// TODO: Fix: This Codemonkey's implementation is NOT Performant.
    /// </summary>
    protected virtual void Update()
    {
        
        // TODO: Fix: This Codemonkey's implementation is NOT Performant.
        //
        _textMeshPro.text = _gridObject.ToString();
    }

    #endregion Unity Methods
    
    
    #region Custom Methods
    
    public virtual void SetGridObject(object gridObject)
    {
        this._gridObject = gridObject;
    }

    #endregion Custom Methods
}
