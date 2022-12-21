/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using System;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    #region Attributes

    [Tooltip("Reference to the Prefab: ActionButtonUI")]
    [SerializeField]
    private Transform _actionButtonUIPrefab;

    [Tooltip("Reference to the UI Layout Container GameObject in the scene: ActionButtonContainer")]
    [SerializeField]
    private Transform _actionButtonContainerTransform;

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
        // Access the Event:  When an Unit is Selected (using the Mouse Click as input)
        // Subscribe to the Event:  (..we want to Trigger an (UI) Event / Action / Method in the future if someone selects an Unit...): the update of the current's Actions in UI:
        //
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        
        //  Create the UI Buttons  (for the current selected UNIT):
        //
        CreateUnitActionUIButtons();
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
            // Instantiate in the UI GameObject (Layout Group) Container ('_actionButtonContainerTransform') a '_actionButtonUIPrefab' per each item (BaseAction Child/Extend) in the Array[] (BaseAction[]):  selectedUnit.GetBaseActionArray():
            //
            Instantiate(_actionButtonUIPrefab, _actionButtonContainerTransform);
            //
            // selectedUnit.GetBaseActionArray()[i]
        
        }//End for

    }//End CreateUnitActionUIButtons()


    #region Listening to EVENTS: Selecting a Unit (with a mouse click)

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
    }
    

    #endregion Listening to EVENTS: Selecting a Unit (with a mouse click)


    #endregion My Custom Methods

}
