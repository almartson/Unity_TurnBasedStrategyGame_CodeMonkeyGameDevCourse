/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using UnityEngine;

/// <summary>
/// Unit's Ragdoll GameObject Logic. <br />
/// This should be attached to a UnitRagdoll Prefab. <br />
/// This is spawned in-place of an Unit / Character that is dying.
/// </summary>
public class UnitRagdoll : MonoBehaviour
{
    #region Attributes

    #region RAGDOLL's Bone Transforms
    
    [Tooltip("Ragdoll's ->Root<- (Bone) reference: it will be used to paste on them all the original Character's Bones Transforms values (position, rotation, scale) to make the Ragdoll match the Character's Position right at the moment of its death event.../n/n This makes the Ragdoll's Animation consistent with the Pose the Unit/Character had a frame before spawning.")]
    [SerializeField]
    private Transform _ragdollRootBone;
    
    
    // NOTE:  This one is on the   UnitRagdollSpawner.cs   SCRIPT.
    //
    // [Tooltip("(Original) Character reference to its Bones that are part of the Ragdoll's Hierarchy. These Bones' (List) Transform will be copied to: the RAGDOLL's Bones List, for using them when Spawning it in the Place of the Original Unit, to play the Animation of 'dropping death'.")]
    // [SerializeField]
    // private Transform[] _originalCharacterBonesThatAreRagdollized;
    
    
    [Tooltip("(Clone / RAGDOLL's) Ragdoll reference to its Bones that are part of the 3D Character (i.e.: The Unit) Hierarchy. These Bones' (List) Transform's will be substituted by: the Unit's (Original Character's) Bones List, for using them when Spawning it in the Place of the Original Unit, to play the Animation of 'dropping death'.")]
    [SerializeField]
    private Transform[] _ragdollPrefabsCharacterBonesAreRagdollized;
    //
    /// <summary>
    /// Property Accessor to Private Field "_ragdollPrefabsCharacterBonesAreRagdollized".
    /// </summary>
    public Transform[] RagdollPrefabsCharacterBonesAreRagdollized { get => _ragdollPrefabsCharacterBonesAreRagdollized; private set => _ragdollPrefabsCharacterBonesAreRagdollized = value; }

    #endregion RAGDOLL's Bone Transforms
    
    
    #region Explosion Force
    
    [Tooltip("Explosion Force to be (optionally) applied to the Ragdoll in the moment it spawns in the place of the Original Unit./n/n This makes the Ragdoll's Animation a little bit spicier and fun when spawning.")]
    [SerializeField]
    private float _explosionForce = 300.0f;
    
    [Tooltip("Range of the Explosion, to be (optionally) applied to the Ragdoll in the moment it spawns in the place of the Original Unit./n/n This makes the Ragdoll's Animation a little bit spicier and fun when spawning.")]
    [SerializeField]
    private float _explosionRange = 10.0f;


    [Tooltip("Whether you want (Realistic) Physics-based Explosions (i.e: set it to TRUE); or an funny (exaggerated) Explosion (Recommended, (i.e: set it to FALSE))./n/n This makes the Ragdoll's Animation a little bit spicier and fun when spawning.")]
    [SerializeField]
    private bool _realisticExplosionPhysics = false;

    
    [Tooltip("Displacement (Offset) from the (original) center of the Sphere of Explosion, to be applied. This is a proportional number to the Radius, meaning that it is measured in terms of '_explosionRange' times. This is OPTIONAL: it is to generate a Push-Effect on the Unit (Target) that receives the Attack./n/n This makes the Ragdoll's Animation a little bit spicier and fun.")]
    [Range( -1.1f, 1.1f)]
    [SerializeField]
    private float _explosionOffsetInHorizontalAxisPositioningProportionalToExplosionRange = 0.077f;
    
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

    
    #region Setup

