using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class (i.e.: it will not be used as an Object itself, only as a Parent / Interface / Contract
///..to other Classes)... representing the base each ACTION CLASS must have
///..(Attributes) & implement (Methods).
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
    
    
    #region Delegates: Purpose: Managing (allowing only...) just ONE Action at a Time
   
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


    /// <summary>
    /// Generic Method for getting the ACTION from the GUI (the Payer's).
    /// This must be reimplemented / overriden in each Concrete (derivated, child) 'SomethingAction' Class.
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <param name="onActionComplete"></param>
    /// <returns></returns>
    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);


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
    

    #endregion Actions' Validations


    #endregion My Custom Methods

}
