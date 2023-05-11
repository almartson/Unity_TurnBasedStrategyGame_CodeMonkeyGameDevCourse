/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ENEMIES' (NPC's) Artificial Intelligence (A.I.) MAIN BRAIN MANAGER. <br /> <br />
/// Originally it is implementing a "Bad Practice" or "dirty" way of using a FINITE STATE MACHINE Pattern... because it is based on SWITCH - CASE _state....<br />
/// It also uses a similar approach (using DELEGATES, Action Events...) as:  UnitActionSystem.cs   (see for more details). <br />
/// TODO:   Change it later to the Jason Weimann's general solution: The 'State Machine Pattern' (FSM) using Delegates + Dictionaries (i.e: the "State" Pattern, see my example on GitHub:  https://github.com/almartson/AI_StateMachine_DronesDemo  ).
/// </summary>
public class EnemyAI : MonoBehaviour
{

    #region Attributes

    #region A.I. Finite State Machine

    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,
    }

    /// <summary>
    /// Current STATE (A.I. Finite State Machine)
    /// </summary>
    private State _state;

    #endregion A.I. Finite State Machine

    
    #region Timer: Semi-realistic "time delay" in-between FSM States
    
    /// <summary>
    /// Total Timer Time, from this number the Timer will start decreasing... until it reaches to Zero.
    /// </summary>
    [Tooltip("Waiting delay before TAKING AN ACTION, to add more realism to the sequence of actions")]
    [SerializeField]
    private float _timeAmountBeforeTakingAnAction = 0.5f;
    
    /// <summary>
    /// Total Timer Time, from this number the Timer will start decreasing... until it reaches to Zero.
    /// </summary>
    private const float _TOTAL_MAX_TIME_IN_TIMER = 2.0f;
    
    /// <summary>
    /// Timer will Decrease the TIME in milliseconds that have passed, until it reaches zero (0).
    /// </summary>
    private float _timer;

    #endregion Timer: Semi-realistic "time delay" in-between FSM States

    
    #region A.I. DEBUG
    
    [Tooltip("Keeping track of the  BEST 'Enemy A.I. ACTION' (that's possible to choose):  Positional DATA.")]
    [SerializeField]
    private EnemyAIActionData _bestEnemyAIActionData;

    [Tooltip("Keeping track of the  BEST 'Enemy A.I. ACTION' (that's possible to choose):  ACTION (object instance of 'BaseAction' Class)) chosen.")]
    [SerializeField]
    private BaseAction _bestBaseAction;
    
    [Tooltip("Keeping track of the  BEST 'Enemy A.I. ACTION' (that's possible to choose):  ACTION (object instance of 'BaseAction' Class)) chosen.")]
    [SerializeField]
    private string _bestBaseActionName;
    

    [Tooltip("In the end of the BEST ENEMY A.I. ACTION: \nIf there WAS a PREVIOUS 'BEST'  'Action' \n\n * TEST: \n\n2- 'Test' to see:  What ACTION is the BEST ?? \n\n * The results of the intermediate TEST is saved here\n\n * In the END: this the 'SECOND TO BEST'  ACTION chosen, generally.")]
    [SerializeField]
    private EnemyAIActionData _testEnemyAIActionData;


    #endregion A.I. DEBUG
    
    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>
    private void Awake()
    {
        // Default init FSM State:
        //
        _state = State.WaitingForEnemyTurn;

    }//End Awake


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start()
    {
        // Add a Delegate (Event LISTENER)
        //..this will add a Function to be executed by the .OnTurnChanged()
        //...when the Turn Changes:
        //
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        
    }//End Start()


    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        // If it is the Player's Turn:   Do NOT do anything
        //
        if (TurnSystem.Instance.IsPlayerTurn)
        {
            return;
        }

        // Check for the current  FSM  State:
        //
        switch (_state)
        {
            default:
                
            case State.WaitingForEnemyTurn:
                break;
            
            case State.TakingTurn:
                
                // Decrease the value of Time that have passed, until it reaches to zero
                //
                _timer -= Time.deltaTime;
        
                // We use the Timer here, to make it more "REALISTIC" (if we would not...)
                //...then the A.I. ACTIONs would be TOO IMMEDIATE & QUICK (i.e.: it's a small "delay" before the A.I. Action).
                // If _timer is zero:  Take ACTION!
                //
                if (_timer <= 0.0f)
                {
                    
                    // (Try to...)  Take ACTION!
                    //
                    if (TryTakeEnemyAIAction(SetStateTakingTurnAndSetMarkUnitThatIsPlayingNow))
                    {
                        
                        // Set the NEXT STATE after this one:
                        // A.I. is  BUUUSYYY... Working... Taking ACTION!
                        // Set MUTEX LOCK.
                        //
                        _state = State.Busy;
                    }
                    else
                    {
                        // No ENEMY (in the List<_enemyUnit>) could "Take A.I. Action"...
                        // ...So:   End THIS TURN   (for the ENEMY A.I.).
                        //
                        TurnSystem.Instance.NextTurn();

                    }//End else of if (TryTakeEnemyAIAction...

                }//End if (_timer <= 0.0f)
                
                break;
            
            case State.Busy:
                break;
            
        }//End switch (_state)

    }//End Update()

    #endregion Unity Methods
    

    #region My Custom Methods

    
    #region A.I. Finite State Machine

    /// <summary>
    /// It clears the BUSY state and works as the delegate-callback function call to be used in 'TurnSystem_OnTurnChanged'
    /// This is a MUTEX LOCK that happens while this Class is Busy TAKING AN ACTION... it is for validation, because it can not take more actions while it is ALREADY working... <br />
    /// So: it RESETS the STATE of the FSM back to:   State.TakingTurn <br /> <br />
    ///
    /// Also: it marks in the TurnSystem.Instance.UnitThatPlaysNow()  the (Enemy A.I.) Unit that is Playing, taking the Turn. This is done for Debugging Purposes.
    /// </summary>
    private void SetStateTakingTurnAndSetMarkUnitThatIsPlayingNow()
    {
        // Small DELAY, to make it look more realistic:
        //
        _timer = _timeAmountBeforeTakingAnAction;
        
        // Set the STATE:   Taking this TURN
        //
        _state = State.TakingTurn;
        
        // (Debug & Experimental) Also: it marks in the TurnSystem.Instance.UniThatPlaysNow()  the (Enemy A.I.) Unit that is Playing, taking the Turn. This is done for Debugging Purposes.
        //
        TurnSystem.Instance.UnitThatIsPlayingNow = _bestBaseAction.GetUnit();

    }// End SetStateTakingTurn
    
    /// <summary>
    /// Function to be executed when the 'Turn Changes'.
    /// It Sets the CURRENT STATE.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        // In the ENEMY'S TURN:
        //
        if (!TurnSystem.Instance.IsPlayerTurn)
        {
            // Set the current "_state"
            //
            _state = State.TakingTurn;

            // Set the Timer to the MAX NUMBER:
            //
            _timer = _TOTAL_MAX_TIME_IN_TIMER;

        }//End if
        
    }// End TurnSystem_OnTurnChanged


    #region A.I. "TakeAction"
    
    /// <summary>
    /// Executes the (current FSM state)  A.I. 'ACTION':   for all "Units" in the ENEMY's TEAM. <br/> <br/>
    ///
    /// (Optimized Code Version (v-2.0). AlMartson's Implementation)
    /// </summary>
    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {

        #region Optimized Code Version (v-2.0). AlMartson's Implementation

        // Cycling through every ENEMY Unit..  ( Get Enemy Unit List )
        //
        List<Unit> enemyUnitList = UnitManager.Instance.GetEnemyUnitList();
        //
        // Lenght of the List
        //
        int enemyUnitListLenght = enemyUnitList.Count;

        
        // Cycling through every ENEMY Unit..
        //
        for (int i = 0; i < enemyUnitListLenght; i++)
        {

            // Make the ENEMY UNIT take "ACTION"
            //
            if (TryTakeEnemyAIAction(enemyUnitList[i], onEnemyAIActionComplete))
            {

                // If the ACTION is Completed, for ANY ENEMY:  end this Loop
                // ...(so, we will have to do another for the NEXT ENEMY later... and so on,... until all ENEMIES have been checked - tried to execute an "A.I. ACTION"):
                //
                return true;

            }//if (TryTakeEnemyAIAction...

        }//End for

        // No success in "Taking A.I. ACTION"... for ANY Enemy Unit (in the whole ENEMY TEAM), at all:
        //
        return false;
        
        #endregion Optimized Code Version (v-2.0). AlMartson's Implementation

    }// End TryTakeEnemyAIAction


    /// <summary>
    /// Given an "Enemy Unit": <br />
    /// It Executes a particular ENEMY Unit  'ACTION'  (A.I.)
    /// </summary>
    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        
        // GOAL: Get the BEST  ACTION   (as object)  possible to take:

        // (Debug & Experimental) Also: it marks in the TurnSystem.Instance.UniThatPlaysNow()  the (Enemy A.I.) Unit that is Playing, taking the Turn. This is done for Debugging Purposes.
        //
        TurnSystem.Instance.UnitThatIsPlayingNow = enemyUnit;

        
        // CALCULATIONS:

        // 0- Keeping track of the  BEST "Enemy A.I. ACTION"  (possible to choose:
        //    0.1- Position - DATA
        //
        _bestEnemyAIActionData = null;
        //
        //    0.2- ACTION  object
        //
        _bestBaseAction = null;
        //
        // DEBUG:   Reset the name of the _bestBaseActionName
        //
        _bestBaseActionName = "";
        

        #region Find: the BEST TYPE of ACTION the NPC can afford with its actionPoints: AlMartson's (performance-oriented) Implementation
        
        // 0- Get all possible kind of   ACTIONS   the NPC  (Enemy) can take:
        //
        BaseAction[] baseActionList = enemyUnit.GetBaseActionArray();
        //
        // Lenght of the Array:
        //
        int baseActionListLenght = baseActionList.Length;
            
        // Cycle through all ACTIONS
        //
        for (int i = 0; i < baseActionListLenght; i++)
        {
            
            // 0- Cache the "possible" ACTION to evaluate:
            //
            BaseAction baseAction = baseActionList[i];


            // For each ACTION...  See:
            // 1- Has enough  Action "POINTS" ?
            //
            if (! enemyUnit.CanSpendActionPointsToTakeAction( baseAction ))
            {
                // This ENEMY-NPC does NOT have enough "POINTS" to take THIS ACTION
                // (Enemy cannot afford this Action)
                // Ignore this Action... SKIP
                //
                continue;

            }//End if (! enemyUnit.CanSpendActionPointsToTakeAction...
            
            
            // This ENEMY-NPC has enough "POINTS"  ... for this ACTION
            // Check: 
            // Is there a previous BEST ONE ?  ( "bestEnemyAIActionData" )
            //
            if (_bestEnemyAIActionData == null)
            {

                // The was NO PREVIOUS BEST  "Action"

                // 2- Use:  the FIRST ONE we just found  ("baseAction", 2 lines above):
                
                //   2.1- Get all DATA for THIS action, which is casted into - selected - (for each TYPE of)  ACTION
                //
                _bestEnemyAIActionData = baseAction.GetBestEnemyAIActionData();
                //
                //   2.2- Save the Best ACTION  type
                //
                _bestBaseAction = baseAction;
                //
                //     2.2.1- Save the Best ACTION NAME
                //
                _bestBaseActionName = _bestBaseAction.GetActionNameByStrippingClassName();


            }//End if (bestEnemyAIActionData...
            else
            {
                // The WAS a PREVIOUS "BEST"  "Action"
                
                // TEST
                // 2- "Test" to see:  What ACTION is the BEST ??

                _testEnemyAIActionData = baseAction.GetBestEnemyAIActionData();

                //    2.1-  TEST:
                //      a) testEnemyAIActionData      NOT NULL
                // ...( CAN the ENEMY-NPC  "TAKE"  the Action ? )
                //
                //      b) Compare:  "actionValue"    (GREATER means BETTER)
                //
                if ( (_testEnemyAIActionData != null) && (_testEnemyAIActionData.actionValue > _bestEnemyAIActionData.actionValue) )
                {

                    // testEnemyAIActionData & bestBaseAction   WIN!

                    // 3- Copy its DATA
                    //
                    //   3.1- Get all DATA for THIS action, which is casted into - selected - (for each TYPE of)  ACTION
                    //
                    _bestEnemyAIActionData = _testEnemyAIActionData;
                    //
                    //   3.2- Save the Best ACTION  type
                    //
                    _bestBaseAction = baseAction;
                    //
                    //     3.2.1- Save the Best ACTION NAME
                    //
                    _bestBaseActionName = _bestBaseAction.GetActionNameByStrippingClassName();


                }//End if ( (testEnemyAIActionData != null)...
            }//End if (bestEnemyAIActionData...

        }//End for... (looking for the BEST  ACTION TYPE (type of action)... the ENEMY-NPC can AFFORD).
        
        #endregion Find: the BEST TYPE of ACTION the NPC can afford with its actionPoints: AlMartson's (performance-oriented) Implementation

        
        #region Take the  BEST  Action Logic

        // Take the calculated  "BEST ACTION"
        
        // Validate  ACTION != null  &&   Have enough (ACTION) POINTS    &&  ACTION is  MEANINGFULLY?
        //
        if ( ((_bestEnemyAIActionData != null) && (_bestBaseAction != null))  &&  ( _bestEnemyAIActionData.actionValue > 0 && enemyUnit.TrySpendActionPointsToTakeAction(_bestBaseAction) ) )
            /* Rework this Next Guard, because some ACTIONS can have a VALUE=0 in THIS TURN... for the SAKE OF A MORE COMPLEX CHAINED-STRATEGY of 2, 3 OR MORE CONSECUTIVE STEPS-TURNS:  See below:
             
             && (_bestEnemyAIActionData.actionValue > 0)*/ /* This means: Just TAKE ACTION when the ACTION is MEANINGFULLY... more that ZERO POINTS in VALUE / WORTH */
        {

            // .2.0- The actionPoints are Spent / used by now, already...
                    
            // .2- 'Take the Action'
            //
            // .2.1- Save the Valid GridPosition:
            //
            // .2.1.1- Save the original Mouse Position (just in case... as a backup)
            //
            enemyUnit.MousePosition.Set(_bestEnemyAIActionData.gridPosition.x, 0, _bestEnemyAIActionData.gridPosition.z);
            //
            // .2.1.1- In _selectedUnit, for later use in 'TakeAction()':
            //
            enemyUnit.SetFinalGridPositionOfNextPlayersAction(_bestEnemyAIActionData.gridPosition);
            

            // .3- Set this Class (SERVICE) Methods as: BUSY .. until it ends:  Set MUTEX ON
            //
            // SetBusy();  // This is not necessary here, because it happens in the Update() of this STATE MACHINE Script.
            //
            // .4- TakeAction() , A.I. ACTION
            // ( ClearBusy():  tells the World that this ROUTINE JUST ENDED: ) -> Sets Mutex OFF (when TakeAction() Ends...)
            //
            _bestBaseAction.TakeAction(onEnemyAIActionComplete);


            // Return the "Success" State of the 'Take Action' process:
            //
            return true;
            
        }//End if (bestEnemyAIActionData != null)...
        else
        {
            // Did not pass the Validation:     TrySpendActionPointsToTakeAction(...)  or "bestEnemyAIActionData"  was NULL   or  maybe even the ACTION CHOSEN wan not MEANINGFUL... more that ZERO POINTS in VALUE / WORTH:
            //
            // Could not TAKE the "BEST" ACTION
            //
            //     3.2.1- Save the "NULL":  Best ACTION NAME
            //
            _bestBaseActionName = $"Did not pass the Validation:     TrySpendActionPointsToTakeAction(...)  or 'bestEnemyAIActionData'  was NULL. \n\n * Could not TAKE the 'BEST' ACTION & RETURNING 'false' in CLASS:  {this.GetType().Name} \n * ... METHOD:  private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)";
            
            // Return the Success/Failure State of the 'Take Action' process:  false (failure)
            //
            return false;

        }//End else of if (bestEnemyAIActionData != null)...
        
        #endregion Take the  BEST  Action Logic

    }// End TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    
    
    
    #region (Deprecated) A.I. "TakeAction"

    /// <summary>
    /// (Deprecated because of performance issues - a "foreach" is inside)
    /// Given an "Enemy Unit": <br /> 
    /// It Executes a particular ENEMY Unit  'ACTION'  (A.I.)
    /// </summary>
    /// <param name="enemyUnit"></param>
    /// <param name="onEnemyAIActionComplete"></param>
    /// <returns></returns>
    [Obsolete("This method is deprecated. Use: 'private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)' instead", true)]
    private bool DeprecatedTryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {

        // GOAL: Get the BEST  ACTION   (as object)  possible to take:
        // CALCULATIONS:

        // 0- Keeping track of the  BEST "Enemy A.I. ACTION"  (possible to choose:
        //    0.1- Position - DATA
        //
        EnemyAIActionData bestEnemyAIActionData = null;
        //
        //    0.2- ACTION  object
        //
        BaseAction bestBaseAction = null;
        

        #region Find: the BEST TYPE of ACTION the NPC can afford with its actionPoints: Original (non-performant) CodeMonkey's Implementation
        
        // 0- Get all possible kind of   ACTIONS   the NPC  (Enemy) can take:
        // Cycle through all ACTIONS
        //
        foreach (BaseAction baseAction in enemyUnit.GetBaseActionArray())
        {
            // For each ACTION...  See:
            // 1- Has enough  Action "POINTS" ?
            //
            if (!enemyUnit.CanSpendActionPointsToTakeAction(baseAction))
            {
                // This ENEMY-NPC does NOT have enough "POINTS" to take THIS ACTION
                // (Enemy cannot afford this Action)
                // Ignore this Action... SKIP
                //
                continue;

            }//End if (enemyUnit.CanSpendActionPointsToTakeAction...
            
            
            // This ENEMY-NPC has enough "POINTS"  ... for this ACTION
            // Check: 
            // Is there a previous BEST ONE ?  ( "bestEnemyAIActionData" )
            //
            if (bestEnemyAIActionData == null)
            {

                // The was NO PREVIOUS BEST  "Action"

                // 2- Use:  the FIRST ONE we just found  ("baseAction", 2 lines above):
                
                //   2.1- Get all DATA for THIS action, which is casted into - selected - (for each TYPE of)  ACTION
                //
                bestEnemyAIActionData = baseAction.GetBestEnemyAIActionData();
                //
                //   2.2- Save the Best ACTION  type
                //
                bestBaseAction = baseAction;

            }//End if (bestEnemyAIActionData...
            else
            {
                // The WAS a PREVIOUS "BEST"  "Action"
                
                // TEST
                // 2- "Test" to see:  What ACTION is the BEST ??

                EnemyAIActionData testEnemyAIActionData = baseAction.GetBestEnemyAIActionData();
                
                //    2.1-  TEST:
                //      a) testEnemyAIActionData      NOT NULL
                // ...( CAN the ENEMY-NPC  "TAKE"  the Action ? )
                //
                //      b) Compare:  "actionValue"    (GREATER means BETTER)
                //
                if ( (testEnemyAIActionData != null) && (testEnemyAIActionData.actionValue > bestEnemyAIActionData.actionValue) )
                {

                    // testEnemyAIActionData & bestBaseAction   WIN!

                    // 3- Copy its DATA
                    //
                    //   3.1- Get all DATA for THIS action, which is casted into - selected - (for each TYPE of)  ACTION
                    //
                    bestEnemyAIActionData = testEnemyAIActionData;
                    //
                    //   3.2- Save the Best ACTION  type
                    //
                    bestBaseAction = baseAction;

                }//End if ( (testEnemyAIActionData != null)...

            }//End if (bestEnemyAIActionData...

        }//End foreach (looking for the BEST  ACTION TYPE (type of action)... the ENEMY-NPC can AFFORD).
        
        #endregion Find: the BEST TYPE of ACTION the NPC can afford with its actionPoints: Original (non-performant) CodeMonkey's Implementation

        
        #region Take the  BEST  Action Logic

        // Take the calculated  "BEST ACTION"
        
        // Validate  ACTION != null  &&   Have enough (ACTION) POINTS ?
        //
        if ( (bestEnemyAIActionData != null)  &&  (enemyUnit.TrySpendActionPointsToTakeAction(bestBaseAction)) )
        {

            // .2.0- The actionPoints are Spent / used by now, already...
                    
            // .2- 'Take the Action'
            //
            // .2.1- Save the Valid GridPosition:
            //
            // .2.1.1- Save the original Mouse Position (just in case... as a backup)
            //
            enemyUnit.MousePosition.Set(bestEnemyAIActionData.gridPosition.x, 0, bestEnemyAIActionData.gridPosition.z);
            //
            // .2.1.1- In _selectedUnit, for later use in 'TakeAction()':
            //
            enemyUnit.SetFinalGridPositionOfNextPlayersAction(bestEnemyAIActionData.gridPosition);
            

            // .3- Set this Class (SERVICE) Methods as: BUSY .. until it ends:  Set MUTEX ON
            //
            // SetBusy();  // This is not necessary here, because it happens in the Update() of this STATE MACHINE Script.
            //
            // .4- TakeAction() , A.I. ACTION
            // ( ClearBusy():  tells the World that this ROUTINE JUST ENDED: ) -> Sets Mutex OFF (when TakeAction() Ends...)
            //
            bestBaseAction.TakeAction(onEnemyAIActionComplete);


            // Return the "Success" State of the 'Take Action' process:
            //
            return true;
            
        }//End if (bestEnemyAIActionData != null)...
        else
        {
            // Did not pass the Validation:     TrySpendActionPointsToTakeAction(...)  or "bestEnemyAIActionData"  was NULL:
            // Could not TAKE the "BEST" ACTION
            // Return the Success/Failure State of the 'Take Action' process:  false (failure)
            //
            return false;

        }//End else of if (bestEnemyAIActionData != null)...
        
        #endregion Take the  BEST  Action Logic

    }// End DeprecatedTryTakeEnemyAIAction(Action onEnemyAIActionComplete)


    /// <summary>
    /// (Deprecated, do not use - Original (non-performant) CodeMonkey's Implementation) <br /><br />
    /// 
    /// Executes the (current FSM state)  A.I. 'ACTION':   for all "Units" in the ENEMY's TEAM.
    /// </summary>
    [Obsolete("This method is deprecated (due to performance reasons). Use: 'private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)' instead", true)]
    private bool DeprecatedTryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        #region Original (non-performant) CodeMonkey's Implementation

        // Cycling through every ENEMY Unit..
        //
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            // Make the ENEMY UNIT take "ACTION"
            //
            if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
            {
        
                // If the ACTION is Completed:
                //
                return true;
        
            }//if (TryTakeEnemyAIAction...
        
        }//End foreach
        
        // No success in "Taking A.I. ACTION":
        //
        return false;
        
        #endregion  Original (non-performant) CodeMonkey's Implementation
 
    }// End TryTakeEnemyAIAction
    
    #endregion (Deprecated) A.I. "TakeAction"

    #endregion A.I. "TakeAction"

    #endregion A.I. Finite State Machine
    
    #endregion My Custom Methods

}
