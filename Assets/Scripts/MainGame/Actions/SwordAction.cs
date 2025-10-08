/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using System;
using System.Collections.Generic;
using UnityEngine;


public class SwordAction : BaseAction
{

    #region Attributes

    #region Events, Listeners

    public static event EventHandler OnAnySwordHit;

    public event EventHandler OnSwordActionStarted;
    
    public event EventHandler OnSwordActionCompleted;
    
    #endregion Events, Listeners
    
    
    private enum State 
    { 
        SwingingSwordBeforeHit,
        SwingingSwordAfterHit,
    }

    private State _state;
    private float _stateTimer;
    
    #region Validations: of the Action
    
    /// <summary>
    /// Max Sword Distance \n Max DISTANCE, (number of Grid Cells) the character can Attack from, in one Turn.
    /// </summary>
    [SerializeField]
    private int _maxSwordDistance = 1;

    #endregion Validations: of the Action

    #region BaseParameters (INPUT) for calling this action as a GENERIC ACTION, with the function:  TakeAction

    /// <summary>
    /// BaseParameters (INPUT) for calling this action as a GENERIC ACTION, with the function:  TakeAction
    /// </summary>
    private SwordActionBaseParameters _swordActionBaseParameters = new SwordActionBaseParameters();

    #endregion BaseParameters (INPUT) for calling this action as a GENERIC ACTION, with the function:  TakeAction

    /// <summary>
    /// Target Unit character.
    /// </summary>
    private Unit _targetUnit;
    
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
        
        
        // Set the Timer: (To change STATES correctly)
        // Decrement the Time this STATE has left (until ZERO (0))
        //
        _stateTimer -= Time.deltaTime;
        
        // Checking the States of the Game's Shooting Action:
        //
        switch (_state)
        {
            case State.SwingingSwordBeforeHit:
                
                // Rotate towards the TARGET, and Aim at it:
                // .1- Direction Vector3 to shoot (normalized)
                //
                Vector3 aimDirection = ( _targetUnit.GetWorldPosition() - _unit.GetWorldPosition() ).normalized;
                //
                // .2- Rotate, Animation:
                //
                float rotateSpeed = 10.0f;
                RotateUnitUsingVector3SlerpApproach( aimDirection, rotateSpeed );

                break;
            
            case State.SwingingSwordAfterHit:

                break;

            
            default:
                Debug.LogError("Reached an Exceptional 'case' in the Switch - Case... in Class: '" + GetType().Name + "'!.");
                break;
        }//End switch (_state)
        
