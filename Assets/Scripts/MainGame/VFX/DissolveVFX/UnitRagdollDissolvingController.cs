/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using UnityEngine;


public class UnitRagdollDissolvingController : BaseDissolvingController
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
    protected /*override*/ void Start()
    {
        // base.Start();
        
        // Reset the VFX's (Dissolve) Values:
        //
        if (TryUndoVFX())
        {
            // Execute the VFX:
            //
            // Start the VFX as a Coroutine
            //
            StartCoroutine(DoStartVFX());

        }

    }// End Start()
    

    #endregion Unity Methods
    

    #region My Custom Methods
    
    
    

    #endregion My Custom Methods

}