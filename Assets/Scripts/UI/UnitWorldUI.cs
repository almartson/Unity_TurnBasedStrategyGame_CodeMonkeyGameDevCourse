/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/

using System;
using TMPro;
using UnityEngine;

public class UnitWorldUI : MonoBehaviour
{

    #region Attributes

    [Tooltip("(Reference to) the ACTION POINTS (that the Unit/Character has left) - UI TextMeshPRO GameObject")]
    [SerializeField]
    private TextMeshProUGUI _actionPointsText;

    
    [Tooltip("(Reference to) Unit/Character - GameObject")]
    [SerializeField]
    private Unit _unit;
    

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
        // Listen - CallBack:  (Unit.OnAnyActionPointsChanged  is a STATIC DELEGATE... meaning it comes with the WHOLE CLASS, not just a particular 'Unit / Character' GameObject), so:
        // Everytime anybody's 'Action Point' variable changes:  DO THIS CALLBACK:
        // (actually this is non-performant if you have 1000 Units moving at the same time, changing that number... but for this case we'll only have ON1 (1) changing per Turn, so it's perfect)... The IDEAL Perfect solution would be NOT to use a 'STATIC DELEGATE' () 
        //
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        
        // Update the Unit's: 'Action Points' TEXT UI:
        //
        UpdateActionPointsText();
        
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

    #endregion My Custom Methods

}
