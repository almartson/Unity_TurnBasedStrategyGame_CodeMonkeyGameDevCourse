using System;
using UnityEngine;

public class Unit : MonoBehaviour
{

    #region Attributes
    
    /// <summary>
    /// Keeping track of the CURRENT GridPosition of this Unit.
    /// </summary>
    private GridPosition _gridPosition;

    
    /// <summary>
    /// Keeping track of the TARGET (...NEXT...) GridPosition of the Player's MOVE / ACTION.
    /// This Position comes from a Conversion of the MousePointer Coordinates into: a valid GridPosition.
    /// This is already Validated.
    /// </summary>
    private GridPosition _finalGridPositionOfNextAction;


    #region ACTIONS
    
    #region Action's List
    
    /// <summary>
    /// LIST of ALL ACTIONS that can be performed (by this type of Unit / Character of the game). 
    /// </summary>
    private BaseAction[] _baseActionArray;
    
    
    /// <summary>
    /// Reference to the MoveAction script to make the Units / Characters able to execute the Action: 'TakeAction' (Walk, etc...). 
    /// </summary>
    private MoveAction _moveAction;
    
    /// <summary>
    /// Reference to the SpinAction script to make the Units / Characters able to execute the Action: 'TakeAction' (Rotate). 
    /// </summary>
    private SpinAction _spinAction;

    #endregion Action's List


    #region POINTS  - for every Action
    
    /// <summary>
    /// MAXIMUM Total amount of Points PER TURN (spendable); to be spent, each time this Character/Unit performs an Action. <br /> <br />
    /// Note: Each Action has a value in Points. So this variable is like the CURRENCY or MONEY to spend by taking any Action. <br />
    /// Default value : 2
    /// </summary>
    private const int _ACTION_POINTS_PER_TURN_MAX = 2;
    
    
    /// <summary>
    /// Total amount of Points (spendable), to spend each time this Character/Unit performs an Action. <br />
    /// Each Action has a value in Points. So this variable is like the CURRENCY or MONEY to spend by taking any Action. <br />
    /// Default value : 2
    /// </summary>
    [Tooltip("Total amount of Points (spendable), to spend each time this Character/Unit performs an Action. /n Each Action has a value in Points. So this variable is like the CURRENCY or MONEY to spend by taking any Action. /n Default value : 2")]
    [SerializeField]
    private int _actionPoints = _ACTION_POINTS_PER_TURN_MAX;
    
    
    #region Turn System

    /// <summary>
    /// Listener / Delegate Event that will be STATIC (i.e.: depending only of the CLASS not any instanced Object of this Class)...
    /// ...that will Update the Whole Turn System accordingly when ANY change
    /// ...in Action Points occurs (NOTE: It does not indicate that a turn has changed).<br />
    /// NOTE: This STATIC EVENT fixes the problems: There are 2 classes (this one - Unit - AND TurnSystem, that will fire an EVEN when the '_actionPoints for an Unit' Changes.... so if the ORDER OF EXECUTION is incorrect, there will be a Bug. This EVENT SHOULD BE THE FIRST ONE to be fired when we use _actionPoints, so making it a STATIC EVENT will make it be Triggered always first.
    /// </summary>
    public static event EventHandler OnAnyActionPointsChanged; 

    
    #endregion Turn System
    
    
    #endregion POINTS  - for every Action
    
    #endregion ACTIONS
    
    
    /// <summary>
    /// Last Valid Action's: Mouse Position
    /// (it is validated inside the Grid, see trace for:  MousePosition)
    /// </summary>
    private Vector3 _mousePosition = Vector3.zero;
    /// <summary>
    /// Property Accessor to Private Field:
    /// Public Getter and Setter for _mousePosition
    /// Last Valid Action's: Mouse Position
    /// (it is validated inside the Grid, see trace for:  MousePosition)
    /// </summary>
    public Vector3 MousePosition { get => _mousePosition; set => _mousePosition = value; }
    
    
    #endregion Attributes
    
    
    #region Unity Methods
    
    private void Awake()
    {
        // Get the Actions & the List of ALL Actions:        
        //
        _moveAction = GetComponent<MoveAction>();
        _spinAction = GetComponent<SpinAction>();
        //
        // Get the List of Actions:
        // NOTE: GetComponentSSSS<TYPE>: gets ALL Components of TYPE=<SetBaseAction>
        // ..(or that are children / Extend from <SetBaseAction>)... in this GameObject.
        //
        _baseActionArray = GetComponents<BaseAction>();
    }

    
    // Start is called before the first frame update
    private void Start()
    {
        // Setting the Unit on the LevelGrid (Script) Object, on THIS GridPosition...
        //
        // We need LevelGrid to be a Singleton, for accessing a Script outside of
        //...THIS Prefab.
        // SOLUTION: The same with 'UnitActionSystem'
        //
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        //
        // Initialization (final position = current position), not very logical (but it doesn't matter...):
        //
        _finalGridPositionOfNextAction = _gridPosition;
        //
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
        
        
        // LISTENER: Listen to the   OnTurnChanged   EVENT:
        //...(with our own Function, defined here on this Class):
        //
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

    }//End Start()
    

