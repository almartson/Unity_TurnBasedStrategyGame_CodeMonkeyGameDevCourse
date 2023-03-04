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

    /// <summary>
    /// Maximum (possible) Health for this game.
    /// </summary>
    private int _HEALTH_MAX;

    #endregion Health

    
    #region Event - Listeners - CallBacks

    /// <summary>
    /// Even to be called/invoked when <code>_health == 0</code>.
    /// </summary>
    public event EventHandler OnDead;
    
    /// <summary>
    /// Even to be called/invoked when <code>_health</code> is dimished... that means:  when the Character receives DAMAGE.
    /// </summary>
    public event EventHandler OnDamaged;
    
    #endregion Event - Listeners - CallBacks
    
    
    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>
    private void Awake()
    {
        // Initialize the Max Health pseudo CONSTANT for this game:
        //
        _HEALTH_MAX = _health;

    }// End Awake


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
        
        
        // Invoke the Event:  when this Character is DAMAGED:
        //
        OnDamaged?.Invoke(this, EventArgs.Empty);
        
        
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

    
    #region Health Value

    /// <summary>
    /// Calculates (and returns) the (percentage of) % Health, based on the range [0.0, 1.0] 
    /// </summary>
    /// <returns></returns>
    public float GetHealthNormalized()
    {
        // Calculate the (percentage of) % Health, based on the range [0.0, 1.0]
        //
        return (float)_health / _HEALTH_MAX;

    }// End GetHealthNormalized
    
    /// <summary>
    /// Calculates (and returns) the (percentage of) % Health, based on the range [0, 100] 
    /// </summary>
    /// <returns></returns>
    public float GetHealthPercent()
    {
        // Calculate the (percentage of) % Health, based on the range [0.0, 1.0]
        //
        return (GetHealthNormalized() * 100.0f);

    }// End GetHealthNormalized
    
    #endregion Health Value
    
    
    #region Damage Value
    
    /// <summary>
    /// Calculates (and returns) the COMPLEMENT of Health (that means: the percentage DAMAGE & TAKEN); NORMALIZED: based on the range [0.0, 1.0] 
    /// </summary>
    /// <returns></returns>
    public float GetDamageTakenOfHealthNormalized()
    {
        // Calculate the COMPLEMENT of Health (a.k.a.: Damage taken...), based on the range [0.0, 1.0]
        //
        return (float)(1.0000f - GetHealthNormalized());

    }// End GetDamageTakenOfHealthNormalized
    
    /// <summary>
    /// Calculates (and returns) the (percentage of) the COMPLEMENT of Health (that means: the percentage DAMAGE & TAKEN), based on the range [0 %, 100 %] 
    /// </summary>
    /// <returns></returns>
    public float GetDamageTakenOfHealthPercent()
    {

        // Calculate the PERCENTAGE (%) COMPLEMENT of Health (a.k.a.: Damage taken...), based on the range [0 - 100 %]
        //
        return (GetDamageTakenOfHealthNormalized() * 100.0f);

    }// End GetDamageTakenOfHealthPercent
    
    #endregion Damage Value
    
    
    #endregion My Custom Methods

}
