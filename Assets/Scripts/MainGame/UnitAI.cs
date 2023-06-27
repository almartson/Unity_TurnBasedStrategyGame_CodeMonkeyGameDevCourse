using UnityEngine;

/// <summary>
/// A standard Enemy's (A.I.) Brain Logic. <br />
/// It handles all the A.I. behaviour for an ENEMY Unit's functionality
/// (Unit: it is a Prefab that is spawned in the Scene, (representing the Player's team players...) <br />
///..and it contains several scripts related to ACTIONS the player <br />
///..can execute in order to play in each turn, such as: MoveAction, ShootAction, SpinAction, etc.).
/// </summary>
public class UnitAI : Unit
{
    #region Attributes
    
    #region A.I. - More Complex A.I. Decisions
    
    // 5 pixels of spacing here, in the Unity Inspector:
    [Space(5)]
    [Header("A.I. - More Complex A.I. Decisions")]

    [Tooltip("'Aggressiveness' variable for the 'Enemy A.I. ACTIONs' and some (Player and A.I. attacks):  Higher values would provoke the 'Unit' to become aggressive towards the other Team Player's characters (Units).\n\n * NOTE: \n\n 1- This value is normalized, in a base to 1.0f. \n\n 2- 1.0f is: A Total value, equals 100%... for 'Aggressiveness' + 'Defensiveness' (Player Stats). \n\n 3- Defensiveness = [1.0 - Aggro], in a base to 1.0f.")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float _aggroStat = 0.5f;
    //
    /// <summary>
    /// Property Accessor for Field:  _aggroStat
    /// </summary>
    public float AggroStat
    {
        get => _aggroStat;
        private set => _aggroStat = value;
    }
    
    /// <summary>
    /// Total value, equals 100%... for 'Aggressiveness' + 'Defensiveness' (Player Stats)..\n\n * NOTE: Defensiveness = [this value - Aggro], in a base to 1.0f.
    /// </summary>
    private const float _TOTAL_STAT = 1.0f;
    
    [Tooltip("(DEBUG READONLY VALUE:) Maximum number of 'GridPosition's  ( STEPS ) that this particular A.I. is willing to take towards its selected Goal... (executing only 'MoveActions'); this is a value used when executing a 'More Complex A.I. Algorithm', for Characters with HIGH values of AGGRO, that are allowed to a 'Hunter' or any 'Enemy A.I.' that wants to chase after another (that means, its _aggroStat value is high).\n\n * NOTE: \n 1- This value will change after this Unit gets to its 'Target' and Kills it (or Dies). \n 2- Calculation:  _aggroStat * (EnemyAI._MAXIMUM_GRID_POSITIONS_IN_TOTAL_ALLOWED_TO_ANY_AGGRO_AI_CHASER) * (EnemyAI._randomPossibilityOfChasingTheTargetInThisTurn).  \n\n Usual values: around 25 when it is Aggro'ing (Hunting).... \n 3- Maximum (usual) value: 100.")]
    [SerializeField]
    private int _maximumGridPositionsThisAIisWillingToTakeTowardsAChosenGoal = 0;
    //
    /// <summary>
    /// Property Accessor for Field:  _maximumGridPositionsIAmWillingToTakeTowardsAChosenGoal
    /// </summary>
    public int MaximumGridPositionsIAmWillingToTakeTowardsAChosenGoal
    {
        get => _maximumGridPositionsThisAIisWillingToTakeTowardsAChosenGoal;
        set => _maximumGridPositionsThisAIisWillingToTakeTowardsAChosenGoal = value;
    }
    
    #endregion A.I. - More Complex A.I. Decisions
    
    #endregion Attributes
    
    
    #region Unity Methods
    
    // protected override void Awake()
    // {
    //     // Execute the "Awake" of the (Abstract and Implemented here)
    //     // ..Parent of this class:
    //     //
    //     base.Awake();
    //     
    // }//End Awake()

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
    
    #region Actions
   
    #region POINTS  - for every Action

    /// <summary>
    /// Checks if this Unit / Character is able to spend ActionPoints on a series of consecutive  SELECTED  Actions.
    /// </summary>
    /// <param name="baseActionsArray"></param>
    /// <returns></returns>
    public bool CanSpendActionPointsToTakeAChainOfActions(BaseAction[] baseActionsArray)
    {
        // Array Lenght
        //
        int baseActionsArrayLength = baseActionsArray.Length;

        // Auxiliary variable to save a copy of the  ActionPoints
        //
        int actionPoints = _actionPoints;
        
        // Loop through all the Actions in the List (Array):
        //
        for (int i = 0; i < baseActionsArrayLength; i++)
        {

            // Cache of an Item
            //
            BaseAction baseAction = baseActionsArray[i];
            
            // Use (Experimentally)  the  ActionPoints
            //
            actionPoints -= baseAction.GetActionPointsCost(); 


            // Validate that the ActionPoints  are > 0
            //
            if ( actionPoints <= 0 )
            {

                return false;
            }

        }//End for

        // If it could endure all the tests, then it is TRUE: it can Spend the ActionPoints
        //
        return true;

    }//End CanSpendActionPointsToTakeAChainOfActions

    #endregion POINTS  - for every Action
    
    #endregion Actions
    
    #endregion My Custom Methods
}
