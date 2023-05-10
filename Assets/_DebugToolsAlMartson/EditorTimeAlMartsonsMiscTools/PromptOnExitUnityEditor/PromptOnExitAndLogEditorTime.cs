/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/
using System.Globalization;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Shows a window to Prompt (YES / NO) before Exiting the Unity Editor.
/// </summary>
public static class PromptOnExitAndLogEditorTime
{
    #region Attributes
    
    /// <summary>
    /// Latest Prompt On Exit Decision.
    /// </summary>
    private static bool _exitGameDecision = false;
    //
    public static bool ExitGameDecision => _exitGameDecision;
    
    #endregion Attributes
    
    
    #region Methods
    
    #region Unity Methods
    
    /// <summary>
    /// Subscribe to A Delegate Event, to Prompt the User with a Nice Window before Closing the Unity Editor.
    /// </summary>
    [InitializeOnLoadMethod]
    static void OnLoad()
    {
        EditorApplication.wantsToQuit -= OnBeforeQuitting;
        EditorApplication.wantsToQuit += OnBeforeQuitting;

    }// End OnLoad
    
    #endregion Unity Methods
    
    
    #region Custom Methods

    /// <summary>
    /// Delegate Function that shows a Window before Quitting the Unity Editor.
    /// </summary>
    /// <returns></returns>
    private static bool OnBeforeQuitting()
    {
        // Shows a Window before Quitting the Unity Editor.
        //
        _exitGameDecision = ExitGameConfirmation();
        //
        // Return the User's decision:  Close Unity... or Not?
        //
        return _exitGameDecision;

    }//End OnBeforeQuitting
    
    /// <summary>
    /// Shows a Prompt Window before Quitting the Unity Editor.
    /// </summary>
    /// <returns></returns>
    private static bool ExitGameConfirmation()
    {
        bool decision = EditorUtility.DisplayDialog(
            "Exit the Unity3D Editor", // title
            "Quit the Unity3D Editor?", // description
            "Quit", // OK button
            "Cancel" // Cancel button
        );

        if (decision)
        {
            Debug.Log($"Quitting the Editor... \n... being up for: {EditorApplication.timeSinceStartup.ToString(CultureInfo.InvariantCulture)} seconds.");
        }
        else
        {
            Debug.Log($"Continue with the Unity Editor... \n... being up for: {EditorApplication.timeSinceStartup.ToString(CultureInfo.InvariantCulture)} seconds.");
        }

        return decision;

    }// End ExitGameConfirmation
    
    #endregion Custom Methods

    #endregion Methods
    
}// End Class PromptOnExitAndLogEditorTime
