/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;  

/// <summary>
/// Manages the Turn System on the GUI side, setting and resetting the TurnNumber and some other Visual representations on screen.
/// </summary>
public class TurnSystemUI : MonoBehaviour
{

    #region Attributes

    [Tooltip("End Turn Button - UI Element: Button (Reference to...)")]
    [SerializeField]
    private Button _endTurnBtn;

    [Tooltip("Turn Number Text - UI Element: TextMeshProUGUI (Reference to...)")]
    [SerializeField]
    private TextMeshProUGUI _turnNumberText;
    
    [Tooltip("Enemy Turn UI 'Rect GameObject', containing Image and Text as children - UI Element (Reference to...)")]
    [SerializeField]
    private GameObject _enemyTurnVisualGameObject;

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
        // Listener to listen to any Changes in:   _endTurnBtn
        // Anonymous Function ( () => {...} ) inside the Listener:
        //
        _endTurnBtn.onClick.AddListener(() =>
        {
            // (When _endTurnBtn is Clicked:...do...) => Invoke 'NextTurn()'
            //..so the NextTurn is processed:
            //
            TurnSystem.Instance.NextTurn();
        });
        
        // Subscribe to the Event:   OnTurnChanged
        //...to Manage it:
        //
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        

        // Update the Turn Number UI TEXT:
        //
        UpdateTurnText();
        
        // Update the Enemy's Turn UI Image (switch from: visible to invisible):
        //
        UpdateEnemyTurnVisual();

        // ShowAndSetMaterial the UI Button 'End Turn' Button: when it is the Player's Turn
        //
        UpdateEndButtonVisibility();

    }//End Start


    /// <summary>
    /// Update is called once per frame
    /// </summary>


    #endregion Unity Methods


    #region My Custom Methods
    
    #region Observer Pattern Methods

    
    /// <summary>
    /// Delegate Method that will be triggered when the Turn Changes to the NEXT. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        // Update the Turn Text
        //...when the Turn changes to the next
        //
        UpdateTurnText();
        
        // Update the Enemy's Turn UI Image (switch from: visible to invisible, and vice-versa):
        //
        UpdateEnemyTurnVisual();
        
        // ShowAndSetMaterial the UI Button 'End Turn' Button: when it is the Player's Turn
        //
        UpdateEndButtonVisibility();
        
    }//End TurnSystem_OnTurnChanged
    
    #endregion Observer Pattern Methods


    /// <summary>
    /// Updates the UI Text showing the TURN NUMBER. 
    /// </summary>
    private void UpdateTurnText()
    {
        // Update the Text of the TextMeshProUGUI element:
        //
        _turnNumberText.text = "TURN " + TurnSystem.Instance.GetTurnNumber();

    }//End UpdateTurnText


    /// <summary>
    /// Updates the UI Rect GameObject ( <code>_enemyTurnVisualGameObject</code> )
    /// ..making it visible only when it is the Enemy's Turn.
    /// (..i.e.: when it is NOT the Player's Turn).
    /// </summary>
    private void UpdateEnemyTurnVisual()
    {
        // Make it visible only when it is the Enemy's Turn.
        //
        _enemyTurnVisualGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn);
    }


    /// <summary>
    /// Updates the Visibility (Visible or invisible - GameObject Enabled or Disabled)
    /// ..according to the Enemy's Turn: <br />
    /// Enemy Turn:   Hide it,<br />
    /// Player Turn:  ShowAndSetMaterial it. <br />
    /// </summary>
    private void UpdateEndButtonVisibility()
    {
        // ShowAndSetMaterial the UI Button 'End Turn' Button: when it is the Player's Turn
        //
        _endTurnBtn.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn);
    }
    
    #endregion My Custom Methods

}
