using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This concrete class, (derived from BaseAction), handles the execution of the MOVE Action (Animations, timers, stages of the animation / action itself - even if it is a chain of animations tied up to each other and triggered together, - etc) <br />
/// </summary>
[System.Serializable]
public class MoveAction : BaseAction
{
    #region Attributes

    [Tooltip("Destination (x, y, z) Position of the Movement Action")]
    [SerializeField]
    private Vector3 _targetPosition;
    
    // Movement:
    //
    [Tooltip("Speed when Translating the Character / Unit (walking, moving)")]
    [SerializeField] private float _moveSpeed = 4.0f;
    
    // Rotation:
    //
    [Tooltip("Speed when Rotating the Character / Unit (walking, moving)")]
    [SerializeField] private float _rotateSpeed = 10.0f;

    
    #region BaseParameters (INPUT) for calling this action as a GENERIC ACTION, with the function:  TakeAction

    /// <summary>
    /// BaseParameters (INPUT) for calling this action as a GENERIC ACTION, with the function:  TakeAction
    /// </summary>
    private MoveActionBaseParameters _moveActionBaseParameters = new MoveActionBaseParameters();
    

    #endregion BaseParameters (INPUT) for calling this action as a GENERIC ACTION, with the function:  TakeAction
    
    
    #region Animator & Animations

    /// <summary>
    /// CallBack Listener (Delegate Function) for executing when Starting the Animation.
    /// </summary>
    public event EventHandler OnStartMovingAnimation;
    
    /// <summary>
    /// CallBack Listener (Delegate Function) for executing when Stopping / Ending the Animation.
    /// </summary>
    public event EventHandler OnStopMovingAnimation;

    #endregion Animator & Animations
    
    
    #region Utils
    
    [Tooltip("The Tolerance number to accept that a value is = ZERO")]
    [SerializeField]
    private float _stoppingDistance = 0.1f;
    
    private float _sqrStoppingDistance = 0.1f;

    #endregion Utils
    
    
    #region Validations: Movement
    
    /// <summary>
    /// Max number of Grid Cells the character can move in one Turn.
    /// </summary>
    [SerializeField]
    private int _maxMoveDistance = 4;
    
    #endregion Validations: Movement
    
    
    #region A.I. - AI
    
    // /// <summary>
    // /// (DEFAULT VALUE of...) Cost "PER UNIT" of this ACTION, for any ENEMY A.I., in terms of (CURRENCY = ) 'Action Points' <br />
    // /// ...this value should be summed to any other values, to represent the TOTAL "WORTH" of Taking This ACTION  (vs.  "Not Taking It").
    // /// </summary>
    // [Tooltip("(DEFAULT VALUE of...) Cost \"PER UNIT\" of this ACTION, for any ENEMY A.I., in terms of (CURRENCY = ) 'Action Points'\n...this value should be summed to any other values, to represent the TOTAL \"WORTH\" of Taking This ACTION  (vs.  \"Not Taking It\").")]
    // [SerializeField]
    // protected int _AI_DEFAULT_UNITARY_ACTION_POINT_COST_VALUE_FOR_ANY_ENEMY_AI_TO_DECIDE_ON_THIS_ACTION = 10;

    #endregion A.I. - AI
    
    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>
    protected override void Awake()
    {
        // Execute the "Awake" of the (Abstract and Implemented here)
        // ..Parent of this class:
        //
        base.Awake();
        
        #region Utils
        
        // Done: Misc Optimization: Calculating the (accepted) Square Min Distance.
        //
        _sqrStoppingDistance = _stoppingDistance * _stoppingDistance;
        
        #endregion Utils
        
        
        // Initialize Target Position to this Script's base GameObject.
        //
        _targetPosition = this.transform.position;
        
    }


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>



    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        // Check whether this "Action" is Active or Inactive:
        // Mutex flag check:...
        //
        if (!_isActive)
        {
            // Its DISABLED so...
            // End it here  (for THIS FRAME)
            //
            return;
        }
        
