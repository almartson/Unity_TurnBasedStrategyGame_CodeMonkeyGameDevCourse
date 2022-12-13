using System;
using UnityEngine;


public abstract class BaseAction : MonoBehaviour
{

    #region Attributes

    [Tooltip("Sets this 'Action' as: Enabled  or  Disabled.")]
    [SerializeField]
    protected bool _isActive = false;
    
    [Tooltip("Reference to the Unit / Character to apply the 'Action' to...")]
    protected Unit _unit;


    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>
    protected virtual void Awake()
    {
        // Get the Unit / Character this Script is attached to in the Unity Editor.
        //
        _unit = GetComponent<Unit>();
        
        
    }//End Awake()


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>



    /// <summary>
    /// Update is called once per frame
    /// </summary>


    #endregion Unity Methods
    

    #region My Custom Methods





    #endregion My Custom Methods

}
