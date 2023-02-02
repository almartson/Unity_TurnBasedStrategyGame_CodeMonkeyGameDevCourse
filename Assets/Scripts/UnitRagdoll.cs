/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using UnityEngine;


public class UnitRagdoll : MonoBehaviour
{
    #region Attributes

    [Tooltip("Ragdoll's ->Root<- (Bone) reference: it will be used to paste on them all the original Character's Bones Transforms values (position, rotation, scale) to make the Ragdoll match the Character's Position right at the moment of its death event.../n/n This makes the Ragdoll's Animation consistent with the Pose the Unit/Character had a frame before spawning.")]
    [SerializeField]
    private Transform _ragdollRootBone;

    
    #region Explosion Force
    
    [Tooltip("Explosion Force to be (optionally) applied to the Ragdoll in the moment it spawns in the place of the Original Unit./n/n This makes the Ragdoll's Animation a little bit spicier and fun when spawning.")]
    [SerializeField]
    private float _explosionForce = 300.0f;
    
    [Tooltip("Range of the Explosion, to be (optionally) applied to the Ragdoll in the moment it spawns in the place of the Original Unit./n/n This makes the Ragdoll's Animation a little bit spicier and fun when spawning.")]
    [SerializeField]
    private float _explosionRange = 10.0f;

    #endregion Explosion Force
    
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


    #endregion Unity Methods


    #region My Custom Methods

    /// <summary>
    /// Function to initiate a:  Copy all the ORIGINAL 3D CHARACTER'S (i.e.: Unit) Transform (of every Bone...) to the RAGDOLL Bones... so the Ragdoll will be spawned in the same Pose as the original Character / Unit (and not in T-POSE, as it is by default).
    /// </summary>
    /// <param name="originalCharactersRootBone"></param>
    public void Setup(Transform originalCharactersRootBone, bool applyExplosionForceToRagdollWhenSpawning)
    {
        // Call the Recursive+Iterative Function that will COPY & PASTE all the Original Unit Skeleton Bone Transforms... to the RAGDOLL's:
        //
        MatchAllChildrenTransforms(originalCharactersRootBone, _ragdollRootBone);
        
        // Add some Explosion Force Effect to the Ragdoll, because we want to see it flying... :)
        //
        if (applyExplosionForceToRagdollWhenSpawning)
        {
            // Explosion:
            //
            ApplyExplosionToTargetRagdoll(_ragdollRootBone, _explosionForce, transform.position, _explosionRange);
    
        }//End if (applyExplosionForceToRagdollWhenSpawning)
        
    }// End Setup

    
    /// <summary>
    /// 'Iterative + Recursive' Function to COPY all the ORIGINAL 3D CHARACTER'S (i.e.: Unit) Transform (of every Bone...) to the RAGDOLL Bones... so the Ragdoll will be spawned in the same Pose as the original Character / Unit (and not in T-POSE, as it is by default).
    /// </summary>
    /// <param name="root">(COPY FROM HERE) The ROOT of the ORIGINAL Skeleton (i.e: the Bone Hierachy). This is what we want to duplicate.</param>
    /// <param name="clone">(PASTE HERE) The ROOT of the CLONE Skeleton (i.e: the Bone Hierachy). This is the 'new' Skeleton, we are creating...</param>
    private void MatchAllChildrenTransforms(Transform root, Transform clone)
    {
        #region CodeMonkey's Implementation (using FOREACH)...non performant
        
        // Go through each Child Bone in the Bone Hierarchy...
        //
        foreach (Transform child in root)
        {
            // 1- Try to FIND the Clone's CHILD 
            //
            Transform cloneChild = clone.Find(child.name);
            //
            // 2- If there is a Bone in the CHILD's Skeleton with that NAME / ID:
            //
            if (clone != null)
            {
                // 3- Copy & Paste all the Transform values to it (to the:  cloneChild)
                //   .1- POSITION
                //
                clone.position = child.position;
                //
                //   .2- ROTATION
                //
                clone.rotation = child.rotation;

                // 4- Go to the NEXT LEVEL (down) in the Bone Hierarchy (of the Skeleton):
                // RECURSIVE Function: this function calls itself, but now with the children instead of the root bones as Parameters:
                //
                MatchAllChildrenTransforms(child, cloneChild);
                
            }//End if (clone != null)

            // When there are no more Bones to check, this function ends at all levels (calls/invocations):
            //... DONE!
            
        }//End foreach (Transform child in root)
        
        #endregion CodeMonkey's Implementation (using FOREACH)...non performant
        
        
        #region AlMartson's Implementation (using FOREACH)...Optimized for the Best Performance possible in this case
        
        #endregion AlMartson's Implementation (using FOREACH)...Optimized for the Best Performance possible in this case
        
    }// End MatchAllChildrenTransforms


    /// <summary>
    /// Recursive + Iterative function that applies a Force, (as some sort of explosion), to every Bone in the Ragdoll... so it looks like it has been shot by a Cannon (a Tank...).
    /// </summary>
    private void ApplyExplosionToTargetRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
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
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
                
            }//End if (child.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            
            
            // 3- Go to the NEXT LEVEL (down) in the Bone Hierarchy (of the Skeleton):
            // RECURSIVE Function: this function calls itself, but now with the children instead of the root bones as Parameters:
            //
            ApplyExplosionToTargetRagdoll(child, explosionForce, explosionPosition, explosionRange);
            
        }//End foreach (Transform child in root)
        
        // When there are no more Bones to check, this function ends at all levels (calls/invocations):
        //... DONE!

    }// End ApplyExplosionToTargetRagdoll

    #endregion My Custom Methods

}
