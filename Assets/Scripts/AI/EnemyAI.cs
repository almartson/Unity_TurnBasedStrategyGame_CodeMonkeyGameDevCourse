/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Originally it is implementing a "Bad Practice" or "dirty" way of using a FINITE STATE MACHINE Pattern... because it is based on SWITCH - CASE _state....<br />
/// It also uses a similar approach (using DELEGATES, Action Events...) as:  UnitActionSystem.cs   (see for more details). <br />
/// TODO:   Change it later to the Jason Weimann's general solution: FSM using Delegates + Dictionaries (i.e: the "State" Pattern, see my example on GitHub:  https://github.com/almartson/AI_StateMachine_DronesDemo  ).
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
                    if (TryTakeEnemyAIAction(SetStateTakingTurn))
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
    /// So: it RESETS the STATE of the FSM back to:   State.TakingTurn
    /// </summary>
    private void SetStateTakingTurn()
    {
        // Small DELAY, to make it look more raelistic:
        //
        _timer = _timeAmountBeforeTakingAnAction;
        
        // Set the STATE:   Taking this TURN
        //
        _state = State.TakingTurn;

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
    /// Executes the (current FSM state)  A.I. 'ACTION':   for all "Units" in the ENEMY's TEAM.
    /// </summary>
    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        #region Original (non-performant) CodeMonkey's Implementation

        // // Cycling through every ENEMY Unit..
        // //
        // foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        // {
        //     // Make the ENEMY UNIT take "ACTION"
        //     //
        //     if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
        //     {
        //
        //         // If the ACTION is Completed:
        //         //
        //         return true;
        //
        //     }//if (TryTakeEnemyAIAction...
        //
        // }//End foreach
        //
        // // No success in "Taking A.I. ACTION":
        // //
        // return false;
        
        #endregion  Original (non-performant) CodeMonkey's Implementation
        
        
        #region Optimized Code Version (v-2.0). AlMartson's Implementation

        // Cycling through every ENEMY Unit..  ( Get Enemy Unit List )
        //
        List<Unit> enemyUnitList = UnitManager.Instance.GetEnemyUnitList();
        //
        // Lenght of th List
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
        // 0- Simple Version:  just a SPIN ACTION:
        
        // Get the ACTION   (as object)
        //
        SpinAction spinAction = enemyUnit.GetSpinAction();

        
        #region TakeAction Logic

        // Try to TakeAction the selected Unit... (by Raycasting on the Ground Plane (Mask Layer...))
        //
        if (MouseWorld.TryGetPosition(out Vector3 mousePosition))
        {
            // Get the CENTER of the selected "GridPosition", instead of a corner or any random position inside of it
            // ...because sometimes the Player/user clicks in random places of a Cell/Square/Grid,
            // ...not necessarily in the CENTER of it:
            //
            GridPosition actionGridPosition = enemyUnit.GetGridPosition();

            // Take the Action.
            
            // 'TakeAction' method has a particular Implementation in each of the derived Classes (e.g.: MoveAction, SpinAction, etc.).
            //
            //  .1- Validation of the Action:
            //
            if (   spinAction.IsValidActionGridPosition(actionGridPosition))
            {

                // Try to Spend this Unit's available "actionPoints": on this Action
                //
                if (enemyUnit.TrySpendActionPointsToTakeAction(   spinAction))
                {
                    // .2.0- The actionPoints are Spent / used by now, already...
                    
                    // .2- 'Take the Action'
                    //
                    // .2.1- Save the Valid GridPosition:
                    //
                    // .2.1.1- Save the original Mouse Position (just in case... as a backup)
                    //
                    enemyUnit.MousePosition.Set(actionGridPosition.x, 0, actionGridPosition.z);
                    //
                    // .2.1.1- In _selectedUnit, for later use in 'TakeAction()':
                    //
                    enemyUnit.SetFinalGridPositionOfNextPlayersAction(actionGridPosition);
                
                
                    // .3- Set this Class (SERVICE) Methods as: BUSY .. until it ends:  Set MUTEX ON
                    //
                    // SetBusy();  // This is not necessary here, because it happens in the Update() of this STATE MACHINE Script.
                    //
                    // .4- TakeAction() , A.I. ACTION
                    // ( ClearBusy():  tells the World that this ROUTINE JUST ENDED: ) -> Sets Mutex OFF (when TakeAction() Ends...)
                    //
                    spinAction.TakeAction(onEnemyAIActionComplete);
                    
                    
                    // // Update the GUI for ActionPoints:
                    // //
                    // OnActionStarted?.Invoke(this, EventArgs.Empty);

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
        
        #endregion TakeAction Logic
        
    }// End TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    
    
    #endregion A.I. "TakeAction"

    #endregion A.I. Finite State Machine
    
    #endregion My Custom Methods

}
