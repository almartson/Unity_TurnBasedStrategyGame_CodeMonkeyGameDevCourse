using UnityEngine;


public class SpinAction : MonoBehaviour
{

    #region Attributes

    [Tooltip("Set to TRUE if you want to start the Spinning Action.")]
    [SerializeField]
    private bool _startSpinning = false;

    
    [Tooltip("Number of Degrees to Rotate each time this Action is Activated")]
    [SerializeField]
    [Range(-360f, 360f)]
    private float _spinDegrees = 360.0f;

    /// <summary>
    /// For performance reasons: cached Vector3 for Added Rotation Amount.
    /// </summary>
    private Vector3 _cachedRotationVector = new Vector3(0, 0, 0);
    
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
        if (_startSpinning)
        {
            // Set the Rotation Velocity:
            //
            float spinAddAmount = _spinDegrees * Time.deltaTime;
            //
            // Set the Rotation Vector3 Coordinates:
            //
            _cachedRotationVector.y = spinAddAmount;
            //
            // Set the Rotation as euler angles, in the GameObject's Transform:
            //
            transform.eulerAngles += _cachedRotationVector;
        }
    }

    #endregion Unity Methods
    

    #region My Custom Methods
    
    /// <summary>
    /// Makes the GameObject Spin / Rotate.
    /// </summary>
    public void Spin()
    {
        _startSpinning = true;
        
        // Debug
        //
        Debug.Log("Spin");
    }


    #endregion My Custom Methods

}
