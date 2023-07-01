/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains the UNIT (game CHARACTERS:  the PLAYER'S Characters and the ENEMIES' too)... for a better and easier 'Enemy Artificial Intelligence (A.I.)' resolution.
/// </summary>
public class UnitManager : MonoBehaviour
{
    #region Attributes
    
    #region Singleton Pattern's
    
    [Tooltip("Singleton Pattern's Main Key: Instance of this very Class")] 
    public static UnitManager Instance { get; private set; }

    #endregion Singleton Pattern's

    #region Unit / Character Lists
    
    /// <summary>
    /// List of ALL game CHARACTERS (of type: 'Unit')
    /// </summary>
    private List<Unit> _getUnitList;
    
    /// <summary>
    /// List of all 'Friendly' CHARACTERS (i.e.: THE PLAYER'S TEAM... of type: 'Unit')
    /// </summary>
    private List<UnitPlayer> _getFriendlyUnitList;
    
    /// <summary>
    /// List of all 'Enemy' CHARACTERS (i.e.: THE ENEMY'S TEAM... of type: 'Unit')
    /// </summary>
    private List<UnitEnemy> _enemyUnitList;



    #endregion Unit / Character Lists

    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>
    private void Awake()
    {
        #region Singleton Pattern's
        
        // Singleton Pattern, protocol:
        // Validation: There MUST be ONLY ONE Instance of this Class (i.e.: ONE Object):
        //
        if (Instance != null)
        {
            Debug.LogError($"There's more than one '{GetType().Name}'!. GameObject: ---> {transform} - {Instance}");
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

        
        #region Unit Lists - Initializations

        _getUnitList = new List<Unit>();
        _getFriendlyUnitList = new List<UnitPlayer>();
        _enemyUnitList = new List<UnitEnemy>();

        #endregion Unit Lists - Initializations
        
    }// End Awake


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start()
    {
        #region Subscribing to: Event - Delegates -  listener - CallBacks

        // Subscribe to the:  Event: Listen to the   OnAnyUnitSpawned   EVENT:
        //...(with our own Function, defined here in this Class):
        //
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;

        // Subscribe to the:  Event: Listen to the   OnAnyUnitDead   EVENT:
        //...(with our own Function, defined here in this Class):
        //
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;

        #endregion Subscribing to: Event - Delegates -  listener - CallBacks
        
    }// End Start


    /// <summary>
    /// Update is called once per frame
    /// </summary>


    #endregion Unity Methods
    

    #region My Custom Methods
    
    #region Events - Triggered Functions
    
    /// <summary>
    /// Method that will be executed ANYTIME when the Listener / Delegate Event ( Unit.OnAnyUnitSpawned ) is TRIGGERED.
    /// This Event will Update the Whole Turn System accordingly when ANY change
    /// ...as a POP-UP or SPAWN occurs (i.e.: an Unit or Player instantiation in the Scene...):  It will be Triggered. <br /><br />
    /// NOTE: This STATIC EVENT will be triggered first, not associated with any specific GameObject or instance of the Unit.cs Class... so it will be executed faster and before any other.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        // Add the "Unit" to the corresponding List<Unit>(s...)
        
        // 0- Get the Unit from CASTING the sender Object:
        //
        Unit unit = sender as Unit;
        //
        // 0.2- Validation =! null
        // Todo:  Optimization Fix: Substitute  !=  (non performant) by its best equivalent.
        //
        if (unit != null)
        {
            // 1- Add it to:  the ALL in..:  List<Unit>
            //
            _getUnitList.Add(unit);

            // 2- Add it to:  FRIENDLY or ENEMY Lists:
            //
            if (unit.IsEnemy())
            {
                // Add the new (Spawned) Unit to the "ENEMY List"
                //
                _enemyUnitList.Add((UnitEnemy) unit);
            
            }//End if (unit.IsEnemy...
            else
            {
                // "Friendly"
                // Add:
                //
                _getFriendlyUnitList.Add((UnitPlayer) unit);
            
            }//End else of if (unit.IsEnemy...
            
        }//End if ( unit != null...
        
        
    }// End Unit_OnAnyUnitSpawned
    
    /// <summary>
    /// Method that will be executed ANYTIME when the Listener / Delegate Event ( Unit.OnAnyUnitDead ) is TRIGGERED.
    /// This Event will Update the Whole Turn System accordingly when ANY change
    /// ...in the LIFE / DEATH occurs:  i.e.: When an Unit (Player) DIES: It will be Triggered. <br /><br />
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        // Remove the "Unit" from the corresponding List<Unit>(s...)
        
        // 0- Get the referenced "Unit", from CASTING the "sender" Object:
        //
        Unit unit = sender as Unit;
        //
        // 0.2- Validation =! null
        // Todo:  Optimization Fix: Substitute  !=  (non performant) by its best equivalent.
        //
        if (unit != null)
        {
            // 1- Remove it from:  the ALL in..:  List<Unit>
            //
            _getUnitList.Remove(unit);

            // 2- Remove it from:  FRIENDLY or ENEMY Lists:
            //
            if (unit.IsEnemy())
            {
                // Remove the new (Spawned) from:  Unit to the "ENEMY List"
                //
                _enemyUnitList.Remove((UnitEnemy) unit);
            
            }//End if (unit.IsEnemy...
            else
            {
                // "Friendly"
                // Remove:
                //
                _getFriendlyUnitList.Remove((UnitPlayer) unit);
            
            }//End else of if (unit.IsEnemy...
            
        }//End if ( unit != null...
        
    }// End Unit_OnAnyUnitDead

    
    #endregion Events - Triggered Functions

    
    #region Getters and Setters (Properties)

    public List<Unit> GetUnitList() => _getUnitList;

    public List<UnitPlayer> GetFriendlyUnitList() => _getFriendlyUnitList;

    public List<UnitEnemy> GetEnemyUnitList() => _enemyUnitList;

    #endregion Getters and Setters (Properties)

    #endregion My Custom Methods

}
