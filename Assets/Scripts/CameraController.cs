using System;
using Cinemachine;
using UnityEngine;

/// <summary>
/// This allows to move the 'CameraController' (which is the Cinemachine's
///...Target to Follow and to Aim to...) via C# Code, anytime I want
///...(and that will be done in each Turn during the game,
///...when a new action happens...), so the Cinemachine Camera System
///...will follow this GameObject and C# code.
/// </summary>
public class CameraController : MonoBehaviour
{

    #region Attributes

    #region Customizing the Cameras' Target Follow Offset (the "Y" value)
    
    /// <summary>
    /// Constant to limit the Camera Movements. Minimum Y
    /// </summary>
    private const float _MIN_FOLLOW_Y_OFFSET = 2f;
    
    /// <summary>
    /// Constant to limit the Camera Movements. Maximun Y
    /// </summary>
    private const float _MAX_FOLLOW_Y_OFFSET = 12f;
    
    
    [Tooltip("Virtual Camera associated to this CameraController")]
    [SerializeField]
    private CinemachineVirtualCamera _cinemachineVirtualCamera;

    /// <summary>
    /// New Calculated Value of Cinemachine's 'm_FollowOffset' variable, which acts as a zoom variable. It  is calculated when the user uses the mouse wheel.
    /// </summary>
    private Vector3 _targetFollowOffset;
    
    
    [Tooltip("Virtual Camera Component-Behaviour, associated to this CameraController's Transform...Position")]
    private CinemachineTransposer _cinemachineTransposer;
    
    #endregion  Customizing the Cameras' Target Follow Offset (the "Y" value)

    
    // Translation Movement
    
    // Input
    
    [Tooltip("Current Movement Direction Vector3, gotten from the user's Input")]
    [SerializeField]
    private Vector3 _inputMoveDirection = new Vector3(0, 0, 0);
    
    // Movement
    
    [Tooltip("Current Movement Direction Vector3")]
    [SerializeField]
    private Vector3 _moveVector = new Vector3(0, 0, 0);
    
    [Tooltip("Current Movement Speed")]
    [SerializeField] 
    private float _moveSpeed = 10.0f;

    // Rotation Movement
    
    [Tooltip("Current Rotation (movement) Vector3")]
    [SerializeField]
    private Vector3 _rotationVector = new Vector3(0, 0, 0);

    [Tooltip("Current Rotation Speed")]
    [SerializeField] 
    private float _rotationSpeed = 100.0f;
    
    // Optimizations
    
    /// <summary>
    /// Transform cache
    /// </summary>
    private Transform _cachedTransform;



    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round.
    /// </summary>
    private void Awake()
    {
        // Optimization: accessing original Transform only once, not more times.
        //
        _cachedTransform = transform;
    }


    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        // We access 'CinemachineTransposer.cs' to try and change the "y" value of the Pan of the Cinemachine Camera, if the user uses the mouse scrollwheel:
        //
        _cinemachineTransposer = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        //
        // Initial value of 'm_FollowOffset' (it will change):
        //
        _targetFollowOffset = _cinemachineTransposer.m_FollowOffset;
    }


    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        // Translation - Movement:
        //
        HandleMovement();
        
        // Rotation:
        //
        HandleRotation();
        
        // Zoom:
        //
        HandleZoom();
    }
    
    #endregion Unity Methods
    

    #region My Custom Methods

    private void HandleMovement()
    {
        // 1- Reset the 'Movement' Input Vector: Set it as stationary every frame
        //
        _inputMoveDirection.Set(0, 0, 0);
        //
        // 2- Get the Player's Input, and Set it in the CameraController.
        //
        if (Input.GetKey(KeyCode.W))
        {
            _inputMoveDirection.z = +1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            _inputMoveDirection.z = -1f;
        }

        if (Input.GetKey(KeyCode.Q)) // ORIGINAL: (KeyCode.A))
        {
            _inputMoveDirection.x = -1f;
        }

        if (Input.GetKey(KeyCode.E)) // ORIGINAL: (KeyCode.D))
        {
            _inputMoveDirection.x = +1f;
        }

        //
        // 3- Set the variables with the new movement value(s):
        //
        // .1- We Set the Movement Vector3 from: the user's Input (that's Vector3_1) + Current Position Vector3 (that's another one Vector3_2), we sum (Sum of Vector3's) them all: 
        //
        _moveVector = _cachedTransform.forward * _inputMoveDirection.z + _cachedTransform.right * _inputMoveDirection.x;
        //
        // .2- Finally: we apply the resulting Vector3 (of Movement) taking into account the Speed * deltaTime:
        //
        _cachedTransform.position += _moveVector * (_moveSpeed * Time.deltaTime);
    }

    
    /// <summary>
    /// Handles the Rotation of the Camera, whe the user presses a key on the Keyboard.
    /// </summary>
    private void HandleRotation()
    {
        // 0- Reset the 'Rotation' Input Vector: Set it as stationary every frame
        //
        _rotationVector.Set(0, 0, 0);

        // 1- Get the Rotation (user's)  Input:
        //
        if (Input.GetKey(KeyCode.A)) // ORIGINAL: (KeyCode.Q))
        {
            _rotationVector.y = +1f;
        }

        if (Input.GetKey(KeyCode.D)) // ORIGINAL: (KeyCode.E))
        {
            _rotationVector.y = -1f;
        }

        // 2- Set the new Rotation, by using the eulerAngles:
        //
        _cachedTransform.eulerAngles += _rotationVector * (_rotationSpeed * Time.deltaTime);
    }

    
    /// <summary>
    /// Manages and updates the Zoom (in and out), when the user uses the mouse wheel.
    /// </summary>
    private void HandleZoom()
    {
        float zoomAmount = 1f;

        // We update Cinemachine's "y" value of the Pan of Camera:
        // It could be a ZOOM-IN or a ZOOM-OUT, dependong on the
        //...direction of the movement of the mouse's scrollwheel:
        if (Input.mouseScrollDelta.y > 0)
        {
            _targetFollowOffset.y -= zoomAmount;
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            _targetFollowOffset.y += zoomAmount;
        }

        // We limit the Camera Movement to the Constraints:
        //
        _targetFollowOffset.y = Mathf.Clamp(_targetFollowOffset.y, _MIN_FOLLOW_Y_OFFSET, _MAX_FOLLOW_Y_OFFSET);
        //
        // Updating the Camera's "Y" (m_FollowOffset)  Value:
        //
        float zoomSpeed = 5f;
        //
        _cinemachineTransposer.m_FollowOffset = Vector3.Lerp(_cinemachineTransposer.m_FollowOffset, _targetFollowOffset,
            Time.deltaTime * zoomSpeed);
    }



    #endregion My Custom Methods

}
