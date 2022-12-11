using System.Collections.Generic;
using UnityEngine;


public class MoveAction : MonoBehaviour
{
    #region Attributes

    [Tooltip("Reference to the Unit / Character to apply the MoveAction to...")]
    private Unit _unit;
    
    
    [Tooltip("Destination (x, y, z) Position of the Movement Action")]
    [SerializeField]
    private Vector3 _targetPosition;
    
    // Movement:
    //
    [Tooltip("Speed when Translating the Character / Unit (walking, moving)")]
    [SerializeField] private float _moveSpeed = 4.0f;
    
    // Rotation:
    //
    [Tooltip("Speed when Rotating the Character / Unit (walking, moving)")]
    [SerializeField] private float _rotateSpeed = 10.0f;

    
    #region Animator & Animations

    [Tooltip("3D Character's Animator")]
    [SerializeField] private Animator _unitAnimator;
    
    // Hash ANIMATOR (Parameter) CONSTANTS representing the States of the Animator
    // ...(i.e.: to trigger the Animation Clips).
    //
    /// <summary>
    /// Hash ANIMATOR (Parameter) CONSTANTS:   IsWalking  (Animator State)
    /// </summary>
    private static readonly int _IS_WALKING_ANIMATOR_PARAMETER = Animator.StringToHash("IsWalking");

    #endregion Animator & Animations
    
    
    #region Utils
    
    [Tooltip("The Tolerance number to accept that the value is ZERO")]
    [SerializeField]
    private float _stoppingDistance = 0.1f;
    
    private float _sqrStoppingDistance = 0.1f;

    #endregion Utils
    
    
    #region Validations: Movement
    
    /// <summary>
    /// Max number of Grid Cells the character can move in one Turn.
    /// </summary>
    [SerializeField]
    private int _maxMoveDistance = 4;
    
    
    #endregion Validations: Movement
    
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

        // Get the Unit / Character this Script is attached to in the Unity Editor.
        //
        _unit = GetComponent<Unit>();
        
        // Initialize Target Position to this Script's base GameObject.
        //
        _targetPosition = this.transform.position;
    }


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>



    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        // Update the Unit Movement (Walking...)
        //
        UpdateUnitMove();
    }

    
    #endregion Unity Methods
    

    #region My Custom Methods


    public void Move(Vector3 newTargetPosition)
    {
        _targetPosition = newTargetPosition;
    }
    
    /// <summary>
    /// Makes the Unit / Character to Move.
    /// </summary>
    void UpdateUnitMove()
    {
        
        // Calculate the Distance... to see how close or far
        // ...is the Mouse Pointer -> from -> The Unit we want to Move().
        //
        if (Vector3.SqrMagnitude(transform.position - _targetPosition) > _sqrStoppingDistance)
        {
            Vector3 moveDirection = (_targetPosition - transform.position).normalized;

            // Move
            // 1- Translation:
            //
            transform.position += moveDirection * (_moveSpeed * Time.deltaTime);
            //
            // 2- Rotation
            //
            RotateUnitUsingVector3SlerpApproach(moveDirection);
            
            
            // 3- Update the Animator's Parameter: Start/Keep on Walking.
            //
            _unitAnimator.SetBool(_IS_WALKING_ANIMATOR_PARAMETER, true);
        }
        else
        {
            // Update the Animator's Parameter:  STOP  (Walking).
            //
            _unitAnimator.SetBool(_IS_WALKING_ANIMATOR_PARAMETER, false);
            
        }//End else of if (Vector3.SqrMagnitude...
    }

    
    #region Rotation: LERP vs. SLERP 
    
    /// <summary>
    /// Quaternions + Spherical Interpolation, SLERP, (Quaternions behind the Scenes):
    ///...it rotates in a better way (first Rotates, then Walks):
    /// </summary>
    /// <param name="moveDirection"></param>
    private void RotateUnitUsingVector3SlerpApproach(Vector3 moveDirection)
    {
        
        // Quaternions + Spherical Interpolation, SLERP, (Quaternions behind the Scenes):
        //...it rotates in a better way (first Rotates, then Walks):
        //
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * _rotateSpeed);
    }
    
    /// <summary>
    /// Linear Interpolation, LERP: Original, CodeMonkey's: No Quaternions involved.
    ///...it rotates, but the SLERP option is way better
    ///...(LERP produces this effect: first walks backwards 1 or 2 steps, like Michael Jackson...
    ///...then Rotates, then Walks the rest...):
    /// </summary>
    /// <param name="moveDirection"></param>
    private void RotateUnitUsingVector3LerpApproach(Vector3 moveDirection)
    {
        // Linear Interpolation, LERP: Original, CodeMonkey's:
        //
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * _rotateSpeed);
    }
    
    #endregion  Rotation: LERP vs. SLERP

    
    #region Movement Validations

    /// <summary>
    /// Get a List of the Valid places where the Unit/Character can Move to (i.e.: GridPosition(s)).
    /// This method cycles through the squares/Grids...(using FOR )... to get a list of the valid ones.
    /// </summary>
    /// <returns>Valid (GridPosition(s)) places where the Unit/Character can Move to, in this Turn.</returns>
    public List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        
        // Get the Unit's GridPosition
        //
        GridPosition unitGridPosition = _unit.GetGridPosition();
        
        // Cycle through the Rows and Columns (Cells in general) to find the Valid ones for Moving() in.. in this Turn
        //
        for (int x = -_maxMoveDistance; x <= _maxMoveDistance; x++)
        {
            for (int z = -_maxMoveDistance; z <= _maxMoveDistance; z++)
            {
                // Create a GridPosition to Validate it:
                //
                GridPosition offsetGridPosition = new GridPosition(x, z);

                // All Actions are attached to an Unit, so we can get a reference to an Unit from this class/object and then from Unit to -> its Position / Grid.
                // Test a given GridPosition, moving it a little bit using the 'offsetGridPosition' (summing it, +), so we can Validate it:
                //
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                
                // Validation:
                //
                // 1- "GridPosition" Must be inside the Grid System, not off-limits:
                //
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    // Not Valid: continue / SKIP: to the NEXT ITERATION.
                    continue;
                }
                //
                // 2- "GridPosition" Must be different from the current one.
                //
                if (unitGridPosition == testGridPosition)
                {
                    // Not Valid: Same Grid Position where the Player / Unit is already at.
                    // Skip to next iteration:
                    //
                    continue;
                }
                //
                // 3- "GridPosition" Must NOT be previously occupied.
                //
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Not Valid:
                    // Grid Position is already occupied with another Unit. Skip to next iteration:
                    //
                    continue;
                }
                
                
                // For Testing, Delete or Comment later:
                //
                Debug.Log(testGridPosition);

            } // End for 2
        }//End for 1
    
        return validGridPositionList;
    }


    #endregion Movement Validations
    
    #endregion My Custom Methods

}