    // Update is called once per frame
    void Update()
    {
        // Setting the Unit on the LevelGrid (Script) Object, on THIS GridPosition...
        //
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        //
        // Compare the Old Position with the new...
        //
        if (newGridPosition != _gridPosition)
        {
            
            // This Unit changed its (Grid) Position
            //
            LevelGrid.Instance.UnitMovedGridPosition(this, _gridPosition, newGridPosition);
            
            // Update also the CURRENT GridPosition ref here
            //
            _gridPosition = newGridPosition;
        }
        
    }//End Update()

    
    #endregion Unity Methods
    
    
    #region My Custom Methods
    
    #region Actions
   
    /// <summary>
    /// Getter for <code>SetBaseAction[]</code>
    /// </summary>
    /// <returns></returns>
    public BaseAction[] GetBaseActionArray()
    {
        return _baseActionArray;
    }
    
    /// <summary>
    /// Getter for <code>MoveAction</code>
    /// </summary>
    /// <returns></returns>
    public MoveAction GetMoveAction()
    {
        return _moveAction;
    }
    
    /// <summary>
    /// Getter for <code>SpinAction</code>
    /// </summary>
    /// <returns></returns>
    public SpinAction GetSpinAction()
    {
        return _spinAction;
    }

    
    #region POINTS  - for every Action


    /// <summary>
    /// Tries to: Execute the GENERAL process of SPENDING the <code>actionPoints</code> by <code>the amount INPUT PARAMETER</code> on this ACTION.
    /// If this Unit does NOT have the necessary actionPoints to pay for this Action, nothing else is done, and a <code>false</code> bool is returned.
    /// </summary>
    /// <returns>Ending Success / Failure boolean state</returns>
    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        // Tries to spend the ActionPoint to pay for: the Action
        //
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            
            // Spend the actionPoints
            //
            SpendActionPoints(baseAction.GetActionPointsCost());

            return true;
            
        }//End CanSpendActionPointsToTakeAction
        else
        {
            return false;

        }//End else of if (CanSpendActionPointsToTakeAction
        
    }//End TrySpendActionPointsToTakeAction
    
    
    /// <summary>
    /// Checks if this Unit / Character is able to spend any Point on this CURRENTLY SELECTED Action.
    /// </summary>
    /// <param name="baseAction"></param>
    /// <returns></returns>
    private bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (_actionPoints >= baseAction.GetActionPointsCost())
        {
            return true;
        }
        else
        {
            return false;
            
        }//End else
        
    }//End CanSpendActionPointsToTakeAction


    /// <summary>
    /// Executes the process of SPENDING the <code>actionPoints</code> by <code>the amount INPUT PARAMETER</code> on this ACTION.
    /// </summary>
    /// <param name="amount">Cost - this is going to be spent</param>
    private void SpendActionPoints(int amount)
    {
        _actionPoints -= amount;

        // Validation
        // Must not be under zero - 0:
        //
        if (_actionPoints < 0)
        {
            _actionPoints = 0;
        }
        // As a Ternary Expression:
        // _actionPoints = (_actionPoints < 0) ? 0 : _actionPoints;

        
        // Fire the FIRST (STATIC) EVENT  related to _actionPoints CHANGING:
        //
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        
    }//End SpendActionPoints


    /// <summary>
    /// Getter for <code>_actionPoints</code>
    /// </summary>
    /// <returns></returns>
    public int GetActionPoints()
    {
        return _actionPoints;
    }
    
    
    #endregion POINTS  - for every Action
    
    #endregion Actions
    
    
    /// <summary>
    /// Getter for <code>GridPosition</code>
    /// </summary>
    /// <returns></returns>
    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }
    
    /// <summary>
    /// Getter for <code>GridPosition</code> of the Player's Next Action (e.g.: Move Action, Spin Action, etc).
    /// </summary>
    /// <returns></returns>
    public GridPosition GetFinalGridPositionOfNextPlayersAction()
    {
        return _finalGridPositionOfNextAction;
    }
    
    /// <summary>
    /// Setter for <code>GridPosition</code> of the Player's Next Action (e.g.: Move Action, Spin Action, etc).
    /// </summary>
    /// <returns></returns>
    public void SetFinalGridPositionOfNextPlayersAction(GridPosition gridPosition)
    {
        _finalGridPositionOfNextAction = gridPosition;
    }

    #endregion My Custom Methods
    
    
    #region Turn System
    
    #region Turn System - Delegates Listeners of On Turn Changed

    /// <summary>
    /// Listener / Delegate that Listens to:  a CHANGE in the Current TURN
    /// ...(to the NEXT TURN). <br />
    /// And then: resets the variables for the:  NEXT Turn <br />
    /// Such as:  _actionPoints
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        // Reset the variables for the:  NEXT Turn
        // _actionPoints:
        //
        _actionPoints = _ACTION_POINTS_PER_TURN_MAX;
               
        // Fire the FIRST (STATIC) EVENT  related to ANY _actionPoints CHANGING:
        //
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);

    }//End TurnSystem_OnTurnChanged
    
    #endregion Turn System - Delegates Listeners of On Turn Changed
        
    #endregion Turn System
    
}
