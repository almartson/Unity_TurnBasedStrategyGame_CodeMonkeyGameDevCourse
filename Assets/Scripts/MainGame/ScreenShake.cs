/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/

using System;
using UnityEngine;
using Cinemachine;



public class ScreenShake : MonoBehaviour
{

    #region Attributes

    [Tooltip("[CinemachineImpulseSource] Trigger that will fire the Movement - Shake Command, in the Cinemachine Camera.")]
    [SerializeField] private CinemachineImpulseSource _cinemachineImpulseSource;


    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>
    private void Awake()
    {
        _cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        
    }


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>



    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        // Debug Bit:
        // We capture the Keyboard button "R", to fire the ScreenShake:
        //
        if (Input.GetKeyDown(KeyCode.R))
        {
            _cinemachineImpulseSource.GenerateImpulse();    
        }
        
    }

    #endregion Unity Methods
    

    #region My Custom Methods





    #endregion My Custom Methods

}
