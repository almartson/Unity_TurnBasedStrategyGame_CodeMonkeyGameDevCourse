/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This concrete class, (derived from BaseAction), handles the execution of the Shooting Action (Animations, timers, stages of the animation itself - even if it is a chain of animations tied up to each other and triggered together, - etc) <br />
/// Here it is implemented a State Machine, handling STATES which are on an Enum variable, for the Animation STATES of the 'Shooting'.
/// </summary>
[System.Serializable]
public class ShootAction : BaseAction
{
    #region Attributes

    #region State Machine Pattern (simplified by using switch - case)
    
    /// <summary>
    /// States (..Machine..) of this 'Shoot' Action
    /// </summary>
    private enum State
    {
        Aiming,     /* Start of the Shooting Animation, it is a Rotation Animation towards the Enemy */
        Shooting,   /* Animation: Shooting one (or multiple...) Bullet(s)  */
        Cooloff,    /* Delay, ending of the Animations */
    }

    /// <summary>
    /// Specific State (i.e.: in our State Machine): of our currently selected 'Unit'
    ///...for the Shooting ACTION.
    /// </summary>
    private State _state;
    
    #endregion State Machine Pattern (simplified by using switch - case)
    
    
    #region Validations: of the Action
    
    /// <summary>
    /// Max DISTANCE, (number of Grid Cells) the character can 'Shoot' from, in one Turn.
    /// </summary>
    [SerializeField]
    private int _maxShootDistance = 7;

    
    // This is in the Class:  'GridSystemVisual'
    // #region Obstacles for Shooting (Experimental)
    //
    // [Tooltip("Obstacles Label-LayerMask in the AIM or 'Shooting Path'")]
    // [SerializeField]
    // private LayerMask _obstaclesLayerMask;
    // //
    // /// <summary>
    // /// Getter and Setter Property for Field: _obstaclesLayerMask
    // /// </summary>
    // public LayerMask ObstaclesLayerMask { get => _obstaclesLayerMask; private set => _obstaclesLayerMask = value; }
    //
    // #endregion Obstacles for Shooting (Experimental)
    

    #endregion Validations: of the Action

    
    #region Damage (to the Health System of the Target) by each Shoot

    [Tooltip("Damage (to the Health System of the Target) by each Shoot")]
    [SerializeField]
    private int _shootPowerDamageInHealthPoints = 40;
    
    #endregion Damage (to the Health System of the Target) by each Shoot
    
    
    #region Taking the Action
    
    /// <summary>
    /// Timer to set the change between Transitions and States of this (Shooting) ACTION.
    /// </summary>
    private float _stateTimer;
    
    /// <summary>
    /// Unit / Character we will Shoot To. <br />
    /// NOTES: <br />
    /// 1- WARNING:  It is usually NULL during the preparatory stage and even the VALIDATION stages..., until the SHOOT ACTION is finally started (a.k.a.: "TakeAction()"  method).
    /// </summary>
    private Unit _targetUnit;

    /// <summary>
    /// Flag to activate / enable the Animation for Shooting a Bullet.
    /// </summary>
    private bool _canShootBullet;
    
    
    #region Animator & Animations

    #region Misc, Physics, Directional Vectors, etc
    
    /// <summary>
    /// Direction (Vector3) of the Character's (Unit) Animation AIM. 
    /// </summary>
    private Vector3 _aimDirection;
    /// <summary>
    /// Field (Property Accessor) to: _aimDirection
    /// </summary>
    public Vector3 AimDirection { get => _aimDirection;
        private set => _aimDirection = value;
    }
    
    #endregion Misc, Physics, Directional Vectors, etc
    
    
    #region Events - Listeners - CallBack
    
    /// <summary>
    /// CallBack Listener (Delegate Function) for executing when Starting the Animation (i.e.: Shoot).
    /// </summary>
    public event EventHandler<OnShootAnimationEventArgs> OnShootAnimation;
    
    #region Custom Event Class - redefinition - customization

    /// <summary>
    /// Custom EventArgs Class for handling custom Events with some custom
    /// ...Input - Output Parameters (to be used by the Functions that are to be triggered).
    /// </summary>
    public class OnShootAnimationEventArgs : EventArgs
    {
        /// <summary>
        /// Unit / Character that is the TARGET.
        /// </summary>
        public Unit targetUnit;
        
