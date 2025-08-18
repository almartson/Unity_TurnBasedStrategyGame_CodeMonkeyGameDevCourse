/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/

using System;
using UnityEngine;


public class InteractSphere : MonoBehaviour, IInteractable
{

    #region Attributes

    [Tooltip("Green Material \n Material to be applied via InteractAction.cs")]
    [SerializeField]
    private Material _greenMaterial;
    
    [Tooltip("Red Material \n Material to be applied via InteractAction.cs")]
    [SerializeField]
    private Material _redMaterial;
    
    [Tooltip("Mesh Renderer \n Renderer to swap the its Material.")]
    [SerializeField]
    private MeshRenderer _meshRenderer;

    private bool _isGreen;

    private GridPosition _gridPosition;
    
    #region Delegates, Events

    private Action _onInteractionComplete;

    #endregion Delegates, Events

    private float _timer;
    private bool _isActive;

    
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
 
        // 1- Set the Interactable GameObject (e.g.: DOOR) at this position on the board (i.e.: GridPosition & GridObject) 
        //
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        //
        // Set the Interactable GameObject (e.g.: DOOR) at this position on the board (i.e.: GridPosition & GridObject)
        //
        LevelGrid.Instance.SetInteractableAtGridPosition( _gridPosition, this );

        // 2- Set the INITIAL STATE of the Interactable GameObject (e.g.: DOOR)
        //
        SetColorGreen();
        
    }//End Start


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
            _onInteractionComplete();
        }

    }//End Update

    #endregion Unity Methods


    #region My Custom Methods

    private void SetColorGreen()
    {
        _isGreen = true;
        _meshRenderer.sharedMaterial = _greenMaterial;
    }

    private void SetColorRed()
    {
        _isGreen = false;
        _meshRenderer.sharedMaterial = _redMaterial;
    }

    /// <summary>
    /// IInteractable signature Method.
    /// </summary>
    /// <param name="onInteractionComplete"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Interact(Action onInteractionComplete)
    {
        // Set the Callback
        //
        this._onInteractionComplete = onInteractionComplete;
        _isActive = true;
        //
        // Set the TRANSITION Time on this Timer:
        //
        _timer = 0.5f;
        
        if (_isGreen)
        {
            SetColorRed();
        }
        else
        {
            SetColorGreen();
        }
    }//End Interact()

    #endregion My Custom Methods
}
