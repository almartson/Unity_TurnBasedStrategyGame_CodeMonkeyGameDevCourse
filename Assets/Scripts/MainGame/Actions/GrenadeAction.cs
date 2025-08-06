/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using System;
using System.Collections.Generic;
using UnityEngine;


public class GrenadeAction : BaseAction
{

    #region Attributes

    [Tooltip("...")]
    [SerializeField]
    private int _myDefaultVar;

    
    
    #region BaseParameters (INPUT) for calling this action as a GENERIC ACTION, with the function:  TakeAction

    /// <summary>
    /// BaseParameters (INPUT) for calling this action as a GENERIC ACTION, with the function:  TakeAction
    /// </summary>
    private GrenadeActionBaseParameters _grenadeActionBaseParameters = new GrenadeActionBaseParameters();

    #endregion BaseParameters (INPUT) for calling this action as a GENERIC ACTION, with the function:  TakeAction


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

        ActionComplete();

    }//End Update()

    #endregion Unity Methods
    

    #region My Custom Methods

    public override string GetActionName()
    {
        return "Grenade";
    }

    public override void TakeAction(Action onActionComplete)
    {
        Debug.Log($"He did a GrenadeAction!");
        
        ActionStart(onActionComplete);
    }//End TakeAction

    
    /// <summary>
    /// (Calculates and...):  Gets the "A.I. ACTION" data ("Cost" Value, final, calculated "Points", to see if it's worth it...) that is possible in a given,  "Grid Position". <br /><br />
    /// Strategy: To Shoot to the "Weakest Player First"... that means: assigning more "Value" to the "GridPosition" where the Player with the "least amount of HEALTH" is located.
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <param name="initialAdditionalAIActionPointCostValueOfThisAction">_myAIFinalActionPointCostValueForAnyEnemyAIToDecideOnThisAction's INITIAL VALUE to add</param>
    /// <returns>A set of DATA  (note: specially the "Cost" of taking THIS ACTION...) for taking this selected ACTION.</returns>
    public override EnemyAIActionData GetEnemyAIActionData(GridPosition gridPosition, int initialAdditionalAIActionPointCostValueOfThisAction)
    {
        // Getting the "Weakest" Character (i.e.: Target) to Shoot at:  We need the Health of each Character of the Opposite Team:
        //
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        
        // ////////////////
        // Debug.Log($"(Before using 'targetUnit.GetDamageTakenOfHealthPercent()...' ->  )_myAIFinalActionPointCostValueForAnyEnemyAIToDecideOnThisAction = {_myAIFinalActionPointCostValueForAnyEnemyAIToDecideOnThisAction.ToString()} ... ... Attacker = {_unit} | Target = {_targetUnit} ");
        // ////////////////
        
        
        // Calculate the "Target"'s TOTAL DAMAGE TAKEN so far in the game..., and add it as a VALUE to the "Action WORTH-Value" (for the Enemy A.I. to decide on the Greatest one):
        //
        int unitTargetTotalDamageTaken = Mathf.RoundToInt(targetUnit.GetDamageTakenOfHealthPercent());

        // Execute the "Base Action" routine:
        //
        EnemyAIActionData enemyAIActionData = base.GetEnemyAIActionData(gridPosition, unitTargetTotalDamageTaken);

        
        // ////////////////
        // Debug.Log($"(Using 'targetUnit.GetDamageTakenOfHealthPercent()...' ->  )_myAIFinalActionPointCostValueForAnyEnemyAIToDecideOnThisAction = {_myAIFinalActionPointCostValueForAnyEnemyAIToDecideOnThisAction.ToString()} ... ... Attacker = {_unit} | Target = {_targetUnit} ");
        // ////////////////
        
        // Return DATA
        //
        return enemyAIActionData;
        
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
        _grenadeActionBaseParameters.TargetGridPositionOfSelectedAction =
            this._unit.GetFinalGridPositionOfNextPlayersAction();

    }//End GenerateInputParameters

    
    /// <summary>
    /// Get a List of the Valid places where the Unit/Character can 'TakeAction(...)' to (i.e.: GridPosition(s)).
    /// This method cycles through the squares/Grids...(using FOR )... to get a list of the valid ones.
    /// </summary>
    /// <returns>Valid (GridPosition(s)) places where the Unit/Character can TakeAction to, in this Turn.</returns>
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        // Validate that it can perform the Action in the same GridPosition it is standing NOW:
        // Get the Unit's GridPosition
        //
        GridPosition unitGridPosition = _unit.GetGridPosition();
        
        return new List<GridPosition>
        {
            unitGridPosition
        };
    }


    #endregion My Custom Methods
    
}//End GrenadeAction Class


/// <summary>
/// Concrete-particular Class (derived as a child of "BaseParameters") for the Input Parameters,
/// ..of every Function call to: 'TakeAction()'
/// </summary>
public class GrenadeActionBaseParameters : BaseParameters
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

}//End Class GrenadeActionBaseParameters