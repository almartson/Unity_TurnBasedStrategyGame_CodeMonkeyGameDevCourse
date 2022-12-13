using System;
using System.Collections.Generic;
using UnityEngine;


public class MoveAction : BaseAction
{
    #region Attributes

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
    
    [Tooltip("The Tolerance number to accept that a value is = ZERO")]
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
    protected override void Awake()
    {
        // Execute the "Awake" of the (Abstract and Implemented here)
        // ..Parent of this class:
        //
        base.Awake();
        
        #region Utils
        
        // Done: Misc Optimization: Calculating the (accepted) Square Min Distance.
        //
        _sqrStoppingDistance = _stoppingDistance * _stoppingDistance;
        
        #endregion Utils
        
        
        // Initialize Target Position to this Script's base GameObject.
        //
        _targetPosition = this.transform.position;
        
        
        #region Stop all Movement Action & Animation
        
        // Stop Move() MoveAction + STOP ANIMATION:
        //
        StopMoveAction();
        
        #endregion Stop all Movement Action & Animation
    }


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>



    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        // Check whether this "Action" is Active or Inactive:
        //
        if (!_isActive)
        {
            // Its DISABLED so...
            // End it here  (for THIS FRAME)
            //
            return;
        }
        
        // Update the Unit Movement (Walking...)
        //
        UpdateUnitMove();
    }

    
    #endregion Unity Methods
    

    #region My Custom Methods

    #region Stop all Movement Action & Animation
    
    /// <summary>
    /// Stops Move() and Movement Animations.
    /// </summary>
    public void StopMoveAction()
    {
        // Update the Animator's Parameter:  STOP  (Walking).
        //
        _unitAnimator.SetBool(_IS_WALKING_ANIMATOR_PARAMETER, false);
            
        // Set this "Action" as DISABLED
        //
        _isActive = false;
    }
    
    #endregion Stop all Movement Action & Animation
    
    
    /// <summary>
    /// Moves the Unit / Character to the specified (x, y, z) Position (Grid).
    /// </summary>
    /// <param name="gridPosition"></param>
    public void Move(GridPosition gridPosition)
    {
        // Get the WorldPosition, based on a "GridPosition" as Input.
        //
        _targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        
        // Set this "Action" as DISABLED
        //
        _isActive = true;
    }
    
    
    /// <summary>
    /// Moves the Unit / Character to the specified (x, y, z) Position (Grid).
    /// </summary>
    /// <param name="newTargetPosition"></param>
    [Obsolete("This method is deprecated. Use: 'public void Move(GridPosition gridPosition)' instead", true)]
    public void Move(Vector3 newTargetPosition)
    {
        _targetPosition = newTargetPosition;
        
        // Set this "Action" as ENABLED
        //
        _isActive = true;
    }
    
    /// <summary>
    /// Makes the Unit / Character to Move.
    /// </summary>
    private void UpdateUnitMove()
    {
        // Calculate the Vector3 of the DIRECTION of Movement:
        //
        Vector3 moveDirection = (_targetPosition - transform.position).normalized;
        
        // Calculate the Distance... to see how close or far
        // ...is the Mouse Pointer -> from -> The Unit we want to Move().
        //
        if (Vector3.SqrMagnitude(transform.position - _targetPosition) > _sqrStoppingDistance)
        {
            // Move
            // 1- Translation:
            //
            transform.position += moveDirection * (_moveSpeed * Time.deltaTime);

            // 3- Update the Animator's Parameter: Start/Keep on Walking.
            //
            _unitAnimator.SetBool(_IS_WALKING_ANIMATOR_PARAMETER, true);
        }
        else
        {
            // Stop Move() MoveAction + STOP ANIMATION:
            //
            StopMoveAction();

        }//End else of if (Vector3.SqrMagnitude...
        //
        // 2- Rotation
        //
        RotateUnitUsingVector3SlerpApproach(moveDirection);

    }//End UpdateUnitMove()

    
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
    /// Tells you whether the user/Player's selection (Grid Position) is Valid to Move to. It uses several criteria, such as: the Position must be unoccupied, must be inside the Grid System, etc. 
    /// </summary>
    /// <returns>True or False to: is the selected "GridPosition" Valid??</returns>
    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        // Get a List of ALL Valid (Grid) Positions:
        //
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        //
        // Return: Does this VALID LIST contain also the requested GridPosition (this Method's INPUT)? gridPosition 
        //
        return validGridPositionList.Contains(gridPosition);
    }
    
    
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
                
                // Finally, Conclusion: Add the Tested & Valid GridPosition to the Local VALID List
                //
                validGridPositionList.Add(testGridPosition);

            } // End for 2
        }//End for 1
    
        return validGridPositionList;
    }


    #endregion Movement Validations
    
    #endregion My Custom Methods

}
