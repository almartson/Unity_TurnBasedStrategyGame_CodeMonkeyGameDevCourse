/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using System;
using System.Collections.Generic;
using UnityEngine;


public class GrenadeAction : BaseAction
{

    #region Attributes

    [Tooltip("[_grenadeProjectilePrefab]")]
    [SerializeField]
    private Transform _grenadeProjectilePrefab;
    
    #region BaseParameters (INPUT) for calling this action as a GENERIC ACTION, with the function:  TakeAction

    /// <summary>
    /// BaseParameters (INPUT) for calling this action as a GENERIC ACTION, with the function:  TakeAction
    /// </summary>
    private GrenadeActionBaseParameters _grenadeActionBaseParameters = new GrenadeActionBaseParameters();

    #endregion BaseParameters (INPUT) for calling this action as a GENERIC ACTION, with the function:  TakeAction

    
    #region Validations: of the Action
    
    /// <summary>
    /// Max DISTANCE, (number of Grid Cells) the character can 'Shoot' from, in one Turn.
    /// </summary>
    [SerializeField]
    private int _maxThrowDistance = 4;

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

        //ActionComplete();

    }//End Update()

    #endregion Unity Methods
    

    #region My Custom Methods

    public override string GetActionName()
    {
        return "Grenade";
    }

    /// <summary>
    /// Event / Callback to announce the "END" of the:
    /// .."Shooting a Grenade" process. 
    /// </summary>
    private void OnGrenadeBehaviourComplete()
    {
        ActionComplete();

    }//End GrenadeBehaviourComplete
    
    
    /// <summary>
    /// Makes the Payers Character (Unit): Shoot to the Target.
    /// </summary>
    public override void TakeAction(Action onActionComplete)
    {

        // 0- Get the Input Base Parameters (for this function call):
        //
        GenerateInputParameters();

        // Instantiate the Projectile:
        //
        Transform grenadeProjectileTransform = Instantiate(_grenadeProjectilePrefab, _unit.GetWorldPosition(), Quaternion.identity);
        
        // Get its: "Grenade Projectile" Component.
        //
        GrenadeProjectile grenadeProjectile = grenadeProjectileTransform.GetComponent<GrenadeProjectile>();
        
        // Initialize the Projectile:
        //
        grenadeProjectile.Setup(_grenadeActionBaseParameters.TargetGridPositionOfSelectedAction, OnGrenadeBehaviourComplete);
        
        // Callback, delegate broadcast:
        //
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
        for (int x = -_maxThrowDistance; x <= _maxThrowDistance; x++)
        {
            for (int z = -_maxThrowDistance; z <= _maxThrowDistance; z++)
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
                

                /////////// TODO ////////// Temporary: Circular shape made with square pixels:
                // Todo: Standardize this code, with a proper Architecture (a class + its own function, maybe a Singleton and a Function inside, etc).
                //
                int testDistance = (x * x) + (z * z);
                
                if (testDistance > ((_maxThrowDistance * _maxThrowDistance) + _maxThrowDistance + 0.25f))
                {
                    continue;
                }
                /////////////////
                
                // // 2- "GridPosition" MUST be previously occupied  (by the ENEMY of the current's TURN UNIT'S TEAM).
                // //
                // if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                // {
                //     // Not Valid:   Grid Position is EMPTY, no Unit.
                //     // Skip to next iteration:
                //     //
                //     continue;
                // }
                //
                // // 3- Check to see if there is an Unit of MY SAME TEAM, A FRIENDLY Unit in this GRID / CELL:   (so we do NOT Shoot at it)
                // //
                // Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                // //
                // // Check:  Are you an 'Enemy' like myself?... or:  Are you a 'Player'
                // //..as myself?
                // //
                // if ((targetUnit.IsEnemy() && _unit.IsEnemy()) || ((!targetUnit.IsEnemy()) && (!_unit.IsEnemy())) || (targetUnit == _unit))
                // {
                //     // Not Valid:   Both Units on same 'Team'.
                //     // Skip to next iteration:
                //     //
                //     continue;
                // }

                #region Experimental Validation:  Can not shoot behind WALLS or OBSTACLES
                
                // Validate: Can NOT shoot behind WALLS or OBSTACLES
                // TODO: put this Variable in a correct class, following the S.O.L.I.D. Principle:
                //
                // float shoulderHeightForLineOfSight = _unit.ShoulderHeightForUnitCharacter;
                //
                if (GridSystemVisual.Instance.ValidateIsBlockedTheLineOfSightBetweenTwoGridPositions(unitGridPosition, testGridPosition,  _unit.ShoulderHeightForUnitCharacter, GridSystemVisual.Instance.ObstaclesLayerMask))
                {
                    continue;
                }

                #endregion Experimental Validation:  Can not shoot behind WALLS or OBSTACLES
                
                
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
    /// Gets the (Character) Unit, that is the TARGET.
    /// </summary>
    /// <returns></returns>
    // public Unit GetTargetUnit()
    // {
    //     return _targetUnit;
    // }

    /// <summary>
    /// Gets the <code>_maxThrowDistance</code>.
    /// </summary>
    /// <returns>_maxThrowDistance</returns>
    public int GetMaxShootDistance()
    {
        return _maxThrowDistance;
    }
    
    #endregion Misc, Getters, Setters, etc

    
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