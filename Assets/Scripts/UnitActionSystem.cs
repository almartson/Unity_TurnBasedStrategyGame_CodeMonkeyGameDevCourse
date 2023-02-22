using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Uses the Observer Pattern PLUS a Singleton Pattern (for easily assigning it to the GameObject of its related Prefab...)
/// It is the (Main and only...): Subject / Publisher of the Events. 
/// </summary>
public class UnitActionSystem : MonoBehaviour
{
    #region Attributes
    
    #region Singleton Pattern's
    
    [Tooltip("Singleton Pattern's Main Key: Instance of this very Class")] 
    public static UnitActionSystem Instance { get; private set; }

    #endregion Singleton Pattern's
    
    
    #region Observer Pattern's
    
    /// <summary>
    /// Observer Pattern's: Event, to be Fired/Published when the User clicks on any Character of his/her Team (called 'Unit'). Using the Observer Pattern.
    /// </summary>
    [Tooltip("Observer Pattern's: Event, to be Fired/Published when the User clicks on any Character of his/her Team (called 'Unit'). Using the Observer Pattern.")]
    public event EventHandler OnSelectedUnitChanged;
    
    
    /// <summary>
    /// Observer Pattern's: Event, to be Fired/Published when the User clicks on any UI ACTION Button... so we are using Delegates to Listen to when a Click on an GUI ACTION (Button) happens. Using the Observer Pattern here too, as above with the Units.
    /// </summary>
    [Tooltip("Observer Pattern's: Event, to be Fired/Published when the User clicks on any UI ACTION Button... so we are using Delegates to Listen to when a Click on an GUI ACTION (Button) happens. Using the Observer Pattern here too, as above with the Units.")]
    public event EventHandler OnSelectedActionChanged;
    
    
    /// <summary>
    /// Observer Pattern's: Event, to be Fired/Published when the System (UI + Logic) is BUSY working on an ACTION: An Animation is being displayed and the Logic being processed + the UI would ShowAndSetMaterial an Image stating:  I AM BUSY...... so we are using Delegates to Listen to when a Click on an GUI ACTION (Button) happens, which Triggers a method called 'Busy'... we are ACTUALLY (IN REALITY...): listening to when 'Busy' is FIRED. Using the Observer Pattern here too, as above with the Units.
    /// </summary>
    [Tooltip("Observer Pattern's: Event, to be Fired/Published when the System (UI + Logic) is BUSY working on an ACTION: An Animation is being displayed and the Logic being processed + the UI would ShowAndSetMaterial an Image stating:  I AM BUSY...... so we are using Delegates to Listen to when a Click on an GUI ACTION (Button) happens, which Triggers a method called 'Busy'... we are ACTUALLY (IN REALITY...): listening to when 'Busy' is FIRED. Using the Observer Pattern here too, as above with the Units.")]
    public event EventHandler<bool> OnBusyWorkingOnAnActionChanged;
    
    
    #region Action Points GUI: UI Representation, Visuals for ActionPoints
    
    /// <summary>
    /// Observer Pattern's: Event, to be Fired/Published when the Player starts an Action (UI + Logic). At that moment we want the NEW (Updated) available ActionPoints UI Visual number to be displayed on the screen accordingly.
    /// </summary>
    [Tooltip("Observer Pattern's: Event, to be Fired/Published when the Player starts an Action (UI + Logic). At that moment we want the NEW (Updated) available ActionPoints UI Visual number to be displayed on the screen accordingly.")]
    public event EventHandler OnActionStarted;
    
    
    #endregion Action Points GUI: UI Representation, Visuals for ActionPoints
    
    #endregion Observer Pattern's
    
    
    #region Mutex LOCK: Managing (allowing only...) just ONE Action at a Time
    
