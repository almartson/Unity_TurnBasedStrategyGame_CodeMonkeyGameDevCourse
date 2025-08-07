/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/

using System;
using UnityEngine;


public class ScreenShakeActions : MonoBehaviour
{

    #region Attributes

    [Tooltip("[_grenadeExplosionCameraImpulse] Value for Cinemachine Camera's (Impulse listener) Shake.")]
    [SerializeField]
    private float _grenadeExplosionCameraImpulse = 5.0f;


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
        ShootAction.OnAnyShootAnimation += ShootAction_OnAnyShootAnimation;
        
        GrenadeProjectile.OnAnyGrenadeExploded += GrenadeProjectile_OnAnyGrenadeExploded;
        
    }//End Start


    /// <summary>
    /// Update is called once per frame
    /// </summary>


    #endregion Unity Methods
    

    #region My Custom Methods

    private void ShootAction_OnAnyShootAnimation(object sender, ShootAction.OnShootAnimationEventArgs e)
    {
        // Fire the Impulse... so a Cinemachine Listener will hear it, and react with the Screen Shake Movement:
        //
        ScreenShake.Instance.Shake();
        
    }//End ShootAction_OnAnyShootAnimation
    
    
    private void GrenadeProjectile_OnAnyGrenadeExploded(object sender, EventArgs e)
    {
        // Fire the Impulse... so a Cinemachine Listener will hear it, and react with the Screen Shake Movement:
        //
        ScreenShake.Instance.Shake(_grenadeExplosionCameraImpulse);
        
    }//End GrenadeProjectile_OnAnyGrenadeExploded



    #endregion My Custom Methods

}
