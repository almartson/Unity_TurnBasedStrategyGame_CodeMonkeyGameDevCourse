using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class (i.e.: it will not be used as an Object itself, only as a Parent / Interface / Contract
///..to other Classes)... representing the base each ACTION CLASS must have
///..(Attributes) and implement (Methods).
/// Each concrete 'Action Class' that will derive from this one, will override most of this class' Methods.
/// </summary>
public abstract class BaseAction : MonoBehaviour
{
    #region Attributes

    [Tooltip("Sets this 'Action' as: Enabled  or  Disabled.")]
    [SerializeField]
    protected bool _isActive = false;
    
    [Tooltip("Reference to the Unit / Character to apply the 'Action' to...")]
    protected Unit _unit;


    /// <summary>
    /// Part of the ACTION Name, that, coming from this derivates' (ergo: this Children's) Class Names
    ///...must be Stripped.
    /// </summary>
    private const string _CLASS_ACTION_NAME_PART_TO_BE_STRIPPED = "Action";


    #region Action Points Cost  (of this Action)

    [Tooltip("(DEFAULT VALUE of...) Cost of this ACTION, in terms of (CURRENCY = ) 'Action Points'")]
    [SerializeField]
    protected int _myActionPointCost = 1;
    
    #endregion Action Points Cost  (of this Action)
    
    
    #region Delegates: Purpose: Managing (allowing only...) just ONE Action at a Time
   
    /// <summary>
    /// Delegate: to tell when ANY [ TakeAction ] (Action & the ActionStart() function...) Routine STARTS.
    /// It is related to this CLASS, NOT to a specific Object - instance of this Class, SO IT DOESN'T DEPEND OF: Move, Spin, Shoot Action, etc... -  (although we can get the Character/'Unit' GameObject from the SENDER of the Event (by casting to 'Unit')...). ANY TIME any Action is called: (a descendant of BaseAction, THIS CALLBACK WILL BE TRIGGERED / CALLED).
    /// Note:  The Type: EventHandler   is a System-defined type, of Standard DELEGATE.
    /// </summary>
    public static event EventHandler OnAnyActionStarted;
    
    /// <summary>
    /// Delegate: to tell when ANY [ TakeAction ] (Action & the ActionStart() function...) Routine ENDS.
    /// It is related to this CLASS, NOT to a specific Object - instance of this Class.
    /// </summary>
    public static event EventHandler OnAnyActionCompleted;
    
    
    // Delegate: For 'ACTION Completed': Purpose: Telling everyone when the TakeAction (Action) Routine ends.
    /// <summary>
    /// Delegate: to tell when the: [ TakeAction ] (Action) Routine ends.
    /// Note:  The Type: ACTION   is a System-defined type, of Standard DELEGATE.
    /// </summary>
    protected Action onActionComplete;
    
    #endregion Delegates: Purpose: Managing (allowing only...) just ONE Action at a Time
    
    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>
    protected virtual void Awake()
    {
        // Get the Unit / Character this Script is attached to in the Unity Editor.
        //
        _unit = GetComponent<Unit>();
        
    }//End Awake()


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>



    /// <summary>
    /// Update is called once per frame
    /// </summary>


    #endregion Unity Methods


    #region My Custom Methods

    /// <summary>
    /// Base Declaration for: GetActionName.
    /// (Must be overriden in each concrete derived Class).
    /// </summary>
    /// <returns></returns>
    public abstract string GetActionName();

    
    /// <summary>
    /// (Base Concrete implementation of:) Gets this ACTION'S Name.
    /// </summary>
    /// <returns></returns>
    public string GetActionNameByStrippingClassName()
    {
        // AlMartsons' version:  Get the Class Name, but maybe the keyword = "Action"  needs to be stripped
        //...from the rest of the word (e.g.: SpinAction... to just: TakeAction - remove 'TakeAction' -, etc.).
        //
        // Strip the word "Action" (= _CLASS_ACTION_NAME_PART_TO_BE_STRIPPED) and return:
        //
        return (GetType().Name).Replace(_CLASS_ACTION_NAME_PART_TO_BE_STRIPPED, "");
    }


    #region 'Taking the Action' Logic
    
    /// <summary>
    /// Generic Method for getting the ACTION from the GUI (the Payer's).
    /// This must be reimplemented / overriden in each Concrete (derived, child) as: 'SomethingAction' Class.
    /// </summary>
    /// <returns></returns>
    public abstract void TakeAction(Action onActionComplete);  //   (GridPosition gridPosition, Action onActionComplete);

    
    /// <summary>
    /// Generic Method for generating the necessary Input Parameters that are used in the calling of
    /// ..the Function Call to the generic: 'TakeAction'
    /// This must be reimplemented / overriden in each Concrete (derived, child).
    /// </summary>
    public abstract void GenerateInputParameters();

    
    /// <summary>
    /// Common Logic that is executed every time at the BEGINNING of the 'TakeAction()'  Method. <br />
    /// Sets the <code>_isActive</code>  flag to TRUE (i.e.: this ACTION IS TAKING PLACE)... + Sets the CallBack: <code>this.onActionComplete</code> to the Input Parameter 'onActionComplete' of this function.
    /// </summary>
    /// <param name="onActionComplete"></param>
    protected void ActionStart(Action onActionComplete)
    {
        // MUTEX flag  +  Set the Callback
        //
        _isActive = true;
        this.onActionComplete = onActionComplete;
        
        
        // When ANY Action Starts:   Call the CallBack:
        //
        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
        
    }// End ActionStart
    
    /// <summary>
    /// Common Logic that is executed every time at the END of the 'TakeAction()'  Method. <br />
    /// Here we:  Release the (mutex) flag <code>_isActive = false</code>... +  We CALL our DELEGATE:  tells everyone that the TakeAction routine ENDED:<code>onActionComplete()</code>
    /// </summary>
    protected void ActionComplete()
    {
        // Release the (mutex) flag
        //
        _isActive = false;
        
        // We CALL our DELEGATE:  tells everyone that the TakeAction routine ENDED:
        //
        onActionComplete?.Invoke();
        
        
        // CALL to DELEGATE:  tells everyone that ANY TakeAction (i.e.: Shoot, Spin, Move, etc) routine ENDED:
        //
        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }

    #endregion 'Taking the Action' Logic
    

    #region Actions' Validations

    /// <summary>
    /// Tells you whether the user/Player's selection (Grid Position) is Valid to TakeAction to. It uses several criteria, such as: the Position must be unoccupied, must be inside the Grid System, etc. 
    /// </summary>
    /// <returns>True or False to: is the selected "GridPosition" Valid??</returns>
    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        // Get a List of ALL Valid (Grid) Positions:
        //
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        //
        // Return: Does this VALID LIST contain also the requested GridPosition (this Method's INPUT)? gridPosition 
        //
        return validGridPositionList.Contains(gridPosition);
    }


    /// <summary>
    /// Gets a List of the Valid places where the Unit/Character can TakeAction to (i.e.: GridPosition(s)).
    /// This method cycles through the squares/Grids...(using FOR )... to get a list of the valid ones.
    /// </summary>
    /// <returns>Valid (GridPosition(s)) places where the Unit/Character can TakeAction to, in this Turn.</returns>
    public abstract List<GridPosition> GetValidActionGridPositionList();
    
    
    #region POINTS  - for every Action

    /// <summary>
    /// Gets the ActionPoints (Cost) for this Current SELECTED ACTION.
    /// </summary>
    /// <returns></returns>
    public int GetActionPointsCost()
    {
        // Return DEFAULT value:   (usually is 1)
        //
        return _myActionPointCost;
    }

    #endregion POINTS  - for every Action

    #endregion Actions' Validations
    
    #region Misc, Getters, Setters, etc

    /// <summary>
    /// Gets the (Character) Unit   (i.e.: the INITIATOR of this Action).
    /// </summary>
    /// <returns></returns>
    public Unit GetUnit()
    {
        return _unit;
    }

    #endregion Misc, Getters, Setters, etc

    #endregion My Custom Methods

}//End Class BaseAction



/// <summary>
/// Base Class for the Input Parameters, of every Function call to: 'TakeAction()'
/// </summary>
public abstract class BaseParameters
{

    #region Attributes
    
    
    protected float exampleOfSyntaxProtectedIsLikeAPublic = 0.0f;
    
    #endregion Attributes
    
    
    #region Methods

    

    #endregion Methods

}//End Class BaseParameters