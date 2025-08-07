/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/

using System;
using UnityEngine;


public class GrenadeProjectile : MonoBehaviour
{

    #region Attributes
    
    #region Events

    public static event EventHandler OnAnyGrenadeExploded;
    
    private Action _onGrenadeBehaviourComplete;
    
    #endregion Events
    
    
    [Tooltip("[_targetPosition] Target-destination of this process.")]
    private Vector3 _targetPosition;

    [Tooltip("[_moveSpeed] Speed of the Projectile.")]
    [SerializeField]
    private float _moveSpeed = 15.0f;

    [Tooltip("[_damageRadius] Radius (Area) of Explosion, of the Projectile.")]
    [SerializeField]
    private float _damageRadius = 4.0f;

    [Tooltip("[_grenadeActionDamage] Damage dealt by the Explosion, of the Projectile.")]
    [SerializeField]
    private int _grenadeActionDamage = 36;

    #region Physics Misc - Hit Colliders, etc
    
    [Tooltip("[_maxNumberOfPhysicsColliders] Maximum Number of Physics Colliders used in calculations (for: Damage dealt by the Explosion, of the Projectile; for instance")]
    [SerializeField]
    [Range(1, 20)]
    private int _maxNumberOfPhysicsColliders = 17;

    #endregion Physics Misc - Hit Colliders, etc

    [Tooltip("[_grenadeExplodeVfxPrefab] 'Transform', of the VFX, of the Projectile.")] [SerializeField]
    private Transform _grenadeExplodeVfxPrefab;

    
    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>



    /// <summary>
    /// Start is called before the first frame update
    /// </summary>



    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        // Projectile Movement - update -
        //
        Vector3 moveDirection = (_targetPosition - transform.position).normalized;
        
        transform.position += moveDirection * (_moveSpeed * Time.deltaTime);

        // Tolerance value for the calculations: Distance Projectile vs. Target (coming to zero) 
        float reachedTargetDistance = 0.2f;

        if (Vector3.Distance(transform.position, _targetPosition) < reachedTargetDistance)
        {
            // (Do a Physics check...) Check to see what Targets it kills / destroys:
            
            #region (My Way - Optimized) Using Physics.OverlapSphereNonAlloc()

            Collider[] colliderArray = new Collider[_maxNumberOfPhysicsColliders];
            int numColliders = Physics.OverlapSphereNonAlloc(_targetPosition, _damageRadius, colliderArray);
            
            for (int i = 0; i < numColliders; i++)
            {
                
                // if there's Unit Component: :=> Apply Damage to that "Unit".
                //
                if (colliderArray[i].TryGetComponent<Unit>(out Unit targetUnit))
                {
                    
                    // Damage
                    //
                    targetUnit.Damage(_grenadeActionDamage);
                    
                }//End if (colliderArray[i].TryGetComponent<Unit>
                
            }//End for

            #endregion (My Way - Optimized) Using Physics.OverlapSphereNonAlloc()

            
            #region (Deprecated) CodeMonkey way using Physics.OverlapSphere() generating some Garbage
            
            // Collider[] colliderArray = Physics.OverlapSphere(_targetPosition, _damageRadius);
            //
            // // Cycle through the colliderArray and apply "Damage()" to every Collider found within range, in there;
            // //
            // foreach (Collider collider in colliderArray)
            // {
            //     // if there's Unit Component: :=> Apply Damage to that "Unit".
            //     //
            //     if (collider.TryGetComponent<Unit>(out Unit targetUnit))
            //     {
            //         // Generalize this var as a Field attribute
            //         //
            //         int grenadeActionDamage = 36;
            //         
            //         // Damage
            //         //
            //         targetUnit.Damage(grenadeActionDamage);
            //         
            //     }//End if (collider.TryGetComponent<Unit>...
            // }//End foreach (Collider collider in...

            #endregion (Deprecated) CodeMonkey way using Physics.OverlapSphere() generating some Garbage

            // Destroy / Remove the Projectile from the scene:
            // Send a static event that marks the end of this process:
            //
            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);
            
            // Spawn the "Explosion VFX": Instantiate its Prefab:
            //
            Instantiate(_grenadeExplodeVfxPrefab, _targetPosition + Vector3.up * 1f, Quaternion.identity);
            
            // Remove the Projectile......when it reaches its Target.
            //
            Destroy(gameObject);
            
            // Call the Callback (event): After destroying the GameObject:
            //
            _onGrenadeBehaviourComplete();

        }//End if (Vector3.Distance...

    }//End Update

    #endregion Unity Methods


    #region My Custom Methods

    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        // Save the Event:
        //
        this._onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        
        _targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

    }//End Setup



    #endregion My Custom Methods

}
