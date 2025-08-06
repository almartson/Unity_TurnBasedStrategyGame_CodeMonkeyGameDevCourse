/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/

using System;
using UnityEngine;


public class GrenadeProjectile : MonoBehaviour
{

    #region Attributes
    
    [Tooltip("[_targetPosition] Target-destination of this process.")]
    private Vector3 _targetPosition;

    [Tooltip("[_moveSpeed] Speed of the Projectile.")]
    [SerializeField]
    private float _moveSpeed = 15.0f;




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
            // Remove the Projectile...
            // ..when it reaches its Target.
            Destroy(gameObject);
        }

    }//End Update

    #endregion Unity Methods


    #region My Custom Methods

    public void Setup(GridPosition targetGridPosition)
    {
        _targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

    }//End Setup



    #endregion My Custom Methods

}
