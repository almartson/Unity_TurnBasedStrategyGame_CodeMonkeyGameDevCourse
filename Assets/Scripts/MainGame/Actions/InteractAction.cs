/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using System;
using System.Collections.Generic;
using UnityEngine;


public class InteractAction : BaseAction
{

    #region Attributes

    [Tooltip("[_grenadeProjectilePrefab]")]
    [SerializeField]
    private Transform _grenadeProjectilePrefab;
    
    #region BaseParameters (INPUT) for calling this action as a GENERIC ACTION, with the function:  TakeAction

    /// <summary>
    /// BaseParameters (INPUT) for calling this action as a GENERIC ACTION, with the function:  TakeAction
    /// </summary>
    private InteractActionBaseParameters _interactActionBaseParameters = new InteractActionBaseParameters();

    #endregion BaseParameters (INPUT) for calling this action as a GENERIC ACTION, with the function:  TakeAction

    
    #region Validations: of the Action
    
    /// <summary>
    /// Max DISTANCE, (number of Grid Cells) the character can 'Shoot' from, in one Turn.
    /// </summary>
    [SerializeField]
    private int _maxInteractDistance = 1;

    #endregion Validations: of the Action

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
    private void Update()
    {
        if (!_isActive)
        {
            return;
        }

    }//End Update()

    #endregion Unity Methods
    

    #region My Custom Methods

    public override string GetActionName()
    {
        return "Interact";
    }
    
    
    /// <summary>
    /// Makes the Payers Character (Unit): Shoot to the Target.
    /// </summary>
    public override void TakeAction(Action onActionComplete)
    {
        // 0- Get the Input Base Parameters (for this function call):
        //
        GenerateInputParameters();
        
        // 1- Get the DOOR from this GridPosition  (if a Door exists... there)
        //
        Door door = LevelGrid.Instance.GetDoorAtGridPosition(_interactActionBaseParameters.TargetGridPositionOfSelectedAction);
        
        // 2- If there is a DOOR:  INTERACT with it!
        // ...We are also calling the CallBack:   OnInteractComplete 
        //
        door.Interact( OnInteractComplete );
        
        // Callback, delegate broadcast:
        //
        ActionStart(onActionComplete);

    }//End TakeAction


    /// <summary>
    /// Callback to be called during TAKE ACTION.
    /// </summary>
    private void OnInteractComplete()
    {
        ActionComplete();
    }
    
    
    /// <summary>
    /// (Calculates and...):  Gets the "A.I. ACTION" data ("Cost" Value, final, calculated "Points", to see if it's worth it...) that is possible in a given,  "Grid Position". <br /><br />
    /// Strategy: To Shoot to the "Weakest Player First"... that means: assigning more "Value" to the "GridPosition" where the Player with the "least amount of HEALTH" is located.
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <param name="initialAdditionalAIActionPointCostValueOfThisAction">_myAIFinalActionPointCostValueForAnyEnemyAIToDecideOnThisAction's INITIAL VALUE to add</param>
    /// <returns>A set of DATA  (note: specially the "Cost" of taking THIS ACTION...) for taking this selected ACTION.</returns>
    public override EnemyAIActionData GetEnemyAIActionData(GridPosition gridPosition, int initialAdditionalAIActionPointCostValueOfThisAction)
    {

        // End the algorithm here
        //
        return base.GetEnemyAIActionData(gridPosition, 0);

    }// End GetEnemyAIActionData
    

    /// <summary>
    /// Generic Method for generating the necessary Input Parameters that are used in the calling of
    /// ..the Function Call to the generic: 'TakeAction'
    /// This must be reimplemented / overriden in each Concrete (derived, child).
    /// We need inside this class: <code>GridPosition</code>
    /// </summary>
    public override void GenerateInputParameters()
    {
        // Generate:
        //
        // 1- TARGET GridPosition (i.e.: the Destination of the Movement...)
        //
        // This works only for HUMAN PLAYERS... NOT for ENEMY A.I.:  _shootActionBaseParameters.TargetGridPositionOfSelectedAction = UnitActionSystem.Instance.GetSelectedUnit().GetFinalGridPositionOfNextPlayersAction();
        //
        // Getting the "GridPosition" of the Target, regardless of the Team that is playing (CPU or Player's):
        //
        _interactActionBaseParameters.TargetGridPositionOfSelectedAction =
            this._unit.GetFinalGridPositionOfNextPlayersAction();

    }//End GenerateInputParameters
    


