using System;
using UnityEngine;

public class Unit : MonoBehaviour
{

    #region Attributes
    
    /// <summary>
    /// Keeping track of the CURRENT GridPosition of this Unit.
    /// </summary>
    private GridPosition _gridPosition;

    
    #region Action's List
    
    /// <summary>
    /// LIST of ALL ACTIONS that can be performed (by this type of Unit / Character of the game). 
    /// </summary>
    private BaseAction[] _baseActionArray;
    
    
    /// <summary>
    /// Reference to the MoveAction script to make the Units / Characters able to execute the Action: 'Move' (Walk, etc...). 
    /// </summary>
    private MoveAction _moveAction;
    
    /// <summary>
    /// Reference to the SpinAction script to make the Units / Characters able to execute the Action: 'Spin' (Rotate). 
    /// </summary>
    private SpinAction _spinAction;

    #endregion Action's List
    
    
    /// <summary>
    /// Mouse Position
    /// </summary>
    private Vector3 _mousePosition = Vector3.zero;
    /// <summary>
    /// Property Accessor to Private Field:
    /// Public Getter & Setter for _mousePosition
    /// </summary>
    public Vector3 MousePosition { get => _mousePosition; set => _mousePosition = value; }
    
    #endregion Attributes
    
    
    #region Unity Methods
    
    private void Awake()
    {
        // Get the Actions & the List of ALL Actions:        
        //
        _moveAction = GetComponent<MoveAction>();
        _spinAction = GetComponent<SpinAction>();
        //
        // Get the List of Actions:
        // NOTE: GetComponentSSSS<TYPE>: gets ALL Components of TYPE=<SetBaseAction>
        // ..(or that are children / Extend from <SetBaseAction>)... in this GameObject.
        //
        _baseActionArray = GetComponents<BaseAction>();
    }

    
    // Start is called before the first frame update
    private void Start()
    {
        // Setting the Unit on the LevelGrid (Script) Object, on THIS GridPosition...
        //
        // We need LevelGrid to be a Singleton, for accessing a Script outside of
        //...THIS Prefab.
        // SOLUTION: The same with 'UnitActionSystem'
        //
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        //
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
    }
    

    // Update is called once per frame
    void Update()
    {
        // Setting the Unit on the LevelGrid (Script) Object, on THIS GridPosition...
        //
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        //
        // Compare the Old Position with the new...
        //
        if (newGridPosition != _gridPosition)
        {
            
            // This Unit changed its (Grid) Position
            //
            LevelGrid.Instance.UnitMovedGridPosition(this, _gridPosition, newGridPosition);
            
            // Update also the CURRENT GridPosition ref here
            //
            _gridPosition = newGridPosition;
        }
        
    }//End Update()

    
    #endregion Unity Methods
    
    
    #region My Custom Methods
    
    #region Actions
   
    /// <summary>
    /// Getter for <code>SetBaseAction[]</code>
    /// </summary>
    /// <returns></returns>
    public BaseAction[] GetBaseActionArray()
    {
        return _baseActionArray;
    }
    
    /// <summary>
    /// Getter for <code>MoveAction</code>
    /// </summary>
    /// <returns></returns>
    public MoveAction GetMoveAction()
    {
        return _moveAction;
    }
    
    /// <summary>
    /// Getter for <code>SpinAction</code>
    /// </summary>
    /// <returns></returns>
    public SpinAction GetSpinAction()
    {
        return _spinAction;
    }
    
    #endregion Actions
    
    
    /// <summary>
    /// Getter for <code>GridPosition</code>
    /// </summary>
    /// <returns></returns>
    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }
    
    

    #endregion My Custom Methods
    
}