        // ENDING of every STATE:
        // Set the (variables for) Ending of this State:
        // => BEGINNING of Next state:
        //
        if (_stateTimer <= 0f)
        {

            NextState();

        }//End if (_stateTimer <= 0f)

        
    }//End Update

    #endregion Unity Methods
    

    #region My Custom Methods


    public override string GetActionName()
    {
        return "Sword";
    }
    
     /// <summary>
    /// (Calculates and...):  Gets the "A.I. ACTION" data ("Cost" Value, final, calculated "Points", to see if it's worth it...) that is possible in a given,  "Grid Position". <br /><br />
    /// Strategy: To Shoot to the "Weakest Player First"... that means: assigning more "Value" to the "GridPosition" where the Player with the "least amount of HEALTH" is located.
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <param name="initialAdditionalAIActionPointCostValueOfThisAction">[200 is suggested]_myAIFinalActionPointCostValueForAnyEnemyAIToDecideOnThisAction's INITIAL VALUE to add</param>
    /// <returns>A set of DATA  (note: specially the "Cost" of taking THIS ACTION...) for taking this selected ACTION.</returns>
    public override EnemyAIActionData GetEnemyAIActionData(GridPosition gridPosition, int initialAdditionalAIActionPointCostValueOfThisAction)
    {
        // Getting the "Weakest" Character (i.e.: Target) to Shoot at:  We need the Health of each Character of the Opposite Team:
        //
        //Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        
        // Execute the "Base Action" routine:
        //
        EnemyAIActionData enemyAIActionData = base.GetEnemyAIActionData(gridPosition, initialAdditionalAIActionPointCostValueOfThisAction);

        
        // ////////////////
        // Debug.Log($"(Using 'targetUnit.GetDamageTakenOfHealthPercent()...' ->  )_myAIFinalActionPointCostValueForAnyEnemyAIToDecideOnThisAction = {_myAIFinalActionPointCostValueForAnyEnemyAIToDecideOnThisAction.ToString()} ... ... Attacker = {_unit} | Target = {_targetUnit} ");
        // ////////////////
        
        // Return DATA
        //
        return enemyAIActionData;
        
    }// End GetEnemyAIActionData
    
    public override void TakeAction(Action onActionComplete)
    {
        // 0- Get the Input Base Parameters (for this function call):
        //
        GenerateInputParameters();
        
        // Define the target unit
        //
        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(_swordActionBaseParameters.TargetGridPositionOfSelectedAction);
        
        // ENDING of this STATE:
        // Set the (variables for) Ending of this State:
        // => BEGINNING of Next state
        //
        // 1- Change the STATE variable to -> NEXT State
        //
        _state = State.SwingingSwordBeforeHit;
                
        // 2- Set THAT (Next..) STATE's Timer
        //
        float beforeHitStateTimer = 0.7f;
        _stateTimer = beforeHitStateTimer;
        
        // Most important:
        // 3- Set the Animation TRIGGER (event):
        //
        OnSwordActionStarted?.Invoke(this, EventArgs.Empty);
        
        ActionStart(onActionComplete);

    }//End TakeAction


    public override void GenerateInputParameters()
    {
        // Generate:
        //
        // 1- TARGET GridPosition (i.e.: the Destination of the Movement...)
        //
        // This works only for HUMAN PLAYERS... NOT for ENEMY A.I.:  _swordActionBaseParameters.TargetGridPositionOfSelectedAction = UnitActionSystem.Instance.GetSelectedUnit().GetFinalGridPositionOfNextPlayersAction();
        //
        // Getting the "GridPosition" of the Target, regardless of the Team that is playing (CPU or Player's):
        //
        _swordActionBaseParameters.TargetGridPositionOfSelectedAction =
            this._unit.GetFinalGridPositionOfNextPlayersAction();

    }//End GenerateInputParameters

    
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        // Get the Unit's GridPosition
        //
        GridPosition unitGridPosition = _unit.GetGridPosition();


        // Cycle through the Rows and Columns (Cells in general) to find the Valid ones for Tak(ing)Action (i.e.: Shooting...) to.. in this Turn
        //
        for (int x = -_maxSwordDistance; x <= _maxSwordDistance; x++)
        {
            for (int z = -_maxSwordDistance; z <= _maxSwordDistance; z++)
            {
                // Create a GridPosition to Validate it:
                //
                GridPosition offsetGridPosition = new GridPosition(x, z, 0);

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
                
                if (testDistance > ((_maxSwordDistance * _maxSwordDistance) + _maxSwordDistance + 0.25f))
                {
                    continue;
                }
                /////////////////
                
                // 2- "GridPosition" MUST be previously occupied  (by the ENEMY of the current's TURN UNIT'S TEAM).
                //
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Not Valid:   Grid Position is EMPTY, no Unit.
                    // Skip to next iteration:
                    //
                    continue;
                }
                
                // 3- Check to see if there is an Unit of MY SAME TEAM, A FRIENDLY Unit in this GRID / CELL:   (so we do NOT Shoot at it)
                //
                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                //
                // Check:  Are you an 'Enemy' like myself?... or:  Are you a 'Player'
                //..as myself?
                //
                if ((targetUnit.IsEnemy() && _unit.IsEnemy()) || ((!targetUnit.IsEnemy()) && (!_unit.IsEnemy())) || (targetUnit == _unit))
                {
                    // Not Valid:   Both Units on same 'Team'.
                    // Skip to next iteration:
                    //
                    continue;
                }

                #region Experimental Validation:  Can not shoot behind WALLS or OBSTACLES
                
                // // Validate: Can NOT shoot behind WALLS or OBSTACLES
                // // TODO: put this Variable in a correct class, following the S.O.L.I.D. Principle:
                // //
                // // float shoulderHeightForLineOfSight = _unit.ShoulderHeightForUnitCharacter;
                // //
                // if (GridSystemVisual.Instance.ValidateIsBlockedTheLineOfSightBetweenTwoGridPositions(unitGridPosition, testGridPosition,  _unit.ShoulderHeightForUnitCharacter, GridSystemVisual.Instance.ObstaclesLayerMask))
                // {
                //     continue;
                // }

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
    
    
    #region Finite State Machine Methods
    
    /// <summary>
    /// Handles the Finite State Machine STATES. <br />
    /// ENDING of any STATE related to the SHOOTING ACTION. <br />
    /// Changes the variables to the NEXT STATE, whenever the Timer for that specific State reaches to ZERO (0).
    /// </summary>
    private void NextState()
    {
        // Checking the States of the Game's Shooting Action:
        //
        switch (_state)
        {
            case State.SwingingSwordBeforeHit:
                
                // ENDING of this STATE:
                // Set the (variables for) Ending of this State:
                // => BEGINNING of Next state
                //
                // 1- Change the STATE variable to -> NEXT State
                //
                _state = State.SwingingSwordAfterHit;
                
                // 2- Set THAT (Next..) STATE's Timer
                //
                float afterTheHitStateTimer = 0.5f;
                _stateTimer = afterTheHitStateTimer;
                
                // Apply actual DAMAGE related to the action + animation:
                //
                _targetUnit.Damage(100);
                
                // Register (that a Sword Hit is about to be performed) and invoke the event:
                //
                OnAnySwordHit?.Invoke(this, EventArgs.Empty);

                break;
            
            case State.SwingingSwordAfterHit:
                
                // ENDING of this STATE:
                // Set the Animation TRIGGER (event): to END / IT'S COMPLETED:
                //
                OnSwordActionCompleted?.Invoke(this, EventArgs.Empty);
                
                // Set the (variables for) Ending of this State:
                // => BEGINNING of Next state:  Nothing, ACTION IS COMPLETED!
                //
                ActionComplete();
                
                break;
            
            default:
                Debug.LogError("Reached an Exceptional 'case' in the Switch - Case... in Class: '" + GetType().Name + "'!.");
                break;
        }//End switch (_state)
        
        
        // // Debug, remove soon
        // //
        // Debug.Log(_state);
        
    }//End NextState(...)
    
    
    #endregion Finite State Machine Methods
    
    #region Rotation: LERP vs. SLERP

    ///  <summary>
    ///  Quaternions + Spherical Interpolation, SLERP, (Quaternions behind the Scenes):
    /// ...it rotates in a better way (first Rotates, then Walks):
    ///  </summary>
    ///  <param name="moveDirection"></param>
    ///  <param name="rotateSpeed"></param>
    private void RotateUnitUsingVector3SlerpApproach(Vector3 moveDirection, float rotateSpeed)
    {
        // Quaternions + Spherical Interpolation, SLERP, (Quaternions behind the Scenes):
        //...it rotates in a better way (first Rotates, then Walks):
        //
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }
    
    #endregion Rotation: LERP vs. SLERP 

    
    #region Getters, Setters

    /// <summary>
    /// Gets the <code>_maxSwordDistance</code>.
    /// </summary>
    /// <returns>_maxThrowDistance</returns>
    public int GetMaxAttackDistance()
    {
        return this._maxSwordDistance;
    }

    #endregion Getters, Setters
    
    #endregion My Custom Methods

}// End SwordAction


/// <summary>
/// Concrete-particular Class (derived as a child of "BaseParameters") for the Input Parameters,
/// ..of every Function call to: 'TakeAction()'
/// </summary>
public class SwordActionBaseParameters : BaseParameters
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

}//End Class SwordActionBaseParameters