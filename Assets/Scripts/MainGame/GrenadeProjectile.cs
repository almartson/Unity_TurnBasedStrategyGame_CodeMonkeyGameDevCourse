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

    #region Physics Misc - Hit Colliders, etc
    
    [Tooltip("Target Position \n Target-destination of this process.")]
    private Vector3 _targetPosition;
    
    [Tooltip("Total Distance \n Distance: 'Projectile <--vs.--> Target'.")]
    private float _totalDistance;
    
    [Tooltip("Position XZ \n Position of this Projectile on the plane XZ.")]
    private Vector3 _positionXZ;

    [Tooltip("[ Use = 4f ] Factor to divide by the Maximum Height Of Projectile Trail Curve \n Used for the 'Trail' of the Projectile.")]
    [SerializeField]
    [Range(1.0f, 10.0f)]
    private float _factorToDivideByMaxHeightOfProjectileTrailCurve = 4.0f;


    [Tooltip("Move Speed \n Speed of the Projectile.")]
    [SerializeField]
    private float _moveSpeed = 15.0f;

    [Tooltip("Damage Radius \n Radius (Area) of Explosion, of the Projectile.")]
    [SerializeField]
    private float _damageRadius = 4.0f;

    [Tooltip("Grenade Action Damage \n Damage dealt by the Explosion, of the Projectile.")]
    [SerializeField]
    private int _grenadeActionDamage = 44;
    
    [Tooltip("Max Number Of Physics Colliders \n Maximum Number of Physics Colliders used in calculations (for: Damage dealt by the Explosion, of the Projectile; for instance")]
    [SerializeField]
    [Range(1, 20)]
    private int _maxNumberOfPhysicsColliders = 17;
    
    

    #endregion Physics Misc - Hit Colliders, etc

    
    #region VFX, Visuals (Shader, Material, etc)
    
    [Tooltip("Trail Renderer \n Visual 'Trail' that this Projectile will leave behind when flying throuh the air.")]
    [SerializeField]
    private TrailRenderer _trailRenderer;

    [Tooltip("Arc Y Animation Curve \n Animation Curve for the Parabolic Movement (according to physics) of the Projectile.")]
    [SerializeField]
    private AnimationCurve _arcYAnimationCurve;


    
    [Tooltip("[_grenadeExplodeVfxPrefab] 'Transform', of the VFX, of the Projectile.")] [SerializeField]
    private Transform _grenadeExplodeVfxPrefab;

    #endregion VFX, Visuals (Shader, Material, etc)

    
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
        Vector3 moveDirection = (_targetPosition - _positionXZ).normalized;
        
        _positionXZ += moveDirection * (_moveSpeed * Time.deltaTime);

        // Calculate the CURRENT Distance (to the Target):
        //
        float distance = Vector3.Distance(_positionXZ, _targetPosition);
        float distanceNormalized = 1 - distance / _totalDistance;
        
        // Apply the ANIMATION CURVE Physics to shape the Curve of the Trail:
        //
        float maxHeightAnimationCurve = distance / _factorToDivideByMaxHeightOfProjectileTrailCurve;
        //
        float positionY = _arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeightAnimationCurve;
        
        // Position (please PLACE...) the Projectile: according to the ANIMATION CURVE
        //
        transform.position = new Vector3(_positionXZ.x, positionY, _positionXZ.z);
        
        // Tolerance value for the calculations: Distance Projectile vs. Target (coming to zero) 
        float reachedTargetDistance = 0.2f;

        if (Vector3.Distance(_positionXZ, _targetPosition) < reachedTargetDistance)
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
                
                // Destructible props: Crates, etc
                //
                if (colliderArray[i].TryGetComponent<DestructibleCrate>(out DestructibleCrate destructibleCrate))
                {
                    
                    // Damage
                    //
                    destructibleCrate.Damage();
                    
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
            
            // "Destroy" the Trail Rendered in the air:
            //
            _trailRenderer.transform.parent = null;
            
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

        // Position of this Projectile on the "XZ Plane" (the Ground / Floor):
        //
        _positionXZ = transform.position;
        _positionXZ.y = 0;
        
        // Total Distance (to the Target):
        //
        _totalDistance = Vector3.Distance(_positionXZ, _targetPosition);

    }//End Setup


    #region Getters, Setters

    public int GetGrenadeActionDamage()
    {
        return _grenadeActionDamage;
    }
    
    /// <summary>
    /// Sets the value of the "Grenade Action Damage" property.
    /// </summary>
    /// <param name="grenadeActionDamage"></param>
    public void SetGrenadeActionDamage(int grenadeActionDamage)
    {
        this._grenadeActionDamage = grenadeActionDamage;
    }


    #endregion Getters, Setters

    #endregion My Custom Methods

}
