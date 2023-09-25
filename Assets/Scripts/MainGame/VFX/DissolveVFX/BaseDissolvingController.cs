/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public abstract class BaseDissolvingController : MonoBehaviour
{

    #region Attributes

    #region VFX Coroutine Management
    
    [Tooltip("[Readonly for Debug] Is the VFX (shader) Coroutine running now?")]
    [SerializeField]
    protected bool _isRunningVFXCoroutine = false;
    
    #endregion VFX Coroutine Management
    
    
    #region Materials

    #region Materials Case Scenario  1-: SkinnedMeshRenderer's Materials

    [Tooltip("SkinnedMeshRenderer, To get the materials from it.")]
    [SerializeField]
    protected SkinnedMeshRenderer _skinnedMeshRenderer;

    [Tooltip("[ReadOnly for Debug] Array of Materials that belong to the '3D Character'.")]
    [SerializeField]
    protected Material[] _arrayOfCachedSkinnedMeshRendererMaterials;

    #endregion Materials Case Scenario  1-: SkinnedMeshRenderer's Materials


    #region Materials Case Scenario  2-: Mesh Renderer's Materials

    [Tooltip("MeshRenderer, To get the materials from it.")] [SerializeField]
    protected MeshRenderer _meshRenderer;

    [Tooltip("[ReadOnly for Debug] Array of Materials that belong to the '3D Mesh'.")]
    [SerializeField]
    protected Material[] _arrayOfCachedMeshRendererMaterials;

    #endregion Materials Case Scenario  2-: Mesh Renderer's Materials


    [Tooltip("[ReadOnly for Debug] Array of ALL Materials that will be 'VFX Dissolve'd' :) (that belong to the '3D Mesh').")]
    [SerializeField]
    protected Material[] _arrayOfCachedMaterials;

    #endregion Materials


    #region VFX Shader: Dissolve VFX's: Value and Time Rates

    // Option 1:  Calculate everything based on TOTAL TIME for the VFX.

    [Tooltip("RECOMMENDED, NON ZERO: Let it be zero (0.0f) if you don't want to use it!")]
    [SerializeField]
    protected float _useTotalDissolveTime = 1.5f; // 1.5f


    // Option 2:  Specify every value here for the VFX.

    [Tooltip("Rate of change per frame of the Dissolving effect.")]
    [SerializeField]
    protected float _dissolveChangeRate = 0.0111f; // 0.0125f;

    [Tooltip("Time to 'yield return WaitForSeconds(this time var...)' between any change in Dissolve in this VFX's Coroutine")]
    [SerializeField]
    protected float _refreshRateDeltaTime = 0.0123f; // 0.025f;

    /// <summary>
    /// Cache of:  Variable that represents the Amount of "Erosion" (i.e.: Dissolution...) on the Material shown.
    /// </summary>
    private static readonly int _DissolveAmount = Shader.PropertyToID("_DissolveAmount");

    #endregion VFX Shader: Dissolve VFX's: Value and Time Rates


    #region VFX Graph (particles effect)

    [Tooltip("VFX Graph component reference.")]
    [SerializeField]
    protected VisualEffect _VFXGraph;


    #endregion VFX Graph (particles effect)


    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>
    protected virtual void Awake()
    {
        #region Materials List

        // 1- 3D  Characters:
        // Add the reference to 'SkinnedMeshRenderer' to get the Materials from it.
        //
        if (_skinnedMeshRenderer != null)
        {
            _arrayOfCachedSkinnedMeshRendererMaterials = _skinnedMeshRenderer.materials;
        }

        //
        // 2- 3D (Static, Not Rigged)  Meshes:
        // Add the reference to 'MeshRenderer' to get the Materials from it.
        //
        if (_meshRenderer != null)
        {
            _arrayOfCachedMeshRendererMaterials = _meshRenderer.materials;
        }

        //
        // 3- Grab all Materials into one Array []
        // 3.1 - Length, auxiliary variables
        //
        int lengthOfArrayOfCachedSkinnedMeshRendererMaterials = _arrayOfCachedSkinnedMeshRendererMaterials.Length;
        int lengthOfArrayOfCachedMeshRendererMaterials = _arrayOfCachedMeshRendererMaterials.Length;
        //
        // 3.2- Fill in the Array:
        //   3.2.1 - Create Array:
        //
        _arrayOfCachedMaterials = new Material [lengthOfArrayOfCachedSkinnedMeshRendererMaterials +
                                                lengthOfArrayOfCachedMeshRendererMaterials];
        //
        //   3.2.2 - Fill in the Array
        //
        _arrayOfCachedSkinnedMeshRendererMaterials.CopyTo(_arrayOfCachedMaterials, 0);
        _arrayOfCachedMeshRendererMaterials.CopyTo(_arrayOfCachedMaterials, lengthOfArrayOfCachedSkinnedMeshRendererMaterials);

        #endregion Materials List


        #region VFX Shader: Dissolve VFX's: Value and Time Rates

        // Initialize Boolean Flag for the VFX (Shader Effect's) Coroutine:
        //
        _isRunningVFXCoroutine = false;
        
        #region Option 1:  Calculate everything based on TOTAL TIME for the VFX.

        CalculateDissolveChangeRateAndTimeBetweenVFXChanges();

        #endregion Option 1:  Calculate everything based on TOTAL TIME for the VFX.

        #endregion VFX Shader: Dissolve VFX's: Value and Time Rates

    }


    // End Awake()


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    // protected virtual void Start()
    // {
    // }// End Start()


    /// <summary>
    /// Update is called once per frame
    /// </summary>



    #endregion Unity Methods


    #region My Custom Methods


    /// <summary>
    /// Starts the whole VFX.
    /// It works as a Coroutine.
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator DoStartVFX()
    {
        
        // Mark that the Coroutine Started:
        //
        _isRunningVFXCoroutine = true;

        
        // Null check validations:

        // 1- VFX Graph (particles) effect:
        //
        if ((_VFXGraph != null) && (_VFXGraph.enabled))
        {
            _VFXGraph.Play();
        }

        // 2- VFX Shader effect:
        //
        if ((_arrayOfCachedMaterials.Length > 0) && (_arrayOfCachedMaterials[0] != null))
        {
            // newDissolveAmount variable, to change the "Dissolve Amount" parameter
            //
            float newDissolveAmount = 0.0f;

            while (_arrayOfCachedMaterials[0].GetFloat(_DissolveAmount) < 1)
            {

                // Increase the "Dissolve Amount" parameter:
                //
                CalculateDissolveChangeRateAndTimeBetweenVFXChanges();
                //
                newDissolveAmount += _dissolveChangeRate;

                // Assign the new "Dissolve Amount" value:
                //
                for (int i = 0; i < _arrayOfCachedMaterials.Length; i++)
                {
                    // Set the new value of "_DissolveAmount" in the Shader
                    //
                    _arrayOfCachedMaterials[i].SetFloat(_DissolveAmount, newDissolveAmount);


                    // Return of this Coroutine
                    //
                    yield return new WaitForSeconds(_refreshRateDeltaTime);

                } //End for

            } //End while (_cachedSkinnedMeshRendererMaterials[0].GetFloat("") < 1)

        } //End if (_cachedSkinnedMeshRendererMaterials.Length > 0)

        
        // Mark that the Coroutine Ended:
        //
        _isRunningVFXCoroutine = false;
        
    } // End DoStartVFX()


    /// <summary>
    /// Restore the 3D Mesh (VFX) to it's Initial state.
    /// </summary>
    /// <returns></returns>
    protected virtual bool TryUndoVFX()
    {
        // Success in this method
        //
        bool successInThisMethod = true;

        // Null check validations:

        // 1- VFX Graph (particles) effect:
        //
        if ((_VFXGraph != null) && (_VFXGraph.enabled))
        {
            _VFXGraph.Stop();
        }

        // 2- VFX Shader effect:
        //   2.1- Stop Coroutine
        //
        // This does not work: StopCoroutine("DoStartVFX");
        //
        StopAllCoroutines();

        // Validate and reassign to zero the Shader's "Dissolve Amount" value
        //
        if ((_arrayOfCachedMaterials.Length > 0) && (_arrayOfCachedMaterials[0] != null))
        {
            // newDissolveAmount variable, to change the "Dissolve Amount" parameter
            //
            float newDissolveAmount = 0.0f;


            // Assign the new "Dissolve Amount" value:
            //
            for (int i = 0; i < _arrayOfCachedMaterials.Length; i++)
            {

                // Set the new value of "_DissolveAmount" in the Shader
                //
                _arrayOfCachedMaterials[i].SetFloat(_DissolveAmount, newDissolveAmount);

            } //End for

        } //End if (_cachedSkinnedMeshRendererMaterials.Length > 0)
        else
        {
            successInThisMethod = false;
        }

        return successInThisMethod;

    } // End UndoVFX()


    #region Misc Methods
    
    /// <summary>
    /// Option 1:  Calculate everything based on TOTAL TIME ( _useTotalDissolveTime ) for the VFX.
    /// </summary>
    private void CalculateDissolveChangeRateAndTimeBetweenVFXChanges()
    {
        // Option 1:  Calculate everything based on TOTAL TIME for the VFX.
        //
        if (_useTotalDissolveTime > 0.0f)
        {
            
            // Fix Time between VFX small Changes:
            //
            _refreshRateDeltaTime = Time.deltaTime;
                
            // [ DissolveAmountThisFrame = ?? = 
            // = ( timeDeltaTimeOfDissolve * (1.0)TotalDissolveAmount ) / MY TotalDissolveTime ]

            _dissolveChangeRate = _refreshRateDeltaTime / _useTotalDissolveTime;
            
        } // End Option 1:  Calculate everything based on TOTAL TIME for the VFX.
    }// End CalculateDissolveChangeRate

    
    #endregion Misc Methods

    #endregion My Custom Methods

}

