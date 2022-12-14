using Unity.VisualScripting;
using UnityEngine;


public class SpinAction : BaseAction
{
    #region Attributes
    
    #region Delegates: Purpose: Managing (allowing only...) just ONE Action at a Time
    
    /// <summary>
    /// Delegate: Spin Complete ACTION: Purpose: Telling everyone when the Spin (Action) Routine ends.
    /// </summary>
    public delegate void SpinCompleteDelegate();

    /// <summary>
    /// Variable to tell when the: [ Spin ] (Action) Routine ends. 
    /// </summary>
    private SpinCompleteDelegate _onSpinComplete;


    #endregion Delegates: Purpose: Managing (allowing only...) just ONE Action at a Time
    
    
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
            _onSpinComplete();
        }

    }

    #endregion Unity Methods
    

    #region My Custom Methods
    
    /// <summary>
    /// Makes the GameObject Spin / Rotate.
    /// </summary>
    public void Spin(SpinCompleteDelegate onSpinComplete)
    {
        // Sets the DELEGATE variable to tell the World that this ROUTINE JUST ENDED:
        //
        _onSpinComplete = onSpinComplete;
        
        // Sets a mutext flag:
        //
        _isActive = true;
        
        // Reset the Accumulated Rotation
        //
        _totalSpinAmount = 0.0f;
    }


    #endregion My Custom Methods

}
