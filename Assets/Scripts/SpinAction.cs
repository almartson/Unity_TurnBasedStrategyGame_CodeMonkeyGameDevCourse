using UnityEngine;


public class SpinAction : MonoBehaviour
{

    #region Attributes

    [Tooltip("Set to TRUE if you want to start the Spinning Action.")]
    [SerializeField]
    private bool _isActive = false;

    
    [Tooltip("(Rotation Speed):Degrees per second.\n\na POSITIVE NUMBER (means = Rotate clockwise) or a NEGATIVE NUMBER (which means = Rotate counter-clockwise).")]
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
            _isActive = false;
        }

    }

    #endregion Unity Methods
    

    #region My Custom Methods
    
    /// <summary>
    /// Makes the GameObject Spin / Rotate.
    /// </summary>
    public void Spin()
    {
        _isActive = true;
        
        // Reset the Accumulated Rotation
        //
        _totalSpinAmount = 0.0f;
    }


    #endregion My Custom Methods

}
