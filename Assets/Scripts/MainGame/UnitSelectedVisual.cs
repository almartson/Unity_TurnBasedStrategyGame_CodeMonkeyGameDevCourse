using System;
using UnityEngine;

/// <summary>
/// This Class Updates the GUI (colors)  to Highlight the Player (Unit) that is currently Selected.<br />
/// Observer Pattern: This is an Observer / Subscriber.<br />
/// It listens to the EVENTS fired by the Subject / Publisher.<br />
/// </summary>
public class UnitSelectedVisual : MonoBehaviour
{

    #region Attributes
    
    [SerializeField] private Unit _unit;

    [Tooltip("Mesh corresponding to the Visual's Feedback (GUI) for the Event: Selecting an 'Unit' (i.e.: Character)")]
    private MeshRenderer _meshRenderer;
    
    #endregion Attributes


    #region Unity Methods

    private void Awake()
    {
        // Initialize: Mesh corresponding to the Visual's Feedback (GUI)
        //...for the Event: Selecting an 'Unit' (i.e.: Character).
        //
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        // We Subscribe to the Event of the Subject / PUBLISHER on Start(). Reason: to give it a chance (to the Publisher) to get initialized in its Awake() function Time.
        // Susbcription TIME!:
        //
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChange;
        
        //...Update the Game Logic: Visual feedback.
        //
        UpdateVisual();
    }

    #endregion Unity Methods
    
    
    #region My Custom Methods
    
    #region Observer Pattern: As a Subscriber / Events Listener

    
    /// <summary>
    /// Actions this Observer/Subscriber must do when it Subscribes to an Event.
    /// Game Logic: is Triggered here.
    /// </summary>
    private void UnitActionSystem_OnSelectedUnitChange(object sender, EventArgs empty)
    {
        // We received the Event, now:
        //...Update the Game Logic: the Visual feedback.
        //
        UpdateVisual();
    }

    
    /// <summary>
    /// Game Logic: that Updates the GUI (colors)  to Highlight the Player (Unit) that is currently Selected.
    /// </summary>
    private void UpdateVisual()
    {
        // Check if the Selected 'Unit' (...Character) is Different from the previous one:
        //
        if (UnitActionSystem.Instance.GetSelectedUnit() == _unit)
        {
            // Set the (originally GREEN-COLORED) Quad: Visible,
            //...Visually Highlighting the Player that is Selected
            //...Visual (GUI)'s feedback: 
            //
            _meshRenderer.enabled = true;
        }
        else
        {
            // Set the (originally GREEN-COLORED) Quad: Invisible.
            //
            _meshRenderer.enabled = false;
        }
    }//End Method

    
    /// <summary>
    /// Handles unsubscribing from the Events / Listeners 
    /// </summary>
    private void OnDestroy()
    {
        // Unsubscribe from the Events / Listeners, to avoid any MissingReferenceException 
        //
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChange;
        
    }//End OnDestroy()

    #endregion Observer Pattern: As a Subscriber / Events Listener
    
    #endregion My Custom Methods
    
}
