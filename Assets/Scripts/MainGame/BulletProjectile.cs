/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using UnityEngine;

/// <summary>
/// Bullet Projectile's GameObject Logic. <br />
/// This should be attached to a BulletProjectile Prefab, and instantiated when it is been Fired. <br />
/// </summary>
public class BulletProjectile : MonoBehaviour
{
    #region Attributes

    /// <summary>
    /// Bullet's Target (Location: (x, y, z) ).
    /// </summary>
    private Vector3 _targetPosition;

    [Tooltip("Bullet's Speed")]
    [SerializeField]
    [Range(1.0f, 1000.0f)]
    private float _moveSpeed = 200.0f;

    
    #region Particle VFXs and Trail Renderer

    [Tooltip("Trail Renderer, a Particle Effect that is drawn behind the Bullet.")]
    [SerializeField]
    private TrailRenderer _trailRenderer;

    [Tooltip("VFX PArticle System, (multiple yellow sparks) that is drawn and lives after the Bullet HITS a Target.")]
    [SerializeField]
    private Transform _bulletHitVfxPrefab;

    #endregion Particle VFXs and Trail Renderer
    
    
    #region Utils
    
    [Tooltip("The Tolerance number to accept that a value is = ZERO")]
    [SerializeField]
    private float _stoppingDistance = 0.1f;
    
    /// <summary>
    /// The square value of: <code>_stoppingDistance</code>.
    /// </summary>
    private float _sqrStoppingDistance = 0.1f;
    
    /// <summary>
    /// Direction of the Flying Bullet, when it's shot.
    /// </summary>
    [Tooltip("Direction of the Flying Bullet, when it's shot.")]
    //[ReadOnlyInspector]
    [SerializeField]
    private Vector3 _moveDirection = Vector3.zero;
    //
    /// <summary>
    /// Property Accessor to Private Field '_moveDirection'.
    /// </summary>
    public Vector3 MoveDirection { get => _moveDirection ; private set => _moveDirection = value; }
    
    
    /// <summary>
    /// Cached Transform of the Flying Bullet, at any moment.
    /// </summary>
    [Tooltip("Cached Transform of the Flying Bullet, at any moment.")]
    private Transform _cachedTransform;

    #endregion Utils
    
    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>
    private void Awake()
    {
        #region Utils
        
        // Done: Misc Optimization: Calculating the (accepted) Square Min Distance.
        //
        _sqrStoppingDistance = _stoppingDistance * _stoppingDistance;
        
        #endregion Utils
        
        #region Initializing the Bullet
        //
        // Simple Logic to:
        // 1- Move (the BULLET...) towards the:  TargetPosition.
        // .1- Direction (Vector3)
        //   .1.1- Cache (for performance optimization): transform
        //
        _cachedTransform = transform;
        // //
        // // .1- Direction   (of the Bullet-Projectile while its FLYING):
        // //
        // _moveDirection = (_targetPosition - _cachedTransform.position).normalized;
        //
        #endregion Initializing the Bullet

    }//End Awake()



    /// <summary>
    /// Start is called before the first frame update
    /// </summary>



    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        #region Initializing the Bullet
        
        // Simple Logic to:
        // 1- Move (the BULLET...) towards the:  TargetPosition.
        // .1- Direction (Vector3)
        //   .1.1- Cache (for performance optimization): transform.position
        // :
        //   .1.1- Cache (for performance optimization): transform.position
        //
        // (This is done in the Awake:)  _cachedTransform = transform.position;
        // Another cache, valid only during the scope of each the Update() for: each frame (changes with each frame):
        //
        Vector3 position = _cachedTransform.position;
        //
        // .1- Direction   (of the Bullet-Projectile while its FLYING):
        //
        _moveDirection = (_targetPosition - position).normalized;

        
        #endregion Initializing the Bullet
        
        
        // To avoid Overshooting the Target:
        // Compare DISTANCE before moving with AFTER moving...
        // Distance BEFORE Moving:
        //
        // CodeMonkey Original:     float distanceBeforeMoving = Vector3.Distance(position, _targetPosition);
        // AlMartson Optimization Fix:
        //
        float sqrDistanceBeforeMoving = Vector3.SqrMagnitude(position - _targetPosition);
        
        // .2- MOVE: Update->Increase the Position (i.e.: Move the Bullet):
        //
        position += _moveDirection * (_moveSpeed * Time.deltaTime);
        _cachedTransform.position = position;

        // Distance AFTER Moving:
        //
        // CodeMonkey Original:     float distanceAfterMoving = Vector3.Distance(position, _targetPosition);
        // AlMartson Optimization Fix:
        //
        float sqrDistanceAfterMoving = Vector3.SqrMagnitude(position - _targetPosition);
        
        // Debug TIP: With fast paced moving objects it is very common to
        //..overshoot or overpass the 'Target' (because they move too quickly)
        //...so it is better to use some Debugging Visual Cues for some time...
        //..to adjust the Speed parameter and the 'Checking Distance':
        // Destroy this GameObject when there is overshoot
        // Check / Validation for that (above described):
        // DISTANCE before Moving should NOT be SMALLER than AFTER (moving):
        //..that means (SMALLER): there is OVERSHOOT (we passed by the Target... and continued towards the Infinity in the Horizon...) 
        // ORIGINAL CodeMonkey's Code (NOT Optimized):
        //
        // if (distanceBeforeMoving < distanceAfterMoving)
        // {
        //     // We Overshot the Target:
        //     //
        //     Destroy(gameObject);
        // }
        // NOTE: There is an alternative (..because it overshoots 50% of times, then in the NEXT FRAME the object is destroyed)
        // --->  Alternative:   https://community.gamedev.tv/t/calculating-bulletprojectile-wrong/205214/3
        //
        // Calculate the Square Distances & Compare them... to see if we overshot:
        //
        if (sqrDistanceBeforeMoving < sqrDistanceAfterMoving)
        {
            // We Overshot the Target:
            
            // 0- Save the Move Direction (_moveDirection) of this Bullet (pass it to the TargetUnit, the receiver...), to use it as an Explosion Vector, for Animations on the Target:
            //...see it in the Awake()... it is initialized there as:  _moveDirection
            

            // 1- Place the Trail + Bullet (i.e.: this GameObject) just on the Target's Position
            //..(to avoid a visual Glitch: it tends to overshoot and goes +1 mtr, beyond the target):
            // 
            _cachedTransform.position = _targetPosition;

            // 2- Destroy / Clean the Memory:
            // 2.1- Un-parent the Visual Trail Renderer (VFX Particle Effect), that will make it self-destroy in the next frame:
            //
            _trailRenderer.transform.parent = null;
            //
            // 2.2- Destroy the GameObject:
            //
            Destroy(gameObject);
            
            // 3- Instantiate (make spawn..) a (VFX) Particle System for: 'sparks after the Bullet Hits the Target'
            //
            Instantiate(_bulletHitVfxPrefab, _targetPosition, Quaternion.identity);

        }//End if (sqrDistanceBeforeMoving < sqrDistanceAfterMoving)

    }//End Update()

    #endregion Unity Methods


    #region My Custom Methods

    /// <summary>
    /// Sets up the Bullet, for moving "like" a Physics GameObject <br />
    /// ..(variables, parameters, etc) but, actually: it is NOT a Physics GameObject <br />
    /// ..(because it does not have, neither, a 3D RigidBody Component nor a 3D Collider... - it will be moved through this Script, updating its Transform every frame).
    /// </summary>
    public void Setup(Vector3 targetPosition)
    {
        this._targetPosition = targetPosition;
    }



    #endregion My Custom Methods

}
