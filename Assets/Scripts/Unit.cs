using UnityEngine;

public class Unit : MonoBehaviour
{

    #region Attributes

    [SerializeField]
    private Vector3 _targetPosition;

    [Tooltip("The Tolerance number to accept that the value is ZERO")]
    [SerializeField]
    private float _stoppingDistance = 0.1f;
    
    private float _sqrStoppingDistance = 0.1f;

    // Movement:
    //
    [Tooltip("Speed when Translating the Character / Unit (walking, moving)")]
    [SerializeField] private float _moveSpeed = 4.0f;
    
    // Rotation:
    //
    [Tooltip("Speed when Rotating the Character / Unit (walking, moving)")]
    [SerializeField] private float _rotateSpeed = 10.0f;
    
    /// <summary>
    /// Mouse Position
    /// </summary>
    private Vector3 _mousePosition = Vector3.zero;
    /// <summary>
    /// Property Accessor to Private Field:
    /// Public Getter & Setter for _mousePosition
    /// </summary>
    public Vector3 MousePosition { get => _mousePosition; set => _mousePosition = value; }

    
    #region Animator & Animations

    [Tooltip("3D Character's Animator")]
    [SerializeField] private Animator _unitAnimator;
    
    // Hash ANIMATOR (Parameter) CONSTANTS representing the States of the Animator
    // ...(i.e.: to trigger the Animation Clips).
    //
    /// <summary>
    /// Hash ANIMATOR (Parameter) CONSTANTS:   IsWalking  (Animator State)
    /// </summary>
    private static readonly int _IS_WALKING_ANIMATOR_PARAMETER = Animator.StringToHash("IsWalking");

    #endregion Animator & Animations

    #endregion Attributes
    
    #region Unity Methods

    private void Awake()
    {
        // Misc Optimization: Calculating the (accepted) Square Min Distance.
        //
        _sqrStoppingDistance = _stoppingDistance * _stoppingDistance;
        
        // Initialize Target Position to this Script's base GameObject.
        //
        _targetPosition = this.transform.position;
    }

    // Start is called before the first frame update
    // void Start()
    // {
    //     
    // }

    // Update is called once per frame
    void Update()
    {
        
        UpdateUnitMove();
        
    }

    
    #endregion Unity Methods
    
    #region My Custom Methods
    
    
    public void Move(Vector3 newTargetPosition)
    {
        _targetPosition = newTargetPosition;
    }
    
    void UpdateUnitMove()
    {
        
        // Calculate the Distance... to see how close or far
        // ...is the Mouse Pointer -> from -> The Unit we want to Move().
        //
        if (Vector3.SqrMagnitude(transform.position - _targetPosition) > _sqrStoppingDistance)
        {
            Vector3 moveDirection = (_targetPosition - transform.position).normalized;

            // Move
            // 1- Translation:
            //
            transform.position += moveDirection * (_moveSpeed * Time.deltaTime);
            //
            // 2- Rotation
            //
            RotateUnitUsingVector3SlerpApproach(moveDirection);
            
            
            // 3- Update the Animator's Parameter: Start/Keep on Walking.
            //
            _unitAnimator.SetBool(_IS_WALKING_ANIMATOR_PARAMETER, true);
        }
        else
        {
            // Update the Animator's Parameter:  STOP  (Walking).
            //
            _unitAnimator.SetBool(_IS_WALKING_ANIMATOR_PARAMETER, false);
            
        }//End else of if (Vector3.SqrMagnitude...
    }

    
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

    #endregion My Custom Methods
    
}