        /// <summary>
        /// Unit / Character that shoots / fires the Bullet.
        /// </summary>
        public Unit shootingUnit;

    }//End public class OnShootAnimationEventArgs : EventArgs
    
    #endregion Custom Event Class - redefinition - customization
    
    #endregion Events - Listeners - CallBack

    #endregion Animator & Animations
    
        
    #region Rotations
    
    [Tooltip("Speed when Rotating the Character / Unit (walking, moving)")]
    [SerializeField]
    private float _rotateSpeed = 10.0f;
    
    #endregion Rotations
    
    #endregion Taking the Action


    #region BaseParameters (INPUT) for calling this action as a GENERIC ACTION, with the function:  TakeAction

    /// <summary>
    /// BaseParameters (INPUT) for calling this action as a GENERIC ACTION, with the function:  TakeAction
    /// </summary>
    private ShootActionBaseParameters _shootActionBaseParameters = new ShootActionBaseParameters();

    #endregion BaseParameters (INPUT) for calling this action as a GENERIC ACTION, with the function:  TakeAction
    
    
    #region A.I. - AI
    
    // /// <summary>
    // /// (DEFAULT VALUE of...) Cost "PER UNIT" of this ACTION, for any ENEMY A.I., in terms of (CURRENCY = ) 'Action Points' <br />
    // /// ...this value should be summed to any other values, to represent the TOTAL "WORTH" of Taking This ACTION  (vs.  "Not Taking It").
    // /// </summary>
    // [Tooltip("(DEFAULT VALUE of...) Cost \"PER UNIT\" of this ACTION, for any ENEMY A.I., in terms of (CURRENCY = ) 'Action Points'\n...this value should be summed to any other values, to represent the TOTAL \"WORTH\" of Taking This ACTION  (vs.  \"Not Taking It\").")]
    // [SerializeField]
    // protected int _AI_DEFAULT_UNITARY_ACTION_POINT_COST_VALUE_FOR_ANY_ENEMY_AI_TO_DECIDE_ON_THIS_ACTION = 100;

    #endregion A.I. - AI
    
    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        
        #region A.I. - AI
        
        // Here we accept the "base.Awake();"  code regarding A.I., because the DEFAULT VALUE of an A.I. ACTION is (already set)  OK   for "ShootAction"
        
