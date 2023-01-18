/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButtonUI : MonoBehaviour
{
    #region Attributes

    [Tooltip("Reference to the TextMeshProUGUI, for editing the: Text (GUI element)")]
    [SerializeField]
    private TextMeshProUGUI _textMeshProUGUI;

    [Tooltip("Reference to the Button (UI), for editing")]
    [SerializeField]
    private Button _button;
    
    [Tooltip("Reference to the SELECTED: Button (UI) GameObject, for Enabling / Disabling it... also for editing")]
    [SerializeField]
    private GameObject _selectedGameObject;

    
    #region Currently Active ACTIONS and their related: UI Buttons

    /// <summary>
    /// Currently Active ACTION   (for the Player / Unit)
    /// </summary>
    private BaseAction _baseAction;
    
    
    #endregion Currently Active ACTIONS and their related: UI Buttons
    
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
    /// Sets the Base Action in the UI Button element:
    /// 1- It gets the name of the Action, from the Class Name and stripping the rest of the unnecessary characters.
    /// 2- It makes the GUI Button interactable: add the onClick functionality, to make it work, accordingly to the ACTION.
    /// </summary>
    /// <param name="baseAction"></param>
    public void SetBaseAction(BaseAction baseAction)
    {
        // We get and Set / Save a Reference to the CURRENTLY SELECTED ON GUI: baseAction
        //
        _baseAction = baseAction;
        
        
        #region Get Action Name
        
        // // CodeMonkey: Get the ACTION NAME, and write it on the TextMeshProUGUI element:
        // //
        // _textMeshProUGUI.text = baseAction.GetActionName().ToUpper();
        
        // AlMartson: Get the ACTION NAME (from the Class Type.Name), and write it on the TextMeshProUGUI element:
        //
        _textMeshProUGUI.text = baseAction.GetActionNameByStrippingClassName().ToUpper();
        
        #endregion Get Action Name
        
        #region Make the GUI Button Interactable to perform its ACTION
        
        // 2- To make the GUI Button interactable: add the onClick functionality, to make it work, accordingly to the ACTION. 
        //
        _button.onClick.AddListener(() => 
        {
            /* Note: This is a call to a delegate of type 'UnityAction'. Delegates
             * are  * also 'Anonymous Functions'. This example here is called:
             * Anonymous Function   OR   Lambda Notation: () => {...}
            */
            
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
        //
        //
        // Example B:  Calling explicitly a NON-ANONIMOUS FUNCTION, but a Delegate Function with a proper name:
        //
        // _button.onClick.AddListener(MoveActionBtn_Click);
        
        #endregion Make the GUI Button Interactable to perform its ACTION
        
    }//End SetBaseAction(...)
    
    // /// <summary>
    // /// THIS IS AN EXAMPLE: This is a Function created to be used as a Delegate Function, for the EXAMPLE above.
    // /// </summary>
    // private void MoveActionBtn_Click()
    // {
    // }


    #region Functionalities UPDATE

    /// <summary>
    /// Updates the UI Button VISUALS.
    /// </summary>
    public void UpdateSelectedVisual()
    {
        
        // 1- Compare the CURRENT (selected) ACTION to the former-last Selected: ACTION
        // 1.1- CURRENT SELECTED (on GUI): Action
        //
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        //
        // NOTE: Previously Validated and accepted and used BASE ACTION:  _baseAction
        
        // Enable / Disable this Visual:
        // If (FORMER Action == NEW SELECTED) on GUI  THEN...=> Enable / UPDATE the Visuals: 
        //
        _selectedGameObject.SetActive( selectedBaseAction == _baseAction );
        
    }//End UpdateSelectedVisual()

    #endregion Functionalities UPDATE
    

    #endregion My Custom Methods

}
