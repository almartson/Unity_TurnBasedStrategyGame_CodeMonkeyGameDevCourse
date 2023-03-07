using System;
using System.Collections.Generic;
using System.Linq;
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
    
    [Tooltip("Reference to the Unit / Character this Script is attached to in the Unity Editor. \n(i.e.: the INITIATOR of this Action)")]
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

    
    #region A.I. - AI
    
    /// <summary>
    /// (DEFAULT VALUE of...) Cost "PER UNIT" of this ACTION, for any ENEMY A.I., in terms of (CURRENCY = ) 'Action Points' <br />
    /// ...this value should be summed to any other values, to represent the TOTAL "WORTH" of Taking This ACTION  (vs.  "Not Taking It").
    /// </summary>
    [Tooltip("(DEFAULT VALUE of...) Cost \"PER UNIT\" of this ACTION, for any ENEMY A.I., in terms of (CURRENCY = ) 'Action Points'\n...this value should be summed to any other values, to represent the TOTAL \"WORTH\" of Taking This ACTION  (vs.  \"Not Taking It\").")]
    [SerializeField]
    protected int _AI_DEFAULT_UNITARY_ACTION_POINT_COST_VALUE_FOR_ANY_ENEMY_AI_TO_DECIDE_ON_THIS_ACTION = 100;
    
    [Tooltip("(MULTIPLIER VALUE of...) Cost of this ACTION, for any ENEMY A.I., in terms of (CURRENCY = ) 'Action Points'\n\nNOTE:\n\n* This value is a multiplier, so it has to be multiplied by '_AI_DEFAULT_UNITARY_ACTION_POINT_COST_VALUE_FOR_ANY_ENEMY_AI_TO_DECIDE_ON_THIS_ACTION' in the AWAKE() code of this Class.")]
    [SerializeField]
    protected int _myAIMultiplierActionPointCostValueForAnyEnemyAIToDecideOnThisAction = 0;

    [Tooltip("(FINAL VALUE of...) Cost of this ACTION, for any ENEMY A.I., in terms of (CURRENCY = ) 'Action Points.")]
    [SerializeField]
    protected int _myAIFinalActionPointCostValueForAnyEnemyAIToDecideOnThisAction = 0;
    
    #endregion A.I. - AI
    
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
        
        
        #region A.I. - AI

        // Set the Default COST "Value" of THIS ACTION for A.I.: so any ENEMY A.I. can decide on choosing this ACTION over another (...i.e.: the GREATER the value, the BETTER):
        //
        _myAIFinalActionPointCostValueForAnyEnemyAIToDecideOnThisAction = _AI_DEFAULT_UNITARY_ACTION_POINT_COST_VALUE_FOR_ANY_ENEMY_AI_TO_DECIDE_ON_THIS_ACTION * _myAIMultiplierActionPointCostValueForAnyEnemyAIToDecideOnThisAction;

        #endregion A.I. - AI
        
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
    /// Generic Method for getting the ACTION from the GUI (the Player's).
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
    
    
    #region A.I. - AI

    /// <summary>
    /// Given THIS concrete-particular <code>XxyyzzAction : BaseAction</code> <br />
    /// (Calculates and then...):  Gets the BEST possible DATA - "Grid Position(s)" to execute THIS "A.I. ACTION": the casted ACTION. <br />
    /// (EXAMPLE:   ShootAction myShootAction = myBaseAction as ShootAction; ) ... so "myShootAction" is THAT "A.I. ACTION" we are talking about..., <br />
    /// THAT "A.I. ACTION" will be used to test it out in each "GridPosition", and we get the ASSOCIATED DATA TO TAKING THAT ACTION..., <br />
    /// ..after considering all (A.I. ACTIONS in each "GridPosition"...) possibilities (of Positions / Locations == Grid Positions) at this moment in the "Present" (right now):  we return the BEST ONE, based on the ACTION POINTS VALUE.
    /// </summary>
    /// <returns>The DATA of the BEST possible A.I. ACTION (BaseAction):  "EnemyAIActionData"... <br />,
    /// ...based on the ACTION POINTS VALUE of each Action / Possibility</returns>
    public EnemyAIActionData GetBestEnemyAIActionData()
    {

        // Make a List of DATA of: "ENEMY A.I. ACTION"(s):
        //
        List<EnemyAIActionData> enemyAIActionDataList = new List<EnemyAIActionData>();
        
        // Cycle through all the "Valid"  GridPositions  for THIS selected (..each..) ACTION
        // ..("BaseAction" is casted as a derived-child "ConcreteAction"... so for THAT ONE):
        //
        List<GridPosition> validActionGridPositionList = GetValidActionGridPositionList();
        //
        // Lenght of the List:
        //
        int validActionGridPositionListLenght = validActionGridPositionList.Count;
        

        #region (Generate the  DATA  for:   ENEMY "A.I. ACTION").  Performance-oriented AlMartson's Implementation

        // Cycle through all the "Valid"  GridPositions  for THIS selected (..each..) ACTION
        //
        for (int i = 0; i < validActionGridPositionListLenght; i++)
        {
            // We want to:
            // Generate the  DATA  for:   ENEMY "A.I. ACTION"
            // ..for:
            // 1- on this (each... Grid) POSITION ...
            //
            // GridPosition gridPosition = validActionGridPositionList[i];

            // 2- THIS test  "ACTION"  (BaseAction casted-as a SPECIFIC ACTION)   (selected)
            //
            EnemyAIActionData enemyAIActionData = GetEnemyAIActionData( validActionGridPositionList[i] );
            //
            // 3- Add the ACTION to the LIST
            //
            enemyAIActionDataList.Add(enemyAIActionData);
        
        }//End for...  (Cycle through all the "Valid"  GridPositions  for THIS selected (..each..) ACTION)
        
        #endregion Performance-oriented AlMartson's Implementation

        
        // Final Step:
        // Check to see if it found   ANY Possible "Grid Positions" (Positional DATA)... where to TAKE THIS Action:
        //
        if (enemyAIActionDataList.Count > 0)
        {
            
            // Final:
            // SORT the possible DATA ACTIONS... to get the BEST of the BEST,.. 
            //...to execute it FIRST!
            // Sorted based on "ActionValue":
            //
            enemyAIActionDataList.Sort((EnemyAIActionData a, EnemyAIActionData b) => b.actionValue - a.actionValue);

            // Return THE BEST ONE:   i.e.: the one at Index: [0]
            //
            return enemyAIActionDataList[0];

        }
        else
        {
            // There are  No possible ENEMY A.I. ACTIONS
            //
            return null;

        }//End else of if (enemyAIActionList.Count > 0)

    }// End GetBestEnemyAIActionData
    


    /// <summary>
    /// (Calculates and...):  Gets the "A.I. ACTION" DATA ("Cost" Value, final, calculated "Points", to see if it's worth it + LOCATION to move to: "Grid Position"...)  that is possible in a given (as INPUT:),  "Grid Position".
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns>A set of  DATA  for taking this selected ACTION <br />
    /// Specially: <br /><br />
    /// 1- "Cost" of taking THIS ACTION... <br />
    /// 2- "Location":  GridPosition
    /// </returns>
    public virtual EnemyAIActionData GetEnemyAIActionData(GridPosition gridPosition)
    {
        // Calculate the "Cost" & GridPosition  DATA:
        //
        _myAIFinalActionPointCostValueForAnyEnemyAIToDecideOnThisAction = _myAIMultiplierActionPointCostValueForAnyEnemyAIToDecideOnThisAction * _AI_DEFAULT_UNITARY_ACTION_POINT_COST_VALUE_FOR_ANY_ENEMY_AI_TO_DECIDE_ON_THIS_ACTION;
        
        // Return the basic DATA:
        //
        return new EnemyAIActionData()
        {
            gridPosition = gridPosition,
            actionValue = _myAIFinalActionPointCostValueForAnyEnemyAIToDecideOnThisAction,
        };
    }// End GetEnemyAIActionData

    
    
    #region Deprecated Methods
    
    /// <summary>
    /// (Deprecated as it uses a FOREACH, and it has been replaced by a for + other micro-optimizations)... <br /> <br />
    /// (Calculates and then...):  Gets the BEST possible DATA - "Grid Position(s)" to execute THIS "A.I. ACTION": the casted ACTION. <br />
    /// (EXAMPLE:   ShootAction myShootAction = myBaseAction as ShootAction; ) ... so "myShootAction" is THAT "A.I. ACTION" we are talking about..., <br />
    /// THAT "A.I. ACTION" will be used to test it out in each "GridPosition", and we get the ASSOCIATED DATA TO TAKING THAT ACTION..., <br />
    /// ..after considering all (A.I. ACTIONS in each "GridPosition"...) possibilities (of Positions / Locations == Grid Positions) at this moment in the "Present" (right now):  we return the BEST ONE, based on the ACTION POINTS VALUE.
    /// </summary>
    /// <returns>The DATA of the BEST possible A.I. ACTION (BaseAction):  "EnemyAIActionData"... <br />,
    /// ...based on the ACTION POINTS VALUE of each Action / Possibility</returns>
    public EnemyAIActionData DeprecatedGetBestEnemyAIActionData()
    {

        // Make a List of DATA of: "ENEMY A.I. ACTION"(s):
        //
        List<EnemyAIActionData> enemyAIActionDataList = new List<EnemyAIActionData>();
        
        // Cycle through all the "Valid"  GridPositions  for THIS selected (..each..) ACTION
        // ..("BaseAction" is casted as a derived-child "ConcreteAction"... so for THAT ONE):
        //
        List<GridPosition> validActionGridPositionList = GetValidActionGridPositionList();
        
        #region Original (foreach - non-performant) CodeMonkey Implementation

        foreach (GridPosition gridPosition in validActionGridPositionList)
        {
            // We want to:
            // Generate the  DATA  for:   ENEMY "A.I. ACTION"
            // ..for:
            // 1- THIS (BaseAction casted-as a SPECIFIC ACTION)   (selected)
            // 2- on this (each... Grid) POSITION ...
            //
            EnemyAIActionData enemyAIActionData = GetEnemyAIActionData( gridPosition );
            //
            // 3- Add the ACTION to the LIST
            //
            enemyAIActionDataList.Add(enemyAIActionData);
        
        }//End foreach
        
        #endregion Original (foreach - non-performant) CodeMonkey Implementation

        
        // Final Step:
        // Check to see if it found   ANY Possible "Grid Positions" (Positional DATA)... where to TAKE THIS Action:
        //
        if (enemyAIActionDataList.Count > 0)
        {
            
            // Final:
            // SORT the possible DATA ACTIONS... to get the BEST of the BEST,.. 
            //...to execute it FIRST!
            // Sorted based on "ActionValue":
            //
            enemyAIActionDataList.Sort((EnemyAIActionData a, EnemyAIActionData b) => b.actionValue - a.actionValue);

            // Return THE BEST ONE:   i.e.: the one at Index: [0]
            //
            return enemyAIActionDataList[0];

        }
        else
        {
            // There are  No possible ENEMY A.I. ACTIONS
            //
            return null;

        }//End else of if (enemyAIActionList.Count > 0)

    }// End DeprecatedGetBestEnemyAIActionData

    #endregion Deprecated Methods
    
    #endregion A.I. - AI

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