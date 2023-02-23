/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using System;
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
                    
                    // Set the NEXT STATE after this one:
                    // A.I. is  BUUUSYYY... Working... Taking ACTION!
                    //
                    _state = State.Busy;
                    
                    // Take ACTION!
                    //
                    TakeEnemyAIAction( SetStateTakingTurn );

                    
                    // End this Unit's Turn  (this is an Enemy, so...
                    // ..this means:  the Enemy's Turn is OVER):
                    //
                    TurnSystem.Instance.NextTurn();

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


    /// <summary>
    /// Executes the (current FSM state)  A.I. 'ACTION':
    /// </summary>
    private void TakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        
    }// End TakeEnemyAIAction
    
    #endregion A.I. Finite State Machine
    
    #endregion My Custom Methods

}
