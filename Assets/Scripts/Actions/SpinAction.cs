using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This concrete class, (derived from BaseAction), handles the execution of the SPIN Action (Animations, timers, stages of the animation / action itself - even if it is a chain of animations tied up to each other and triggered together, - etc) <br />
/// </summary>
public class SpinAction : BaseAction
{
    #region Attributes

    [Tooltip("(Rotation Speed): Degrees per second.\n\nA POSITIVE NUMBER (means = Rotate clockwise) or a NEGATIVE NUMBER (which means = Rotate counter-clockwise).")]
    [SerializeField]
    [Range(-360f, 360f)]
    private float _spinVelocityAndDirectionInDegreesPerSecond = 360.0f;

    /// <summary>
    /// Number of Degrees to Rotate
    /// </summary>
    private float _totalSpinAmount = 0f;
    
    /// <summary>
    /// For performance reasons: cached Vector3 for Added Rotation Amount.
    /// </summary>
    private Vector3 _cachedRotationVector = new Vector3(0, 0, 0);

    
    [Tooltip("(Rotation Goal): Number of Degrees to Rotate")]
    [SerializeField]
    [Range(0f, 360f)]
    private float _rotationGoal = 360.0f;

    
    #region BaseParameters (INPUT) for calling this action as a GENERIC ACTION, with the function:  TakeAction

    /// <summary>
    /// BaseParameters (INPUT) for calling this action as a GENERIC ACTION, with the function:  TakeAction
    /// </summary>
    private SpinActionBaseParameters _spinActionBaseParameters = new SpinActionBaseParameters();
    

    #endregion BaseParameters (INPUT) for calling this action as a GENERIC ACTION, with the function:  TakeAction
    
    #region A.I. - AI
    
    // /// <summary>
    // /// (DEFAULT VALUE of...) Cost "PER UNIT" of this ACTION, for any ENEMY A.I., in terms of (CURRENCY = ) 'Action Points' <br />
    // /// ...this value should be summed to any other values, to represent the TOTAL "WORTH" of Taking This ACTION  (vs.  "Not Taking It").
    // /// </summary>
    // [Tooltip("(DEFAULT VALUE of...) Cost \"PER UNIT\" of this ACTION, for any ENEMY A.I., in terms of (CURRENCY = ) 'Action Points'\n...this value should be summed to any other values, to represent the TOTAL \"WORTH\" of Taking This ACTION  (vs.  \"Not Taking It\").")]
    // [SerializeField]
    // protected int _AI_DEFAULT_UNITARY_ACTION_POINT_COST_VALUE_FOR_ANY_ENEMY_AI_TO_DECIDE_ON_THIS_ACTION = 0;

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
        
        // Here we accept the "base.Awake();"  code regarding A.I., because the DEFAULT VALUE of an A.I. ACTION is (already set to 0 Zero)  OK   for "Spin Action"
        
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
        
        // Set the Rotation Velocity:
        //
        float spinAddAmount = _spinVelocityAndDirectionInDegreesPerSecond * Time.deltaTime;
        //
        // Set the Rotation Vector3 Coordinates:
        //
        _cachedRotationVector.y = spinAddAmount;
        //
        // Set the Rotation as euler angles, in the GameObject's Transform:
        //
        transform.eulerAngles += _cachedRotationVector;
        
        // Total Degrees to Rotate:  increment each frame until we reach a certain Degree Gaol:
        //
        _totalSpinAmount += Mathf.Abs(spinAddAmount);
        //
        // If we reach the Rotation (Degrees..)  GOAL:  STOP:
        //
        if (Mathf.Abs(_totalSpinAmount) >= Mathf.Abs(_rotationGoal))
        {
            // Release the (mutex) flag
            // +  We CALL our DELEGATE:  tells everyone that the TakeAction routine ENDED:
            //
            ActionComplete();
            
            
            // // It meant before:
            // // Release the (mutex) flag
            // //
            // _isActive = false;
            //
            // // We CALL our DELEGATE:  tells everyone that the TakeAction routine ENDED:
            // //
            // onActionComplete();
            // //
            // // Example: other ways of calling or executing the Delegate:
            // // onActionComplete?.Invoke();
            // //
            // // if (onActionComplete != null)
            // // {
            // //     onActionComplete();
            // // }
        }

    }

    #endregion Unity Methods
    

    #region My Custom Methods
    
    /// <summary>
    /// Makes the Payers Character (Unit): Spin /Rotate.
    /// </summary>
    public override void TakeAction(Action onSpinActionComplete)   //  (GridPosition gridPosition /* Not necessary for Spin Action */, Action onSpinComplete)
    {
        // 0- Get the Input Base Parameters (for this function call):
        //
        GenerateInputParameters();
        
        
        // Reset the Accumulated Rotation
        //
        _totalSpinAmount = 0.0f;
        
        
        // 1- Here we assign the Function/Procedure (i.e.: Method) to the 'DELEGATE variable'
        // 2- In another line (latter, in another Script.cs),
        // we'll do a calling / invoke, something like:   'onActionComplete()'
        // ..., to call the latest Method that was added in this Method TakeAction().
        // Purpose of this callback (Delegate):  to tell the World that this ROUTINE JUST ENDED (..the STARTING PHASE...):
        //
        ActionStart(onSpinActionComplete);
        
        // // This meant before:
        // this.onActionComplete = onSpinActionComplete;
        //
        // // Sets a mutex flag:
        // //
        // _isActive = true;

    }// End TakeAction
    

    /// <summary>
    /// Generic Method for generating the necessary Input Parameters that are used in the calling of
    /// ..the Function Call to the generic: 'TakeAction'
    /// This must be reimplemented / overriden in each Concrete (derived, child).
    /// We need inside this class: <code>GridPosition</code>
    /// </summary>
    public override void GenerateInputParameters()
    {
        // // 2- Get the Player's currently: SELECTED ACTION
        // //
        // // UnitActionSystem.Instance.GetSelectedAction();
        //
        // // Generate:
        // //
        // 1- TARGET GridPosition (i.e.: the Destination of the Movement...)
        //
        // This does NOT work when a NPC ENEMY A.I. is Playing (because: UnitActionSystem.Instance.GetSelectedUnit()... only saves the last HUMAN PLAYER that was playing in the last turn...):   this._spinActionBaseParameters.TargetGridPositionOfSelectedAction = UnitActionSystem.Instance.GetSelectedUnit().GetGridPosition();
        
    }//End GenerateInputParameters


    #region Action Validations

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
        return "Spin";
    }
    
    #endregion UI related utils

    
    #region A.I. - AI

    /// <summary>
    /// (Calculates and...):  Gets the "A.I. ACTION" data ("Cost" Value, final, calculated "Points", to see if it's worth it...) that is possible in a given,  "Grid Position".
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns>A set of DATA  (note: specially the "Cost" of taking THIS ACTION...) for taking this selected ACTION.</returns>
    public override EnemyAIActionData GetEnemyAIActionData(GridPosition gridPosition)
    {
        // Execute the "Base Action" routine:
        //
        return base.GetEnemyAIActionData(gridPosition);

    }// End GetEnemyAIActionData
    
    
    #endregion A.I. - AI

    
    #endregion My Custom Methods

}//End Class SpinAction



/// <summary>
/// Concrete-particular Class (derived as a child of "BaseParameters") for the Input Parameters,
/// ..of every Function call to: 'TakeAction()'
/// </summary>
public class SpinActionBaseParameters : BaseParameters
{

    #region Attributes

    
    #endregion Attributes
    
    
    #region Methods

    

    #endregion Methods

}//End Class SpinActionBaseParameters
