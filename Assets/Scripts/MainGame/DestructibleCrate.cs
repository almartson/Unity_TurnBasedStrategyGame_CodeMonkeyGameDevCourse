/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/

using System;
using UnityEngine;


public class DestructibleCrate : MonoBehaviour
{

    #region Attributes

    [Tooltip("Crate Destroyed Prefab \n ")]
    [SerializeField]
    private Transform _crateDestroyedPrefab;
    
    
    /// <summary>
    /// Location in the GRID of the Destructible Prop to update (now it isn't there anymore)
    /// </summary>
    private GridPosition _gridPosition;
    
    #region Events

    public static event EventHandler OnAnyDestroyed;

    #endregion Events


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
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

    }// End Start


    /// <summary>
    /// Update is called once per frame
    /// </summary>


    #endregion Unity Methods


    #region My Custom Methods

    
    #region Explosion
    
    /// <summary>
    /// Todo: Optimize with priority Yellow (not too important). The destruction that occurs here could be avoided by using Unity Pooling System/asset. 
    /// (Candidate for Deprecation, for performance reasons:   a main 'Foreach' Iterative Loop + Recursive calls to itself...) <br /> <br />
    /// 
    /// Recursive + Iterative function that applies a Force, (as some sort of explosion), to every Bone in the Children of the "root" Input Parameter... so it looks like it has been shot by a Cannon (a Tank...).
    /// </summary>
      private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
      {
        // 1- Ride through every Child Bone...
        //
        foreach (Transform child in root)
        {
            //   .1- Try to Get the CHILD's RigidBody Component:
            //
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                // 2- Apply the EXPLOSION
                //
                // ORIGINAL: childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
                //
                //public void AddExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius, float upwardsModifier = 0.0f, ForceMode mode = ForceMode.Force));
                //
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange /* , 0.5f*/);

            }//End if (child.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            
            
            // 3- Go to the NEXT LEVEL (down) in the Bone Hierarchy (of the Skeleton):
            // RECURSIVE Function: this function calls itself, but now with the children instead of the root bones as Parameters:
            //
            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);
            
        }//End foreach (Transform child in root)
        
        // When there are no more Bones to check, this function ends at all levels (calls/invocations):
        //... DONE!

    }// End ApplyExplosionToTargetRagdoll

    #endregion Explosion
    
    
    /// <summary>
    /// Todo: Optimize with priority Orange (important but avoidable unless it doesn't work on Mobile Platforms). The destruction ( Destroy(gameObject); ) that occurs here could be avoided by using Unity Pooling System/asset; that way we would avoid the hipcut in Performance. 
    /// </summary>
    public void Damage()
    {
        // Instantiate in the scene: "Destroyed Parts" of the same GameObject (Meshes)
        //
        Transform crateDestroyedTransform = Instantiate(_crateDestroyedPrefab, transform.position, transform.rotation);
        
        // Call the EXPLOSION method, just for the FUN!  (so the objects parts will blow! away with Physics)
        //
        ApplyExplosionToChildren(crateDestroyedTransform, 150f, transform.position, 10f);
        
        // Destroy the GameObject
        //
        Destroy(gameObject);
        
        // Listen to the event, and act:
        //
        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
    }

    #region Setters, Getters

    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }
    
    #endregion Setters, Getters

    #endregion My Custom Methods

}
