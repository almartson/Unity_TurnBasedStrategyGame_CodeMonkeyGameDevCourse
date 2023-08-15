/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/

using System;
using System.Collections;
using UnityEngine;


public class DissolvingControllerTest : MonoBehaviour
{

    #region Attributes

    [Tooltip("SkinnedMeshRenderer, To get the materials from it.")]
    [SerializeField]
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    
    [Tooltip("[ReadOnly for Debug] Array of Materials that belong to the Character.")]
    [SerializeField]
    private Material[] _cachedSkinnedMeshRendererMaterials;

    
    #region Dissolve VFX's: Value and Time Rates
    
    [Tooltip("Rate of change per frame of the Dissolving effect.")]
    [SerializeField]
    private float _dissolveChangeRate = 0.0125f;

    [Tooltip("Time to 'yield return WaitForSeconds(this time var...)' between any change in Dissolve in this VFX's Coroutine")]
    [SerializeField]
    private float _refreshRateDeltaTime = 0.025f;

    #endregion Dissolve VFX's: Value and Time Rates

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
        if (_skinnedMeshRenderer != null)
        {
            _cachedSkinnedMeshRendererMaterials = _skinnedMeshRenderer.materials;
        }
        
    }// End Start()


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
            // Start the VFX as a Coroutine
            //
            StartCoroutine(DoStartVFX());
        }
        
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            // Restore the original state of the character
            //
            TryUndoVFX();
        }
        
    }// End Update()

    #endregion Unity Methods
    

    #region My Custom Methods


    /// <summary>
    /// Starts the whole VFX.
    /// It works as a Coroutine.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoStartVFX()
    {

        // Null check validation
        //
        if ((_cachedSkinnedMeshRendererMaterials.Length > 0) && (_cachedSkinnedMeshRendererMaterials[0] != null))
        {
            // newDissolveAmount variable, to change the "Dissolve Amount" parameter
            //
            float newDissolveAmount = 0;
            
            while (_cachedSkinnedMeshRendererMaterials[0].GetFloat("_DissolveAmount") < 1)
            {
                
                // Decrease the "Dissolve Amount" parameter:
                //
                newDissolveAmount += _dissolveChangeRate;

                // Assign the new "Dissolve Amount" value:
                //
                for (int i = 0; i < _cachedSkinnedMeshRendererMaterials.Length; i++)
                {
                    // Set the new value of "_DissolveAmount" in the Shader
                    //
                    _cachedSkinnedMeshRendererMaterials[i].SetFloat("_DissolveAmount", newDissolveAmount);
                    
                    
                    // Return of this Coroutine
                    //
                    yield return new WaitForSeconds(_refreshRateDeltaTime);

                }//End for

            }//End while (_cachedSkinnedMeshRendererMaterials[0].GetFloat("") < 1)
            
        }//End if (_cachedSkinnedMeshRendererMaterials.Length > 0)
        
}// End DoStartVFX()


    /// <summary>
    /// Restore the 3D Mesh (VFX) to it's Initial state.
    /// </summary>
    /// <returns></returns>
    public bool TryUndoVFX()
    {
        // Success in this method
        //
        bool successInThisMethod = true;
        
        // Null check validation
        //
        if ((_cachedSkinnedMeshRendererMaterials.Length > 0) && (_cachedSkinnedMeshRendererMaterials[0] != null))
        {
            // newDissolveAmount variable, to change the "Dissolve Amount" parameter
            //
            float newDissolveAmount = 0.0f;
            

            // Assign the new "Dissolve Amount" value:
            //
            for (int i = 0; i < _cachedSkinnedMeshRendererMaterials.Length; i++)
            {
                
                // Set the new value of "_DissolveAmount" in the Shader
                //
                _cachedSkinnedMeshRendererMaterials[i].SetFloat("_DissolveAmount", newDissolveAmount);
                
            }//End for
            
        }//End if (_cachedSkinnedMeshRendererMaterials.Length > 0)
        else
        {
            successInThisMethod = false;
        }

        return successInThisMethod;

    }// End UndoVFX()



    #endregion My Custom Methods

}
