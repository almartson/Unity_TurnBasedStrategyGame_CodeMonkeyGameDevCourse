/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/

using System;
using UnityEngine;


public class UnitAnimator : MonoBehaviour
{

    #region Attributes
    
    #region Animator & Animations
    
    [Tooltip("3D Character's Animator")]
    [SerializeField]
    private Animator _unitAnimator;
    
    
    #region Animation Parameters
    
    // Hash ANIMATOR (Parameter) CONSTANTS representing the States of the Animator
    // ...(i.e.: to trigger the Animation Clips).

    #region 1- MoveAction - Animation Parameters

    /// <summary>
    /// Hash ANIMATOR (Parameter) CONSTANTS:   IsWalking  (Animator State)
    /// </summary>
    private static readonly int _IS_WALKING_ANIMATOR_PARAMETER = Animator.StringToHash("IsWalking");
    
    #endregion 1- MoveAction - Animation Parameters

    #region 2- ShootAction - Animation Parameters

    /// <summary>
    /// Hash ANIMATOR (Parameter) CONSTANTS:   Shoot  (Animator State)
    /// </summary>
    private static readonly int _SHOOT_ANIMATOR_PARAMETER = Animator.StringToHash("Shoot");
    
    
    [Tooltip("Bullet (Visuals) Prefab, which will be instantiated in the Scene, via Code")]
    [SerializeField]
    private Transform _bulletProjectilePrefab;

    [Tooltip("Transform (Place - Origin) for the Initial Point where to Spawn the Bullet (Visuals) Prefab, which will be instantiated in the Scene, via Code")]
    [SerializeField]
    private Transform _shootPointTransform;

    
    #endregion 2- ShootAction - Animation Parameters

    #endregion Animation Parameters
    
    #endregion Animator & Animations

    
    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>
    private void Awake()
    {
        
        // Initialize each Actions' Animations
        // 1- MOVE ACTION
        //
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            // Assign the Delegates-Callbacks-Listeners to their respective Functions,
            // ..for STARTING & STOPPING the Animations:
            //
            moveAction.OnStartMovingAnimation += MoveAction_OnStartMovingAnimation;
            moveAction.OnStopMovingAnimation += MoveAction_OnStopMovingAnimation;

        }//End if (TryGetComponent<MoveAction>...
        //
        // 2- SHOOT ACTION
        //
        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            // Assign the Delegates-Callbacks-Listeners to their respective Functions,
            // ..for STARTING & STOPPING the Animations:
            //
            shootAction.OnShootAnimation += ShootAction_OnShootAnimation;

        }//End if (TryGetComponent<ShootAction>...
        
    }//End Awake()


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>



    /// <summary>
    /// Update is called once per frame
    /// </summary>


    #endregion Unity Methods


    #region My Custom Methods

    #region CallBacks - Listeners: Move Action
    /// <summary>
    /// Function to be triggered when the Unit STARTS the Moving Animation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MoveAction_OnStartMovingAnimation(object sender, EventArgs e)
    {
        // 1- Update the Animator's Parameter: Start/Keep on Walking.
        //
        _unitAnimator.SetBool(_IS_WALKING_ANIMATOR_PARAMETER, true);

    }//End MoveAction_OnStartMovingAnimation

    /// <summary>
    /// Function to be triggered when the Unit STOPS the Moving Animation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MoveAction_OnStopMovingAnimation(object sender, EventArgs e)
    {
        // 1- Update the Animator's Parameter:  STOP  (Walking).
        //
        _unitAnimator.SetBool(_IS_WALKING_ANIMATOR_PARAMETER, false);

    }//End MoveAction_OnStopMovingAnimation

    #endregion CallBacks - Listeners: Move Action
    
    
    #region CallBacks - Listeners: Shoot Action
    
    /// <summary>
    /// Function to be triggered when the Unit STARTS the Shoot(ing) Animation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ShootAction_OnShootAnimation(object sender, ShootAction.OnShootAnimationEventArgs e)
    {
        // 1- Update the Animator's Parameter:  SHOOT
        //
        _unitAnimator.SetTrigger(_SHOOT_ANIMATOR_PARAMETER);
        
        // 2- Instantiate the BULLET Prefab in the Scene, as a GameObject, at the Position:   _shootPointTransform.position
        //
        Transform bulletProjectileTransform = Instantiate(_bulletProjectilePrefab, _shootPointTransform.position, Quaternion.identity);
        //
        //   2.2- Get the 'BulletProjectile' Script (it is a Component attached to the 'BulletProjectile' Prefab):
        //
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();
        
        // 3- Setup the BulletProjectile   (for moving through its Transform every frame..., not via Physics)
        //   .1- Get the Target's (Vector3) Position at his Feet (i.e.: y = 0)  (in World Game Coordinates)
        //
        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();
        //
        //   .2- Set the y-Coordinate (the Height) of the BULLET as a Constant (for starting the SHOOTING Animation), so it will be pointing towards the Center of the Target-GameObject:  the Bullet movement will be HORIZONTAL thanks to that:
        //
        targetUnitShootAtPosition.y = _shootPointTransform.position.y;
        // 3- Setup the BulletProjectile
        bulletProjectile.Setup( targetUnitShootAtPosition );

    }//End ShootAction_OnShootAnimation
    
    
    #endregion CallBacks - Listeners: Shoot Action
    
    #endregion My Custom Methods

}
