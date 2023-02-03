/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/

using System;
using UnityEngine;


public class UnitRagdollSpawner : MonoBehaviour
{

    #region Attributes

    [Tooltip("Reference to: A Ragdoll Prefab to be instantiated in-place of an Unit, to simulate the Physics of it 'falling down to the ground' after dying.")]
    [SerializeField]
    private Transform _ragdollPrefab;

    [Tooltip("(Original) Character's ->Root<- (Bone) reference: it will be used to copy from them all the original Character's Bones 'Transforms' values (position, rotation, scale) to make the Ragdoll match the Character's Position right at the moment of its death event.../n/n This makes the Ragdoll's Animation consistent with the Pose the Unit/Character had a frame before spawning.")]
    [SerializeField]
    private Transform _originalCharactersRootBone;
    
    
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
        unitRagdoll.Setup(_originalCharactersRootBone, gameObject.GetComponent<UnitAnimator>().MoveDirectionOfBulletProjectileThatJustHitMe);

    }//End HealthSystem_OnDead


    #endregion My Custom Methods

}
