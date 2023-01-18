/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using System;
using UnityEngine;

/// <summary>
/// Manages the Turn System, setting and resetting the TurnNumber and some other variables. <br />
/// It uses: Singleton and Observer Pattern, to be easily accessed from ANYWHERE in code and also to implement Delegates + Listeners (thus avoiding to use
///...a MonoBehaviour Update() updating the UI every frame, we only want to update it WHEN IT CHANGES, not always (Performance wise).
/// </summary>
public class TurnSystem : MonoBehaviour
{

    #region Attributes

    [Tooltip("Current Turn ID number")]
    [SerializeField]
    private int _turnNumber;

    
    #region Singleton Pattern's
    
    [Tooltip("Singleton Pattern's Main Key: Instance of this very Class")] 
    public static TurnSystem Instance { get; private set; }

    #endregion Singleton Pattern's

        
    #region Observer Pattern's
    
    /// <summary>
    /// Observer Pattern's: Event, to be Fired/Published when the CURRENT TURN Changes (ends... so another one Starts). Using the Observer Pattern.
    /// </summary>
    [Tooltip("Observer Pattern's: Event, to be Fired/Published when the CURRENT TURN Changes (ends... so another one Starts). Using the Observer Pattern.")]
    public event EventHandler OnTurnChanged;
    
    #endregion Observer Pattern's
    
    
    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>
    private void Awake()
    {
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

    
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>



    /// <summary>
    /// Update is called once per frame
    /// </summary>


    #endregion Unity Methods


    #region My Custom Methods


    /// <summary>
    /// Processes the variables for setting the NEXT TURN.
    /// </summary>
    public void NextTurn()
    {
        // Increase the Turn Number
        //
        _turnNumber++;
        
        // When the Turn Changes:  Fire this Delegate 'OnTurnChanged'
        // EVENT:
        //
        OnTurnChanged?.Invoke(this, EventArgs.Empty);

    }//End NextTurn
    
    /// <summary>
    /// Getter for TurnNumber.
    /// </summary>
    public int GetTurnNumber()
    {
        return _turnNumber;
    }

    #endregion My Custom Methods

}
