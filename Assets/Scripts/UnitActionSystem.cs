using System;
using UnityEngine;

/// <summary>
/// Uses the Observer Pattern + Singleton Pattern (for easily assigning it to the GameObject of its related Prefab...)
/// It is the (Main and only...): Subject / Publisher of the Events. 
/// </summary>
public class UnitActionSystem : MonoBehaviour
{

    #region Attributes
    
    [Tooltip("Singleton Pattern's Main Key: Instance of this very Class")] 
    public static UnitActionSystem Instance { get; private set; }
    
    /// <summary>
    /// Event, to be Fired/Published when the User clicks on any Character of his/her Team (called 'Unit'). Using the Observer Pattern.
    /// </summary>
    [Tooltip("Event, to be Fired/Published when the User clicks on any Character of his/her Team (called 'Unit'). Using the Observer Pattern.")]
    public event EventHandler OnSelectedUnitChanged;
    
    [Tooltip("Selected Character of the User's Team (called 'Unit').")]
    [SerializeField]
    private Unit _selectedUnit;
    
    
    /// <summary>
    /// Raycast Hit (past) Summary / info.
    /// </summary>
    private RaycastHit[] _raycastHitInfo;


    /// <summary>
    /// What Layer is this affecting to? => Unit (LayerMask)
    /// </summary>
    [SerializeField] private LayerMask _unitLayerMask = 7;
    /// <summary>
    /// Public Getter for _unitLayerMask
    /// </summary>
    public int UnitLayerMask => _unitLayerMask;

    
    #endregion Attributes
    
    
    #region Unity Methods

    // Start is called before the first frame update
    private void Awake()
    {
        
        // RayCast info (for the Collisions):
        //
        _raycastHitInfo = new RaycastHit[3];
        
        // Singleton Pattern, protocol:
        //
        // Validation: There MUST be ONLY ONE Instance of this Class (i.e.: ONE Object):
        //
        if (Instance != null)
        {
            
            Debug.LogError("There's more than one 'UnitActionSystem'!. GameObject: ---> " + transform + "  - " + Instance);
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
        
    }//End Awake
    
    private void Update()
    {
        
        // Get the Mouse Pointer Click & (x, y, z) Screen Position.
        //
        if (Input.GetMouseButtonDown(0))
        {
            // Try to select an Unit (a Character / Soldier, etc):
            //
            if (TryHandleUnitSelection()) return;

            
            // Try to Move the selected Unit... (by Raycasting on the Ground Plane (Mask Layer...))
            //
            if (MouseWorld.TryGetPosition(out Vector3 mousePosition))
            {
                
                // Get the CENTER of the selected "GridPosition", instead of a corner or any random position inside of it
                // ...because sometimes the Player/user clicks in random places of a Cell/Square/Grid,
                // ...not necessarily in the CENTER of it:
                //
                GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(mousePosition);
                
                // Save the original Mouse Position (just in case... as a backup)
                //
                _selectedUnit.MousePosition.Set(mouseGridPosition.x, 0, mouseGridPosition.z);
            
                
                // Validate:
                // Whether the selected GridPosition (x, 0, z) is empty & 100% correct for the MoveAction:
                //
                if (_selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
                {
                    
                    // Move() where the Mouse Pointer CLICK has been Pressed!
                    //
                    _selectedUnit.GetMoveAction().Move( mouseGridPosition );
                    
                }//End if

            }//End if (MouseWorld.TryGetPosition

        }//End if (Input.GetMouseButtonDown(0))

        // Spin Action
        // Temporary Debug TEST   (by the Code Monkey & Alec AlMartson)
        //
        if (Input.GetMouseButtonDown(1))
        {
            // Enable: Spin
            //
            _selectedUnit.GetSpinAction().Spin();
            
        }//End if (Input.GetMouseButtonDown(1))
        
        
    }//End Update

    #endregion Unity Methods
    
    #region My Custom Methods


    /// <summary>
    /// Allows you to select an UNIT (Character), for giving him/her orders later.
    /// How? By shooting a Raycast from the camera across the Mouse Pointer to the Game World, and returning the hit data.
    /// </summary>
    /// <returns>
    /// <p>True    if the Raycast is successful in hitting an UNIT (i.e.: there is HitData type: RaycastHit)</p>
    /// False   if the user clicked on an not permitted area,
    ///...or if in any case the Raycast is NOT successful in hitting an UNIT (i.e.: filtered by the LayerMask).
    /// </returns>
    private bool TryHandleUnitSelection()
    {
      
        // Collision Check, using a Raycast:
        // Get the Main Scene Camera:
        //
        if (MouseWorld.MainSceneCamera != null)         // TODO: Fix the != NULL (OPTIMIZATION)
        {
            
            // Check the Mouse-Pointer Coordinates on the Screen: 
            //
            Ray ray = MouseWorld.MainSceneCamera.ScreenPointToRay(Input.mousePosition);
            //
            // Physics RayCast:
            //
            if (Physics.RaycastNonAlloc(ray, _raycastHitInfo, float.MaxValue,
                    _unitLayerMask) > 0)
            {
                
                // 'Try' to check (it also returns a Boolean...): does it have a Component of Type <Unit>??
                //
                if (_raycastHitInfo[0].transform.TryGetComponent<Unit>(out Unit unit))
                {
                    
                    SetSelectedUnit(unit);
                    return true;
                }
                
            }//End if ((Physics.RaycastNonAlloc...
            
        }//End if (_camera != null)
        else
        {
            // Camera is NULL...
            // Log Error:
            //
            ////Debug.LogError("'MouseWorld.MainSceneCamera' is NULL");

            return false;
        }
        
        return false;

    }//End Method
    
    
    #region Observer Pattern Methods

    private void SetSelectedUnit(Unit unit)
    {
        
        _selectedUnit = unit;
        
        // Fire the EVENT (Observer Pattern) from the PUBLISHER (i.e.: represented by this Class).
        // 1- Do a NULL check on the EventHandler:
        // 2- Trigger the Event..: OnSelectedUnitChanged()
        //   2.1- Code OPTION 1:   Verbose Way:
        //
        // if (OnSelectedUnitChanged != null)
        // {
        //     OnSelectedUnitChanged(this, EventArgs.Empty);
        // }
        //
        //   2.2- Code OPTION 2:   Short - Compact Way:
        //...? -> Represents the NULL Check (i.e.: if (OnSelectedUnitChanged != null)...)
        //....Invoke(this, EventArgs.Empty) -> Represents the EVENT call: (i.e.:   OnSelectedUnitChanged(this, EventArgs.Empty) )
        //
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);

    }


    public Unit GetSelectedUnit()
    {
        return _selectedUnit;
    }
    
    
    #endregion Observer Pattern Methods
    
    #endregion My Custom Methods

    
}
