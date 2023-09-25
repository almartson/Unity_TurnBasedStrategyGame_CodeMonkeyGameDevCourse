/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using UnityEngine;


public class UnitRagdollDissolvingController : BaseDissolvingController
{

    #region Attributes
    
    #region Dissolve VFX: Suplementary Actions
    
    #region Detachment from the VFX
    
    // #region Before the VFX starts
    //
    // [Tooltip("[Before the VFX starts] Array of GameObjects that will be detached (and left on the Scene, untouched) from its Parent GameObject (i.e.: the one which will be 'Dissolved' with the VFX).\n\n Example: Items such as: Guns, Rifles, Magic Wands, Hats, etc..")]
    // [SerializeField]
    // private GameObject[] _arrayOfGameObjectsToDetachFromParentVFXsGameObjectBeforeVFXStarts;
    //
    // [Tooltip("[Before the VFX starts] Array of 3D Colliders that will be 'Disabled' just before the VFX starts; so it won't affect them and the Physics will get disabled on them.\n\n * Example: Items such as: Guns, Rifles, Magic Wands, Hats, etc..")]
    // [SerializeField]
    // private Collider[] _arrayOfCollidersToDisableBeforeVFXStarts;
    //
    // #endregion Before the VFX starts
    
    
    #region After the VFX ends
    
    [Tooltip("[After the VFX ends] Array of GameObjects that will be detached (and left on the Scene, untouched) from its Parent GameObject (i.e.: the one which will be 'Dissolved' with the VFX).\n\n * Example: Items such as: Guns, Rifles, Magic Wands, Hats, etc..")]
    [SerializeField]
    private GameObject[] _arrayOfGameObjectsToDetachFromParentVFXsGameObjectAfterVFXEnds;

    [Tooltip("[After the VFX ends] Array of 3D Colliders that will be 'Disabled' right after the VFX ends; so it won't affect them and the Physics will get disabled on them.\n\n * Example: Items such as: Guns, Rifles, Magic Wands, Hats, etc..")]
    [SerializeField]
    private Collider[] _arrayOfCollidersToDisableAfterVFXEnds;

    #endregion After the VFX ends
    
    #endregion Detachment from the VFX

    #endregion Dissolve VFX: Suplementary Actions

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

            // #region Before the VFX starts
            //
            // // Disable Colliders before VFX Starts
            // //
            // DisableColliders(_arrayOfCollidersToDisableBeforeVFXStarts);
            //
            // // Detach GameObjects from their Parents:
            // //
            // DetachAllGameObjectsFromTheirParents(_arrayOfGameObjectsToDetachFromParentVFXsGameObjectBeforeVFXStarts);
            //
            // #endregion Before the VFX starts
            
            
            // 2- Execute the VFX:
            //
            // Start the VFX as a Coroutine
            //
            StartCoroutine(DoStartVFX());

        }//End if (TryUndoVFX())

    }// End Start()


    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        
        // Execute other actions just when the Dissolve effect (VFX's Shader) ENDS.
        //
        DoExecuteOtherActionsAfterShadersVFXEnds();

    }// End Update()

    #endregion Unity Methods
    

    #region My Custom Methods
    
    #region Dissolve VFX: Suplementary Actions

    #region After the VFX ends
    
    /// <summary>
    /// Execute other action just when the Dissolve effect (VFX's Shader) ENDS.
    /// </summary>
    private void DoExecuteOtherActionsAfterShadersVFXEnds()
    {
        
        // Execute Other actions when our Shaders Effect (VFX's) Ends:
        //
        if (!_isRunningVFXCoroutine)
        {
            
            #region After the VFX ends
            
            // Disable Colliders after VFX Ends
            //
            DisableColliders(_arrayOfCollidersToDisableAfterVFXEnds);
            
            // Detach GameObjects from their Parents:
            //
            DetachAllGameObjectsFromTheirParents(_arrayOfGameObjectsToDetachFromParentVFXsGameObjectAfterVFXEnds);
            
            // Disable the Ragdolls (i.e.: the Character's) main Parent GameObject
            //
            
            // Destroy it (the Ragdolls (i.e.: the Character's)... even this script...)
            //
            
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

                // Detaches the transform from its parent.
                //
                arrayOfGameObjectsToDetachFromParent[i].transform.parent = null;

            }//End for
            
        }//End if ( arrayOfColliders != null )
        
    }// End DetachAllGameObjectsFromTheirParents
    
    #endregion Detachment from the VFX
    #endregion Dissolve VFX: Suplementary Actions

    

    #endregion My Custom Methods

}
