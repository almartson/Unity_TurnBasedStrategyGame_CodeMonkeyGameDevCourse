/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the execution of the Shooting Action (Animations, timers, stages of the animation itself - even if it is a chain of animations tied up to each other and triggered together, - etc)
/// </summary>
public class ShootAction : BaseAction
{
    #region Attributes

    [Tooltip("(Rotation Speed): Degrees per second.\n\nA POSITIVE NUMBER (means = Rotate clockwise) or a NEGATIVE NUMBER (which means = Rotate counter-clockwise).")]
    [SerializeField]
    [Range(-360f, 360f)]
    private float _spinVelocityAndDirectionInDegreesPerSecond = 360.0f;

    /// <summary>
    /// Number of Degrees to Rotate
    /// (this is used for the Character / Unit to rotate towards the Target, to Aim towards it... in the initial state of Animation)
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
    private ShootActionBaseParameters _shootActionBaseParameters = new ShootActionBaseParameters();
    

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
            //
            _isActive = false;
            
            // We CALL our DELEGATE:  tells everyone that the TakeAction routine ENDED:
            //
            onActionComplete?.Invoke();
            //
            // Example: other ways of calling or executing the Delegate:
            // onActionComplete?.Invoke();
            //
            // if (onActionComplete != null)
            // {
            //     onActionComplete();
            // }
        }

    }

    #endregion Unity Methods
    

    #region My Custom Methods
    
    /// <summary>
    /// Makes the Payers Character (Unit): Shoot to the Target.
    /// </summary>
    public override void TakeAction(Action onShootComplete)
    {
        // This is greyed out because currently there are no INPUT PARAMETERS FOR THIS ACTION: .
        
        // 0- Get the Input Base Parameters (for this function call):
        //
        GenerateInputParameters();
        
        
        // 1- Here we assign the Function/Procedure (i.e.: Method) to the 'DELEGATE variable'
        // 2- In another line (latter, in another Script.cs),
        // we'll do a calling / invoke, something like:   'onActionComplete()'
        // ..., to call the latest Method that was added in this Method TakeAction().
        // Purpose of this callback (Delegate):  to tell the World that this ROUTINE JUST ENDED (..the STARTING PHASE...):
        //
        this.onActionComplete = onShootComplete;
        
        // Sets a mutex flag:
        //
        _isActive = true;
        
        // Reset the Accumulated Rotation
        //
        _totalSpinAmount = 0.0f;
        
    }//End TakeAction(...)

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
        // this._shootActionBaseParameters.TargetGridPositionOfSelectedMovement = UnitActionSystem.Instance.GetSelectedUnit().GetGridPosition();
        
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

    /// <summary>
    /// Gets the ActionPoints (Cost) for this Current SELECTED ACTION.
    /// </summary>
    /// <returns></returns>
    public override int GetActionPointsCost()
    {
        return 2;
    }
    

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


    #endregion My Custom Methods

}


/// <summary>
/// Concrete-particular Class (derived as a child of "BaseParameters") for the Input Parameters,
/// ..of every Function call to: 'TakeAction()'
/// </summary>
public class ShootActionBaseParameters : BaseParameters
{

    #region Attributes

    
    #endregion Attributes
    
    
    #region Methods

    

    #endregion Methods

}

