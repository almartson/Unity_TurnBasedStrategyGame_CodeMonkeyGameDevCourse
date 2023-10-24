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

    /// <summary>
    /// Direction of the Shader VFX  (it's Value may INCREASE or DECREASE): <br /><br />
    /// * TRUE:   NORMAL DIRECTION:  0.0f -> 1.0f <br /><br />
    /// * FALSE:  REVERSE DIRECTION:  1.0f -> 0.0f <br /><br />
    /// </summary>
    [Tooltip("Direction of the Shader VFX's value (INCREASE or DECREASE): \n\n* TRUE:  NORMAL DIRECTION:  0.0f -> 1.0f \n\n* FALSE:  REVERSE DIRECTION:  1.0f -> 0.0f")]
    [SerializeField]
    protected bool _normalDirectionForShaderValueIncrease = true;

    [Tooltip("[READONLY, for DEBUG PURPOSES] Sign of the 'INCREASE' in the VFX Shader's Value (+1  or -1).")]
    [SerializeField]
    protected int _shaderVFXDirectionIncrease = 1;

    
    [Space(10)]
    [Header("[Readonly for Debugging] Current 'Dissolve Value' for the Shader (VFX):")]

    [Tooltip("[Readonly for Debugging purposes] Cache of:  Variable that represents the Amount of 'Erosion' (i.e.: Dissolution...) on the Material shown.")]
    [SerializeField]
    protected float _dissolveAmount = 0.0f;
    
    [Tooltip("Starting / INITIAL Value for the VFX's Shader Effect (i.e.: '_dissolveAmount').")]
    [SerializeField]
    protected float _dissolveAmountInitialization = 0.0f;
    
    [Tooltip("Limit that the VFX's Shader Effect (i.e.: '_dissolveAmount') must try to get in to.")]
    [SerializeField]
    protected float _dissolveAmountLimit = 1.0f;
    
    #region VFX Shader: Dissolve VFX's: Value and Time Rates
    
    [Space()]   [Header("VFX Shader: Dissolve VFX's: Value and Time Rates")]
    [Space(10)]

    // Option 1:  Calculate everything based on TOTAL TIME for the VFX.

    [Header("Option 1:  Calculate everything based on TOTAL TIME for the VFX. ZERO (0) means 'false', so it would NOT be used.")]
    
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
    protected float _yieldDuringThisRefreshRateOrDeltaTimeOfEachFrame = 0.0123f; // 0.025f;

    
    /// <summary>
    /// Cache of:  Variable that represents the Amount of "Erosion" (i.e.: Dissolution...) on the Material shown.
    /// </summary>
    private static readonly int _DissolveAmount = Shader.PropertyToID("_DissolveAmount");
    
    #endregion VFX Shader: Dissolve VFX's: Value and Time Rates
    
    
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

    #endregion VFX Shader Graph

        
    #region Dissolve VFX: Suplementary Actions
    
    #region Detachment from the VFX
    
    #region Before the VFX starts
    
    [Space()]   [Header("Before the VFX starts")]
    [Space(10)]
    [Header("DETACH GameObjects...")]
    
    [Tooltip("[Before the VFX starts] Do you need to DETACH an Array of GameObjects (from the Main Parent)? \n(and left on the Scene, untouched) from its Parent GameObject (i.e.: the one which will be 'Dissolved' with the VFX).\n\n * Example: Items such as: Guns, Rifles, Magic Wands, Hats, etc..")]
    [SerializeField]
    protected bool _detachGameObjectsFromParentVFXsGameObjectBeforeVFXStarts = false;

    [Tooltip("[Before the VFX starts] Array of GameObjects that will be detached (and left on the Scene, untouched) from its Parent GameObject (i.e.: the one which will be 'Dissolved' with the VFX).\n\n Example: Items such as: Guns, Rifles, Magic Wands, Hats, etc..")]
    [SerializeField]
    protected GameObject[] _arrayOfGameObjectsToDetachFromParentVFXsGameObjectBeforeVFXStarts;
    
    
    [Space(10)]
    [Header("[ENABLE / DISABLE] 3D PHYSICS...")]

    [Tooltip("[Before the VFX starts] Do you need to CHANGE the STATE (i.e.: ENABLED -> TO -> DISABLED) of an Array of 3D Colliders AND Rigidbodies? \n\n * Example: Items such as: Guns, Rifles, Magic Wands, Hats, etc..")]
    [SerializeField]
    protected bool _changeStateEnableOrDisableCollidersAndRigidbodiesPhysicsJustBeforeVFXStarts = false;

    [Tooltip("[Before the VFX starts] State (ENABLE or DISABLE) to leave these Components with (Array of 3D Colliders AND Rigidbodies). \n\n * Example: Items such as: Guns, Rifles, Magic Wands, Hats, etc..")]
    [SerializeField]
    protected bool _enableOrDisableCollidersAndRigidbodiesPhysicsJustBeforeVFXStarts = false;
    
    
    [Tooltip("[Before the VFX starts] Array of 3D 'Colliders' that will be 'Disabled' just before the VFX starts; so it won't affect them and the Physics will get disabled on them.\n\n * Example: Items such as: Guns, Rifles, Magic Wands, Hats, etc..")]
    [SerializeField]
    protected Collider[] _arrayOfCollidersToDisableBeforeVFXStarts;
    
    [Tooltip("[Before the VFX starts] Array of 3D 'Rigidbodies' that will be 'Disabled' just before the VFX starts; so it won't affect them and the Physics will get disabled on them.\n\n * Example: Items such as: Guns, Rifles, Magic Wands, Hats, etc..")]
    [SerializeField]
    protected Rigidbody[] _arrayOfRigidbodiesToDisableBeforeVFXStarts;
    
    /// <summary>
    /// (For Validations. Initialize as: FALSE) Boolean Flag to mark the fact that this set of actions have been already executed, so they don't get executed more than once by mistake.
    /// </summary>
    protected bool _hasFinishedExecutionOfActionsBeforeVFXStarts = false;
    
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
    
    [Tooltip("[After the VFX ends] Time Delay (in seconds) to wait ('After the VFX ends') to: Start this Action / behaviour")]
    [Range(0.0f, 183.33f)]
    [SerializeField]
    protected float _timeDelayToDetachFromParentVFXsGameObjectAfterVFXEnds = 0.0f;
    
    [Space(10)]
    [Header("[ENABLE / DISABLE] 3D PHYSICS...")]

    [Tooltip("[After the VFX ends] Do you need to CHANGE the STATE (i.e.: ENABLED -> TO -> DISABLED) of an Array of 3D Colliders AND Rigidbodies? \n(...right after the VFX ends; so it won't affect them and the Physics will get disabled on them).\n\n * Example: Items such as: Guns, Rifles, Magic Wands, Hats, etc..")]
    [SerializeField]
    protected bool _changeStateEnableOrDisableCollidersAndRigidbodiesPhysicsAfterVFXEnds = false;

    [Tooltip("[After the VFX ends] State (ENABLE or DISABLE) to leave these Components with (Array of 3D Colliders AND Rigidbodies). \n(...right after the VFX ends; so it won't affect them and the Physics will get disabled on them).\n\n * Example: Items such as: Guns, Rifles, Magic Wands, Hats, etc..")]
    [SerializeField]
    protected bool _enableOrDisableCollidersAndRigidbodiesPhysicsAfterVFXEnds = false;

    
    [Tooltip("[After the VFX ends] Array of 3D Colliders that will be 'Disabled' right after the VFX ends; so it won't affect them and the Physics will get disabled on them.\n\n * Example: Items such as: Guns, Rifles, Magic Wands, Hats, etc..")]
    [SerializeField]
    protected Collider[] _arrayOfCollidersToDisableAfterVFXEnds;
    
    [Tooltip("[After the VFX ends] Array of 3D 'Rigidbodies' that will be 'Disabled' right after the VFX ends; so it won't affect them and the Physics will get disabled on them.\n\n * Example: Items such as: Guns, Rifles, Magic Wands, Hats, etc..")]
    [SerializeField]
    protected Rigidbody[] _arrayOfRigidbodiesToDisableAfterVFXEnds;
    
    [Tooltip("[After the VFX ends] Time Delay (in seconds) to wait ('After the VFX ends') to: Start this Action / behaviour")]
    [Range(0.0f, 183.33f)]
    [SerializeField]
    protected float _timeDelayToEnableOrDisableCollidersAndRigidbodiesPhysicsAfterVFXEnds = 0.0f;

    
    [Space(10)]
    [Header("DISABLE / DESTROY THIS SCRIPT or others...")]

    
    [Tooltip("[After the VFX ends, READONLY] Largest Time Delay (in seconds) to wait ('After the VFX ends') to: Start this Action / behaviour. THIS WILL BE USED FOR THE LAST FINAL ACTIONS, SEE BELOW:")]
    [SerializeField]
    protected float _largestTimeDelayInSeconds = 0.0f;
    
    [Tooltip("[After the VFX ends] Do you want to DISABLE THIS SCRIPT at the end of the VFX (and not Destroy all these GameObjects)?")]
    [SerializeField]
    protected bool _disableThisScript = false;
    
    [Tooltip("[After the VFX ends] Do you want to DISABLE ITS PARENT GAME OBJECT (thus, this will Disable all Components - Colliders, etc - including their Update() Loops)?")]
    [SerializeField]
    protected bool _disableParentGameObjectAndEveryBehaviourToo = false;
    
    [Tooltip("[After the VFX ends] Do you want to DESTROY THE GAMEOBJECT where THIS SCRIPT resides..? at the end of the VFX?")]
    [SerializeField]
    protected bool _destroyParentGameObjectAndEverything = false;
    
    
    /// <summary>
    /// (For Validations. Initialize as: FALSE) Boolean Flag to mark the fact that this set of actions have been already executed, so they don't get executed more than once by mistake.
    /// </summary>
    protected bool _hasFinishedExecutionOfActionsAfterVFXEnds = false;
    
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
        
        #region Option 1 and Option 2:  Calculate everything based on TOTAL TIME for the VFX.

        CalculateDissolveChangeRateAndTimeBetweenVFXChanges();

        #endregion Option 1 and Option 2:  Calculate everything based on TOTAL TIME for the VFX.

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
    /// Starts the whole VFX. <br /> <br />
    /// It works as a Coroutine. <br /> <br />
    /// The Shader Effect treated here can be one of tow options: <br /> <br />
    /// 1- From:   Zero (0.0f) -> to -> One  (1.0f):  NORMAL DIRECTION = true <br />
    /// 2- From:   One  (1.0f) -> to -> Zero (0.0f).  NORMAL DIRECTION = false; it is a REVERSE. <br />
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator DoStartVFX(bool normalDirectionForShaderValueIncrease)
    {
        
        // 0- ACTIONS TO execute:  Before VFX Starts
        //
        if (!_hasFinishedExecutionOfActionsBeforeVFXStarts)
        {
            DoExecuteOtherActionsBeforeShadersVFXStarts();
        }
        
        
        // 0.2- Mark that the Coroutine Started:
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
            // _dissolveAmount = _arrayOfCachedMaterials[0].GetFloat(_DissolveAmount);
            //
            _dissolveAmount = InitializeVFXsShaderValue( normalDirectionForShaderValueIncrease, _arrayOfCachedMaterials[0].GetFloat(_DissolveAmount) );


            while ( CheckShaderValueCondition(_dissolveAmount, _dissolveAmountLimit, normalDirectionForShaderValueIncrease) )
            {

                // Increase the "Dissolve Amount" parameter:
                //
                CalculateDissolveChangeRateAndTimeBetweenVFXChanges();
                //
                _dissolveAmount += (_dissolveChangeRate * _shaderVFXDirectionIncrease);

                // Assign the new "Dissolve Amount" value:
                //
                for (int i = 0; i < _arrayOfCachedMaterials.Length; i++)
                {
                    // Set the new value of "_DissolveAmount" in the Shader
                    //
                    _arrayOfCachedMaterials[i].SetFloat(_DissolveAmount, _dissolveAmount);


                    // (YIELD / PAUSE for a time)...  Return of this Coroutine
                    //
                    if (_useTotalDissolveTime > 0.0f)
                    {
                        
                        //Debug.Log( $"yield return null", this);
                            
                        // Pause this Coroutine's execution, until the next Frame:
                        //
                        yield return null;
                    }
                    else
                    {
                        // Pause this Coroutine's execution, until this number of seconds has passed:  _yieldDuringThisRefreshRateOrDeltaTimeOfEachFrame
                        //
                        // Original unoptimized Code:  yield return new WaitForSeconds(_yieldDuringThisRefreshRateOrDeltaTimeOfEachFrame);
                        
                        /* Optimized version of Coroutines:
                          * From Bunny83 (Unity Game Dev): https://forum.unity.com/threads/when-we-need-new-instance-of-intrinsic-yieldinstruction.1014100/#post-6575215
                          * 
                          * The usage is pretty straight forward. Place the assembly in your project and you should be able to use
                        Code (CSharp):
                        1. yield return WaitForSecondsSingleton.Get(5f);
                        
                        instead of
                        Code (CSharp):
                        1. yield return new WaitForSeconds(5f);
                        
                        Keep in mind that you must not cache the return value of the Get method as it returns the one single static instance every time. So other calls to Get will modify the internal wait time. Just use the static Get method whenever you want to wait for a certain amount of seconds without any garbage allocations.
                        */
                        yield return WaitForSecondsSingleton.Get(_yieldDuringThisRefreshRateOrDeltaTimeOfEachFrame);
                        
                        //Debug.Log( $"yield return WaitForSecondsSingleton.Get(_yieldDuringThisRefreshRateOrDeltaTimeOfEachFrame) | _yieldDuringThisRefreshRateOrDeltaTimeOfEachFrame = {_yieldDuringThisRefreshRateOrDeltaTimeOfEachFrame}", this);


                    }// End (YIELD / PAUSE for a time)...  Return of this Coroutine
                    
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
    /// Checks to see if the Shader's conditions are already met (so we would stop a Coroutine or Method Loop). <br /><br />
    /// For example:   if ( _dissolveAmount < 1 ) ...
    /// </summary>
    /// <param name="shaderValue"></param>
    /// <param name="currentValue"></param>
    /// <param name="limitValue"></param>
    /// <param name="normalDirectionForShaderValueIncrease"></param>
    protected virtual bool CheckShaderValueCondition(float currentValue, float limitValue, bool normalDirectionForShaderValueIncrease)
    {
        
        if ( normalDirectionForShaderValueIncrease )
        {
            return (currentValue <= limitValue);
        }
        else
        {
            return (currentValue >= limitValue);
        }

    }// End CheckShaderValueCondition
    

    /// <summary>
    /// Restore the 3D Mesh (VFX) to it's Initial state.
    /// </summary>
    /// <returns></returns>
    protected virtual bool TryUndoVFX()
    {
        // Success in this method
        //
        bool successInThisMethod = true;

        // Restore all Boolean Flags
        //
        _hasFinishedExecutionOfActionsBeforeVFXStarts = false;
        _hasFinishedExecutionOfActionsAfterVFXEnds = false;
        _isRunningShaderEffectFromVFXCoroutine = false;
        
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
            _dissolveAmount = _dissolveAmountInitialization;


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
    /// Initializes the Shader's Value + the '_dissolveChangeRate' sign <br /><br />
    /// +  for NORMAL DIRECTION / INCREASE Shader's VFX value) <br /><br />
    /// -  for INVERSE DIRECTION / DECREASE Shader's VFX value) <br /><br />
    /// </summary>
    /// <param name="normalDirectionForShaderValueIncrease"></param>
    /// <param name="currentVFXShaderValue"></param>
    /// <returns></returns>
    protected virtual float InitializeVFXsShaderValue(bool normalDirectionForShaderValueIncrease, float currentVFXShaderValue)
    {

        // Step 0:    Calculate the sign of the INCREASE / DECREASE of the Shader Effect:
        //
        if (normalDirectionForShaderValueIncrease)
        {
            _shaderVFXDirectionIncrease = (+1);
        }
        else
        {
            _shaderVFXDirectionIncrease = (-1);

        }// if (normalDirectionForShader)
        
        // Return the Shader Value:
        //
        return currentVFXShaderValue;

    }// End InitializeVFXsShaderValue

    
    /// <summary>
    /// Option 1 or Option 2:  Calculate everything based on TOTAL TIME ( _useTotalDissolveTime ) for the VFX.
    /// </summary>
    protected virtual void CalculateDissolveChangeRateAndTimeBetweenVFXChanges()
    {

        // Option 1:  Calculate everything based on TOTAL TIME for the VFX.
        //
        if (_useTotalDissolveTime > 0.0f)
        {
            
            // Fix Time between VFX small Changes:
            //
            _yieldDuringThisRefreshRateOrDeltaTimeOfEachFrame = Time.deltaTime;
                
            // [ DissolveAmountThisFrame = ?? = 
            // = ( timeDeltaTimeOfDissolve * (1.0)TotalDissolveAmount ) / MY TotalDissolveTime ]

            _dissolveChangeRate = _yieldDuringThisRefreshRateOrDeltaTimeOfEachFrame / _useTotalDissolveTime;
            
        } // End Option 1:  Calculate everything based on TOTAL TIME for the VFX.
        
        // NOTE:  Option 2 is happening with no further Calculations, just by using the initial values from the Inspector.
        // So it complies with the condition:   if (_useTotalDissolveTime <= 0.0f)
        
    }// End CalculateDissolveChangeRate

    
    #endregion Misc Methods

    
      
    #region Dissolve VFX: Suplementary Actions


    #region Before the VFX starts
    
    /// <summary>
    /// Execute other action just: "Before" the VFX (Dissolve effect (VFX's Shader)) STARTS. <br />
    /// <br />
    /// Notice: <br />
    /// 1- This is a VIRTUAL Method, so it can be rewritten in Children Classes (by using the KEYWORD "override" and you may use the Method:  "base.DoExecuteOtherActionsBeforeShadersVFXStarts()" in its first line in children classes to also execute its base behaviour from its definition in this class). <br />
    /// </summary>
    protected virtual void DoExecuteOtherActionsBeforeShadersVFXStarts()
    {
        
        // Execute Other actions BEFORE our Shaders Effect (VFX's) Starts:
        //
        if (!_hasFinishedExecutionOfActionsBeforeVFXStarts)
        {
 
            // 1- Disable Colliders and 3D Rigidbodies   (Before VFX Starts)
            //
            if (_changeStateEnableOrDisableCollidersAndRigidbodiesPhysicsJustBeforeVFXStarts)
            {

                EnableOrDisableCollidersAndRigidbodies(_enableOrDisableCollidersAndRigidbodiesPhysicsJustBeforeVFXStarts, _arrayOfCollidersToDisableBeforeVFXStarts, _enableOrDisableCollidersAndRigidbodiesPhysicsJustBeforeVFXStarts, _arrayOfRigidbodiesToDisableBeforeVFXStarts);
                
            }
            
            // 2- Detach GameObjects from their Parents:
            //
            if (_detachGameObjectsFromParentVFXsGameObjectBeforeVFXStarts)
            {
                DetachAllGameObjectsFromTheirParents(_arrayOfGameObjectsToDetachFromParentVFXsGameObjectBeforeVFXStarts);
            }
            
            // Mark the Boolean Flag as:  "Actions Completed"
            //
            _hasFinishedExecutionOfActionsBeforeVFXStarts = true;

        }//End if 

    }// End DoExecuteOtherActionsBeforeShadersVFXStarts

    #endregion Before the VFX starts


    #region After the VFX ends
    
    /// <summary>
    /// Execute other action just when the Dissolve effect (VFX's Shader) ENDS. <br />
    /// <br />
    /// Notice: <br />
    /// 1- This is a VIRTUAL Method, so it can be rewritten in Children Classes (by using the KEYWORD "override" and you may use the Method:  "base.DoExecuteOtherActionsAfterShadersVFXEnds()" in its first line in children classes to also execute its base behaviour from its definition in this class). <br />
    /// </summary>
    protected virtual void DoExecuteOtherActionsAfterShadersVFXEnds()
    {
        // IMPORTANT NOTICE:
        //
        // * The FINAL ACTIONS of this group:   should be executed only after the largest Coroutine of the group has ended already (to avoid Bugs related to Coroutines and Destroyed / null / Disabled GameObjects / Scripts):

        _largestTimeDelayInSeconds = Mathf.Max(_timeDelayToEnableOrDisableCollidersAndRigidbodiesPhysicsAfterVFXEnds, _timeDelayToDetachFromParentVFXsGameObjectAfterVFXEnds); 
        
        // Determine what the Delegate or CallBack function will be (to be executed after the Coroutines end...), if there's a Delay:
        // (NOTICE:  Add all representation of Coroutines here):
        //
        System.Action onCompletionForEnableOrDisableCollidersAndRigidbodiesPhysicsAfterVFXEnds = StartFinalActionsToDisableOrDestroyThisScriptAndParentGameObject;
        //
        System.Action onCompletionToDetachFromParentVFXsGameObjectAfterVFXEnds = StartFinalActionsToDisableOrDestroyThisScriptAndParentGameObject;
        
        
        // Final Validation:   Based on its Boolean Flag of execution (for each case): 
        //
        bool isThisScriptExecutingFinalActionsAsACoroutine = false;
        bool isUsingCoroutineForEnableOrDisableCollidersAndRigidbodiesPhysicsAfterVFXEnds = false;
        bool isUsingCoroutineToDetachFromParentVFXsGameObjectAfterVFXEnds = false;


        if (_largestTimeDelayInSeconds > 0.0f)
        {

            // 1- Coroutine #1
            //
            if ( _changeStateEnableOrDisableCollidersAndRigidbodiesPhysicsAfterVFXEnds && (_timeDelayToEnableOrDisableCollidersAndRigidbodiesPhysicsAfterVFXEnds > 0.0f ) )
            {
                isThisScriptExecutingFinalActionsAsACoroutine = true;
                isUsingCoroutineForEnableOrDisableCollidersAndRigidbodiesPhysicsAfterVFXEnds = true;
            }
            //
            // 2- Coroutine #2
            //
            if ( _detachGameObjectsFromParentVFXsGameObjectAfterVFXEnds && (_timeDelayToDetachFromParentVFXsGameObjectAfterVFXEnds > 0.0f ) )
            {
                isThisScriptExecutingFinalActionsAsACoroutine = true;
                isUsingCoroutineToDetachFromParentVFXsGameObjectAfterVFXEnds = true;
            }
            //
            //... NOTE  WARNING:   Add more: "else if()"  here... if you added more CUSTOM ACTIONS above using a TIME DELAY (COROUTINE)  
            //...
            // else
            // {
            //     // No Coroutines used:    mark as normal:
            //     //
            //     isThisScriptExecutingFinalActionsAsACoroutine = false;
            // }
            
            
            // Final Validation of "The NOT-Largest Coroutines":   (we must set their "onCompletion..." CallBack as NULL)
            // OPTION #2:   Only set the LAST FINAL ACTION with a DELAY Equal to the LARGEST COROUTINE (no matter if it is ENABLED OR NOT for this VFX):   <-- Using this one.
            //
            //
            if ( isUsingCoroutineForEnableOrDisableCollidersAndRigidbodiesPhysicsAfterVFXEnds && isUsingCoroutineToDetachFromParentVFXsGameObjectAfterVFXEnds )
            {

                // Compare against ALL OTHER COROUTINES (only another, the second one), and decide and fix the "onCompletion" s:
                //
                if (_timeDelayToEnableOrDisableCollidersAndRigidbodiesPhysicsAfterVFXEnds > _timeDelayToDetachFromParentVFXsGameObjectAfterVFXEnds)
                {
                    // First Coroutine is Larger
                    // Fix onCompletion of the second(s):  to NULL
                    //
                    onCompletionToDetachFromParentVFXsGameObjectAfterVFXEnds = null;
                }
                else
                {
                    // Second Coroutine is Larger
                    // Fix onCompletion of the First (or "previous" coroutine):  to NULL
                    //
                    onCompletionForEnableOrDisableCollidersAndRigidbodiesPhysicsAfterVFXEnds = null;
                }
            }//End if ( ( isUsingCoroutineForEnableOrDisableCollidersAndRigidbodiesPhy...
            // else
            // {
            //     // This case is NOT IMPORTANT, because all "onCompletion..." delegates are set, so if we are using any Coroutine (the Buggy case could be using: only one Coroutine...):   then the Last (Final) Actions will be performed as the Delegate, in the end. So it would always work fine.
            // }
         
        }//End if (_largestTimeDelayInSeconds > 0.0f)

        
        // Coroutines Executions:
        //
        if (isThisScriptExecutingFinalActionsAsACoroutine)
        {
            // Execute Other actions when our Shaders Effect (VFX's) Ends:
            //
            if (!_isRunningShaderEffectFromVFXCoroutine & !_hasFinishedExecutionOfActionsAfterVFXEnds)
            {

                // 1- Disable Colliders and 3D Rigidbodies    (after VFX Ends)
                //
                if (_changeStateEnableOrDisableCollidersAndRigidbodiesPhysicsAfterVFXEnds)
                {

                    if (_timeDelayToEnableOrDisableCollidersAndRigidbodiesPhysicsAfterVFXEnds > 0.0f)
                    {

                        // Coroutine Implementation
                        //
                        StartCoroutine(EnableOrDisableCollidersAndRigidbodies(
                            _enableOrDisableCollidersAndRigidbodiesPhysicsAfterVFXEnds,
                            _arrayOfCollidersToDisableAfterVFXEnds,
                            _enableOrDisableCollidersAndRigidbodiesPhysicsAfterVFXEnds,
                            _arrayOfRigidbodiesToDisableAfterVFXEnds,
                            _timeDelayToEnableOrDisableCollidersAndRigidbodiesPhysicsAfterVFXEnds, onCompletionForEnableOrDisableCollidersAndRigidbodiesPhysicsAfterVFXEnds));

                    } //End if (_timeDelayTo...
                    else
                    {
                        // OR:  Basic Implementation
                        //
                        EnableOrDisableCollidersAndRigidbodies(
                            _enableOrDisableCollidersAndRigidbodiesPhysicsAfterVFXEnds,
                            _arrayOfCollidersToDisableAfterVFXEnds,
                            _enableOrDisableCollidersAndRigidbodiesPhysicsAfterVFXEnds,
                            _arrayOfRigidbodiesToDisableAfterVFXEnds);

                    } //End else
                } //End:  1- Disable Colliders and 3D Rigidbodies    (after VFX Ends)


                // 2- Detach GameObjects from their Parents:
                //
                if (_detachGameObjectsFromParentVFXsGameObjectAfterVFXEnds)
                {

                    if (_timeDelayToDetachFromParentVFXsGameObjectAfterVFXEnds > 0.0f)
                    {

                        // Coroutine Implementation
                        //
                        StartCoroutine(DetachAllGameObjectsFromTheirParents(
                            _arrayOfGameObjectsToDetachFromParentVFXsGameObjectAfterVFXEnds,
                            _timeDelayToDetachFromParentVFXsGameObjectAfterVFXEnds, onCompletionToDetachFromParentVFXsGameObjectAfterVFXEnds));

                    } //End if (_timeDelay...
                    else
                    {
                        // OR:  Basic Implementation
                        //
                        DetachAllGameObjectsFromTheirParents(
                            _arrayOfGameObjectsToDetachFromParentVFXsGameObjectAfterVFXEnds);

                    } //End else

                } //End:  2- Detach GameObjects from their Parents:

            }//End if (!_isRunningShaderEffectFromVFXCoroutine & !_hasFinishedExecutionOfActionsAfterVFXEnds)
        }
        else
        {
            // No Coroutines were used,  normal execution of "FINAL ACTIONS" within this Time-Frame:
            //
            FinalActionsToDisableOrDestroyThisScriptAndParentGameObject();

        }//End if ( largestTimeDelayInSeconds > 0.0f )
        
    }// End DoExecuteOtherActionsAfterShadersVFXEnds
    
    #endregion After the VFX ends
    
    
    #region Detachment from the VFX

    /// <summary>
    /// Disables all (3D) Colliders and Rigidbodies, that are given as input.
    /// </summary>
    protected virtual void EnableOrDisableCollidersAndRigidbodies(bool flagEnabledOrDisabledForColliders, Collider[] arrayOfColliders, bool flagEnabledOrDisabledForRigidbodies, Rigidbody[] arrayOfRigidbodies)
    {
        
        // 1 of 2:  Colliders
        //
        if ((arrayOfColliders != null) && ((arrayOfColliders.Length > 0) && (arrayOfColliders[0] != null)) )
        {
            
            // Length of the array
            //
            int arrayLength = arrayOfColliders.Length;
        
            for (int i = 0; i < arrayLength; i++)
            {
                // Disable the Collider:
                //
                arrayOfColliders[i].enabled = flagEnabledOrDisabledForColliders;

            }//End for
            
        }//End if ( arrayOfColliders != null )
     
          
        // 2 of 2:  3D Rigidbodies
        //
        if ( (arrayOfRigidbodies != null) && ((arrayOfRigidbodies.Length > 0) && (arrayOfRigidbodies[0] != null)) )
        {
            
            // Length of the array
            //
            int arrayLength = arrayOfRigidbodies.Length;
        
            for (int i = 0; i < arrayLength; i++)
            {
                
                // Disable the Physics calculations related to the 3D Rigidbody:
                //
                EnableOrDisableRigidBody3DPhysics(flagEnabledOrDisabledForRigidbodies, arrayOfRigidbodies[i]);
                
            }//End for
            
        }//End if ( arrayOfRigidbodies != null )
  
    }// End DisableOrEnableCollidersAndRigidbodies
    
    
    /// <summary>
    /// [Coroutine Implementation with a Delay] <br /> <br />
    /// Disables all (3D) Colliders and Rigidbodies, that are given as input.
    /// </summary>
    protected virtual IEnumerator EnableOrDisableCollidersAndRigidbodies(bool flagEnabledOrDisabledForColliders, Collider[] arrayOfColliders, bool flagEnabledOrDisabledForRigidbodies, Rigidbody[] arrayOfRigidbodies, float timeDelayInSeconds, System.Action onCompletion)
    {
        
        // 1- Time Delay:
        //
        yield return WaitForSecondsSingleton.Get(timeDelayInSeconds);
        
        // 2- Execute the ACTION
        //
        EnableOrDisableCollidersAndRigidbodies(flagEnabledOrDisabledForColliders, arrayOfColliders,
            flagEnabledOrDisabledForRigidbodies, arrayOfRigidbodies);
        

        // 3- Call the callback method when the Coroutine ends
        //
        if (onCompletion != null)
        {
            onCompletion();
        }

    }// End DisableOrEnableCollidersAndRigidbodies


    
    /// <summary>
    /// Disables the given Rigidbodies's Physics.
    /// </summary>
    /// <param name="flagEnabledOrDisabled"></param>
    /// <param name="myRigidbody"></param>
    protected virtual void EnableOrDisableRigidBody3DPhysics(bool flagEnabledOrDisabled, Rigidbody myRigidbody)
    {
        // Disable the Physics calculations related to the 3D Rigidbody:
        //
        myRigidbody.isKinematic = !flagEnabledOrDisabled;
        myRigidbody.detectCollisions = flagEnabledOrDisabled;
        myRigidbody.useGravity = flagEnabledOrDisabled;

    }// End EnableOrDisableRigidBody3DPhysics
    
    
    /// <summary>
    /// Detaches (from Parent): all GameObjects that are given as input.
    /// </summary>
    protected virtual void DetachAllGameObjectsFromTheirParents(GameObject[] arrayOfGameObjectsToDetachFromParent)
    {
        
        if ( (arrayOfGameObjectsToDetachFromParent != null) && ((arrayOfGameObjectsToDetachFromParent.Length > 0) && (arrayOfGameObjectsToDetachFromParent[0] != null)) )
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
    
    
    /// <summary>
    /// [Coroutine Implementation: Execute after a set Time Delay, in Seconds] <br /> <br />
    /// Detaches (from Parent): all GameObjects that are given as input.
    /// </summary>
    protected virtual IEnumerator DetachAllGameObjectsFromTheirParents(GameObject[] arrayOfGameObjectsToDetachFromParent, float timeDelayInSeconds, System.Action onCompletion)
    {
        
        // 1- Coroutine Delay
        //
        yield return WaitForSecondsSingleton.Get(timeDelayInSeconds);

        // 2- Execute the Behaviour:
        //
        DetachAllGameObjectsFromTheirParents(arrayOfGameObjectsToDetachFromParent);
        
        
        // 3- Call the callback method when the Coroutine ends
        //
        if (onCompletion != null)
        {
            onCompletion();
        }

    }// End DetachAllGameObjectsFromTheirParents

    
    /// <summary>
    /// Final Actions after:  VFX ENDS + ALL COROUTINES end + everything ends.
    /// </summary>
    protected virtual void StartFinalActionsToDisableOrDestroyThisScriptAndParentGameObject()
    {
        //Debug.Log( $"Beginning:  StartFinalActionsToDisableOrDestroyThisScriptAndParentGameObject() | in: this Object:{this.gameObject.name}", this);
        
        FinalActionsToDisableOrDestroyThisScriptAndParentGameObject();

        //Debug.Log( $"Ending:  StartFinalActionsToDisableOrDestroyThisScriptAndParentGameObject() | in: this Object:{this.gameObject.name}", this);
        
    }// End FinalActionsToDisableOrDestroyThisScriptAndPArentGameObject


    protected virtual void FinalActionsToDisableOrDestroyThisScriptAndParentGameObject()
    {
        
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
            
        // Mark the Boolean Flag as:  "Actions Completed"
        //
        _hasFinishedExecutionOfActionsAfterVFXEnds = true;

    }// End FinalActionsToDisableOrDestroyThisScriptAndPArentGameObject


    #endregion Detachment from the VFX
    #endregion Dissolve VFX: Suplementary Actions
    
    
    #endregion My Custom Methods

}

