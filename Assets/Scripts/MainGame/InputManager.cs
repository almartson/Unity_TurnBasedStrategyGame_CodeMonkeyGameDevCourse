/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
#define USE_NEW_INPUT_SYSTEM

using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    #region Attributes
    
    #region Singleton Pattern's
    
    [Tooltip("Singleton Pattern's Main Key: Instance of this very Class")] 
    public static InputManager Instance { get; private set; }

    #endregion Singleton Pattern's

    
    #region Camera (Cinemachine) Move + Rotation Vector
    
    // Translation Movement
    
    // Input
    [Space(10)] // 10 pixels of spacing here.
    [Header("Movement")]
    //
    [Tooltip("Current Movement Direction Vector3, gotten from the user's Input")]
    [SerializeField]
    private Vector2 _inputMoveDirection = new Vector2(0, 0);
    
    // Rotation Movement
    
    [Space(10)] // 10 pixels of spacing here.
    [Header("Rotation")]
    
    [Tooltip("Current Rotation (movement) Vector3")]
    [SerializeField]
    private Vector3 _rotationVector = new Vector3(0, 0, 0);
    
    #endregion Camera (Cinemachine) Move + Rotation Vector

    
    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>
    private void Awake()
    {

        #region Singleton Pattern's
        
        // Singleton Pattern, protocol:
        //
        // Validation: There MUST be ONLY ONE Instance of this Class (i.e.: ONE Object):
        //
        if (Instance != null)
        {
            Debug.LogError("There's more than one '" + GetType().Name + "'!. GameObject: ---> " + transform + "  - " + Instance);
            //
            // Destroy, to be able to continue the Gameplay (i.e.: Recovery from the Error/Exception...)
            //
            Destroy(gameObject);
            return;
        }
        //
        // If everything went well, create / assign THIS Instance:
        //
        Instance = this;
        
        #endregion Singleton Pattern's
        
    }//End Awake


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>



    /// <summary>
    /// Update is called once per frame
    /// </summary>


    #endregion Unity Methods


    #region My Custom Methods

    public Vector2 GetMouseScreenPosition()
    {
#if USE_NEW_INPUT_SYSTEM
        return MouseWorld.current.position.ReadValue();
#else
        return Input.mousePosition;
#endif
    }

    public bool IsMouseButtonDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    public Vector2 GetCameraMoveVector()
    {
        // 1- Reset the 'Movement' Input Vector: Set it as stationary every frame
        //
        _inputMoveDirection.Set(0, 0);
        //
        // 2- Get the Player's Input, and Set it in the CameraController.
        //
        if (Input.GetKey(KeyCode.W))
        {
            _inputMoveDirection.y = +1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            _inputMoveDirection.y = -1f;
        }

        if (Input.GetKey(KeyCode.Q)) // ORIGINAL: (KeyCode.A))
        {
            _inputMoveDirection.x = -1f;
        }

        if (Input.GetKey(KeyCode.E)) // ORIGINAL: (KeyCode.D))
        {
            _inputMoveDirection.x = +1f;
        }

        return _inputMoveDirection;

    }//End GetCameraMoveVector


    public float GetCameraRotateAmount()
    {
        // 0- Reset the 'Rotation' Input Vector: Set it as stationary every frame
        //
        float rotateAmount = 0f;
        _rotationVector.Set(0, 0, 0);

        // 1- Get the Rotation (user's)  Input:
        //
        if (Input.GetKey(KeyCode.A)) // ORIGINAL: (KeyCode.Q))
        {
            rotateAmount = +1f;
            _rotationVector.y = +1f;
        }

        if (Input.GetKey(KeyCode.D)) // ORIGINAL: (KeyCode.E))
        {
            rotateAmount = -1f;
            _rotationVector.y = -1f;
        }
        
        return rotateAmount;
        
    }//End GetCameraRotateAmount


    public float GetCameraZoomAmount()
    {
        float zoomAmount = 0f;
        
        // We update Cinemachine's "y" value of the Pan of Camera:
        // It could be a ZOOM-IN or a ZOOM-OUT, depending on the
        //...direction of the movement of the mouse's scrollwheel:
        //
        if (Input.mouseScrollDelta.y > 0)
        {
            zoomAmount = -1f;
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            zoomAmount = +1f;
        }

        return zoomAmount;
        
    }//End GetCameraZoomAmount
    
    #endregion My Custom Methods

}
