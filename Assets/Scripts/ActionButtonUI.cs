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
    /// 1- It gets the name of the Action, from the Class Name & stripping the rest of the unnecessary characters.
    /// 2- It makes the GUI Button interactable: add the onClick functionality, to make it work, accordingly to the ACTION.
    /// </summary>
    /// <param name="baseAction"></param>
    public void SetBaseAction(BaseAction baseAction)
    {
        #region Get Action Name
        
        // // CodeMonkey: Get the ACTION NAME, & write it on the TextMeshProUGUI element:
        // //
        // _textMeshProUGUI.text = baseAction.GetActionName().ToUpper();
        
        // AlMartson: Get the ACTION NAME (from the Class Type.Name), & write it on the TextMeshProUGUI element:
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
        
    }


    // /// <summary>
    // /// THIS IS AN EXAMPLE: This is a Function created to be used as a Delegate Function, for the EXAMPLE.
    // /// </summary>
    // private void MoveActionBtn_Click()
    // {
    // }


    #endregion My Custom Methods

}
