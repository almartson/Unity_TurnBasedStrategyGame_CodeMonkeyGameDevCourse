using UnityEngine;

public class Testing : MonoBehaviour
{

    // [Tooltip("Visuals of Grid System, for Visual Debugging in the Unity Editor")]
    // [SerializeField]
    // private Transform gridDebugObjectPrefab;
    //
    // private GridSystem _gridSystem;
    //
    // private GridPosition _gridPosition;
    
    
    /// <summary>
    /// Start is called before the first frame update
    /// TODO: Note ORIGINALLY CodeMonkey used an Start() 
    /// </summary>
    void Awake()
    {
        // _gridSystem = new GridSystem(10, 10, 2f);
        // _gridPosition = new GridPosition(5, 7);
        // //
        // // Create the GameObject that will hold a Visual Representation of the Grid System. Calling the Constructor:
        // //
        // _gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
        
        // Debug.Log(_gridPosition);
    }


    private void Update()
    {
        // Not Optimized call:
        //
        /////Debug.Log(_gridSystem.GetGridPosition(MouseWorld.GetPosition()));
        //
        // Optimized call:
        //
        // _gridSystem.GetGridPosition(ref _gridPosition, MouseWorld.GetPosition());
        // //
        // Debug.Log( _gridPosition );
    }
}
