/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Unit's 'UI Action Points' and data Script - Logic.
/// This should be attached to a UnitWorldUI Canvas - GameObject. <br />
/// </summary>
public class UnitWorldUI : MonoBehaviour
{

    #region Attributes

    [Tooltip("(Reference to) the ACTION POINTS (that the Unit/Character has left) - UI TextMeshPRO GameObject")]
    [SerializeField]
    private TextMeshProUGUI _actionPointsText;

    
    [Tooltip("(Reference to) Unit/Character - GameObject")]
    [SerializeField]
    private Unit _unit;
    
    [Tooltip("(Reference to) the Unit/Character's UI HEALTH BAR IMAGE - UI Element")]
    [SerializeField]
    private Image _healthBarImage;

    [Tooltip("(Reference to) the Unit/Character's HEALTH SYSTEM Script - Logic")]
    [SerializeField]
    private HealthSystem _healthSystem;

    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>



    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start()
    {
        // Listener - CallBack:  (Unit.OnAnyActionPointsChanged  is a STATIC DELEGATE... meaning it comes with the WHOLE CLASS, not just a particular 'Unit / Character' GameObject), so:
        // Everytime anybody's 'Action Point' variable changes:  DO THIS CALLBACK:
        // (actually this is non-performant if you have 1000 Units moving at the same time, changing that number... but for this case we'll only have ON1 (1) changing per Turn, so it's perfect)... The IDEAL Perfect solution would be NOT to use a 'STATIC DELEGATE' () 
        //
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        
        // Listener - CallBack:
        // When the Unit/Character receives DAMAGE (because it was under attack):
        //
        _healthSystem.OnDamaged += HealthSystem_OnDamaged;
        
        // Update the Unit's: 'Action Points' TEXT UI:
        //
        UpdateActionPointsText();
        //
        // Update the Unit's: 'Health Bar' image UI:
        //
        UpdateHealthBar();
        
    }// End Start()


    /// <summary>
    /// Update is called once per frame
    /// </summary>


    #endregion Unity Methods


    #region My Custom Methods
    
    /// <summary>
    /// Updates the UI / GUI TEXT, showing the current value of:  'Action Points'... of the Unit/Character.  
    /// </summary>
    private void UpdateActionPointsText()
    {
        // Updates the ('Action Points') UI TEXT  (with the current vaclue):
        //
        _actionPointsText.text = _unit.GetActionPoints().ToString();

    }// End UpdateActionPointsText


    #region Delegate - Listener When ACTION POINTS change

    /// <summary>
    /// Delegate - Listener When ACTION POINTS change: Update the Unit's (Action Points') UI Text.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        // Update the (Action Points') UI Text:
        //
        UpdateActionPointsText();
        
    }// End Unit_OnAnyActionPointsChanged

    #endregion Delegate - Listener When ACTION POINTS change


    #region UI Health Bar

    /// <summary>
    /// Updates the UI Health Bar.
    /// </summary>
    private void UpdateHealthBar()
    {
        // Update the UI Image's 'Fill Amount' Slider value:
        //
        _healthBarImage.fillAmount = _healthSystem.GetHealthNormalized();

    }// End UpdateHealthBar

    
    #region Listener - CallBack: when this Unit/Character is DAMAGED
    
    /// <summary>
    /// This Event is triggered when this Unit/Character is DAMAGED (i.e.: <code>_health</code> is reduced).
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        
        // Update the UI:   'Health Bar'  (image)
        //
        UpdateHealthBar();
        
    }// End HealthSystem_OnDamaged
    
    #endregion Listener - CallBack: when this Unit/Character is DAMAGED
    
    #endregion UI Health Bar
    
    #endregion My Custom Methods

}
