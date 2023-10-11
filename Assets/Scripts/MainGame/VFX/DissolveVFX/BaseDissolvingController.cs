/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public abstract class BaseDissolvingController : MonoBehaviour
{

    #region Attributes

    #region VFX Graph (particles effect)
    
    [Header("VFX Graph (particles effect) Script:")]

    [Tooltip("VFX Graph component reference.")]
    [SerializeField]
    protected VisualEffect _VFXGraph;
    
    #endregion VFX Graph (particles effect)

    
    #region VFX Shader Graph

    #region VFX Shader Coroutine Management
    
    [Space()]
    [Header("VFX Shader Graph")]

    [Space(10)]
    [Header("VFX Shader Graph: Coroutine Management:")]

    [Tooltip("[Readonly for Debug] Is the VFX (shader) Coroutine running now?")]
    [SerializeField]
    protected bool _isRunningShaderEffectFromVFXCoroutine = false;
    
    #endregion VFX Shader Coroutine Management
    
    
    #region Materials

    #region Materials Case Scenario  1-: SkinnedMeshRenderer's Materials

    [Space()]
    [Header("VFX Shader Graph: Materials")]
    [Space(10)]

    [Tooltip("SkinnedMeshRenderer, To get the materials from it.")]
    [SerializeField]
    protected SkinnedMeshRenderer _skinnedMeshRenderer;

    [Tooltip("[ReadOnly for Debug] Array of Materials that belong to the '3D Character'.")]
    [SerializeField]
    protected Material[] _arrayOfCachedSkinnedMeshRendererMaterials;

    #endregion Materials Case Scenario  1-: SkinnedMeshRenderer's Materials


    #region Materials Case Scenario  2-: Mesh Renderer's Materials

    [Tooltip("MeshRenderer, To get the materials from it.")]
    [SerializeField]
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
    
    [Space()]   [Header("VFX Shader: Dissolve VFX's: Value and Time Rates")]
    [Space(10)]

    // Option 1:  Calculate everything based on TOTAL TIME for the VFX.

    [Header("Option 1:  Calculate everything based on TOTAL TIME for the VFX.")]
    
    [Tooltip("RECOMMENDED, NON ZERO: Let it be zero (0.0f) if you don't want to use it!")]
    [SerializeField]
    protected float _useTotalDissolveTime = 1.5f; // 1.5f


    // Option 2:  Specify every value here for the VFX (and let '_useTotalDissolveTime' = 0).

    [Space()]
    [Header("Option 2:  Specify every value here for the VFX (and let '_useTotalDissolveTime' = 0).")]

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

    [Space(10)]
    [Header("Current 'Dissolve Value' for the Shader (VFX):")]

    [Tooltip("[Readonly for Debugging purposes] Cache of:  Variable that represents the Amount of 'Erosion' (i.e.: Dissolution...) on the Material shown.")]
    [SerializeField]
    protected float _dissolveAmount = 0.0f;
    
    #endregion VFX Shader: Dissolve VFX's: Value and Time Rates
    
    #endregion VFX Shader Graph

        
    #region Dissolve VFX: Suplementary Actions
    
    #region Detachment from the VFX
    
    #region Before the VFX starts
    //
    // [Tooltip("[Before the VFX starts] Array of GameObjects that will be detached (and left on the Scene, untouched) from its Parent GameObject (i.e.: the one which will be 'Dissolved' with the VFX).\n\n Example: Items such as: Guns, Rifles, Magic Wands, Hats, etc..")]
    // [SerializeField]
    // private GameObject[] _arrayOfGameObjectsToDetachFromParentVFXsGameObjectBeforeVFXStarts;
    //
    // [Tooltip("[Before the VFX starts] Array of 3D Colliders that will be 'Disabled' just before the VFX starts; so it won't affect them and the Physics will get disabled on them.\n\n * Example: Items such as: Guns, Rifles, Magic Wands, Hats, etc..")]
    // [SerializeField]
    // private Collider[] _arrayOfCollidersToDisableBeforeVFXStarts;
    //
    #endregion Before the VFX starts
    
    
    #region After the VFX ends
    
    [Space()]   [Header("After the VFX ends")]
    [Space(10)]
    [Header("DETACH GameObjects...")]

    [Tooltip("[After the VFX ends] Do you need to DETACH an Array of GameObjects (from the Main Parent)? \n(and left on the Scene, untouched) from its Parent GameObject (i.e.: the one which will be 'Dissolved' with the VFX).\n\n * Example: Items such as: Guns, Rifles, Magic Wands, Hats, etc..")]
    [SerializeField]
    protected bool _detachGameObjectsFromParentVFXsGameObjectAfterVFXEnds = false;
    
    [Tooltip("[After the VFX ends] Array of GameObjects that will be detached (and left on the Scene, untouched) from its Parent GameObject (i.e.: the one which will be 'Dissolved' with the VFX).\n\n * Example: Items such as: Guns, Rifles, Magic Wands, Hats, etc..")]
    [SerializeField]
    protected GameObject[] _arrayOfGameObjectsToDetachFromParentVFXsGameObjectAfterVFXEnds;
    
    [Space(10)]
    [Header("DISABLE 3D Colliders...")]

    [Tooltip("[After the VFX ends] Do you need to DISABLE an Array of 3D Colliders? \n(...right after the VFX ends; so it won't affect them and the Physics will get disabled on them).\n\n * Example: Items such as: Guns, Rifles, Magic Wands, Hats, etc..")]
    [SerializeField]
    protected bool _disableCollidersToDisableAfterVFXEnds = false;
    
    [Tooltip("[After the VFX ends] Array of 3D Colliders that will be 'Disabled' right after the VFX ends; so it won't affect them and the Physics will get disabled on them.\n\n * Example: Items such as: Guns, Rifles, Magic Wands, Hats, etc..")]
    [SerializeField]
    protected Collider[] _arrayOfCollidersToDisableAfterVFXEnds;
    
    [Space(10)]
    [Header("DISABLE / DESTROY THIS SCRIPT or others...")]

    [Tooltip("[After the VFX ends] Do you want to DISABLE THIS SCRIPT at the end of the VFX (and not Destroy all these GameObjects)?")]
    [SerializeField]
    protected bool _disableThisScript = false;
    
    [Tooltip("[After the VFX ends] Do you want to DISABLE ITS PARENT GAME OBJECT (thus, this will Disable all Components - Colliders, etc - including their Update() Loops)?")]
    [SerializeField]
    protected bool _disableParentGameObjectAndEveryBehaviourToo = false;
    
    [Tooltip("[After the VFX ends] Do you want to DESTROY THE GAMEOBJECT where THIS SCRIPT resides..? at the end of the VFX?")]
    [SerializeField]
    protected bool _destroyParentGameObjectAndEverything = false;
    
    #endregion After the VFX ends
    
    #endregion Detachment from the VFX

    #endregion Dissolve VFX: Suplementary Actions


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
        _isRunningShaderEffectFromVFXCoroutine = false;
        
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
        _isRunningShaderEffectFromVFXCoroutine = true;

        
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

            // Initialize the 'DissolveAmount' variable, to change the "Dissolve Amount" parameter
            //
            _dissolveAmount = _arrayOfCachedMaterials[0].GetFloat(_DissolveAmount);

            while (_arrayOfCachedMaterials[0].GetFloat(_DissolveAmount) < 1)
            {

                // Increase the "Dissolve Amount" parameter:
                //
                CalculateDissolveChangeRateAndTimeBetweenVFXChanges();
                //
                _dissolveAmount += _dissolveChangeRate;

                // Assign the new "Dissolve Amount" value:
                //
                for (int i = 0; i < _arrayOfCachedMaterials.Length; i++)
                {
                    // Set the new value of "_DissolveAmount" in the Shader
                    //
                    _arrayOfCachedMaterials[i].SetFloat(_DissolveAmount, _dissolveAmount);


                    // Return of this Coroutine
                    //
                    yield return new WaitForSeconds(_refreshRateDeltaTime);

                } //End for

            } //End while (_cachedSkinnedMeshRendererMaterials[0].GetFloat("") < 1)

        } //End if (_cachedSkinnedMeshRendererMaterials.Length > 0)

        
        // Mark that the Coroutine Ended:
        //
        _isRunningShaderEffectFromVFXCoroutine = false;
        
        
        // Execute some one-time (rather one-frame) actions just when the Dissolve effect (VFX's Shader) ENDS.
        //
        DoExecuteOtherActionsAfterShadersVFXEnds();
        
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

            // 'DissolveAmount' variable, to change the "Dissolve Amount" parameter
            //
            _dissolveAmount = 0.0f;


            // Assign the new "Dissolve Amount" value:
            //
            for (int i = 0; i < _arrayOfCachedMaterials.Length; i++)
            {

                // Set the new value of "_DissolveAmount" in the Shader
                //
                _arrayOfCachedMaterials[i].SetFloat(_DissolveAmount, _dissolveAmount);

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

    
      
    #region Dissolve VFX: Suplementary Actions

    #region After the VFX ends
    
    /// <summary>
    /// Execute other action just when the Dissolve effect (VFX's Shader) ENDS. <br />
    /// <br />
    /// Notice: <br />
    /// 1- This is a VIRTUAL Method, so it can be rewritten in Children Classes (by using the KEYWORD "override" and you may use the Method:  "base.DoExecuteOtherActionsAfterShadersVFXEnds()" in its first line in children classes to also execute its base behaviour from its definition in this class). <br />
    /// </summary>
    protected virtual void DoExecuteOtherActionsAfterShadersVFXEnds()
    {
        
        // Execute Other actions when our Shaders Effect (VFX's) Ends:
        //
        if (!_isRunningShaderEffectFromVFXCoroutine)
        {
            
            #region After the VFX ends

            // 1- Disable Colliders   (after VFX Ends)
            //
            if (_disableCollidersToDisableAfterVFXEnds)
            {
                DisableColliders(_arrayOfCollidersToDisableAfterVFXEnds);
            }
            
            // 2- Detach GameObjects from their Parents:
            //
            if (_detachGameObjectsFromParentVFXsGameObjectAfterVFXEnds)
            {
                DetachAllGameObjectsFromTheirParents(_arrayOfGameObjectsToDetachFromParentVFXsGameObjectAfterVFXEnds);
            }
            
            // 3- Disable this Script, or not.
            //
            this.enabled = (! _disableThisScript);
            
            
            // 4- Disable Main (Parent) GameObject, or not.
            // NOTE:
            // 1- The Main (Parent) GameObject:  is the GameObject this Script is attached to.
            //
            gameObject.SetActive(! _disableParentGameObjectAndEveryBehaviourToo);
            
            
            // 5- Destroy it (the Ragdolls (i.e.: the Character's)... even this script...)
            //
            if (_destroyParentGameObjectAndEverything)
            {
                // 2.2- Destroy this GameObject  (where this Script is attached to...):
                //
                Destroy(gameObject);
            }
            
            #endregion After the VFX ends
            
        }//End if (!_isRunningVFXCoroutine)

    }// End DoExecuteOtherActionsAfterShadersVFXEnds

    #endregion After the VFX ends
    
    
    #region Detachment from the VFX

    /// <summary>
    /// Disables all (3D) Colliders that are given as input.
    /// </summary>
    private void DisableColliders(Collider[] arrayOfColliders)
    {
        
        if ( (arrayOfColliders != null) && (arrayOfColliders[0] != null) )
        {
            
            // Length of the array
            //
            int arrayLength = arrayOfColliders.Length;
        
            for (int i = 0; i < arrayLength; i++)
            {
                // Disable the Collider:
                //
                arrayOfColliders[i].enabled = false;

            }//End for
            
        }//End if ( arrayOfColliders != null )
        
    }// End DisableColliders
    
    /// <summary>
    /// Detaches (from Parent): all GameObjects that are given as input.
    /// </summary>
    private void DetachAllGameObjectsFromTheirParents(GameObject[] arrayOfGameObjectsToDetachFromParent)
    {
        
        if ( (arrayOfGameObjectsToDetachFromParent != null) && (arrayOfGameObjectsToDetachFromParent[0] != null) )
        {
            
            // Length of the array
            //
            int arrayLength = arrayOfGameObjectsToDetachFromParent.Length;
        
            for (int i = 0; i < arrayLength; i++)
            {

                // Get the current sibling index of the Main Parent GameObject  (it is: where this Script is attached...)
                //
                int mainParentSiblingIndex = gameObject.transform.GetSiblingIndex();
                
                // Detaches the transform from its parent.
                //
                arrayOfGameObjectsToDetachFromParent[i].transform.parent = null;
                
                // Set the sibling index to the desired position in the scene hierarchy
                //
                arrayOfGameObjectsToDetachFromParent[i].transform.SetSiblingIndex(mainParentSiblingIndex + 1);

            }//End for
            
        }//End if ( arrayOfColliders != null )
        
    }// End DetachAllGameObjectsFromTheirParents
    
    #endregion Detachment from the VFX
    #endregion Dissolve VFX: Suplementary Actions
    
    
    #endregion My Custom Methods

}

