using System;
using UnityEngine;


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
            
            // We CALL our DELEGATE:  tells everyone that the Spin routine ENDED:
            //
            onActionComplete();
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
    /// Makes the GameObject Spin / Rotate.
    /// </summary>
    public void Spin(Action onSpinComplete)
    {
        // 1- Here we assign the Function/Procedure (i.e.: Method) to the 'DELEGATE variable'
        // 2- In another line (latter, in another Script.cs),
        // we'll do a calling / invoke, something like:   'onActionComplete()'
        // ..., to call the latest Method that was added in this Method Spin().
        // Purpose of this callback (Delegate):  to tell the World that this ROUTINE JUST ENDED:
        //
        this.onActionComplete = onSpinComplete;
        
        // Sets a mutex flag:
        //
        _isActive = true;
        
        // Reset the Accumulated Rotation
        //
        _totalSpinAmount = 0.0f;
    }

    
        
    #region UI related utils

    // /// <summary>
    // /// (Base Concrete implementation of:) Gets this ACTION'S Name.
    // /// </summary>
    // /// <returns></returns>
    // public string GetActionNameByStrippingClassName()
    // {
    //     // AlMartsons' version:  Get the Class Name, but maybe it needs to be stripped from the rest of the word (e.g.: SpinAction... to just: Spin - remove 'Spin' -, etc.).
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


    #endregion My Custom Methods

}
