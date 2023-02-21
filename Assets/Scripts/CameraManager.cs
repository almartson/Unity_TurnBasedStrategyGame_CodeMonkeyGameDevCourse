/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using System;
using UnityEngine;

/// <summary>
/// It is the Camera Manager for each one of the Unit's Actions: Move, Shoot, Spin, etc..<br />
/// (unlike CameraController.cs, which handles the movement of the 'CameraController' GameObject (which is the Cinemachine's
///...Target to Follow and to Aim to...) via C# Code, anytime I want (and that will be done in each Turn during the game,
///...when a new action happens...), so the Cinemachine Camera System will follow this GameObject and C# code). <br /><br />
/// CameraManager:  handles the Camera's Positions and Transitions, <br />
/// ..depending on (the Logic of) what Action of the Game is being executed...<br />
/// ..(e.g.: MoveAction, SpinAction, ShootAction, etc).<br />
/// </summary>
public class CameraManager : MonoBehaviour
{

    #region Attributes

    [Tooltip("GameObject to run / move the Cinemachine Camera Position")]
    [SerializeField]
    private GameObject _actionCameraGameObject;

    
    #region Camera Positions
    
    #region Shooting Animation
    
    [Tooltip("(Set by the Designer...): Height of the Character-Unit's Shoulder, in Game World Coordinates")]
    [SerializeField]
    [Range(0.0f, 5.0f)]
    private float _shoulderHeightForUnitCharacter = 1.7f;

    /// <summary>
    /// Overall calculated Position to place the Camera during a SHOOTING ANIMATION.
    /// </summary>
    private Vector3 _cameraCharacterHeight;

    #endregion Shooting Animation

    
    #endregion Camera Positions
    
    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>



    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start()
    {
        // Delegate (assign / Subscription), to create a Listener...
        //..so it listens to ANY ACTION when it STARTS
        //..(using the Listener: BaseAction.OnAnyActionStarted)
        //
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        //
        // Delegate (assign / Subscription), to create a Listener...
        //..so it listens to ANY ACTION when it ENDS
        //..(using the Listener: BaseAction.OnAnyActionCompleted)
        //
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;
        //
        // Hide the ACTION CAMERA:
        //
        HideActionCamera();

    }//End Start


    /// <summary>
    /// Update is called once per frame
    /// </summary>


    #endregion Unity Methods


    #region My Custom Methods

    /// <summary>
    /// ShowAndSetMaterial the Action Camera, by Enabling the GameObject its Scripts reside on.
    /// </summary>
    private void ShowActionCamera()
    {
        _actionCameraGameObject.SetActive(true);
        
    }// End ShowActionCamera
    
    /// <summary>
    /// Hides the Action Camera, by Disabling the GameObject its Scripts reside on.
    /// </summary>
    private void HideActionCamera()
    {
        _actionCameraGameObject.SetActive(false);
        
    }// End HideActionCamera

    
    #region Delegates / Listeners / CallBacks

    /// <summary>
    /// Delegate Listener that is triggered when ANY ACTION STARTS (regardless of the Type, any) (reason: i.e.: this is a STATIC EVENT... in the 'BaseAction' Class).
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        // Filter which Type of ACTION
        //
        switch (sender)
        {
            case ShootAction shootAction:
                
                // Set the Camera, for this Action:
                //
                BeginTakeCameraShootAction(shootAction);

                // ShowAndSetMaterial / Initiate the Camera Transitional MOVEMENT:
                //
                ShowActionCamera();
                
                break;
            
        }//End switch (sender)

    }// End BaseAction_OnAnyActionStarted
    
    
    /// <summary>
    /// Delegate Listener that is triggered when ANY ACTION ENDS (regardless of the Type, any) (reason: i.e.: this is a STATIC EVENT... in the 'BaseAction' Class).
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        // Filter which Type of ACTION
        // Optional code:   if (sender as ShootAction) ...
        //
        switch (sender)
        {
            case ShootAction shootAction:
                
                // Camera SET for ending:
                // ..the whole SHOOTING ANIMATIONS:
                //
                EndTakeCameraShootAction();
                
                // Hide the ActionVirtual Camera:
                //
                HideActionCamera();
                
                break;
            
        }//End switch (sender)

    }// End BaseAction_OnAnyActionCompleted


    
    #region Take Camera Action

    #region Shooting Animation
    
    /// <summary>
    /// Does the Mathematical calculations when executing the SHOOT ACTION.
    /// </summary>
    /// <param name="shootAction"></param>
    private void BeginTakeCameraShootAction(ShootAction shootAction)
    {
        // Camera SET for the whole SHOOTING ANIMATIONS:
        // A CloseUp to the Shooter & Target together:

        // ACTORS... of this Action:
        // Initiator of this ACTION:
        //
        Unit shooterUnit = shootAction.GetUnit();
        //
        // TARGET
        //
        Unit targetUnit = shootAction.GetTargetUnit();

        // Calculate the 'SHOULDER HEIGHT', to put the Camera there:
        //
        _cameraCharacterHeight = Vector3.up * _shoulderHeightForUnitCharacter;

        // Calculate (or get) the SHOOT DIRECTION  (vector)
        //
        // ORIGINAL:
        Vector3 shootDirection = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;
        // Optimized:
        //
        // Use:   shootAction.AimDirection
        //
        // Vector3 shootDirection = shootAction.AimDirection;

        // Calculate:  an small Offset to the Camera Rotation, to look in a 3RD CAMERA PERSPECTIVE towards the Target & Shooter together:
        // This is the 'Unit shoulder's lenght'... because we want to displace the Camera View (from the center of the Character's Neck...) to the right shoulder:
        // NOTE: although the exact value, with the current Scale of (1, 1, 1), is: 0.34f.
        //
        float shoulderOffsetAmount = 0.5f;
        //
        // Finally:  Calculate the small ROTATION and POSITION to the side of the HEAD (like a CAMERA MAN would use the camera...):
        //
        Vector3 horizontalShoulderOffset = Quaternion.Euler(0, 90, 0) * shootDirection * shoulderOffsetAmount;

        // Finally:  POSITION the CAMERA:   SUM the Vector3 INVOLVED:
        // 1- At the 'shooterUnit' Position, but still at the ground level, y=0 (shooterUnit.GetWorldPosition()).
        // 2- At the SHOULDER HEIGHT  (_cameraCharacterHeight - because (1) was giving us the FEET POSITION: (X, 0.0f, Z)).
        // 3- (horizontalShoulderOffset) To POSITION IT slightly to the Right of the shoulder.
        // 4- We want to see the SHOOTER too, so we Position the whole setup a little behind the 'Aim Line Vector3' (shootDirection * -1), to see the system: SHOOTER + TARGET:
        //
        Vector3 actionCameraPosition = shooterUnit.GetWorldPosition() + _cameraCharacterHeight + horizontalShoulderOffset +
                                       (shootDirection * -1);

        // Copy the POSITIONAL-calculated Final Vector3:
        //
        _actionCameraGameObject.transform.position = actionCameraPosition;
        //
        // Add a ROTATION:   Looking towards the Target.
        //
        _actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + _cameraCharacterHeight);

    }//End BeginTakeCameraShootAction
    
    
    /// <summary>
    /// Does the Final Mathematical calculations when finishing the executiion of a SHOOT ACTION.
    /// </summary>
    private void EndTakeCameraShootAction()
    {
    }//End EndTakeCameraShootAction
    
    #endregion Shooting Animation
    
    #endregion Take Camera Action
    
    
    #endregion Delegates / Listeners / CallBacks
    
    #endregion My Custom Methods

}