    /// <summary>
    /// Boolean to allow only just ONE Action (i.e.: MoveAction, SpinAction, etc),  at a Time.
    /// </summary>
    [Tooltip("Boolean to allow only just ONE Action (i.e.: MoveAction, SpinAction, etc),  at a Time.")]
    [SerializeField]
    private bool _isBusy;

    
    #endregion Mutex LOCK: Managing (allowing only...) just ONE Action at a Time Time
    
    
    [Tooltip("Selected Character of the User's Team (called 'Unit').")]
    [SerializeField]
    private Unit _selectedUnit;
    
    
    [Tooltip("Selected Action (by Clicking on the GUI) for the Character of the User's Team (called 'Unit').")]
    private BaseAction _selectedAction;
    
    
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
    
    
    private void Awake()
    {
        
        // RayCast info (for the Collisions):
        //
        _raycastHitInfo = new RaycastHit[3];
        
        
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


    private void Start()
    {
        SetSelectedUnit( _selectedUnit );
        
    }//End Start()


    private void Update()
    {
        // Managing (allowing only...) just ONE Action at a Time:
        //
        if (_isBusy)
        {
            return;
        }
        
        // if this is NOT the Player's Turn:  then it is the ENEMY'S Turn
        //...so:  return   (the Player can't play as the CPU...)
        //
        if (!TurnSystem.Instance.IsPlayerTurn)
        {
            return;
        }
        
        // Fix to: Keep the GUI ACTION Buttons on the Top
        // (of the Grid, Game & Units:)
        // NOTE:  This does NOT work on ANDROID, but there is a fix to make it work, based on .fingers... (see my Notes on this Commit and the 2 next Commits, Chapter 04)
        //
        // This is to keep the the GUI always in front of everything,
        //...so if you Click on an Empty Space and at the same time on a
        //...GUI (ACTION: MOVE ACTION, SPIN ACTION, GRENADE, ETC...) Button:
        //  The Game will receive ONLY the GUI order, and it will block the
        //  Mouse Pointer Click to go to the Grid that's behind the GUI Button. 
        //
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // Then (if it's TRUE): the Mouse is over: a GUI Element
            // Stop all the rest of actions, in this Frame:
            // ...we don't want to 'TryHandleUnitSelection'  or  'HandleSelectedAction'  if  the Player clicked on a GUI (ACTION) Button:
            //
            return;
        }
        
        // Get the Mouse Pointer Click & (x, y, z) Screen Position.
        //
        if (Input.GetMouseButtonDown(0))
        {
            #region Try to SELECT a friendly UNIT: CodeMonkey's Original Implementation
            
            // // Try to select an Unit (a Character / Soldier, etc):
            // //
            // if (TryHandleUnitSelection())
            // {
            //     return;
            // }
            
            #endregion Try to SELECT a friendly UNIT: CodeMonkey's Original Implementation
            
            
            #region Try to SELECT a ++ANY++ UNIT: CodeMonkey's Original Implementation
            
            // After the MOUSE CLICK:
            // Try to SELECT a ++ANY++ UNIT (a Character / Soldier... FRIENDLY on my TEAM, ... or an ENEMY).. that was CLICKED ON with the Mouse Pointer:
            //
            if (TryGetClickedUnit(out Unit clickedUnit))
            {
                // Try to get the UNIT that was clicked
                // &
                // Try to handle the UNIT if it's a PLAYER
                //
                if (TryHandlePlayerUnitClicked(clickedUnit))
                {
                    return;

                }//End if (TryHandlePlayerUnitClicked(clickedUnit))
                //
                // Try to handle the UNIT if it's an ENEMY
                //
                if (TryHandleEnemyUnitClicked(clickedUnit))
                {
                    return;

                }//End if (TryHandleEnemyUnitClicked(clickedUnit))

            }//End if (TryGetClickedUnit(..
            
            #endregion Try to SELECT a ++ANY++ UNIT: CodeMonkey's Original Implementation
            
            
            // Try to: Receive (process...) the 'Action'  (from a mouse click on the GUI)
            //
            HandleSelectedAction();

        }//End if (Input.GetMouseButtonDown(0))
        
    }//End Update

    #endregion Unity Methods
    
    
    #region My Custom Methods