    /// <summary>
    /// Function to initiate a:  Copy all the ORIGINAL 3D CHARACTER'S (i.e.: Unit) Transform (of every Bone...) to the RAGDOLL Bones... so the Ragdoll will be spawned in the same Pose as the original Character / Unit (and not in T-POSE, as it is by default).
    /// </summary>
    public void SetupOptimized(Transform[] originalCharacterBonesThatAreRagdollized, Transform[] ragdollPrefabsCharacterBonesAreRagdollized, Transform originalCharactersRootBone, Vector3 bulletProjectileDirectionVector3, Vector3 pointPositionOfImpactVector3)
    {
        // Call the 'Optimized' Function that will COPY & PASTE all the Original Unit Skeleton Bone Transforms... to the RAGDOLL's:
        //
        MatchAllChildrenTransformsOptimized(originalCharacterBonesThatAreRagdollized, ragdollPrefabsCharacterBonesAreRagdollized);
        
        
        // Fin: Add some Explosion Force Effect to the Ragdoll, because we want to see it flying... :)
        //
        if ((bulletProjectileDirectionVector3 != null) &&  !bulletProjectileDirectionVector3.Equals( Vector3.zero ))
        {
            // Explosion:
            //
            // Original: ApplyExplosionToTargetRagdoll(_ragdollRootBone, _explosionForce, transform.position, _explosionRange);
            //
            // Use the Direction Vector3 to calculate a Position 1 mtr before the Target Unit (Character)... so there will be a KnockBack Effect with the Explosion:
            //
            // Auxiliary variables declaration:
            //
            Vector3 normalizedDirection, newPositionOriginForExplosion;
            //
            // Calculations:
            //
            if (_realisticExplosionPhysics)
            {
                
                // Realistic Vector3 (Position of the Explosion) calculation, based on y=the height of the Bullet.
                //
                // (Normalized) Direction of the Bullet:
                //
                normalizedDirection = bulletProjectileDirectionVector3.normalized;
                //
                // New Position (Bullet's Point of Impact, for the EXPLOSION to occur):
                //
                newPositionOriginForExplosion = pointPositionOfImpactVector3 - (normalizedDirection * (_explosionOffsetInHorizontalAxisPositioningProportionalToExplosionRange * _explosionRange));
                //
                // We fix the height of the Explosion (again):
                //
                newPositionOriginForExplosion.y = pointPositionOfImpactVector3.y;
            }
            else
            {
                
                // Non-Realistic Vector3 (Position of the Explosion) calculation, based on y=0.
                //
                // (Normalized) Direction of the Bullet:
                //
                normalizedDirection = bulletProjectileDirectionVector3.normalized;
                //
                // New Position (Bullet's Point of Impact, for the EXPLOSION to occur):
                //
                newPositionOriginForExplosion = transform.position - (normalizedDirection * (_explosionOffsetInHorizontalAxisPositioningProportionalToExplosionRange * _explosionRange));

            }//End if (_realisticExplosionPhysics)
            
            // Debug
            //
            Debug.Log("transform.position = " + transform.position);
            Debug.Log("bulletProjectileDirectionVector3 = " + bulletProjectileDirectionVector3);
            Debug.Log("pointPositionOfImpactVector3 = " + pointPositionOfImpactVector3);
            Debug.Log("newPositionOriginForExplosion = " + newPositionOriginForExplosion);
            //
            ApplyExplosionToTargetRagdollOptimized(ragdollPrefabsCharacterBonesAreRagdollized, _explosionForce, newPositionOriginForExplosion, _explosionRange);
    
        }//End if (bulletProjectileDirectionVector3)

    }// End SetupOptimized
    
    
    /// <summary>
    /// Function to initiate a:  Copy all the ORIGINAL 3D CHARACTER'S (i.e.: Unit) Transform (of every Bone...) to the RAGDOLL Bones... so the Ragdoll will be spawned in the same Pose as the original Character / Unit (and not in T-POSE, as it is by default).
    /// </summary>
    /// <param name="originalCharactersRootBone"></param>
    public void Setup(Transform originalCharactersRootBone, Vector3 applyExplosionForceToRagdollWhenSpawningThisIsTheDirectionOfTheBullet)
    {
        // Call the Recursive+Iterative Function that will COPY & PASTE all the Original Unit Skeleton Bone Transforms... to the RAGDOLL's:
        //
        MatchAllChildrenTransforms(originalCharactersRootBone, _ragdollRootBone);
        
        // Add some Explosion Force Effect to the Ragdoll, because we want to see it flying... :)
        //
        if ((applyExplosionForceToRagdollWhenSpawningThisIsTheDirectionOfTheBullet != null) &&  !applyExplosionForceToRagdollWhenSpawningThisIsTheDirectionOfTheBullet.Equals( Vector3.zero ))
        {
            // Explosion:
            //
            // Original: ApplyExplosionToTargetRagdoll(_ragdollRootBone, _explosionForce, transform.position, _explosionRange);
            //
            // Use the Direction Vector3 to calculate a Position 1 mtr before the Target Unit (Character)... so there will be a KnockBack Effect with the Explosion:
            //
            Vector3 normalizedDirection = applyExplosionForceToRagdollWhenSpawningThisIsTheDirectionOfTheBullet;
            Vector3 newPositionOriginForExplosion = transform.position - (normalizedDirection * (_explosionOffsetInHorizontalAxisPositioningProportionalToExplosionRange * _explosionRange));
            //
            // Vector3 newPositionOriginForExplosion = transform.position - (_explosionOffsetInHorizontalAxisPositioningProportionalToExplosionRange * _explosionRange);
            //
            // Debug.Log("transform.position = " + transform.position);
            // Debug.Log("bulletProjectileDirectionVector3 = " + bulletProjectileDirectionVector3);
            // Debug.Log("newPositionOriginForExplosion = " + newPositionOriginForExplosion);
            //
            ApplyExplosionToTargetRagdoll(_ragdollRootBone, _explosionForce, newPositionOriginForExplosion, _explosionRange);
    
        }//End if (bulletProjectileDirectionVector3)
        
    }// End Setup
    
