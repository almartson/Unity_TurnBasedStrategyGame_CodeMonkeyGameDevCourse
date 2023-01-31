/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/

using System;
using UnityEngine;


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
        // Simple Logic to:
        // Move (the BULLET...) towards the:  TargetPosition.
        // .1- Direction (Vector3)
        //   .1.1- Cache (for performance optimization): transform.position
        //
        var position = transform.position;
        // .1- Direction
        Vector3 moveDir = (_targetPosition - position).normalized;
        
        // To avoid Overshooting the Target:
        // Compare DISTANCE before moving with AFTER moving...
        // Distance BEFORE Moving:
        // Todo: Fix: Optimization NOT using Square root here (1)
        //
        float distanceBeforeMoving = Vector3.Distance(position, _targetPosition);
        
        // .2- MOVE: Increase the Position (i.e.: Move the Bullet):
        //
        position += moveDir * (_moveSpeed * Time.deltaTime);
        transform.position = position;
        
        // Distance AFTER Moving:
        // Todo: Fix: Optimization NOT using Square root here (2)
        //
        float distanceAfterMoving = Vector3.Distance(position, _targetPosition);
        
        // Debug TIP: With fast paced moving objects it is very common to
        //..overshoot or overpass the 'Target' (because they move too quickly)
        //...so it is better to use some Debugging Visual Cues for some time...
        //..to adjust the Speed parameter and the 'Checking Distance':
        // Destroy this GameObject when there is overshoot
        // Check / Validation for that (above described):
        // DISTANCE before Moving should NOT be SMALLER than AFTER (moving):
        //..that means (SMALLER): there is OVERSHOOT (we passed by the Target... and continued towards the Infinity in the Horizon...) 
        //
        if (distanceBeforeMoving < distanceAfterMoving)
        {
            // We Overshot the Target:
            //
            Destroy(gameObject);
        }
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