    #region Action Validations

    /// <summary>
    /// Get a List of the Valid places where the Unit/Character can 'TakeAction(...)' to (i.e.: GridPosition(s)).
    /// This method cycles through the squares/Grids...(using FOR )... to get a list of the valid ones.
    /// </summary>
    /// <returns>Valid (GridPosition(s)) places where the Unit/Character can TakeAction to, in this Turn.</returns>
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        // Get the Unit's GridPosition
        //
        GridPosition unitGridPosition = _unit.GetGridPosition();


        // Cycle through the Rows and Columns (Cells in general) to find the Valid ones for Tak(ing)Action (i.e.: Shooting...) to.. in this Turn
        //
        for (int x = -_maxInteractDistance; x <= _maxInteractDistance; x++)
        {
            for (int z = -_maxInteractDistance; z <= _maxInteractDistance; z++)
            {
                // Create a GridPosition to Validate it:
                //
                GridPosition offsetGridPosition = new GridPosition(x, z);

                // All Actions are attached to an Unit, so we can get a reference to an Unit from this class/object and then from Unit to -> its Position / Grid.
                // Test a given GridPosition, moving it a little bit using the 'offsetGridPosition' (summing it, +), so we can Validate it:
                //
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                
                // Validation:
                //
                // 1- "GridPosition" Must be inside the Grid System, not off-limits:
                //
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    // Not Valid: continue / SKIP: to the NEXT ITERATION.
                    continue;
                }

                // Validation:
                // 2- Is there a DOOR?   (to interact with..?)
                //
                Door door = LevelGrid.Instance.GetDoorAtGridPosition(testGridPosition);

                if (door == null)
                {
                    // No Door on this GridPosition     (Null Door)
                    continue;
                }
                
                // Finally, Conclusion: Add the Tested & Valid GridPosition to the Local VALID List
                //
                validGridPositionList.Add(testGridPosition);

            } // End for 2
        }//End for 1
        
        // Get a List of the Valid places where the Unit/Character can 'TakeAction(...)' to (i.e.: GridPosition(s)).
        //
        return validGridPositionList;

    }// End GetValidActionGridPositionList
    
    #endregion Action Validations

    
    #region Misc, Getters, Setters, etc
    

    /// <summary>
    /// Gets the <code>_maxInteractDistance</code>.
    /// </summary>
    /// <returns>_maxInteractDistance</returns>
    public int GetMaxInteractDistance()
    {
        return _maxInteractDistance;
    }
    
    #endregion Misc, Getters, Setters, etc

    
    #endregion My Custom Methods
    
}//End InteractAction Class


/// <summary>
/// Concrete-particular Class (derived as a child of "BaseParameters") for the Input Parameters,
/// ..of every Function call to: 'TakeAction()'
/// </summary>
public class InteractActionBaseParameters : BaseParameters
{

    #region Attributes

    /// <summary>
    /// Destination-Target Position for this ACTION, of the Player's Unit, in the Cells-Grid.
    /// </summary>
    private GridPosition _targetGridPositionOfSelectedAction;
    //
    /// <summary>
    /// Property Accessor to Private Field "_targetGridPositionOfSelectedAction": <br /><br />
    /// Destination-Target Position for this ACTION, of the Player's Unit, in the Cells-Grid. <br />
    /// </summary>
    /// <value></value>
    public GridPosition TargetGridPositionOfSelectedAction { get => _targetGridPositionOfSelectedAction; set => _targetGridPositionOfSelectedAction = value; }
    
    #endregion Attributes
    
    
    #region Methods

    

    #endregion Methods

}//End Class InteractActionBaseParameters