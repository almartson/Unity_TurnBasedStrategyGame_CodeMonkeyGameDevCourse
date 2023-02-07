/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/

using System;
using UnityEngine;

/// <summary>
/// Allows any GameObject to "look" towards the Camera, constantly, during the regular (MonoBehaviour's) Update() time.
/// </summary>
public class LookAtCamera : MonoBehaviour
{

    #region Attributes

    [Tooltip("Do you want to invert the Horizontal position of the UI Element.")]
    [SerializeField]
    private bool _invertHorizontallySoItsReadable = true;

    [Tooltip("(Reference to...) the Scene's MainCamera GameObject")]
    private Transform _cameraTransform;


    #region Variables for Optimization

    /// <summary>
    /// This associated GameObject's Transform's  (cached for greater performance).
    /// </summary>
    private Transform _cachedTransform;
    
    #endregion Variables for Optimization

    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>
    private void Awake()
    {
        #region Performance Optimization
        
        // Cache the Main Camera's  Transform  (Performance-wise)
        //
        _cameraTransform = Camera.main.transform;
        
        // Cache the Transform
        //
        _cachedTransform = transform;

        #endregion Performance Optimization

    }// End Awake


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>



    /// <summary>
    /// Update is called once per frame... and 'LateUpdate' runs after 'Update' once per frame.
    /// </summary>
    private void LateUpdate()
    {
        //Direct the UI element facing towards the Camera:
        //
        if (_invertHorizontallySoItsReadable)
        {
            // 0- Cache every frame .position... fro performance:
            //
            Vector3 position = _cachedTransform.position;
            
            // Calculate the Vector3: Direction Towards the Camera:
            //
            Vector3 directionToCamera = (_cameraTransform.position - position).normalized;
            
            // Make the UI element's 'back' (bottom) face the Camera, to make it readable for Humans:
            //
            _cachedTransform.LookAt(position + (directionToCamera * -1));
        }
        else
        {
            // Make this GameObject's front face Rotate towards the Camera, facing the Camera:
            //
            _cachedTransform.LookAt(_cameraTransform);
            
        }//End else of if (_invertHorizontallySoItsReadable)
        
    }// End LateUpdate

    #endregion Unity Methods
    

    #region My Custom Methods

    

    #endregion My Custom Methods

}
