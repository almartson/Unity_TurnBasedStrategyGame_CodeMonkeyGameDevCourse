/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/

using System;
using UnityEngine;


public class DestructibleCrate : MonoBehaviour
{

    #region Attributes

    [Tooltip("...")]
    [SerializeField]
    private int _myDefaultVar;
    
    /// <summary>
    /// GLocation in the GRID of the Destructible Prop to update (now it isn't there anymore)
    /// </summary>
    private GridPosition _gridPosition;
    
    #region Events

    public static event EventHandler OnAnyDestroyed;

    #endregion Events


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
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

    }// End Start


    /// <summary>
    /// Update is called once per frame
    /// </summary>


    #endregion Unity Methods


    #region My Custom Methods

    public void Damage()
    {
        Destroy(gameObject);
        
        // Listen to the event, and act:
        //
        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
    }

    #region Setters, Getters

    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }
    
    #endregion Setters, Getters

    #endregion My Custom Methods

}
