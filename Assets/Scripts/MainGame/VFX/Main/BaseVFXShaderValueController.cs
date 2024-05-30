/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public abstract class BaseVFXShaderValueController : MonoBehaviour
{

    #region Attributes

    #region Constants

    #endregion Constants

    #region Enums

    public enum UnityEditorFlowState
    {
        None,
        Awake,
        Start,
        BeforeTheVFXStarts,
        Update,
        LateUpdate,
        AfterTheVFXEnds,
        TheLastOfTheLast,
    }

    #endregion Enums


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
    
    #region Name of the VFX'x Shader's 'MAIN PROPERTY'
    
    [Header("VFX'x Shader's name of 'MAIN PROPERTY' [WARN: SET ON EDITOR TIME only]")]
    
    [Tooltip("[READ WARNING IN THE END] \n\n Name of the VFX'x Shader's 'MAIN PROPERTY', which will change/update in the SHADER's Effect in this VFX's Coroutine. \n\n [WARNING: SET ON EDITOR TIME only, because this will be used in a STATIC CONTEXT. There's a fix in code to change the SHADER PROPERTY on runtime, by using 'strings for Shader Property Names', instead of an Optimized 'Shader.PropertyToID(...)', which we are using because its more optimized...]")]
    [SerializeField]
    protected string _shaderVFXMainValuePropertyNameInShader = "_DissolveAmount"; // "_DissolveAmount";

    /// <summary>
    /// Cache of:  Variable that represents the Shader's PARAMETER THAT CHANGES over time (for the VFX on the Shader side to occur...). For Instance: Amount of "Erosion" (i.e.: Dissolution...) on the Material shown.
    /// </summary>
    private static int _shaderVFXMainPropertyToPlayWith;    // = Shader.PropertyToID(_shaderVFXMainValuePropertyNameInShader);
    
    #endregion Name of the VFX'x Shader's 'MAIN PROPERTY'
    
    [Header("Direction of the Shader VFX's value (INCREASE or DECREASE)")]

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
    protected sbyte _shaderVFXDirectionIncrease = 1;

    [Tooltip("Initializes MIN, MAX and 'Current' Shader Values by itself (NOTE: it uses 0.0f and 1.0f as MIN and MAX DEFAULTS!).")]
    [SerializeField]
    protected bool _smartAutomaticShaderValuesInitialization = true;

    
    [Tooltip("Starting / INITIAL Value for the VFX's Shader Effect we want to INCREASE / DECREASE over time (e.g.: '_dissolveAmount').")]
    [SerializeField]
    protected float _shaderVFXMainValueInitialization = 0.0f;
    
    [Tooltip("MINIMUM Limit that the VFX's Shader Effect we want to INCREASE / DECREASE over time (e.g.: '_dissolveAmount') must try to get in to.")]
    [SerializeField]
    protected float _minimumShaderVFXMainValueLimit = 0.0f;
    
    [Tooltip("MAXIMUM Limit that the VFX's Shader Effect we want to INCREASE / DECREASE over time (e.g.: '_dissolveAmount') must try to get in to.")]
    [SerializeField]
    protected float _maximumShaderVFXMainValueLimit = 1.0f;
    
    
    /// <summary>
    /// (DEFAULT) MINIMUM Limit that the VFX's Shader Effect we want to INCREASE / DECREASE over time (e.g.: '_dissolveAmount') must try to get in to.
    /// </summary>
    protected const float _MINIMUM_DEFAULT_SHADER_VFX_MAIN_VALUE_LIMIT = 0.0f;
    
    /// <summary>
    /// (DEFAULT) MAXIMUM Limit that the VFX's Shader Effect we want to INCREASE / DECREASE over time (e.g.: '_dissolveAmount') must try to get in to.
    /// </summary>
    protected const float _MAXIMUM_DEFAULT_SHADER_VFX_MAIN_VALUE_LIMIT = 1.0f;
    
    
    [Space(10)]
    [Header("[Readonly for Debugging] Current 'Main Value' to play with the Shader (VFX):")]

    [Tooltip("[READONLY for Debugging purposes] Cache of:  Current 'Main Value' (VARIABLE) to play with the Shader (VFX)")]
    [SerializeField]
    protected float _shaderVFXMainValue = 0.0f;
    

    #region VFX Shader: 'Main Value' (VARIABLE): Value and Time Rates
    
    [Space()]   [Header("VFX Shader: 'Main Value' (VARIABLE): Value and Time Rates")]
    [Space(10)]

    // Option 1:  Calculate everything based on TOTAL TIME for the VFX.

    [Header("Option 1:  Calculate everything based on TOTAL TIME for the VFX. ZERO (0) means 'false', so it would NOT be used.")]
    
    [Tooltip("RECOMMENDED, NON ZERO: Let it be zero (0.0f) if you don't want to use it! \nCalculate everything based on TOTAL TIME for the SHADER's VFX.")]
    [SerializeField]
    protected float _useTotalTime = 1.5f; // 1.5f


    // Option 2:  Specify every value here for the VFX (and let '_useTotalTime' = 0).

    [Space()]
    [Header("Option 2:  Specify every value here for the VFX (and let '_useTotalTime' = 0).")]

    [Tooltip("'Rate of change per frame' of the Shader Effect.")]
    [SerializeField]
    protected float _shaderVFXMainValueChangeRate = 0.0111f; // 0.0125f;

    [Tooltip("Time to ('wait'):  'yield return WaitForSeconds(this time var...)' between any changes/updates in the SHADER's Effect in this VFX's Coroutine")]
    [SerializeField]
    protected float _yieldDuringThisRefreshRateOrDeltaTimeOfEachFrame = 0.0123f; // 0.025f;
    
    #endregion VFX Shader: 'Main Value' (VARIABLE): Value and Time Rates
    
    
    [Space(10)]
    [Header("VFX Shader Graph: Coroutine Management:")]

    [Tooltip("[Readonly for Debug] Is the VFX (shader) Coroutine running now?")]
    [SerializeField]
    protected bool _isRunningShaderEffectFromVFXCoroutine = false;
    
    #endregion VFX Shader Coroutine Management
    
    
    #region Materials
    
    [Space()] [Space(10)]
    [Header("VFX Shader Graph: Materials")]
    [Space(10)]
    
    #region 0- Default Materials (NON-VFX)
    //
    // [Header("0- Default Materials (NON-VFX)")]
    //
    // [Header("__________________________________")]
    //
    #endregion 0- Default Materials (NON-VFX)


    #region 1-:VFX Materials
    
    [Space(10)]
    [Header("1-:VFX Materials")]
    [Space(10)]
    
    #region Materials Case Scenario  1-: SkinnedMeshRenderer's Materials

    [Header("1.1-: SkinnedMeshRenderer's Materials")]
    
    // 1- Array of (actual): SkinnedMeshRenderer
    //
    [Tooltip("[Important: Fill this up with ALL the 'SkinnedMeshRenderer(s)' that are present in your GameObject and its children in its Hierarchy].\n\n SkinnedMeshRenderer[] arrays... all in one array.\n\n* This is important to: 1- Work on the 'VFX'; and \n2- Get the 'VFX Materials' from it (they must have been already set in 'Editor Mode' by the 3D Artist).")]
    [SerializeField]
    protected SkinnedMeshRenderer[] _myArrayOfSkinnedMeshRender;

    [Header("__________________________________")]
    
    [Tooltip("VFX's SkinnedMeshRenderer[] array, To get the 'VFX Materials' from it.")]
    [SerializeField]
    protected SkinnedMeshRenderer[] _arrayOfVFXSkinnedMeshRender;

    [Tooltip("NON-VFX's SkinnedMeshRenderer[] array: To set the default (more performant) Materials, AFTER THE VFX ENDS and the Game Continues...")]
    [SerializeField]
    protected SkinnedMeshRenderer[] _arrayOfNonVFXSkinnedMeshRender;
    
    [Tooltip("[ReadOnly for Debug] Array of 'VFX Materials' that belong to the '3D Character' and its 'Rigged 3D Accessories' (such as: Clothes, Flags, etc).")]
    [SerializeField]
    protected Material[] _arrayOfCachedSkinnedMeshRendererVFXMaterials;

    #endregion Materials Case Scenario  1-: SkinnedMeshRenderer's Materials


    #region Materials Case Scenario  2-: Mesh Renderer's Materials

    [Space(10)]
    [Header("1.2-: Mesh Renderer's Materials")]
    
    // 2- Array of (actual): MeshRenderer
    //
    [Tooltip("[Important: Fill this up with ALL the 'MeshRenderer(s)' that are present in your GameObject and its children in its Hierarchy].\n\n MeshRenderer[] arrays... all in one array.\n\n* This is important to: 1- Work on the 'VFX'; and \n2- Get the 'VFX Materials' from it (they must have been already set in 'Editor Mode' by the 3D Artist).")]
    [SerializeField]
    protected MeshRenderer[] _myArrayOfMeshRender;

    [Header("__________________________________")]
    
    [Tooltip("VFX's MeshRenderer[] , To get the 'VFX Materials' from it.")]
    [SerializeField]
    protected MeshRenderer[] _arrayOfVFXMeshRender;

    [Tooltip("NON-VFX's MeshRenderer[] array: To set the default (more performant) Materials, AFTER THE VFX ENDS and the Game Continues...")]
    [SerializeField]
    protected MeshRenderer[] _arrayOfNonVFXMeshRender;
    
    [Space(10)]
    [Tooltip("[ReadOnly for Debug] Array of 'VFX Materials' that belong to the '3D Mesh'.")]
    [SerializeField]
    protected Material[] _arrayOfCachedMeshRendererVFXMaterials;

    #endregion Materials Case Scenario  2-: Mesh Renderer's Materials

    [Header("__________________________________")]

    [Tooltip("[ReadOnly for Debug] Array of ALL 'VFX Materials' that will be processed by the VFX :) (that belong to the '3D Mesh').")]
    [SerializeField]
    protected Material[] _arrayOfCachedVFXMaterials;

    #endregion 1-:VFX Materials
    
    #endregion Materials

    #endregion VFX Shader Graph

        
    #region Shader and VFX: Suplementary Actions
    
    #region Detachment from the VFX
    
    #region Before the VFX starts
    
    [Space()]   [Header("Before the VFX starts")]
    [Space(10)]
    [Header("DETACH GameObjects...")]
    
    [Tooltip("[Before the VFX starts] Do you need to DETACH an Array of GameObjects (from the Main Parent)? \n(and left on the Scene, untouched) from its Parent GameObject (i.e.: the one which will be 'processed' by the VFX).\n\n * Example: Items such as: Guns, Rifles, Magic Wands, Hats, etc..")]
    [SerializeField]
    protected bool _detachGameObjectsFromParentVFXsGameObjectBeforeVFXStarts = false;

    [Tooltip("[Before the VFX starts] Array of GameObjects that will be detached (and left on the Scene, untouched) from its Parent GameObject (i.e.: the one which will be 'processed' by the VFX).\n\n Example: Items such as: Guns, Rifles, Magic Wands, Hats, etc..")]
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
    [Header("[REVERT] Materials (to Default)...")]

    [Tooltip("[After the VFX ends] Do you need to REVERT the 'VFX Materials' to 'Default ones' (i.e.: for Optimization purposes)? \n(...right after the VFX ends; so the 3D GameObject may continue working with 'more performant' (Non-VFX) Materials).")]
    [SerializeField]
    protected bool _revertMaterialsToDefaultOnesAfterVFXEnds = false;
    

    [Space(10)]
    [Header("DETACH GameObjects...")]

    [Tooltip("[After the VFX ends] Do you need to DETACH an Array of GameObjects (from the Main Parent)? \n(and left on the Scene, untouched) from its Parent GameObject (i.e.: the one which will be 'processed' by the VFX).\n\n * Example: Items such as: Guns, Rifles, Magic Wands, Hats, etc..")]
    [SerializeField]
    protected bool _detachGameObjectsFromParentVFXsGameObjectAfterVFXEnds = false;
    
    [Tooltip("[After the VFX ends] Array of GameObjects that will be detached (and left on the Scene, untouched) from its Parent GameObject (i.e.: the one which will be 'processed' by the VFX).\n\n * Example: Items such as: Guns, Rifles, Magic Wands, Hats, etc..")]
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

    #endregion Shader and VFX: Suplementary Actions


    #endregion Attributes


    #region Unity Methods
    
    
    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>
    protected virtual void Awake()
    {
        #region Materials List

        #region 0- Default Materials
        
        // 0- Default Materials
        //
        if ( _revertMaterialsToDefaultOnesAfterVFXEnds  
            && (((_arrayOfNonVFXSkinnedMeshRender == null) || (_arrayOfNonVFXSkinnedMeshRender.Length == 0) || (_arrayOfNonVFXSkinnedMeshRender[0] == null) ) 
            || ((_arrayOfVFXSkinnedMeshRender == null) || (_arrayOfVFXSkinnedMeshRender.Length == 0) || (_arrayOfVFXSkinnedMeshRender[0] == null) )
            || (_myArrayOfSkinnedMeshRender == null)) )
        {

            Debug.LogError( $"{this.name}: It will be impossible to revert the Materials to Default ones because they are not set up previously | in: this Object:{this.gameObject.name}", this);
            //
            // Set the Boolean flag to: false... so we don't try to execute that functionality.
            //
            _revertMaterialsToDefaultOnesAfterVFXEnds = false;

        }// End  0- Default Materials

        #endregion 0- Default Materials


        #region 1- Initialize the Material[]s arrays

        // 1- Initialize the VFX Materials[] arrays, to apply the VFX in sync to their Shaders VFX too.
        //    1.1-  Actual:   SkinnedMeshRender  &&  MeshRender
        //
        GetMaterialsFromRenderers(ref _myArrayOfSkinnedMeshRender, ref _myArrayOfMeshRender, ref _arrayOfCachedSkinnedMeshRendererVFXMaterials, ref _arrayOfCachedMeshRendererVFXMaterials);
        //
        //       1.1.2- Grab all Materials into one Array []
        //              Set the Materials[] array, in:   _arrayOfCachedVFXMaterials
        //
        SetMainArrayOfMaterialsForVFX( ref _arrayOfCachedSkinnedMeshRendererVFXMaterials, ref _arrayOfCachedMeshRendererVFXMaterials, ref _arrayOfCachedVFXMaterials);


        //    1.2-  Default - NON-VFX-:   SkinnedMeshRender  &&  MeshRender   
        //
        // They already are in:   _arrayOfNonVFXSkinnedMeshRender

        //    1.3-  VFX-Material's:   SkinnedMeshRender  &&  MeshRender   OJO:  CHEQUEAR: ESTOS YA ESTAN BIEN
        //
        // They already are in:   _arrayOfNonVFXMeshRender


        #endregion 1- Initialize the Material[]s arrays
        

        #region 1- B) Materials initialization: Deprecated Code
        
        // // 1- 3D  Characters:
        // // Add the reference to 'SkinnedMeshRenderer' to get the Materials from it.
        // //
        // if (_arrayOfVFXSkinnedMeshRender != null)
        // {
        //     _arrayOfCachedSkinnedMeshRendererVFXMaterials = _arrayOfVFXSkinnedMeshRender.sharedMaterials;
        // }
        //
        // //
        // // 2- 3D (Static, Not Rigged)  Meshes:
        // // Add the reference to 'MeshRenderer' to get the Materials from it.
        // //
        // if (_arrayOfVFXMeshRender != null)
        // {
        //     _arrayOfCachedMeshRendererVFXMaterials = _arrayOfVFXMeshRender.sharedMaterials;
        // }
        //
        // //
        // // 3- Grab all Materials into one Array []
        // // 3.1 - Length, auxiliary variables
        // //
        // int lengthOfArrayOfCachedSkinnedMeshRendererMaterials = _arrayOfCachedSkinnedMeshRendererVFXMaterials.Length;
        // int lengthOfArrayOfCachedMeshRendererMaterials = _arrayOfCachedMeshRendererVFXMaterials.Length;
        // //
        // // 3.2- Fill in the Array:
        // //   3.2.1 - Create Array:
        // //
        // _arrayOfCachedVFXMaterials = new Material [lengthOfArrayOfCachedSkinnedMeshRendererMaterials +
        //                                         lengthOfArrayOfCachedMeshRendererMaterials];
        // //
        // //   3.2.2 - Fill in the Array
        // //
        // _arrayOfCachedSkinnedMeshRendererVFXMaterials.CopyTo(_arrayOfCachedVFXMaterials, 0);
        // _arrayOfCachedMeshRendererVFXMaterials.CopyTo(_arrayOfCachedVFXMaterials, lengthOfArrayOfCachedSkinnedMeshRendererMaterials);

        #endregion 1- B) Materials initialization: Deprecated Code
        
        #endregion Materials List


        #region VFX Shader's: 'Main Value': Amount  and  Time Rates

        #region Name of the VFX'x Shader's 'MAIN PROPERTY'
    
        // Initialization of  '_shaderVFXMainPropertyToPlayWith'
        //
        _shaderVFXMainPropertyToPlayWith = Shader.PropertyToID(_shaderVFXMainValuePropertyNameInShader);
    
        #endregion Name of the VFX'x Shader's 'MAIN PROPERTY'
    
        
        // Initialize Boolean Flag for the VFX (Shader Effect's) Coroutine:
        //
        _isRunningShaderEffectFromVFXCoroutine = false;
        
        #region Option 1 and Option 2:  Calculate everything based on TOTAL TIME for the VFX.

        CalculateShaderVFXMainValueChangeRateAndTimeBetweenVFXUpdates();

        #endregion Option 1 and Option 2:  Calculate everything based on TOTAL TIME for the VFX.

        #endregion VFX Shader's: 'Main Value': Amount  and  Time Rates

    }// End Awake()


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
    /// The Shader Effect treated here can be one of two options: <br /> <br />
    /// 1- From:   Zero (0.0f) -> to -> One  (1.0f):  NORMAL DIRECTION = true <br />
    /// 2- From:   One  (1.0f) -> to -> Zero (0.0f).  NORMAL DIRECTION = false; it is a REVERSE. <br />
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator DoStartVFX()
    {
        
        // 0- ACTIONS TO execute:  Before VFX Starts
        //
        if (!_hasFinishedExecutionOfActionsBeforeVFXStarts)
        {
            DoExecuteOtherActionsBeforeShadersVFXStarts();
        }
        
        // 0.1- VFX's Values "Initialization":
        //
        InitializeVFXsShaderParameters();

        
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
        int arrayOfCachedMaterialsLength = _arrayOfCachedVFXMaterials.Length;
        //
        if ((arrayOfCachedMaterialsLength > 0) && (_arrayOfCachedVFXMaterials[0] != null))
        {

            // Initialize the '_shaderVFXMainValue' variable, to change the "Shader's ID_Property" parameter
            //
            _shaderVFXMainValue = _arrayOfCachedVFXMaterials[0].GetFloat( _shaderVFXMainPropertyToPlayWith );

            while ( CheckShaderValueCondition() )
            {

                // Increase the '_shaderVFXMainValue' parameter:
                //
                CalculateShaderVFXMainValueChangeRateAndTimeBetweenVFXUpdates();
                //
                _shaderVFXMainValue += (_shaderVFXMainValueChangeRate * _shaderVFXDirectionIncrease);

                // Assign the new '_shaderVFXMainValue' value:
                //
                for (int i = 0; i < arrayOfCachedMaterialsLength; i++)
                {
                    // Set the new value of '_shaderVFXMainValue' in the Shader
                    //
                    _arrayOfCachedVFXMaterials[i].SetFloat(_shaderVFXMainPropertyToPlayWith, _shaderVFXMainValue);


                    // (YIELD / PAUSE for a time)...  Return of this Coroutine
                    //
                    if (_useTotalTime > 0.0f)
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
        
        
        // Execute some one-time (rather one-frame) actions just when the Effect (VFX Shader's) ENDS.
        //
        DoExecuteOtherActionsAfterShadersVFXEnds();
        
    } // End DoStartVFX()


    /// <summary>
    /// Checks to see if the Shader's conditions are already met (so we would stop a Coroutine or Method Loop). <br /><br />
    /// For example:   <code>if ( _shaderPropertyValue is less than 1.0f )...</code>
    /// </summary>
    /// <returns></returns>
    protected virtual bool CheckShaderValueCondition()   // (float currentValue, float minLimitValue, float maxLimitValue, bool normalDirectionForShaderValueIncrease)
    {
        // Check that the current Shader value is within the established:  Limits
        //
        if ( _normalDirectionForShaderValueIncrease )
        {
            return (_shaderVFXMainValue < _maximumShaderVFXMainValueLimit);
        }
        else
        {
            return (_shaderVFXMainValue > _minimumShaderVFXMainValueLimit);
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
        
        // 0.1- VFX's Values "Initialization":
        //
        InitializeVFXsShaderParameters();

        
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

        // Validate and reassign to zero the Shader's  "Main Variable":
        //
        if ((_arrayOfCachedVFXMaterials.Length > 0) && (_arrayOfCachedVFXMaterials[0] != null))
        {

            // Re-Initialize the Shader's  cached "Main Variable":
            //
            _shaderVFXMainValue = _shaderVFXMainValueInitialization;


            // Assign that  cached "Main Variable"  to the Shader Property  (in each Material):
            //
            for (int i = 0; i < _arrayOfCachedVFXMaterials.Length; i++)
            {

                // Set the (cached "Main Variable")  Value in the Shader
                //
                _arrayOfCachedVFXMaterials[i].SetFloat(_shaderVFXMainPropertyToPlayWith, _shaderVFXMainValue);

            } //End for

        } //End if (_cachedSkinnedMeshRendererMaterials.Length > 0)
        else
        {
            successInThisMethod = false;
        }

        return successInThisMethod;

    }// End TryUndoVFX()


    #region Misc Methods
    
    /// <summary>
    /// Initializes the Shader's Values to work with, such as: <br />
    /// 1- DIRECTION (the sign: (+1) or (-1) will determine a SHADERS VALUE that INCREASES or DECREASES over time) <br />
    /// 2- MINIMUM and MAXIMUM Values for the Shader Value (generally MIN. = 0.0f  and  MAX.= 1.0f) <br /><br />
    /// </summary>
    /// <returns></returns>
    protected virtual void InitializeVFXsShaderParameters()
    {

        // Step 0:    Calculate the sign of the INCREASE / DECREASE of the Shader Effect:
        //
        InitializeVFXsShaderValueDirection();
        
        // Step 1:    Set the MINIMUM and MAXIMUM Values for the Shader Value (generally MIN. = 0.0f  and  MAX.= 1.0f):
        //
        InitializeVFXsShaderMinMaxAndInitValues();
        
    }// End InitializeVFXsShaderValueDirection
    
        
    /// <summary>
    /// Initializes the Shader's Value DIRECTION: the sign: (+1) or (-1) will determine a SHADERS VALUE that INCREASES or DECREASES over time: <br /><br />
    /// +  for NORMAL DIRECTION / INCREASE Shader's VFX value):  from MIN to MAX value. <br /><br />
    /// -  for INVERSE DIRECTION / DECREASE Shader's VFX value):  from MIN to MAX value. <br /><br />
    /// </summary>
    /// <returns></returns>
    protected virtual void InitializeVFXsShaderValueDirection()
    {

        // Step 0:    Calculate the sign of the INCREASE / DECREASE of the Shader Effect:
        //
        if (_normalDirectionForShaderValueIncrease)
        {
            _shaderVFXDirectionIncrease = 1;
        }
        else
        {
            _shaderVFXDirectionIncrease = -1;

        }// if (normalDirectionForShaderValueIncrease)

    }// End InitializeVFXsShaderValueDirection
    
    
    /// <summary>
    /// Initializes the Shader's Values to work with, such as: <br />
    /// 1- MINIMUM and MAXIMUM Values for the Shader Value (generally MIN. = 0.0f  and  MAX.= 1.0f) <br /><br />
    /// 2- INITIAL value and CURRENT value. <br /><br />
    /// NOTE: If the optional boolean flag is set '_smartAutomaticShaderValuesInitialization' = TRUE, then it will do an automatic Initialization the its corresponding MIN or MAX Initial Value.
    /// </summary>
    /// <returns></returns>
    protected virtual void InitializeVFXsShaderMinMaxAndInitValues()
    {

        #region LEGEND:   Values considered here

        // _shaderVFXMainValue = 1;
        // _shaderVFXMainValueInitialization = 1;
        // _minimumShaderVFXMainValueLimit = 1;
        // _maximumShaderVFXMainValueLimit = 1;

        #endregion LEGEND:   Values considered here
        

        // Validation and Correction:
        //
        // 1- LIMITS:   MIN  &  MAX
        //
        if (_minimumShaderVFXMainValueLimit >= _maximumShaderVFXMainValueLimit)
        {

            // Set  MIN and MAX:  to default values
            //
            _minimumShaderVFXMainValueLimit = _MINIMUM_DEFAULT_SHADER_VFX_MAIN_VALUE_LIMIT;
            _maximumShaderVFXMainValueLimit = _MAXIMUM_DEFAULT_SHADER_VFX_MAIN_VALUE_LIMIT;

        }//End if (_minimumShaderVFXMainValueLimit >= _maximumShaderVFXMainValueLimit)
        
        
        // 2-  INITIAL Values:   _shaderVFXMainValueInitialization  and  _shaderVFXMainValue
        //     (NOTE: if '_smartAutomaticShaderValuesInitialization = TRUE', then:  System will Initialize itself, auto-magically :)
        //
        if ( (_smartAutomaticShaderValuesInitialization)
             || (_shaderVFXMainValueInitialization > _maximumShaderVFXMainValueLimit) || (_shaderVFXMainValue > _maximumShaderVFXMainValueLimit) 
             || (_shaderVFXMainValueInitialization < _minimumShaderVFXMainValueLimit) || (_shaderVFXMainValue < _minimumShaderVFXMainValueLimit) )
        {

            // NORMAL: ++ INCREASING value
            //
            if (_normalDirectionForShaderValueIncrease)
            {

                // Wrong values (over the MAX)  or (below the MIN)
                // Correct, set to MINIMUM:
                //
                _shaderVFXMainValueInitialization = _shaderVFXMainValue = _minimumShaderVFXMainValueLimit;
            }
            else
            {
                // REVERSE: -- DECREASING value
                
                // Wrong values (over the MAX)  or (below the MIN)
                // Correct, set to MAXIMUM:
                //
                _shaderVFXMainValueInitialization = _shaderVFXMainValue = _maximumShaderVFXMainValueLimit;

            }//End else of if (_normalDirectionForShaderValueIncrease)


        }//End if ( (_smartAutomaticShaderValuesInitialization)...
        // else
        // {
        //     // Everything is CORRECT!
        // }//End else of if ( (_smartAutomaticShaderValuesInitialization)...

    }// End InitializeVFXsShaderMinMaxAndInitValues

    
    // During:   LOOP TIME  (Calculations):
    
    /// <summary>
    /// Option 1 or Option 2:  Calculate everything based on TOTAL TIME ( _useTotalTime ) for the VFX.
    /// </summary>
    protected virtual void CalculateShaderVFXMainValueChangeRateAndTimeBetweenVFXUpdates()
    {

        // Option 1:  Calculate everything based on TOTAL TIME for the VFX.
        //
        if (_useTotalTime > 0.0f)
        {
            
            // Fix Time between VFX small Changes:
            //
            _yieldDuringThisRefreshRateOrDeltaTimeOfEachFrame = Time.deltaTime;
                
            // [ _shaderVFXMainValue UPDATE only in This Frame = ?? = 
            // = ( timeDeltaTimeOfUpdate * (1.0)Total_shaderVFXMainValue ) / MY Total_shaderVFXMainValue_Time ]

            _shaderVFXMainValueChangeRate = _yieldDuringThisRefreshRateOrDeltaTimeOfEachFrame / _useTotalTime;
            
        } // End Option 1:  Calculate everything based on TOTAL TIME for the VFX.
        
        // NOTE:  Option 2 is happening with no further Calculations, just by using the initial values from the Inspector.
        // So it complies with the condition:   if (_useTotalTime <= 0.0f)
        
    }// End CalculateShaderVFXMainValueChangeRateAndTimeBetweenVFXUpdates

    
    #endregion Misc Methods

    
    #region Materials, Default Materials (swap) and VFX Materials

    #region Materials Initialization

    /// <summary>
    /// Gets a Material[] Array from Renderers.
    /// </summary>
    /// <param name="arrayOfSkinnedMeshRender">Materials[] source a)</param>
    /// <param name="arrayOfMeshRender">Materials[] source b)</param>
    /// <param name="arrayOfMaterialsFromSkinnedMeshRender">Materials[] extracted from (a)</param>
    /// <param name="arrayOfMaterialsFromMeshRender">Materials[] extracted from (b)</param>
    /// <param name="arrayOfAllCachedMaterials">All the Materials extracted in a list (rather an ARRAY[]) for using it in the VFX inside a while-loop and creating the VFX effect over all Materials at the same time,  in sync.</param>
	protected void GetMaterialsFromRenderers (ref SkinnedMeshRenderer[] arrayOfSkinnedMeshRender, ref MeshRenderer[] arrayOfMeshRender, ref Material[] arrayOfMaterialsFromSkinnedMeshRender, ref Material[] arrayOfMaterialsFromMeshRender)
	{

        // 1- SkinnedMeshRenderer[]
		// Get the Materials[] array from it:
		//
		if ((arrayOfSkinnedMeshRender != null) && (arrayOfSkinnedMeshRender.Length > 0) && (arrayOfSkinnedMeshRender[0] != null))
        {
            // Plan b:  Solution for casting all items (objects) in an array:  https://stackoverflow.com/questions/2068120/c-sharp-cast-entire-array
            // Plan A:
            // Casting quickly the SkinnedMeshRenderer[] array to Render[] array:
            //
            Renderer[] arrayOfRendererCastedFromSkinnedMeshRendererAuxiliary = arrayOfSkinnedMeshRender as Renderer[];
            //
            // Function call:
            //
        	GetMaterials( ref arrayOfRendererCastedFromSkinnedMeshRendererAuxiliary, ref arrayOfMaterialsFromSkinnedMeshRender );
        }
        //
		// 2- MeshRenderer[]
		// Get the Materials[] array from it:
		//
		if ((arrayOfMeshRender != null) && (arrayOfMeshRender.Length > 0) && (arrayOfMeshRender[0] != null))
        {
            // Plan b:  Solution for casting all items (objects) in an array:  https://stackoverflow.com/questions/2068120/c-sharp-cast-entire-array
            // Plan A:
            // Casting quickly the SkinnedMeshRenderer[] array to Render[] array:
            //
            Renderer[] arrayOfRendererCastedFromMeshRendererAuxiliary = arrayOfMeshRender as Renderer[];
            
			GetMaterials( ref arrayOfRendererCastedFromMeshRendererAuxiliary, ref arrayOfMaterialsFromMeshRender );
        }

	}//End GetMaterialsFromRenderers()


    //Utils:


    /// <summary>
    /// Sets a Main (cache) Material[] Array from input Arrays  (Notice: The array is overwritten).
    /// </summary>
    protected virtual void SetMainArrayOfMaterialsForVFX(ref Material[] arrayOfMaterialsFromSkinnedMeshRender, ref Material[] arrayOfMaterialsFromMeshRender, ref Material[] arrayOfAllCachedMaterials)
    {

        // 3- Grab all Materials into one Array []
        // 3.1- arrayOfMaterialsFromSkinnedMeshRender
        //
        AddMaterialsToArray( arrayOfMaterialsFromSkinnedMeshRender, ref arrayOfAllCachedMaterials );
        //
        // 3.2- arrayOfMaterialsFromMeshRender
        //
        AddMaterialsToArray( arrayOfMaterialsFromMeshRender, ref arrayOfAllCachedMaterials );
        
    }//End SetMainArrayOfMaterialsForVFX()

    
    /// <summary>
    /// Gets a Material[] Array from a Renderer and saves them into:  arrayOfMaterialsFromRender  (Notice: The array is overwritten).
    /// </summary>
    /// <param name="arrayOfRenders"></param>
    /// <param name="arrayOfMaterialsFromRender"></param>
	protected virtual void GetMaterials (ref Renderer[] arrayOfRenders, ref Material[] arrayOfMaterialsFromRender)
	{

		// Length of Arrays:
        //
        int arrayOfRendersLength = 0;
        int totalNumberOfMaterials = 0;
		//
        if ((arrayOfRenders != null) &&  (arrayOfRenders.Length > 0) && (arrayOfRenders[0] != null))
        {
            
            // 1- Get the correct Lenght of the Array:  Renderer[]
            //
            arrayOfRendersLength = arrayOfRenders.Length;
            
            // 2- 
            // Count the Materials in the Renders[] array:
            //
            totalNumberOfMaterials = CountTheMaterialsInAllRenderers( ref arrayOfRenders );
            

            // 3- Initialization of Array:  (not a good approach, as it could  come from the outside already initialized)
            //
            // Not to use: arrayOfMaterialsFromRender = new Material[ totalNumberOfMaterials ];
            //
            for (int i = 0; i < arrayOfRendersLength; i++)
            {

                // Get the (shared) Materials from the Renderer  +  Copy (them) to the Final Array:
                //
                AddMaterialsToArray( arrayOfRenders[ i ].sharedMaterials, ref arrayOfMaterialsFromRender );

            }//End for

        }//End if ((arrayOfRenders != null) ...

    }//End GetMaterials()


	protected virtual int CountTheMaterialsInAllRenderers( ref Renderer[] arrayOfRenders )
	{
        // Return value:
		//
		int totalMaterialsCount = 0;

        if ((arrayOfRenders != null) && (arrayOfRenders.Length > 0) && (arrayOfRenders[0] != null))
        {

            // Length of Array:
            //
            int arrayOfRendersLength = arrayOfRenders.Length;
            //
            // Loop through all Renders in the Array (arrayOfRenders), and count the: Materials in each Render...
            //
            for (int i = 0; i < arrayOfRendersLength; i++)
            {

                if (arrayOfRenders[ i ] != null)
                {
                    
                    // 1- Get the (shared) Materials from the Renderer
                    //
                    Material[] myMaterialArrayInRender = arrayOfRenders[ i ].sharedMaterials;
                    
                    // Validation
                    //
                    if ((myMaterialArrayInRender != null) && (myMaterialArrayInRender.Length > 0) && (myMaterialArrayInRender[0] != null))
                    {
                        
                        // 2- Count: the (shared) Materials in it ("totalMaterialsCount").
                        //
                        totalMaterialsCount += myMaterialArrayInRender.Length;
                    }
                }//End if ((arrayOfRenders[ i ] != null))
            }//End for
        }//End if ((arrayOfRenders != null)

        // Return the Number of (shared) Materials:
		//
		return totalMaterialsCount;

	}//End CountTheMaterialsInAllRenderers
    
    
    /// <summary>
    /// Add the Materials to the Array (incrementally, meaning: it adds to the previous elements/items the array already has).
    /// </summary>
    /// <param name="arrayOfNewMaterialsToAdd"></param>
    /// <param name="arrayOfAllCachedMaterials">Array to be Filled up with Items</param>
	protected virtual void AddMaterialsToArray(Material[] arrayOfNewMaterialsToAdd, ref Material[] arrayOfAllCachedMaterials)
	{
		
		// 1- Grab all Materials into one Array []
        // 1.1 - Length, auxiliary variables
        //
        int lengthOfArrayOfNewMaterialsToAdd = 0;
        int lengthOfArrayOfAllCachedMaterials = 0;
        //
        // Auxiliary (for not losing the previous Items inside: "arrayOfAllCachedMaterials")
        //
        Material[] auxiliaryArrayOfAllCachedMaterials = null;
        //
        // Validate:  the array to add:
        //
        if ((arrayOfNewMaterialsToAdd != null) && (arrayOfNewMaterialsToAdd.Length > 0) && (arrayOfNewMaterialsToAdd[0] != null))
        {
            
            lengthOfArrayOfNewMaterialsToAdd = arrayOfNewMaterialsToAdd.Length;
            
            
            // Validate:  the Array to receive the new Items:
            //
            if ((arrayOfAllCachedMaterials != null) && (arrayOfAllCachedMaterials.Length > 0) && (arrayOfAllCachedMaterials[0] != null))
            {
            
                lengthOfArrayOfAllCachedMaterials = arrayOfAllCachedMaterials.Length;
                
                
                // 1.2- Fill in the Array:
                //   2.2.1 - Create new Array and an auxiliary:
                //	 Auxiliary (for not losing the previous Items inside: "arrayOfAllCachedMaterials")
                //
                auxiliaryArrayOfAllCachedMaterials = new Material [ lengthOfArrayOfAllCachedMaterials ];
                //
                Array.Copy(arrayOfAllCachedMaterials, 0, auxiliaryArrayOfAllCachedMaterials , 0, lengthOfArrayOfAllCachedMaterials);
                
            }//End if ((arrayOfAllCachedMaterials != null)
            
           
            if (lengthOfArrayOfNewMaterialsToAdd > 0)
            {

                //   2.2.2 - Creating the (empty) Storage for:   the new Array[]
                //
                arrayOfAllCachedMaterials = new Material [lengthOfArrayOfNewMaterialsToAdd +
                                                          lengthOfArrayOfAllCachedMaterials];
                //
                //   1.2.2 - Fill in the Array
                //	 a) Reestablish the initial Items of the previous Array[] (if they exist):
                //
                if ((auxiliaryArrayOfAllCachedMaterials != null) && (auxiliaryArrayOfAllCachedMaterials.Length > 0) && (auxiliaryArrayOfAllCachedMaterials[0] != null))
                {
                    
                    // a) Copy the initial Items of the Array, again:
                    //
                    auxiliaryArrayOfAllCachedMaterials.CopyTo(arrayOfAllCachedMaterials, 0);
                }

                //   b) Copy the new Items into the Array[]
                //
                arrayOfNewMaterialsToAdd.CopyTo(arrayOfAllCachedMaterials, lengthOfArrayOfAllCachedMaterials);

            }//End if (lengthOfArrayOfNewMaterialsToAdd > 0)

        }//End if ((arrayOfNewMaterialsToAdd != null) 

	}//End AddMaterialsToArray()
    
    #endregion Materials Initialization
    
    #endregion Materials, Default Materials (swap) and VFX Materials
    
      
    #region Shader and VFX: Suplementary Actions
    
    #region Before the VFX starts
    
    /// <summary>
    /// Execute other action just: "Before" the VFX (Shader Effect) STARTS. <br />
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
    /// Execute other action just when the Shader Effect ENDS. <br />
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

        
        #region FINAL EXECUTIONS:  Execute Other actions when our Shaders Effect (VFX's) Ends:
        
        // FINAL EXECUTIONS:  Execute Other actions when our Shaders Effect (VFX's) Ends:
        //
        if (!_isRunningShaderEffectFromVFXCoroutine & !_hasFinishedExecutionOfActionsAfterVFXEnds)
        {

            #region a) Non - Coroutines  (based)  Executions:   Normal executions that can be applied as soon as the VFX Ends:
            
            // a) Non - Coroutines  (based)  Executions:   Normal executions that can be applied as soon as the VFX Ends:

            #region 0- REVERT Materials to Default    (after VFX Ends)
            
            // 0- REVERT Materials to Default    (after VFX Ends)

            //   0.1 - Validations:  SkinnedMeshRender case:
            //
            bool isValidMyArrayOfSkinnedMeshRender = ( (_myArrayOfSkinnedMeshRender != null) && (_myArrayOfSkinnedMeshRender.Length > 0) && (_myArrayOfSkinnedMeshRender[0] != null) );
            //
            bool isValidArrayOfNonVFXSkinnedMeshRender = ( (_arrayOfNonVFXSkinnedMeshRender != null) && (_arrayOfNonVFXSkinnedMeshRender.Length > 0) && (_arrayOfNonVFXSkinnedMeshRender[0] != null) );

            //    0.2 - Validations:  MeshRender case:
            //
            bool isValidMyArrayOfMeshRender = ( (_myArrayOfMeshRender != null) && (_myArrayOfMeshRender.Length > 0) && (_myArrayOfMeshRender[0] != null) );
            //
            bool isValidArrayOfNonVFXMeshRender = ( (_arrayOfNonVFXMeshRender != null) && (_arrayOfNonVFXMeshRender.Length > 0) && (_arrayOfNonVFXMeshRender[0] != null) );
            

            // 0- REVERT Materials to Default    (after VFX Ends)
            //
            if (_revertMaterialsToDefaultOnesAfterVFXEnds 
                && ( (isValidMyArrayOfSkinnedMeshRender && isValidArrayOfNonVFXSkinnedMeshRender) 
                     || (isValidMyArrayOfMeshRender && isValidArrayOfNonVFXMeshRender)) )
            {

                // Revert the VFX Materials in the 3D's (MeshRenderer or SkinnedMeshRenderer...)  Mesh.
                // For loop for each every {Skinned}MeshRenderer's Materials[] -> array...
                //
                // 1- Case:  SkinnedMeshRenderer
                //
                // Define auxiliary variables (arrays of Renderer), to handle {SkinnedMeshRender}
                //
                if (isValidMyArrayOfSkinnedMeshRender && isValidArrayOfNonVFXSkinnedMeshRender)
                {

                    Renderer[] myAuxArrayOfSkinnedMeshRender = _myArrayOfSkinnedMeshRender as Renderer[];
                    Renderer[] myAuxArrayOfNonVFXFakeSkinnedMeshRender = _arrayOfNonVFXSkinnedMeshRender as Renderer[];
                    //
                    RevertMaterialsToDefaultOnes(ref myAuxArrayOfSkinnedMeshRender, ref myAuxArrayOfNonVFXFakeSkinnedMeshRender);

                }//End if (isValidMyArrayOfSkinnedMeshRender && isValidArrayOfNonVFXSkinnedMeshRender)
                
                
                // 2- Case:  MeshRenderer
                //
                // Define auxiliary variables (arrays of Renderer), to handle {MeshRender}
                //
                if (isValidMyArrayOfMeshRender && isValidArrayOfNonVFXMeshRender)
                {

                    Renderer[] myAuxArrayOfMeshRender = _myArrayOfMeshRender as Renderer[];
                    Renderer[] myAuxArrayOfNonVFXFakeMeshRender = _arrayOfNonVFXMeshRender as Renderer[];
                    //
                    RevertMaterialsToDefaultOnes(ref myAuxArrayOfMeshRender, ref myAuxArrayOfNonVFXFakeMeshRender);
    
                }//End if (isValidMyArrayOfMeshRender && isValidArrayOfNonVFXMeshRender)
                
            }//End:  0- REVERT Materials to Default    (after VFX Ends)
         
            #endregion 0- REVERT Materials to Default    (after VFX Ends)

            #endregion a) Non - Coroutines  (based)  Executions:   Normal executions that can be applied as soon as the VFX Ends:

            
            #region b) Coroutines  (based)  Executions:
            
            // b) Coroutines  (based)  Executions:
            //
            if (isThisScriptExecutingFinalActionsAsACoroutine)
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

            }//End if (isThisScriptExecutingFinalActionsAsACoroutine)
            else
            {
                // No Coroutines were used,  normal execution of "FINAL ACTIONS" within this Time-Frame:
                //
                FinalActionsToDisableOrDestroyThisScriptAndParentGameObject();

            }//End else of  if (isThisScriptExecutingFinalActionsAsACoroutine)

            #endregion b) Coroutines  (based)  Executions:
            
        }//End if (!_isRunningShaderEffectFromVFXCoroutine & !_hasFinishedExecutionOfActionsAfterVFXEnds)

        #endregion FINAL EXECUTIONS:  Execute Other actions when our Shaders Effect (VFX's) Ends:
        
    }// End DoExecuteOtherActionsAfterShadersVFXEnds
    
    #endregion After the VFX ends
    
    
    #region 0- REVERTING Materials to Default ones
    
    /// <summary>
    /// Reverts the 'VFX Materials' to Default ones. <br /><br />
    /// 
    /// Only Call this function after having validated that, (at least):  the user has set up Materials to Revert to.
    /// </summary>
    /// <param name="myArrayOfRenderer"></param>
    /// <param name="myArrayOfFakeRenderWithDefaultMaterialsToSet"></param>
    protected virtual void RevertMaterialsToDefaultOnes(ref Renderer[] myArrayOfRenderer, ref Renderer[] myArrayOfFakeRenderWithDefaultMaterialsToSet)
    {
        // Validation Flags
        //
        #region 0- Validations

        bool isValidMyArrayOfRenderer = myArrayOfRenderer is {Length: > 0} && (myArrayOfRenderer[0] != null);

        bool isValidMyArrayOfFakeRenderWithDefaultMaterialsToSet = myArrayOfFakeRenderWithDefaultMaterialsToSet is {Length: > 0} && (myArrayOfFakeRenderWithDefaultMaterialsToSet[0] != null) && (myArrayOfRenderer.Length <= myArrayOfFakeRenderWithDefaultMaterialsToSet.Length);

        #endregion 0- Validations
        
        
        #region 1- Revert the Materials to Default ones:
        
        // b) Apply Validations + revert the Materials.
        //
        int myArrayOfRendererLength = myArrayOfRenderer.Length; 
        //
        if (isValidMyArrayOfRenderer && isValidMyArrayOfFakeRenderWithDefaultMaterialsToSet)
        {
            
            for (int i = 0; i < myArrayOfRendererLength; i++)
            {
                
                // Replace the {Skinned{Mesh}}Renderer's  Materials to Default:
                //
                myArrayOfRenderer[i].sharedMaterials = myArrayOfFakeRenderWithDefaultMaterialsToSet[i].sharedMaterials;

            }//End For
            
            // Comment, For Debug Purposes Only:
            //
            Debug.LogWarning($"Executing:... Replace (mySkinnedMeshRenderer) Materials in: this Object:{this.gameObject.name}", this);
        }
        else
        {
            // Log an Error:
            //
            Debug.LogError( $"{this.name}: It is impossible to revert the Materials to Default ones because they were not set up correctly, previously\n (Default Ones, + [SkinnedMeshRenderer, or MeshRenderer]'s reference this Script) | in: this Object:{this.gameObject.name}", this);

        }//End else of if (mySkinnedMeshRenderer != null)
        
        #endregion 1- Revert the Materials to Default ones:
        
    }// End RevertMaterialsToDefaultOnes
    

    #region Deprecated
    
    // OLD FUNCTION, DEPRECATED
    
    /// <summary>
    /// Reverts the 'VFX Materials' to Default ones. <br /><br />
    /// 
    /// Only Call this function after having validated that, at least:  the user set up Materials to Revert to. 
    /// </summary>
    /// <param name="mySkinnedMeshRenderer"></param>
    /// <param name="myMeshRenderer"></param>
    /// <param name="arrayOfDefaultMaterialsToSet"></param>
    [Obsolete("This method is deprecated. Use: 'RevertMaterialsToDefaultOnes(ref Renderer[] myArrayOfSkinnedMeshRenderer, ref Material[] myArrayOfDefaultMaterialsToSet)' instead", true)]
    protected virtual void RevertMaterialsToDefaultOnes(ref SkinnedMeshRenderer mySkinnedMeshRenderer, ref MeshRenderer myMeshRenderer, ref Material[] arrayOfDefaultMaterialsToSet)
    {
        // Validation Flags
        //
        bool isValidArrayOfDefaultMaterialsToSet = false;
        bool isACaseOfSkinnedMeshRenderer = false;
        bool isACaseOfMeshRenderer = false;


        #region 0- Validations
        
        // a) Some validations, setting up the boolean flags:
        //
        if (arrayOfDefaultMaterialsToSet != null)
        {

            // Verify at least the first item:  so the List (of Materials) will be usable:
            //
            if (arrayOfDefaultMaterialsToSet[0] != null)
            {
                isValidArrayOfDefaultMaterialsToSet = true;

            }//End if (arrayOfDefaultMaterialsToSet[0] != null)
            
            // 1 of 2:  SkinnedMeshRenderer
            //
            if ((mySkinnedMeshRenderer != null)  && ( (mySkinnedMeshRenderer.sharedMaterials.Length > 0) && (mySkinnedMeshRenderer.sharedMaterials[0] != null)) )
            {

                isACaseOfSkinnedMeshRenderer = true;

            }//End if (mySkinnedMeshRenderer != null)
            
            // 2 of 2:  MeshRenderer
            //
            if ((myMeshRenderer != null)  && ( (myMeshRenderer.sharedMaterials.Length > 0) && (myMeshRenderer.sharedMaterials[0] != null)) )
            {

                isACaseOfMeshRenderer = true;

            }//End if (myMeshRenderer != null)

        }// Some validations
        
        #endregion 0- Validations
        
        #region 1- Revert the Materials to Default ones:
        
        // b) Apply Validations + revert the Materials.
        // Determine:  is this a case of  "SkinnedMeshRenderer" or "MeshRenderer" (Materials) ?
        //
        if (isValidArrayOfDefaultMaterialsToSet && ( isACaseOfSkinnedMeshRenderer || isACaseOfMeshRenderer) )
        {
            
            if (isACaseOfSkinnedMeshRenderer)
            {
                // Replace the SkinnedMeshRenderer's  Materials to Default:
                //
                mySkinnedMeshRenderer.sharedMaterials  /*materials*/ = arrayOfDefaultMaterialsToSet;
            
                Debug.LogWarning($"Executing:... Replace (mySkinnedMeshRenderer) Materials for:   isACaseOfSkinnedMeshRenderer");

            }
            else // isACaseOfMeshRenderer
            {
                // Replace the MeshRenderer's  Materials to Default:
                //
                myMeshRenderer.sharedMaterials /*materials*/ = arrayOfDefaultMaterialsToSet;
            
                Debug.LogWarning($"Executing:... Replace (myMeshRenderer) Materials for:   isACaseOfMeshRenderer");
                
            }//End else of  if (isACaseOfSkinnedMeshRenderer)

        }
        else
        {
            // Log an Error:
            //
            Debug.LogError( $"{this.name}: It is impossible to revert the Materials to Default ones because they were not set up correctly, previously\n (Default Ones, + [SkinnedMeshRenderer, or MeshRenderer]'s reference this Script) | in: this Object:{this.gameObject.name}", this);

        }//End else of if (mySkinnedMeshRenderer != null)
        
        #endregion 1- Revert the Materials to Default ones:
        
    }// End RevertMaterialsToDefaultOnes

    #endregion Deprecated

    
    #endregion 0- REVERTING Materials to Default ones
    
    
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


    /// <summary>
    /// Disables + Destroys:  This Script + GameObject it is attached to. <br /> <br />
    /// 
    /// Final Actions after:  VFX ENDS + ALL COROUTINES end + everything ends.
    /// </summary>
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
    #endregion Shader and VFX: Suplementary Actions
    
    
    #endregion My Custom Methods

}

