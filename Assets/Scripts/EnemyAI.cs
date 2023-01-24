/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/

using System;
using UnityEngine;


public class EnemyAI : MonoBehaviour
{

    #region Attributes
    
    #region Timer: Initial Temporary Non-AI solution
    
    /// <summary>
    /// Total Timer Time, from this number the Timer will start decreasing... until it reaches to Zero.
    /// </summary>
    private const float _TOTAL_MAX_TIME_IN_TIMER = 2.0f;
    
    /// <summary>
    /// Timer will Decrease the TIME in milliseconds that have passed, until it reaches zero (0).
    /// </summary>
    private float _timer;

    
    #endregion Timer: Initial Temporary Non-AI solution

    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
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
    /// Start is called before the first frame update
    /// </summary>



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

        // Decrease the value of Time that have passed, until it reaches to zero
        //
        _timer -= Time.deltaTime;
        
        // If _timer is zero:  END.
        //
        if (_timer <= 0.0f)
        {
            // End this Unit's Turn  (this is an Enemy, so...
            // ..this means:  the Enemy's Turn is OVER):
            //
            TurnSystem.Instance.NextTurn();

        }//End if (_timer <= 0.0f) 

    }//End Update()

    #endregion Unity Methods
    

    #region My Custom Methods

    #region Timer: Initial Temporary Non-AI solution

    /// <summary>
    /// Function to be executed when the 'Turn Changes'.
    /// This will be temporary.
    /// This will:   Set a 'Timer' (i.e.: un cronometro).
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        // Set the Timer to the MAX NUMBER:
        //
        _timer = _TOTAL_MAX_TIME_IN_TIMER;
    }

    #endregion Timer: Initial Temporary Non-AI solution
    
    #endregion My Custom Methods

}
