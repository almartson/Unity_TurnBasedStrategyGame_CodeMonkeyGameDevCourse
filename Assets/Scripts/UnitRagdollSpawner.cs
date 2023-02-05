/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using System;
using UnityEngine;

/// <summary>
/// This Class handles the Spawning of a RAGDOLL PREFAB, (associated to a Character / Unit), in the place-Transform-Position-Rotation-etc of an Unit, when it dies, to make an Animations of 'Falling down / drop death'.
/// </summary>
public class UnitRagdollSpawner : MonoBehaviour
{

    #region Attributes

    [Tooltip("Reference to: A Ragdoll Prefab to be instantiated in-place of an Unit, to simulate the Physics of it 'falling down to the ground' after dying.")]
    [SerializeField]
    private Transform _ragdollPrefab;

    
    #region Character and Ragdoll's Bones
    
    [Tooltip("(Original) Character's ->Root<- (Bone) reference: it will be used to copy from them all the original Character's Bones 'Transforms' values (position, rotation, scale) to make the Ragdoll match the Character's Position right at the moment of its death event.../n/n This makes the Ragdoll's Animation consistent with the Pose the Unit/Character had a frame before spawning.")]
    [SerializeField]
    private Transform _originalCharactersRootBone;
    
    
    #region RAGDOLL's Bone Transforms
    
    [Tooltip("(Original) Character reference to its Bones that are part of the Ragdoll's Hierarchy. These Bones' (List) Transform will be copied to: the RAGDOLL's Bones List, for using them when Spawning it in the Place of the Original Unit, to play the Animation of 'dropping death'.")]
    [SerializeField]
    private Transform[] _originalCharacterBonesThatAreRagdollized;
    //
    /// <summary>
    /// Property Accessor to Private Field "_originalCharacterBonesThatAreRagdollized".
    /// </summary>
    public Transform[] OriginalCharacterBonesThatAreRagdollized { get => _originalCharacterBonesThatAreRagdollized; private set => _originalCharacterBonesThatAreRagdollized = value; }
    
    
    // NOTE:  This one is on the   UnitRagdoll.cs   SCRIPT.
    //
    // [Tooltip("(Clone / RAGDOLL's) Ragdoll reference to its Bones that are part of the 3D Character (i.e.: The Unit) Hierarchy. These Bones' (List) Transform's will be substituted by: the Unit's (Original Character's) Bones List, for using them when Spawning it in the Place of the Original Unit, to play the Animation of 'dropping death'.")]
    // [SerializeField]
    // private Transform[] _ragdollPrefabsCharacterBonesAreRagdollized;
    //
    #endregion RAGDOLL's Bone Transforms
    
    
    #endregion Character and Ragdoll's Bones
    
    
    [Tooltip("Reference to: The Unit's 'Health System', for keeping control of the event of Dying... (i.e.: when _health == 0)")]
    private HealthSystem _healthSystem;
    
    
    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>
    private void Awake()
    {
        // Get a ref to the 'Health System' Component
        // ..(from the GameObject this script in attached to...)
        //
        _healthSystem = GetComponent<HealthSystem>();
        
        
        // Listen (Subscribe...) to the EVENT:  When the Unit Dies...
        //
        _healthSystem.OnDead += HealthSystem_OnDead;

    }//End Awake()


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>



    /// <summary>
    /// Update is called once per frame
    /// </summary>


    #endregion Unity Methods
    

    #region My Custom Methods

    /// <summary>
    /// Event / CallBack that occurs when <code>_health == 0</code>...
    /// ..in the Unit's 'Health System'. <br />
    /// That means: The Unit just DIED NOW. <br />
    /// Then:  Display the Ragdoll nice physics reaction.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        // 1.1- Spawn the RAGDOLL there & save its Transform:
        //
        Transform ragdollTransform = Instantiate(_ragdollPrefab, transform.position, transform.rotation);
        
        //   1.2- Get the UnitRagdoll.cs Class Script from the 'ragdollTransform':
        //
        UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
        
        
        // 2- Call the UnitRagdoll.cs script's Setup()  function (to set the Transforms of each & every Bone...)
        //
        //   2.1- ORIGINAL, CODEMONKEYS':  Just in case the Bones REFERENCE here are NULL (i.e.: not set by the Designer)
        //
        if ( ((_originalCharacterBonesThatAreRagdollized != null) && 
              (_originalCharacterBonesThatAreRagdollized.Length > 0)) &&
            ((unitRagdoll.RagdollPrefabsCharacterBonesAreRagdollized != null) &&
              (unitRagdoll.RagdollPrefabsCharacterBonesAreRagdollized.Length > 0) ) )
        {
            //   2.2- OPTIMIZED Version (AlMartson's)
            //  By the Designer need to previously link the Bones in the Unit and Ragdoll Prefabs: (in the UnitRagDollSpawner.cs  Script...)
            // // Todo: remove the debug.log
            // //
            // Debug.Log("It is about to execute: SetupOptimized(...), Script: " + this.GetType().Name);
            //
            unitRagdoll.SetupOptimized( _originalCharacterBonesThatAreRagdollized, unitRagdoll.RagdollPrefabsCharacterBonesAreRagdollized, _originalCharactersRootBone, gameObject.GetComponent<UnitAnimator>().MoveDirectionOfBulletProjectileThatJustHitMe );
   
        }
        else
        {
            //   2.1- Set the Transforms of each & every Bone, of the RAGDOLL:
            //
            // // Todo: remove the debug.log
            // //
            // Debug.Log("It is about to execute the NON-OPTIMIZED form of: Setup(...), Script: " + this.GetType().Name);
            //
            unitRagdoll.Setup(_originalCharactersRootBone, gameObject.GetComponent<UnitAnimator>().MoveDirectionOfBulletProjectileThatJustHitMe);
                
        }//End if ((_originalCharacterBonesThatAreRagdollized == null)...
        
    }//End HealthSystem_OnDead


    #endregion My Custom Methods

}
