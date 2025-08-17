/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/

using System;
using UnityEngine;


public class Door : MonoBehaviour
{

    #region Attributes

    #region Delegates, Events

    private Action _onInteractComplete;

    #endregion Delegates, Events

        
    [Tooltip("Is Open \n Boolean Flag for the state of this GameObject.")]
    [SerializeField]
    private bool _isOpen;

    private GridPosition _gridPosition;

    #region Animations
    private Animator _animator;
    private static readonly int _IsOpen = Animator.StringToHash("IsOpen");

    private float _timer;

    private bool _isActive;

    #endregion Animations

    
    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>
    private void Awake()
    {
        _animator = GetComponent<Animator>();

    }//End Awake


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start()
    {
        // 1- Set DOOR at this position on the board (i.e.: GridPosition & GridObject) 
        //
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        //
        // Set DOOR at this position on the board (i.e.: GridPosition & GridObject)
        //
        LevelGrid.Instance.SetDoorAtGridPosition( _gridPosition, this );

        // 2- Check the DOOR state  (Open | Close)  and set it up accordingly
        //
        if (_isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
        
    }// End Start


    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        if (!_isActive)
        {
            return;
        }

        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            // Execute the Callback  (when the timer ends)          //)
            //
            _isActive = false;
            _onInteractComplete();
        }

    }//End Update

    #endregion Unity Methods


    #region My Custom Methods

    public void Interact(Action onInteractComplete)
    {
        // Set the Callback
        //
        this._onInteractComplete = onInteractComplete;
        _isActive = true;
        //
        // Set the TRANSITION Time on this Timer:
        //
        _timer = 0.5f;
        
        // 1- Check the new change in DOOR state  (Open | Close)
        //..and update it.
        //
        if (_isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
        
        
    }//End Interact
    
    private void OpenDoor()
    {
        _isOpen = true;
        
        // Animations:  Update
        //
        _animator.SetBool(_IsOpen, _isOpen);
        
        // Update the Pathfinding Object Nodes, because ...
        //..the Doorway: IS WALKABLE now.
        //
        Pathfinding.Instance.SetIsWalkableGridPosition(_gridPosition, true);
        
    }//End OpenDoor

    private void CloseDoor()
    {
        _isOpen = false;
        
        // Animations:  Update
        //
        _animator.SetBool(_IsOpen, _isOpen);
        
        // Update the Pathfinding Object Nodes, because ...
        //..the Doorway: IS NOT WALKABLE now.
        //
        Pathfinding.Instance.SetIsWalkableGridPosition(_gridPosition, false);


    }//End CloseDoor


    #endregion My Custom Methods

}
