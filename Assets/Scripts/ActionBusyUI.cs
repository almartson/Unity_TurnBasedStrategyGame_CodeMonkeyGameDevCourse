/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/

using UnityEngine;


public class ActionBusyUI : MonoBehaviour
{

    #region Attributes

    // [Tooltip("...")]
    // [SerializeField]
    // private int _myDefaultVar;


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
        // Subscription to the Delegate.
        // To Listen to any Change in the STATE OF BUSY (a boolean)
        //
        UnitActionSystem.Instance.OnBusyWorkingOnAnActionChanged += UnitActionSystem_OnBusyWorkingOnAnActionChanged;
        
        // Start by Hiding the UI Image that says: "I AM BUSY".
        //
        Hide();
        
    }//End Start()


    /// <summary>
    /// Update is called once per frame
    /// </summary>


    #endregion Unity Methods


    #region My Custom Methods

    
    /// <summary>
    /// Enables the GameObject to which this Script is attached to.
    /// </summary>
    private void Show()
    {
        gameObject.SetActive(true);
    }
    
    /// <summary>
    /// Disables the GameObject to which this Script is attached to.
    /// </summary>
    private void Hide()
    {
        gameObject.SetActive(false);
    }

    
    #region Delegates Methods, Subscriptions and Calls

    /// <summary>
    /// Logic itself of the Delegate Method call:
    /// ...Show / Hide a GUI Image that says: "I AM BUSY" on Screen..., according to the "Bussyness", whether its currently working on an Action, or not.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="isBusy"></param>
    private void UnitActionSystem_OnBusyWorkingOnAnActionChanged(object sender, bool isBusy)
    {
        if (isBusy)
        {
            Show();
        }
        else
        {
            Hide();
            
        }//End else
        
    }//End UnitActionSystem_OnBusyWorkingOnAnActionChanged()
    
    #endregion Delegates Methods, Subscriptions and Calls
        
    #endregion My Custom Methods

}
