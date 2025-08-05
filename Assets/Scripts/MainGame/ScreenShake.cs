/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/

using System;
using UnityEngine;
using Cinemachine;


[RequireComponent(typeof(CinemachineImpulseSource))]
public class ScreenShake : MonoBehaviour
{

    #region Attributes
    
    #region Singleton Pattern's
    
    [Tooltip("Singleton Pattern's Main Key: Instance of this very Class")] 
    public static ScreenShake Instance { get; private set; }

    #endregion Singleton Pattern's

    
    #region Cinemachine Camera (Screen Shake)
    
    [Tooltip("[CinemachineImpulseSource] Trigger that will fire the Movement - Shake Command, in the Cinemachine Camera.")]
    [SerializeField]
    private CinemachineImpulseSource _cinemachineImpulseSource;

    #endregion Cinemachine Camera (Screen Shake)



    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>
    private void Awake()
    {
        #region Singleton Pattern's
        
        // Singleton Pattern, protocol:
        //
        // Validation: There MUST be ONLY ONE Instance of this Class (i.e.: ONE Object):
        //
        if (Instance != null)
        {
            Debug.LogError("There's more than one '" + GetType().Name + "'!. GameObject: ---> " + transform + "  - " + Instance);
            //
            // Destroy, to be able to continue the Gameplay (i.e.: Recovery from the Error/Exception...)
            //
            Destroy(gameObject);
            return;
        }
        //
        // If everything went well, create / assign THIS Instance:
        //
        Instance = this;
        
        #endregion Singleton Pattern's

        
        #region Cinemachine Camera (Screen Shake)

        _cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        
        #endregion Cinemachine Camera (Screen Shake)

    }//End Awake


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>



    /// <summary>
    /// Update is called once per frame
    /// </summary>


    #endregion Unity Methods
    

    #region My Custom Methods

    
    /// <summary>
    /// Function that fires an Impulse, as a broadcast... so the listener in Cinemachine will trigger the ScreenShake.
    /// </summary>
    /// <param name="intensity"></param>
    public void Shake(float intensity = 1.0f)
    {
        _cinemachineImpulseSource.GenerateImpulse(intensity);    

    }//End Shake



    #endregion My Custom Methods

}
