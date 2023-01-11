using UnityEngine;

public class MouseWorld : MonoBehaviour
{

    #region  Attributes

    private static MouseWorld _thisInstance;
    
    
    private static Camera _mainSceneCamera;
    //
    /// <summary>
    /// Property Accessor to Private Field "_mainSceneCamera".
    /// </summary>
    /// <value></value>
    public static Camera MainSceneCamera { get => _mainSceneCamera; private set => _mainSceneCamera = value; }
    
    /// <summary>
    /// Raycast Hit (past) Summary / info.
    /// </summary>
    private static RaycastHit[] _raycastHitInfo;


    /// <summary>
    /// What Layer is this affecting to?
    /// </summary>
    [SerializeField] private LayerMask _mousePlaneLayerMask = 6;
    /// <summary>
    /// Public Getter for _mousePlaneLayerMask
    /// </summary>
    public int MousePlaneLayerMask => _mousePlaneLayerMask;
    
    // // OPTION 2::::
    // [field: SerializeField] public LayerMask MousePlaneLayerMask { get; private set; } = 6;

    
    #endregion Attributes


    #region Unity Methods

    // Start is called before the first frame update
    private void Awake()
    {
        _thisInstance = this;
        
        _mainSceneCamera = Camera.main;
        _raycastHitInfo = new RaycastHit[7];

    }

    // void Start()
    // {
    //     
    // }

    // Update is called once per frame
    void Update()
    {
        ///// Debug.Log(Input.mousePosition);
        
        // TakeAction this Emissive Sphere GameObject (..to which this Script is attached...) to the Mouse Pixel-Coordinate Position:
        //
        transform.position = MouseWorld.GetPosition();
    
    }//End update
    
    #endregion Unity Methods

    
    #region Custom Methods

    /// <summary>
    /// Cast a Raycast from the camera across the Mouse Pointer to the Game World, and return the hit data.
    /// </summary>
    /// <returns>
    /// <p>True    if the Raycast is successful (i.e.: there is HitData type: RaycastHit)</p>
    /// False   if the user clicked on an not permitted area,
    ///...or if in any case the Raycast is NOT successful (i.e.: filtered by the LayerMask).
    /// </returns>
    public static bool TryGetPosition(out Vector3 mousePosition)
    {
        // Main Flag of this Method:
        //
        bool isAhit = false;
        
        // Collision Check, using a Raycast:
        //
        if (_mainSceneCamera != null)           // TODO: Fix "!= null" (performance optimization) 
        {
            // Check the Mouse-Pointer Coordinates on the Screen:
            //
            Ray ray = _mainSceneCamera.ScreenPointToRay(Input.mousePosition);
            //
            // Physics RayCast:
            //
            isAhit = ( Physics.RaycastNonAlloc(ray, _raycastHitInfo, float.MaxValue, _thisInstance._mousePlaneLayerMask) > 0 );
            
            /////Debug.Log( isAhit );
         
            // Save the Hit (collision) info into the 'out' (output) variable of this function: a Vector3.
            //
            mousePosition = _raycastHitInfo[0].point;
            //
            // A Hit (or maybe NOT)?
            // Return it:
            //
            return isAhit;

        }//End if (_mainSceneCamera != null)
        
        // Camera is NULL...
        // Return FALSE:
        //
        mousePosition = Vector3.zero;
        return false;
        
    }//End Method
        
    
    /// <summary>
    /// Cast a Raycast from the camera across the Mouse Pointer to the Game World, and return the hit data.
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetPosition()
    {
        if (_mainSceneCamera != null)           // TODO: Fix "!= null" (performance optimization) 
        {
            // Check the Mouse-Pointer Coordinates on the Screen:
            //
            Ray ray = _mainSceneCamera.ScreenPointToRay(Input.mousePosition);
            //
            // Physics RayCast:
            //
            bool isAhit = ( Physics.RaycastNonAlloc(ray, _raycastHitInfo, float.MaxValue, _thisInstance._mousePlaneLayerMask) > 0 );
            
            /////Debug.Log( isAhit );
            
            if (isAhit)
            {            
                
                if (! _raycastHitInfo[0].point.Equals(Vector3.zero))
                {
                
                    //////Debug.Log(_thisInstance._raycastHitInfo[0].point);
       
                    // Return the Contact-Point (of the Raycast):
                    //
                    return _raycastHitInfo[0].point;
                    
                }//End if (! _raycastHitInfo[0].point.Equals(Vector3.zero))
                
            }//End if (isAhit)


            ///// My Original code (AlMartson, 19/10/2022 < 9:49 pm)
            //     if (isAhit)
            //     {            
            //         foreach (var contactPoint in _raycastHitInfo)
            //         {
            //
            //             if (contactPoint.point.Equals(Vector3.zero))
            //                 break;
            //             
            //             Debug.Log(contactPoint.point);
            //
            //         }//End foreach
            //     }//End if (isAhit)
            

        }//End if (_mainSceneCamera != null)
        
        // Return the Contact-Point (of the Raycast): as (0,0,0)
        //...because there was no Hit.
        //
        return Vector3.zero;
        
    }//End Method
    
    
    #endregion Custom Methods
    
}
