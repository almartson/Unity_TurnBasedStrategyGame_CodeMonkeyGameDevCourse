/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/

using System;
using UnityEngine;


public class PathfindingUpdater : MonoBehaviour
{

    #region Attributes

    [Tooltip("...")]
    [SerializeField]
    private int _myDefaultVar;


    

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
        DestructibleCrate.OnAnyDestroyed += DestructibleCrate_OnAnyDestroyed;

    }//End Start


    /// <summary>
    /// Update is called once per frame
    /// </summary>


    #endregion Unity Methods


    #region My Custom Methods

    #region Events

    private void DestructibleCrate_OnAnyDestroyed(object sender, EventArgs e)
    {
        // Get the data about the "GridPosition" and "Pathfinding Node" from the sender:
        //
        DestructibleCrate destructibleCrate = sender as DestructibleCrate;        
        
        // Set that the PROP that was there (A Crate?) is not there anymore:
        // That GridPosition is WALKABLE now.
        //
        Pathfinding.Instance.SetIsWalkableGridPosition(destructibleCrate.GetGridPosition(), true);
        
    }//End event DestructibleCrate_OnAnyDestroyed
    
    #endregion Events



    #endregion My Custom Methods

}