        // Update the Unit Movement (Walking...)
        //
        UpdateUnitMove();
    }

    
    #endregion Unity Methods
    

    #region My Custom Methods

    #region Stop all Movement Action & Animation
    
    /// <summary>
    /// Stops TakeAction() and Movement Animations.
    /// </summary>
    private void StopMoveAction()
    {
        // Invoke: STOP the Animation
        //
        OnStopMovingAnimation?.Invoke(this, EventArgs.Empty);

        
        // Set this "Action" as DISABLED
        // + We CALL our DELEGATE (which is on the PARENT-Base Class):  tells everyone that the 'TakeAction() Action' routine ENDED  +  Set this "Action" as DISABLED:
        //
        ActionComplete();
        
    }//End StopMoveAction()
    
    #endregion Stop all Movement Action & Animation
    
    
    /// <summary>
    /// Moves the Unit / Character to the specified (x, y, z) Position (Grid).
    /// </summary>
    public override void TakeAction(Action onMoveActionComplete)   //   (GridPosition gridPosition /* Used in MoveAction */, Action onMoveActionComplete)
    {
        // 1- Get the Input Base Parameters (for this function call):
        //
        GenerateInputParameters();
        
        
        // Get the WorldPosition, based on a "GridPosition" as Input.
        //
        _targetPosition = LevelGrid.Instance.GetWorldPosition(this._moveActionBaseParameters.TargetGridPositionOfSelectedMovement);
        
        // Invoke: 3D Animation START
        //
        OnStartMovingAnimation?.Invoke(this, EventArgs.Empty);
        
        
        // We CALL our DELEGATE (which is on the PARENT-Base Class):  tells everyone that the 'TakeAction() Action' routine BEGAN (...the ACTIVATION-part of it)   +   Set this "Action" as ENABLED (a mutex flag _isActive)
        //
        ActionStart( onMoveActionComplete );

    }//End TakeAction()
    
    
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
        // Not working for ENEMY A.I.:  this._moveActionBaseParameters.TargetGridPositionOfSelectedMovement = UnitActionSystem.Instance.GetSelectedUnit().GetFinalGridPositionOfNextPlayersAction();
        //
        // Getting the "GridPosition" of the Target, regardless of the Team that is playing (CPU or Player's):
        //
        _moveActionBaseParameters.TargetGridPositionOfSelectedMovement =
            this._unit.GetFinalGridPositionOfNextPlayersAction();
        
    }//End GenerateInputParameters


    /// <summary>
    /// Moves the Unit / Character to the specified (x, y, z) Position (Grid).
    /// </summary>
    /// <param name="newTargetPosition"></param>
    /// <param name="onMoveActionComplete"></param>
    [Obsolete("This method is deprecated. Use: 'public void TakeAction(GridPosition gridPosition)' instead", true)]
    public void Move(Vector3 newTargetPosition, Action onMoveActionComplete)
    {
        
        _targetPosition = newTargetPosition;
        
        // Set this "Action" as ENABLED
        // Set the (mutex) flag:
        //
        _isActive = true;
        
        
        // We CALL our DELEGATE:  tells everyone that the 'TakeAction() Action' routine ENDED:
        //
        this.onActionComplete = onMoveActionComplete;
    }
    
    /// <summary>
    /// Makes the Unit / Character to TakeAction.
    /// </summary>
    private void UpdateUnitMove()
    {
        // Calculate the Vector3 of the DIRECTION of Movement:
        //
        Vector3 moveDirection = (_targetPosition - transform.position).normalized;
        
        // Calculate the Distance... to see how close or far
        // ...is the Mouse Pointer -> from -> The Unit we want to TakeAction().
        //
        if (Vector3.SqrMagnitude(transform.position - _targetPosition) > _sqrStoppingDistance)
        {
            // TakeAction
            // 1- Translation:
            //
            transform.position += moveDirection * (_moveSpeed * Time.deltaTime);
        }
        else
        {
            // Stop TakeAction() MoveAction + STOP ANIMATION:
            //
            StopMoveAction();

        }//End else of if (Vector3.SqrMagnitude...
        //
        // 2- Rotation
        //
        RotateUnitUsingVector3SlerpApproach(moveDirection);

    }//End UpdateUnitMove()

    
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

    
    #region Movement Validations

    // NOTE: Its real implementation: is on the Base Class: 'BaseAction'
    // /// <summary>
    // /// Tells you whether the user/Player's selection (Grid Position) is Valid to TakeAction to. It uses several criteria, such as: the Position must be unoccupied, must be inside the Grid System, etc. 
    // /// </summary>
    // /// <returns>True or False to: is the selected "GridPosition" Valid??</returns>
    // public bool IsValidActionGridPosition(GridPosition gridPosition)

    
    /// <summary>
    /// Get a List of the Valid places where the Unit/Character can TakeAction to (i.e.: GridPosition(s)).
    /// This method cycles through the squares/Grids...(using FOR )... to get a list of the valid ones.
    /// </summary>
    /// <returns>Valid (GridPosition(s)) places where the Unit/Character can TakeAction to, in this Turn.</returns>
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        
        // Get the Unit's GridPosition
        //
        GridPosition unitGridPosition = _unit.GetGridPosition();
        
        // Cycle through the Rows and Columns (Cells in general) to find the Valid ones for Moving() in.. in this Turn
        //
        for (int x = -_maxMoveDistance; x <= _maxMoveDistance; x++)
        {
            for (int z = -_maxMoveDistance; z <= _maxMoveDistance; z++)
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
                //
                // 2- "GridPosition" Must be different from the current one.
                //
                if (unitGridPosition == testGridPosition)
                {
                    // Not Valid: Same Grid Position where the Player / Unit is already at.
                    // Skip to next iteration:
                    //
                    continue;
                }
                //
                // 3- "GridPosition" Must NOT be previously occupied.
                //
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Not Valid:
                    // Grid Position is already occupied with another Unit. Skip to next iteration:
                    //
                    continue;
                }
                
                // Finally, Conclusion: Add the Tested & Valid GridPosition to the Local VALID List
                //
                validGridPositionList.Add(testGridPosition);

            } // End for 2
        }//End for 1
    
        return validGridPositionList;
    }


    #endregion Movement Validations
    
    
    #region UI related utils

    // /// <summary>
    // /// (Base Concrete implementation of:) Gets this ACTION'S Name.
    // /// </summary>
    // /// <returns></returns>
    // public string GetActionNameByStrippingClassName()
    // {
    //     // AlMartsons' version:  Get the Class Name, but maybe it needs to be stripped from the rest of the word (e.g.: SpinAction... to just: TakeAction - remove 'TakeAction' -, etc.).
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
        return "Move";
    }
    
    #endregion UI related utils
    
    
     
    #region A.I. - AI

    /// <summary>
    /// (Calculates and...):  Gets the "A.I. ACTION" data ("Cost" Value, final, calculated "Points", to see if it's worth it...) that is possible in a given,  "Grid Position". <br />
    /// For the "MoveAction":   The A.I. will prioritize "GridPositions" where it can Shoot multiple Targets from... <br />
    /// ...that means:  if "moving to" a certain "GridPosition" normally has a "VALUE" of "10.0f" Action Points,...
    /// ...but from that GridPosition it could shoot to TWO (2) TARGETS (i.e.: Players Characters/Units-...) <br />
    /// ...then this Algorithm would prioritize THAT "child of BaseAction", with THAT "ActionData"... (on that "GridPosition"). <br />
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <param name="initialAdditionalAIActionPointCostValueOfThisAction">_myAIFinalActionPointCostValueForAnyEnemyAIToDecideOnThisAction's INITIAL VALUE to add</param>
    /// <returns>A set of DATA  (note: specially the "Cost" of taking THIS ACTION...) for taking this selected ACTION.</returns>
    public override EnemyAIActionData GetEnemyAIActionData(GridPosition gridPosition, int initialAdditionalAIActionPointCostValueOfThisAction)
    {
        // We want the Player not to move "RANDOMLY", but TOWARDS the weakest Unit-Player of the "Player team" (to attack it and Win in a easier way...):
        //
        // 1- TRY to FIND if there are any "SHOOTABLE" -> "UNIT-PLAYERS (i.e.: "Targets") in the given  "Grid Position"...
        // 2- Use that number of ""SHOOTABLE" -> "UNIT-PLAYERS (i.e.: "Targets")" (from (1)) to calculate the  "WORTHINESS" of this ACTION (i.e.: the ACTION COST-VALUE):
        //
        // Process:
        //
        // 1- TRY to FIND HOW MANY "SHOOTABLE" "Targets" ("UNIT-PLAYERS) are there, from that POSITION  (GridPosition):
        //
        int targetCountAtPosition = _unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);
        //
        // Save the 'targetCountAtPosition'  as the 'Cost Multiplier' of this  "A.I. ACTION":
        //
        _myAIMultiplierActionPointCostValueForAnyEnemyAIToDecideOnThisAction = targetCountAtPosition;

        // 2- Use that number of ""SHOOTABLE" -> "UNIT-PLAYERS (i.e.: "Targets")" (from (1)) to calculate the  "WORTHINESS" of this ACTION (i.e.: the ACTION COST-VALUE):   MULTIPLY IT..!
        //
        //    2.2- Execute the "Base Action" routine:
        //
        EnemyAIActionData enemyAIActionData = base.GetEnemyAIActionData(gridPosition, 0);
        

        // //////////////
        // // Show only when the WORTHINESS OF THIS ACTION surpasses the STANDARD REASON / VALUE (i.e.: 10):
        // //
        // if (enemyAIActionData.actionValue > this._AI_DEFAULT_UNITARY_ACTION_POINT_COST_VALUE_FOR_ANY_ENEMY_AI_TO_DECIDE_ON_THIS_ACTION)
        // {
        //     Debug.Log($"enemyAIActionData.actionValue = {enemyAIActionData.actionValue.ToString()} \n* At enemyAIActionData.gridPosition() = ( {enemyAIActionData.gridPosition.x.ToString()} , 0, {enemyAIActionData.gridPosition.z.ToString()} )");
        // }
        // //////////////
        
        // Return the final ENEMY A.I. DATA:
        //
        return enemyAIActionData;

    }// End GetEnemyAIActionData

    
    #endregion A.I. - AI

    #endregion My Custom Methods

}//End Class MoveAction


/// <summary>
/// Concrete-particular Class (derived as a child of "BaseParameters") for the Input Parameters,
/// ..of every Function call to: 'TakeAction()'
/// </summary>
public class MoveActionBaseParameters : BaseParameters
{

    #region Attributes

    /// <summary>
    /// Destination-Target Position for this Movement, of the Player's Unit, in the Cells-Grid.
    /// </summary>
    private GridPosition _targetGridPositionOfSelectedMovement;
    //
    /// <summary>
    /// Property Accessor to Private Field "_targetGridPositionOfSelectedMovement": <br /><br />
    /// Destination-Target Position for this Movement, of the Player's Unit, in the Cells-Grid. <br />
    /// </summary>
    /// <value></value>
    public GridPosition TargetGridPositionOfSelectedMovement { get => _targetGridPositionOfSelectedMovement; set => _targetGridPositionOfSelectedMovement = value; }


    #endregion Attributes


    #region Methods



    #endregion Methods

}//End Class MoveActionBaseParameters
