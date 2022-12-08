using System;
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

    // Input
    
    [Tooltip("Current Movement Direction Vector3, gotten from the user's Input")]
    [SerializeField]
    private Vector3 _inputMoveDirection = new Vector3(0, 0, 0);

    // Translation Movement
    
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



    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        // 1- Reset the 'Movement' Input Vector: Set it as stationary every frame
        //
        _inputMoveDirection.Set(0, 0, 0 );
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
        if (Input.GetKey(KeyCode.A))
        {
            _inputMoveDirection.x = -1f;
        }
        if (Input.GetKey(KeyCode.D))
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
        
        // Rotation:
        
        // 0- Reset the 'Rotation' Input Vector: Set it as stationary every frame
        //
        _rotationVector.Set(0, 0, 0);
        
        // 1- Get the Rotation (user's)  Input:
        //
        if (Input.GetKey(KeyCode.Q))
        {
            _rotationVector.y = +1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            _rotationVector.y = -1f;
        }
        
        // 2- Set the new Rotation, by using the eulerAngles:
        //
        _cachedTransform.eulerAngles += _rotationVector * (_rotationSpeed * Time.deltaTime);

    }

    #endregion Unity Methods
    

    #region My Custom Methods





    #endregion My Custom Methods

}