    /// <summary>
    /// Selects / Sets an ACTION selected by clicking on the (related Action's...) GUI Button.
    /// </summary>
    /// <returns>True or False for the Success of the previous >Validations, before performing the 'TakeAction()' routine, called inside of this Method.</returns>
    private bool HandleSelectedAction()
    {
        // THERE ARE 2 (Architecture) OPTIONS to make an ACTION Selection:
        //
        // 1- Option 1: Each ACTION is a separate FUNCTION + switch - case (C# 7) calling the Particular one we want.
        //
        // 2- Option 2: Only ONE General Function Handles everything, using Abstract and Virtual Classes (..working as a General Interface, a Contract...) and Functions to be Implemented in each particular way inside each particular ActionClass (derived from BaseAction Class...). This one should take a big number of Input Parameters, that cover ALL scenarios for all the Actions of the Game (although we could create a class for the Input... and make particular children for each Action, so we could cast the particular Type in line one of this Method... but we are not going to cover that in this Game because it would be for bigger AAA Games...)
        // So...
        //
        #region 1- Option 1: Each ACTION is a separate FUNCTION + switch - case (C# 7) calling the Particular one we want.
        
        // // 1- Option 1: Each ACTION is a separate FUNCTION + switch - case (C# 7) calling the Particular one we want.
        // //...since C# 7.0:  we can use switch - case with Types as Scripts and declare them inline in the 'case : '
        // //
        // // Try to TakeAction the selected Unit... (by Raycasting on the Ground Plane (Mask Layer...))
        // //
        // if (MouseWorld.TryGetPosition(out Vector3 mousePosition))
        // {
        //     // Get the CENTER of the selected "GridPosition", instead of a corner or any random position inside of it
        //     // ...because sometimes the Player/user clicks in random places of a Cell/Square/Grid,
        //     // ...not necessarily in the CENTER of it:
        //     //
        //     GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(mousePosition);
        //     
        //     // Save the original Mouse Position (just in case... as a backup)
        //     //
        //     _selectedUnit.MousePosition.Set(mouseGridPosition.x, 0, mouseGridPosition.z);
        //
        //     // Switch - case for > C# 7 : Using inline declarations of Script or Class Types, to compare in the cases...
        //     //
        //     switch (_selectedAction)
        //     {
        //         case MoveAction moveAction:
        //             
        //             // Validate:
        //             // Whether the selected GridPosition (x, 0, z) is empty & 100% correct for the MoveAction:
        //             //
        //             if (moveAction.IsValidActionGridPosition(mouseGridPosition))
        //             {
        //                 // Set this Class (SERVICE) Methods as: BUSY .. until it ends:
        //                 //
        //                 SetBusy();
        //             
        //                 // TakeAction() where the Mouse Pointer CLICK has been Pressed!
        //                 //
        //                 moveAction.TakeAction( mouseGridPosition, ClearBusy );
        //             
        //             }//End if
        //             break;
        //         
        //         case SpinAction spinAction:
        //             
        //             // Set this Class (SERVICE) Methods as: BUSY (so no other ACTION would execute at the same time)..
        //             // ..until THIS PARTICULAR ACTION (i.e.: TakeAction() ): ends:
        //             //
        //             SetBusy();
        //
        //             // Enable: TakeAction
        //             // Invoke / Call the FUnction Delegate: to execute:   ClearBusy()
        //             // ( ClearBusy():  tells the World that this ROUTINE JUST ENDED: )
        //             //
        //             spinAction.TakeAction(ClearBusy);
        //             break;
        //     
        //     }//End switch - case
        //     
        // }//End if (MouseWorld.TryGetPosition
        
        #endregion 1- Option 1: Each ACTION is a separate FUNCTION + switch - case (C# 7) calling the Particular one we want.

        
        #region 2- Option 2: Only ONE General Function Handles everything, using Abstract and Virtual Classes (..working as a General Interface, a Contract...) and Functions to be Implemented in each particular way inside each particular ActionClass (derivated from BaseAction Class...).
        
        // 2- Option 2: Only ONE General Function Handles everything, using Abstract and Virtual Classes (..working as a General Interface, a Contract...) and Functions to be Implemented in each particular way inside each particular ActionClass (derived from BaseAction Class...). This one should take a big number of Input Parameters, that cover ALL scenarios for all the Actions of the Game (although we could create a class for the Input... and make particular children for each Action, so we could cast the particular Type in line one of this Method... but we are not going to cover that in this Game because it would be for bigger AAA Games...)
        //
        // Try to TakeAction the selected Unit... (by Raycasting on the Ground Plane (Mask Layer...))
        //
        if (MouseWorld.TryGetPosition(out Vector3 mousePosition))
        {
            // Get the CENTER of the selected "GridPosition", instead of a corner or any random position inside of it
            // ...because sometimes the Player/user clicks in random places of a Cell/Square/Grid,
            // ...not necessarily in the CENTER of it:
            //
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(mousePosition);

            // Take the Action.
            
            // 'TakeAction' method has a particular Implementation in each of the derived Classes (e.g.: MoveAction, SpinAction, etc.).
            //
            //  .1- Validation of the Action:
            //
            if (_selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {

                // Try to Spend this Unit's available "actionPoints": on this Action
                //
                if (_selectedUnit.TrySpendActionPointsToTakeAction(_selectedAction))
                {
                    // .2.0- The actionPoints are Spent / used by now, already...
                    
                    // .2- 'Take the Action'
                    //
                    // .2.1- Save the Valid GridPosition:
                    //
                    // .2.1.1- Save the original Mouse Position (just in case... as a backup)
                    //
                    _selectedUnit.MousePosition.Set(mouseGridPosition.x, 0, mouseGridPosition.z);
                    //
                    // .2.1.1- In _selectedUnit, for later use in 'TakeAction()':
                    //
                    _selectedUnit.SetFinalGridPositionOfNextPlayersAction(mouseGridPosition);
                
                
                    // .3- Set this Class (SERVICE) Methods as: BUSY .. until it ends:  Set MUTEX ON
                    //
                    SetBusy();
                    //
                    // .4- TakeAction() , asked by the Player, on the Game
                    // ( ClearBusy():  tells the World that this ROUTINE JUST ENDED: ) -> Sets Mutex OFF (when TakeAction() Ends...)
                    //
                    _selectedAction.TakeAction(ClearBusy);
                    
                    
                    // Update the GUI for ActionPoints:
                    //
                    OnActionStarted?.Invoke(this, EventArgs.Empty);

                    // Return the Success/Failure State of the 'Take Action' process:
                    //
                    return true;
                    
                }//End if (_selectedUnit.CanSpendActionPointsToTakeAction...
                else
                {
                    // Did not pass the Validation:     CanSpendActionPointsToTakeAction(...)
                    // Return the Success/Failure State of the 'Take Action' process:  false (failure)
                    //
                    return false;
                    
                }//End else of if (_selectedUnit.CanSpendActionPointsToTakeAction...
                
            }//End if (_selectedAction.IsValidActionGridPosition
            else
            {
                // Return the Success/Failure State of the 'Take Action' process:  false (failure)
                //
                return false;
            }

        }//End if (MouseWorld.TryGetPosition
    
        // Return the Success/Failure State of the 'Take Action' process:  false (failure)
        //
        return false;
        
        
        #endregion 2- Option 2: Only ONE General Function Handles everything, using Abstract and Virtual Classes (..working as a General Interface, a Contract...) and Functions to be Implemented in each particular way inside each particular ActionClass (derivated from BaseAction Class...).
        
    }// End HandleSelectedAction()
    
    
    /// <summary>
    /// (Experimental Implementation, for Clicking on ANY UNIT, even Enemies, and Handling an ACTION on them with the same Mouse Click...) <br />
    /// Case:   Player UNIT was CLICKED <br /> <br />
    /// This is the new function that checks if the CLICKED
    ///..UNIT is the PLAYER and if it is, SELECTS it. <br />
    /// Otherwise: Returns FALSE.
    /// </summary>
    /// <param name="clickedUnit"></param>
    /// <returns></returns>
    private bool TryHandlePlayerUnitClicked(Unit clickedUnit)
    {
        // UNIT is already selected
        //
        if (clickedUnit == _selectedUnit)
        {
            return false;
        }

        // Clicked on an ENEMY
        //
        if (clickedUnit.IsEnemy())
        {
            return false;
        }

        // If a new Unit is being selected, return it + our TRUE flag:
        //
        SetSelectedUnit(clickedUnit);
        //
        return true;

    }// End TryHandlePlayerUnitClicked
    
    
    /// <summary>
    /// (Experimental Implementation, for Clicking on ANY UNIT, even Enemies, and Handling an ACTION on them with the same Mouse Click...) <br />
    /// Case:   ENEMY UNIT was CLICKED <br /> <br />
    /// So far this does not do much. We need to make another
    /// small change before we can complete this function
    /// </summary>
    /// <param name="clickedUnit"></param>
    /// <returns>TRUE if it is an ENEMY. <br /> <br />
    /// FALSE if it is a PLAYER UNIT</returns>
    private bool TryHandleEnemyUnitClicked(Unit clickedUnit)
    {
        // Clicked on a Player unit
        //
        if (!clickedUnit.IsEnemy())
        {
            return false;
        }
        // An ENEMY UNIT was Clicked with the Mouse:
        //
        return true;
        
    }// End TryHandleEnemyUnitClicked
    
    
    /// <summary>
    /// (Experimental Feature: Clicking on ENEMIES (..instead of 'on the Grid Position on the ground...'): to Apply an ACTION on them) <br />
    /// Allows you to select an UNIT (Character: 'Friend' or even 'Enemy'), for giving him/her orders later (if it is a TEAMMATE - a 'Friendly' -)... or to execute an ACTION on it, if it is an 'Enemy' (an Action, such as: a "ShootAction", "Attack with Grenade", etc.).
    /// How? By shooting a Raycast from the camera across the Mouse Pointer to the Game World, and returning the hit data.
    /// </summary>
    /// <returns>
    /// <p>True    if the Raycast is successful in hitting an UNIT or ENEMY UNIT (i.e.: there is HitData type: RaycastHit)</p>
    /// False   if the user clicked on an not permitted area,
    ///...or if in any case the Raycast is NOT successful in hitting an UNIT (i.e.: filtered by the LayerMask).
    /// </returns>
    private bool TryGetClickedUnit(out Unit clickedUnit)
    {
        // Initialization of Input parameter:
        //
        clickedUnit = default;
        
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
                    _unitLayerMask ) > 0)
            {
                
                // 'Try' to check (it also returns a Boolean...): does it have a Component of Type <Unit>??
                //
                if (_raycastHitInfo[0].transform.TryGetComponent<Unit>(out clickedUnit))
                {

                    // We have found a UNIT that was clicked on. return true;
                    //
                    return true;
                    
                }//End if (_raycastHitInfo[0]....
                
            }//End if ((Physics.RaycastNonAlloc...
            
        }//End if (_camera != null)
        else
        {
            // Camera is NULL...
            // Log Error:
            //
            Debug.LogError($"'MouseWorld.MainSceneCamera' is {MouseWorld.MainSceneCamera}, IN:\nScript: ---> {GetType().Name} \nGameObject: ---> {transform} - {Instance}.\nReturning 'false' in Function: 'TryGetClickedUnit(...)'");

            return false;

        }//End else of if (MouseWorld.MainSceneCamera != null)
        
        // There are NO Units / Charactters in that MOUSE POINTER location:
        //
        return false;

    }// End TryGetClickedUnit

    
    /// <summary>
    /// (Original CodeMonkey's implementation): <br />
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
                    
                    // Check if the Unit is already (previously) selected. If that is the case, don't do it again:
                    //
                    if (unit == _selectedUnit)
                    {
                        // The Unit was already selected, just exit, return false:
                        //
                        return false;
                        
                    }//End if (unit == _selectedUnit)

                    // We don't want the Player to select an ENEMY
                    // ...(that would be like breaking the Game's Rules):
                    //
                    if (unit.IsEnemy())
                    {
                        // Clicked on an Enemy
                        return false;

                    }//End if (unit.IsEnemy())
                    
                    // If a new Unit is being selected, return it + our TRUE flag:
                    //
                    SetSelectedUnit(unit);
                    return true;
                    
                }//End if (_raycastHitInfo[0]....
                
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

    /// <summary>
    /// Sets the Unit (Character) that was selected via GUI (by a mouse Click)
    /// ...and also sets the Default Action to be:  MOVE ACTION. <br />
    /// And also: Update the GUI Action Button Visuals: Color + Outline Color.
    /// </summary>
    /// <param name="unit"></param>
    private void SetSelectedUnit(Unit unit)
    {
        // Select the given Unit
        //
        _selectedUnit = unit;
        
        // Select a Default Action for the Character / Unit:  MOVE ACTION
        //
        SetSelectedAction( unit.GetMoveAction() );
        
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
    
    
    /// <summary>
    /// Sets the Unit's (i.e.: Player's Character...) selected Action (given as Input)  (baseAction). <br />
    /// And also: Update the GUI Action Button Visuals: Color + Outline Color.
    /// </summary>
    /// <param name="baseAction"></param>
    public void SetSelectedAction(BaseAction baseAction)
    {
        // Set the Selected ACTION
        //
        _selectedAction = baseAction;
        
        // Invoke the Delegate using this Listener, for Updating THIS GUI Button's Visual:
        //
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Gets the Unit's (i.e.: Player's Character...) current: SELECTED ACTION (given in previous Frames as Input)  (BaseAction).
    /// </summary>
    /// <returns><code>BaseAction</code>The Unit's (i.e.: Player's Character...) current: SELECTED ACTION.</returns>
    public BaseAction GetSelectedAction()
    {
        return _selectedAction;
    }

    
    #endregion Observer Pattern Methods
    
    
    #region Managing (allowing only...) just ONE Action at a Time

    /// <summary>
    /// Sets this whole System Architecture (UnitActionSystem) as BUSY NOW:   releasing a Mutex LOCK.
    /// </summary>
    private void SetBusy()
    {
        _isBusy = true;
        
        // Delegate Method call:
        //...to say: "Busy"  State CHANGED
        //
        OnBusyWorkingOnAnActionChanged?.Invoke(this, _isBusy);
    }

    /// <summary>
    /// Sets this whole System Architecture (UnitActionSystem) as FREE (i.e.: not busy) NOW:  setting a Mutex LOCK.
    /// </summary>
    private void ClearBusy()
    {
        _isBusy = false;
        
        // Delegate Method call:
        //...to say: "Busy"  State CHANGED
        //
        OnBusyWorkingOnAnActionChanged?.Invoke(this, _isBusy);
    }
    
    #endregion Managing (allowing only...) just ONE Action at a Time
    
    #endregion My Custom Methods

    
}
