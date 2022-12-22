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
    /// Sets the Base Action in the UI Button element..
    /// </summary>
    /// <param name="baseAction"></param>
    public void SetBaseAction(BaseAction baseAction)
    {
        // // CodeMonkey: Get the ACTION NAME, & write it on the TextMesProUGUI element:
        // //
        // _textMeshProUGUI.text = baseAction.GetActionName().ToUpper();
        
        // AlMartson: Get the ACTION NAME (from the Class Type.Name), & write it on the TextMesProUGUI element:
        //
        _textMeshProUGUI.text = baseAction.GetActionNameByStrippingClassName().ToUpper();
    }



    #endregion My Custom Methods

}
