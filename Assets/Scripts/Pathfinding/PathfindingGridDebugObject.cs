/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using TMPro;
using UnityEngine;

/// <summary>
/// Class for (Visually) generating a Cell / Grid, only for Testing and Debugging the "Pathfinding" and Nodes ("PathNode" s) Algorithm.
/// </summary>
public class PathfindingGridDebugObject : GridDebugObject
{

    #region Attributes

    /// <summary>
    /// <code>G</code> <br />
    /// 
    /// Walking "Cost" from the Start Node.
    /// </summary>
    [Tooltip("G \n\nWalking 'Cost' from the Start Node.")]
    [SerializeField]
    private TextMeshPro _gCostText;
    
    /// <summary>
    /// <code>H</code> <br />
    /// 
    /// Heuristic "Cost" to reach the "End Node", (from the current "Node"), assuming there are no Walls or Obstacles (i.e.: it can be seen as the simplest or straight or narrow path towards: the "End Node").
    /// </summary>
    [Tooltip("H \n\nHeuristic 'Cost' to reach the 'End Node', (from the current 'Node'), assuming there are no Walls or Obstacles (i.e.: it can be seen as the simplest or straight or narrow path towards: the 'End Node').")]
    [SerializeField]
    private TextMeshPro _hCostText;
    
    /// <summary>
    /// <code>F = G + H</code> <br />
    /// 
    /// The Cost of the "Optimal Route". <br />
    /// We are trying to go through the PATH WHERE = F is the Lowest in each NEXT NODE... <br />
    /// 1. Select the **NEXT NODE** based on: <br />
    ///   1. **F** always has to be the **smallest option**. <br />
    /// 2. **G** always **Increases**  (in each "NEXT NODE"). <br />
    /// 3. **H** always **Decreases**  (in each "NEXT NODE"). <br />
    /// </summary>
    [Tooltip("F = G + H \n\nThe Cost of the 'Optimal Route'. \nWe are trying to go through the PATH WHERE = F is the Lowest in each NEXT NODE... \n  1. Select the **NEXT NODE** based on: \n   1. **F** always has to be the **smallest option**. \n 2. **G** always **Increases**  (in each 'NEXT NODE'). \n 3. **H** always **Decreases**  (in each 'NEXT NODE'). \n")]
    [SerializeField]
    private TextMeshPro _fCostText;

    
    [Tooltip("Debug Visual Cue for representing the Walkable Nodes, for Debugging the Pathfinding Algorithm in this Game.")]
    [SerializeField]
    private SpriteRenderer _isWalkableSpriteRenderer;
    

    #region Game Board - Node Cells

    
    /// <summary>
    /// Node of the Pathfinding Game Board.
    /// </summary>
    private PathNode _pathNode;
    
    #endregion Game Board - Node Cells
    
    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>



    /// <summary>
    /// Start is called before the first frame update
    /// </summary>



    /// <summary>
    /// Update is called once per frame
    /// </summary>
    protected override void Update()
    {
        // Base Implementation:
        //
        base.Update();
        
        // Paint also the G, H, and F Costs of the Pathfinding calculations:
        //
        _gCostText.text = _pathNode.GetGCost().ToString();
        _hCostText.text = _pathNode.GetHCost().ToString();
        _fCostText.text = _pathNode.GetFCost().ToString();
        
        // Render also a Big SpriteRenderer Quad, to check if this is Walkable or Non-Walkable NodePath (i.e.: GridObject)
        // Set the color:
        //...GREEN => WALKABLE  or  RED => NON-WALKABLE  'NodePath'
        //
        _isWalkableSpriteRenderer.color = _pathNode.IsWalkable() ? Color.green : Color.red;

    }// End Update

    
    #endregion Unity Methods
    

    #region My Custom Methods

    /// <summary>
    /// Sets the Cells / Nodes of the ground (game board), i.e.: for painting them... with numbers such as: G, H, F, etc...
    /// </summary>
    /// <param name="gridObject"></param>
    public override void SetGridObject(object gridObject)
    {
        
        // Base implementation:
        //
        base.SetGridObject(gridObject);
        
        // Set the Game Board:  Nodes
        //
        _pathNode = (PathNode) gridObject;
        
    }// End SetGridObject

    #endregion My Custom Methods

}
