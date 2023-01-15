/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{
    #region Attributes

    [Tooltip("Reference to the Prefab: ActionButtonUI")]
    [SerializeField]
    private Transform _actionButtonUIPrefab;

    [Tooltip("Reference to the UI Layout Container GameObject in the scene: ActionButtonContainer")]
    [SerializeField]
    private Transform _actionButtonContainerTransform;

    
    #region GUI Buttons' List (created in runtime in the game)
    
    [Tooltip("GUI Buttons' List (created in runtime in the game)")]
    private List<ActionButtonUI> _actionButtonUIList;
    
    #endregion GUI Buttons' List (created in runtime in the game)
    
    
    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>
    private void Awake()
    {
        // 1- Create the UI Button List
        //
        _actionButtonUIList = new List<ActionButtonUI>();
    }
    

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start()
    {
        // Access the Event:  When an Unit is Selected (using the Mouse Click as input)
        // Subscribe to the Event:  (..we want to Trigger an (UI) Event / Action / Method in the future if someone selects an Unit...): the update of the current's Actions in UI:
        //
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        
        // Subscribe to the Event:  When changing the currently selected: ACTION
        //..(by Mouse Clicking on the UI Button for an ACTION:  MOVE, SPIN, GRENADE, etc...)
        //
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        
        //  Create the UI Buttons  (for the current selected UNIT):
        //
        CreateUnitActionUIButtons();
        //
        // Update the ACTION  UI Buttons VISUAL Green outline COLOR  (for the current selected UNIT):
        //
        UpdateSelectedVisual();
    }


    /// <summary>
    /// Update is called once per frame
    /// </summary>

    #endregion Unity Methods


    #region My Custom Methods

    /// <summary>
    /// Creates UI Buttons for whatever Unit / Character is Selected.
    /// Previously: Deletes (removes) its own former UI Buttons.
    /// </summary>
    private void CreateUnitActionUIButtons()
    {
        // 0- Destroy / Clean: All (current) UI (Action) Buttons:
        //
        // 0.2- Cache the Transforms of the Children of: _actionButtonContainerTransform
        //
        Transform[] actionButtonContainerTransformChildren = _actionButtonContainerTransform.GetComponentsInChildren<Transform>(true);
        //
        // Length:
        //
        int actionButtonContainerTransformChildrenCount = actionButtonContainerTransformChildren.Length;
        //
        for (int i = 1 /* Zero is the Parent GameObject */; i < actionButtonContainerTransformChildrenCount; i++)
        {
            // Destroy the UI (Child) GameObject:
            //
            Destroy( actionButtonContainerTransformChildren[i].gameObject );
            
        }//End for
        //
        // // PLUS+:   CodeMonkey's way:
        // //
        // foreach (Transform buttonTransform in _actionButtonContainerTransform)
        // {
        //     Destroy(buttonTransform.gameObject);
        //     
        // }//End foreach
        
        // 0.3- Clear the List<ActionButtonUI>()  of UI Buttons:
        //
        _actionButtonUIList.Clear();
        
        
        // 1- Get the selected Unit / Character
        //
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        
        // 2- Ask the 'selected' Unit:  What's YOUR List of ACTIONS?
        // Array[] Length:
        //
        int baseActionArrayLenght = selectedUnit.GetBaseActionArray().Length;
        //
        for (int i = 0; i < baseActionArrayLenght; i++)
        {
            // Instantiate in the UI GameObject (Layout Group) Container ('_actionButtonContainerTransform') a '_actionButtonUIPrefab' per each item (SetBaseAction Child/Extend) in the Array[] (SetBaseAction[]):  selectedUnit.GetBaseActionArray():
            //
            Transform actionButtonUITransform = Instantiate(_actionButtonUIPrefab, _actionButtonContainerTransform);
            //
            // selectedUnit.GetBaseActionArray()[i]
            
            // 3- We are setting the Name of the Button: it will be tne Name of the ACTION (i.e.: the Name of the ACTION Class it refers to):
            // 3.1- Getting a reference to the 'ActionButtonUI.cs' script (component in GameObject):
            //
            ActionButtonUI actionButtonUI = actionButtonUITransform.GetComponent<ActionButtonUI>();
            //
            // 3.2- Setting the Base Action Attribute of this class  (using 'selectedUnit.GetBaseActionArray()[i]', as it represents every Type of ACTION this UNIT has:
            //
            actionButtonUI.SetBaseAction( selectedUnit.GetBaseActionArray()[i] );
            
            // 4- We link a pointer to the UI Button GameObject, for reference:
            //
            _actionButtonUIList.Add(actionButtonUI);

        }//End for

    }//End CreateUnitActionUIButtons()


    #region Listening to EVENTS:
    
    #region Selecting a Unit (with a mouse click)

    /// <summary>
    /// Event to Trigger when an Unit / Character is selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        // Run the Method: of this class
        // Trigger:  'CreateUnitActionUIButtons()'
        //
        CreateUnitActionUIButtons();
        
        // Update the UI Button's VISUAL:   with an OUTLINE COLOR:
        //
        UpdateSelectedVisual();
    }

    #endregion Selecting a Unit (with a mouse click)

    
    #region Selecting an ACTION (with a mouse click)

    /// <summary>
    /// Event to Trigger when an ACTION   is Mouse-Clicked
    /// (i.e.: a click on:  an UI Action  Button)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {

        // Update the UI Button's VISUAL:   with an OUTLINE COLOR:
        //
        UpdateSelectedVisual();
    }

    #endregion Selecting an ACTION (with a mouse click)
    
    #endregion Listening to EVENTS:
    
    
    #region Updating UI / GUI Action Buttons' VISUALS    
    
    /// <summary>
    /// Enables / Disables a Green Outline color (that is an GUI Image on the Prefab)...
    /// ..to Show to the user which ACTION is currently Enabled to be used.
    /// </summary>
    private void UpdateSelectedVisual()
    {
        
        // We tell each UI Button to UPDATE ITSELF
        // A- CodeMonkey's way (non-performant...)
        //
        // foreach (ActionButtonUI actionButtonUI in _actionButtonUIList)
        // {
        //     
        //     // Update the GUI Visual for the newly Selected UI Action Button:  an OUTLINE GREEN COLOR:
        //     //
        //     actionButtonUI.UpdateSelectedVisual();
        //     
        // }//End foreach
        //
        // B- AlMartson's way:   Performant: TODO: check this Optimization here.
        //
        int listLength = _actionButtonUIList.Count;
        //
        for (int i = 0; i < listLength; i++)
        {

            // Update the GUI Visual for the newly Selected UI Action Button:  an OUTLINE GREEN COLOR:
            //
            _actionButtonUIList[ i ].UpdateSelectedVisual();

        }//End foreach
        
    }//End UpdateSelectedVisual
    
    #endregion Updating UI / GUI Action Buttons' VISUALS

    #endregion My Custom Methods

}
