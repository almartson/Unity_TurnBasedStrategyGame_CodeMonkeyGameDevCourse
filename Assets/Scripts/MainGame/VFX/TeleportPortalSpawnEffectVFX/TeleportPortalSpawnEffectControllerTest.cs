/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using UnityEngine;


public class TeleportPortalSpawnEffectControllerTest : BaseVFXShaderValueController
{

    #region Attributes

    
    #region Dissolve VFX's: Value and Time Rates

    
    #endregion Dissolve VFX's: Value and Time Rates

    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

    }// End Awake()


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>



    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        // Check to see if the user presses the SPACEBAR Button,
        //..if so, then enable the VFX.
        //
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Restore the original state of the character
            //
            TryUndoVFX();
            
            // Start the VFX as a Coroutine
            //
            StartCoroutine(DoStartVFX());

        }//End if (Input.GetKeyDown...
        
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            // Restore the original state of the character
            //
            TryUndoVFX();

        }//End if (Input.GetKeyDown..
        
    }// End Update()

    #endregion Unity Methods
    

    #region My Custom Methods
    
    
    

    #endregion My Custom Methods

}
