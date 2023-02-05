/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/

using System;
using UnityEngine;


public class HealthSystem : MonoBehaviour
{

    #region Attributes

    #region Health

    [Tooltip("Health Points (i.e.: your current 'health')")]
    [SerializeField]
    private int _health = 100;
    
    #endregion Health

    
    #region Event - Listeners - CallBacks

    /// <summary>
    /// Even to be called/invoked when <code>_health == 0</code>.
    /// </summary>
    public event EventHandler OnDead;
    
    #endregion Event - Listeners - CallBacks
    
    
    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>



    /// <summary>
    /// Start is called before the first frame update
    /// </summary>



    /// <summary>
    /// Update is called once per frame
    /// </summary>


    #endregion Unity Methods


    #region My Custom Methods

    /// <summary>
    /// Computes the 'damage' that is being taken (by an attack, or any similar sort of action).<br />
    /// Also, if <code>_health == 0</code>, then the Die() function will be called.
    /// </summary>
    /// <param name="damageAmount"></param>
    public void Damage(int damageAmount)
    {
        // Take Damage:  decrement 'health'
        //
        _health -= damageAmount;
        
        // Validation:
        // Health must not be under zero
        //
        if (_health < 0)
        {
            _health = 0;
        }
        
        // Check:
        // Enemy Dies (_health = 0) ?
        //
        if (_health == 0)
        {
            Die();
        }

    }//End Damage()


    /// <summary>
    /// When a Character DIES (i.e.: _health is 0), it Calls/Invokes an Event in another script.
    /// </summary>
    private void Die()
    {
        // Call the 'Die'  Event  in another (specialized) Script:
        //
        OnDead?.Invoke(this, EventArgs.Empty);

    }//End Die()

    #endregion My Custom Methods

}
