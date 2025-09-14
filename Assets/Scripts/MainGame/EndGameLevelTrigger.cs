/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/

using System;
using System.Collections.Generic;
using UnityEngine;


public class EndGameLevelTrigger : MonoBehaviour
{

    #region Attributes

    [Tooltip("Tag To Verify -> in the collisions.")]
    private const string _COLLIDER_TAG = "Player";
    
    [Tooltip("Player Beat Level Already [Readonly] -> True: if the Player already Won this game enabling this Trigger before.")]
    [SerializeField]
    private bool _playerBeatLevelAlready = false;


    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>



    /// <summary>
    /// Start is called before the first frame update
    /// </summary>




    private void OnTriggerEnter(Collider other)
    {
        
        // Debug.Log($"Triggered by the Player! with -> {other.gameObject}");

        
        // Check that the collision was with an Unit that belongs to the "Player":
        //
        if (other.gameObject.CompareTag(  _COLLIDER_TAG ) && (! _playerBeatLevelAlready))
        {
            
            // Verify Use Cases of GAME OVER, by BEATING THE LEVEL:
            // 1- ENEMY Unit List all died:  (i.e.: the Human Player WON!)
            //
            List<UnitEnemy> enemyUnitList = UnitManager.Instance.GetEnemyUnitList();
            
            // 2- Find the next available "Unit" / Character.
            // If none is available, if all are dead, then it’s game over.
            //
            if ( enemyUnitList.Count <= 0 )
            {
                // GAME OVER
                // Human Player WON!
                // Set the next available "Unit" / Character
                    
                // TODO: PLEASE INSERT "YOU WIN" + Game Over CODE around these LINES...
                //
                Debug.LogWarning($"++ YOU WIN! ++ \n PLEASE INSERT 'YOU WIN' + Game Over CODE around these LINES...");
            
                
                // 0- Mark that this Trigger is been used now:
                //
                _playerBeatLevelAlready = true;
                
                
                // TODO: Move this logic to a classic GameManager.cs script
                
                // Show a BEAT LEVEL / YOU WIN!  GUI
                //
                UnitActionSystem.Instance.ShowBeatLevelGUI();
            
            }//End if ( enemyUnitList.Count <= 0 )

    
        }//End if (other.gameObject.CompareTag(  _COLLIDER_TAG )..
        
    }//End OnTriggerEnter

    #endregion Unity Methods
    

    #region My Custom Methods
    

    #endregion My Custom Methods

}