        #endregion A.I. - AI

    }// End Awake


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>



    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        // (MUTEX CHECK)
        // If its DISABLED  =>  Skip.
        //
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
            case State.Aiming:

                // Rotate towards the TARGET, and Aim at it:
                // .1- Direction Vector3 to shoot (normalized)
                //
                _aimDirection = ( _targetUnit.GetWorldPosition() - _unit.GetWorldPosition() ).normalized;
                //
                // .2- Rotate, Animation:
                //
                RotateUnitUsingVector3SlerpApproach( _aimDirection );
                
                break;
            
            case State.Shooting:
                
                // Validate & SHOOT!
                //
                if (_canShootBullet)
                {
                    Shoot();
                    
                    // Un-Set Flag for Shooting  (because it already shot)
                    //
                    _canShootBullet = false;
                }
                
                break;
            
            case State.Cooloff:

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
        
    }//End Update()
    

    #endregion Unity Methods
    

    #region My Custom Methods
    
    
    /// <summary>
    /// Makes the Payers Character (Unit): Shoot to the Target.
    /// </summary>
    public override void TakeAction(Action onShootActionComplete)
    {

        // 0- Get the Input Base Parameters (for this function call):
        //
        GenerateInputParameters();
        
        // /////////
        // Debug.Log($"Function:   --->   ' {GetType().AssemblyQualifiedName} \n* Class:{GetType().Name}\n* ' GameObject: ---> {transform} - {this}");
        // /////////
        
        // 2- Calculate the TARGET's constraints
        // ...(to Shoot to - based on a TARGET "GridPosition" selected by a Mouse Click, as Input in the case of a HUMAN PLAYER... if it's a NPC-ENEMY-A.I. the selction of the "Target" happens automatically.)
        //
        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(_shootActionBaseParameters.TargetGridPositionOfSelectedAction);
        

        // // 3- Debug, remove soon
        // //
        // Debug.Log("1- Aiming");

        // 4- Set the STATE variable, for it to START
        //
        _state = State.Aiming;

        // 5- TIMER of this action:
        // Set the (Next..) STATE's Timer
        //
        float aimingStateTimer = 1.0f;
        _stateTimer = aimingStateTimer;
        
        // 6- Set Flags for the Initial SHOOTING ANIMATION
        //
        _canShootBullet = true;

        
        // X- Event Listener (it is a Delegate)
        //   .1- Here we assign the Function/Procedure (i.e.: Method) to the 'DELEGATE variable'
        //   .2- In another line (latter, in another Script.cs),
        // we'll do a calling / invoke, something like:   'onActionComplete()'
        // ..., to call the latest Method that was added in this Method TakeAction().
        // Purpose of this callback (Delegate):  to tell the World that this ROUTINE JUST ENDED (..the STARTING PHASE...):
        //
        ActionStart(onShootActionComplete);
        
        // // This meant before:
        // this.onActionComplete = onShootComplete;
        //
        // //   .3- Sets a mutex flag:
        // //
        // _isActive = true;
        
    }// End TakeAction(...)

    
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
        _shootActionBaseParameters.TargetGridPositionOfSelectedAction =
            this._unit.GetFinalGridPositionOfNextPlayersAction();

    }//End GenerateInputParameters

    
    #region Taking the Action Methods
    
    /// <summary>
    /// It Starts the LOGIC + Animations for the Shooting STATE (of the State Machine in this Script).
    /// </summary>
    private void Shoot()
    {
        // Trigger / START the Animation Event:
        //
        OnShootAnimation?.Invoke(this,
            new OnShootAnimationEventArgs
            {
                targetUnit = _targetUnit,
                shootingUnit = _unit
            });
        
        // It inflicts some DAMAGE
        //
        _targetUnit.Damage(_shootPowerDamageInHealthPoints);
        
    }//End Shoot(...)

    #region Rotation: LERP vs. SLERP 
    
    /// <summary>
    /// Quaternions + Spherical Interpolation, SLERP, (Quaternions behind the Scenes):
    ///...it rotates in a better way (first Rotates, then Walks):
    /// </summary>
    /// <param name="moveDirection"></param>
    private void RotateUnitUsingVector3SlerpApproach(Vector3 moveDirection)
    {
        // Quaternions + Spherical Interpolation, SLERP, (Quaternions behind the Scenes):
        //...it rotates in a better way (first Rotates, then Walks):
        //
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * _rotateSpeed);
    }
    
    /// <summary>
    /// Linear Interpolation, LERP: Original, CodeMonkey's: No Quaternions involved.
    ///...it rotates, but the SLERP option is way better
    ///...(LERP produces this effect: first walks backwards 1 or 2 steps, like Michael Jackson...
    ///...then Rotates, then Walks the rest...):
    /// </summary>
    /// <param name="moveDirection"></param>
    private void RotateUnitUsingVector3LerpApproach(Vector3 moveDirection)
    {
        // Linear Interpolation, LERP: Original, CodeMonkey's:
        //
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * _rotateSpeed);
    }
    
    #endregion  Rotation: LERP vs. SLERP
    
    #endregion Taking the Action Methods
    
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
            case State.Aiming:
                
                // ENDING of this STATE:
                // Set the (variables for) Ending of this State:
                // => BEGINNING of Next state
                //
                // 1- Change the STATE variable to -> NEXT State
                //
                _state = State.Shooting;
                
                // 2- Set THAT (Next..) STATE's Timer
                //
                float shootingStateTimer = 0.1f;
                _stateTimer = shootingStateTimer;

                break;
            
            case State.Shooting:
                
                // ENDING of this STATE:
                // Set the (variables for) Ending of this State:
                // => BEGINNING of Next state
                //
                // 1- Change the STATE variable to -> NEXT State
                //
                _state = State.Cooloff;
                
                // 2- Set THAT (Next..) STATE's Timer
                //
                float coolOffStateTimer = 0.5f;
                _stateTimer = coolOffStateTimer;
                
                break;
            
            case State.Cooloff:
                
                // ENDING of this STATE:
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
    

    #region Action Validations

    /// <summary>
    /// Get a List of the Valid places where the Unit/Character can 'TakeAction(...)' to (i.e.: GridPosition(s)).
    /// This method cycles through the squares/Grids...(using FOR )... to get a list of the valid ones.
    /// </summary>
    /// <returns>Valid (GridPosition(s)) places where the Unit/Character can TakeAction to, in this Turn.</returns>
    public override List<GridPosition> GetValidActionGridPositionList()
    {

        // Get the Unit's GridPosition
        //
        GridPosition unitGridPosition = _unit.GetGridPosition();


        // Get a List of the Valid places where the Unit/Character can 'TakeAction(...)' to (i.e.: GridPosition(s)).
        //
        return GetValidActionGridPositionList(unitGridPosition);

    }// End GetValidActionGridPositionList

    
    /// <summary>
    /// Get a List of the Valid places where the Unit/Character can 'TakeAction(...)' to (i.e.: GridPosition(s)).
    /// This method cycles through the squares/Grids...(using FOR )... to get a list of the valid ones.
    /// </summary>
    /// <returns>Valid (GridPosition(s)) places where the Unit/Character can TakeAction to, in this Turn.</returns>
    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        // Cycle through the Rows and Columns (Cells in general) to find the Valid ones for Tak(ing)Action (i.e.: Shooting...) to.. in this Turn
        //
        for (int x = -_maxShootDistance; x <= _maxShootDistance; x++)
        {
            for (int z = -_maxShootDistance; z <= _maxShootDistance; z++)
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
                

                ///////////////// Temporary: Circular shape made with square pixels:
                int testDistance = (x * x) + (z * z);

                if (testDistance > ((_maxShootDistance * _maxShootDistance) + _maxShootDistance + 0.25f))
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
    
        return validGridPositionList;
    }

    
    #region POINTS  - for every Action

    // Commented: Reason: It is in the BASE ACTION CLASS. And it is based on an "protected _attribute" of that class...
    //...that this Derived (Children) Classes receive as "Inheritance":
    // /// <summary>
    // /// Gets the ActionPoints (Cost) for this Current SELECTED ACTION.
    // /// </summary>
    // /// <returns></returns>
    // public override int GetActionPointsCost()
    // {
    //     return 2;
    // }

    #endregion POINTS  - for every Action

    #endregion Action Validations
    
        
    #region UI related utils

    // /// <summary>
    // /// (Base Concrete implementation of:) Gets this ACTION'S Name.
    // /// </summary>
    // /// <returns></returns>
    // public string GetActionNameByStrippingClassName()
    // {
    //     // AlMartsons' version:  Get the Class Name, but maybe it needs to be stripped from the rest of the word (e.g.: SpinAction... to just: Spin - remove 'Action' -, etc.).
    //     //
    //     return GetType().Name;
    // }
    
    /// <summary>
    /// (Concrete implementation of:) Gets this ACTION'S Name.
    /// </summary>
    /// <returns></returns>
    public override string GetActionName()
    {
        // CodeMonkey' version:  Write a custom class Name (string) here:
        //
        return "Shoot";
    }
    
    #endregion UI related utils

    
    #region Misc, Getters, Setters, etc

    /// <summary>
    /// Gets the (Character) Unit, that is the TARGET.
    /// </summary>
    /// <returns></returns>
    public Unit GetTargetUnit()
    {
        return _targetUnit;
    }

    /// <summary>
    /// Gets the <code>_maxShootDistance</code>.
    /// </summary>
    /// <returns>_maxShootDistance</returns>
    public int GetMaxShootDistance()
    {
        return _maxShootDistance;
    }
    
    #endregion Misc, Getters, Setters, etc

    
    #region A.I. - AI

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
    /// (Calculates and...):  Gets the "NUMBER OF UNIT-PLAYERS" (i.e.: Targets for any ENEMY NPC - A.I...), that are VALID / SHOOTABLES, for this NPC-ENEMY-UNIT in a given GridPosition.
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {

        // Return the Number of VALID TARGETS this ENEMY-NPC-AI can Shoot at:
        //
        return  GetValidActionGridPositionList(gridPosition).Count;

    }// End GetTargetCountAtPosition
    
    #endregion A.I. - AI
    
    #endregion My Custom Methods

}//End Class ShootAction


/// <summary>
/// Concrete-particular Class (derived as a child of "BaseParameters") for the Input Parameters,
/// ..of every Function call to: 'TakeAction()'
/// </summary>
public class ShootActionBaseParameters : BaseParameters
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

}//End Class ShootActionBaseParameters