    #endregion Setup


    #region Copy and Paste Bones Transform:   MatchAllChildrenTransforms

    /// <summary>
    /// 'Iterative', Optimized version of 'MatchAllChildrenTransforms': A Function used to COPY all the ORIGINAL 3D CHARACTER'S (i.e.: Unit) Transform (of every Bone...) to the RAGDOLL Bones... so the Ragdoll will be spawned in the same Pose as the original Character / Unit (and not in T-POSE, as it is by default).<br />
    /// We assume as a PRE-CONDITION:  Input parameters are correcly set before calling this Function.
    /// </summary>
    /// <param name="originalBonesTransformList">(COPY FROM HERE) The ROOT of the ORIGINAL Skeleton (i.e: the Bone Hierachy). This is what we want to duplicate.</param>
    /// <param name="cloneBonesTransformList">(PASTE HERE) The ROOT of the CLONE Skeleton (i.e: the Bone Hierachy). This is the 'new' Skeleton, we are creating...</param>
    private void MatchAllChildrenTransformsOptimized(Transform[] originalBonesTransformList, Transform[] cloneBonesTransformList)
    {
        // Go through each Child Bone that is set as a part of the RAGDOLL, in the array[]:
        // Lenght of the Array
        //
        int arrayLength = Mathf.Min(originalBonesTransformList.Length , cloneBonesTransformList.Length);
        //
        for (int i = 0; i < arrayLength; i++)
        {
            // 1- Copy & Paste all the Transform, (POSITION & ROTATION), values to the:  cloneChild
            //   .1- Cache the item[i]
            //
            Transform originalBonesTransformItem = originalBonesTransformList[i];
            //
            // Copy Position & Rotation:
            //
            cloneBonesTransformList[i].SetPositionAndRotation(originalBonesTransformItem.position, originalBonesTransformItem.rotation);

        }//End for
        
    }// End MatchAllChildrenTransformsOptimized

    
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
        
    }// End MatchAllChildrenTransforms

    #endregion Copy and Paste Bones Transform:   MatchAllChildrenTransforms


    #region Explosion
    
    /// <summary>
    /// Optimized (w/Iterative For-Loop) function that applies a Force, (as some sort of explosion), to every Bone in the Ragdoll... so it looks like it has been shot by a Cannon (a Tank...).
    /// </summary>
    private void ApplyExplosionToTargetRagdollOptimized(Transform[] ragdollBonesTransformList, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {

        // 1- Go through each Child Bone that is set as a part of the RAGDOLL, in the array[]:
        // Lenght of the Array
        //
        int arrayLength = ragdollBonesTransformList.Length;
        //
        for (int i = 0; i < arrayLength; i++)
        {
            //   .1- Try to Get the CHILD's RigidBody Component:
            //
            if (ragdollBonesTransformList[i].TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                // 2- Apply the EXPLOSION
                //
                //public void AddExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius, float upwardsModifier = 0.0f, ForceMode mode = ForceMode.Force));
                //
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange /* , 0.5f*/);
                
            }//End if (child.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))

        }//End for
        
    }// End ApplyExplosionToTargetRagdollOptimized
        
    
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
                // ORIGINAL: childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
                //
                //public void AddExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius, float upwardsModifier = 0.0f, ForceMode mode = ForceMode.Force));
                //
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange /* , 0.5f*/);

            }//End if (child.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            
            
            // 3- Go to the NEXT LEVEL (down) in the Bone Hierarchy (of the Skeleton):
            // RECURSIVE Function: this function calls itself, but now with the children instead of the root bones as Parameters:
            //
            ApplyExplosionToTargetRagdoll(child, explosionForce, explosionPosition, explosionRange);
            
        }//End foreach (Transform child in root)
        
        // When there are no more Bones to check, this function ends at all levels (calls/invocations):
        //... DONE!

    }// End ApplyExplosionToTargetRagdoll

    #endregion Explosion

    #endregion My Custom Methods

}
