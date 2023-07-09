using UnityEngine;

/// <summary>
/// A standard Player's (Human) Logic. <br />
/// It handles all the Player's ('Unit') Character's functionality
/// (Unit: it is a Prefab that is spawned in the Scene, (representing the Player's team players...)<br />
///..and it contains several scripts related to ACTIONS the player <br />
///..can execute in order to play in each turn, such as: MoveAction, ShootAction, SpinAction, etc.).
/// </summary>
public class UnitPlayer : Unit
{

    #region Attributes

    #endregion Attributes
    
    
    #region Unity Methods
    
    protected override void Awake()
    {
        // Execute the "Awake" of the (Abstract and Implemented here)
        // ..Parent of this class:
        //
        base.Awake();
        
        // Set the case that this is a Human PLAYER:
        //
        _isEnemy = false;

    }//End Awake()

    
    // Start is called before the first frame update
    // protected override void Start()
    // {
    //     // Execute the "Start" of the (Abstract and Implemented here)
    //     // ..Parent of this class:
    //     //
    //     base.Start();
    //     
    // }//End Start()
    

    // Update is called once per frame
    // protected override void Update()
    // {
    //     // Execute the "Update" of the (Abstract and Implemented here)
    //     // ..Parent of this class:
    //     //
    //     base.Update();
    //     
    // }//End Update()

    
    #endregion Unity Methods
    
    
    #region My Custom Methods

    #endregion My Custom Methods
}
