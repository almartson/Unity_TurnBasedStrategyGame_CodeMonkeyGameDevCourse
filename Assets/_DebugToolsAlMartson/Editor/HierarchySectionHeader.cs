/* NOTE: Modified Unity C# Script Template by Alec AlMartson...
...on Path:   /PathToUnityHub/Unity/Hub/Editor/UNITY_VERSION_FOR_EXAMPLE__2020.3.36f1/Editor/Data/Resources/ScriptTemplates/81-C# Script-NewBehaviourScript.cs
*/

using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

/// <summary>
/// This Beautifies the Editor... with some colors:
/// 
/// Taken from: https://community.gamedev.tv/t/a-bit-of-organization-in-my-hierarchy/213354/4
/// Courtesy of Thomas Brush (with minor tweaks)
///
/// Example of use:
/// Now when you add an empty GameObject, you can start its name with // to turn it into a header...
/// Write the Following as the name of a GAMEOBJECT used as a GameObject's Separator:
///
/// //[#00ff00]--- UI | GUI ---
///
/// Where: // Means:  Special Colored Separator
///         [#HEXADECIMAL COLOR, EXAMPLE: #ff0000  for  RED ]  EXAMPLE:  //[#ff0000]
///         The rest is the NAME or string that will be shown.
/// 
/// It is important to note what Hugo said, though: Make sure these ‘headers’ are at (0,0,0) or the children might behave funny.
///
/// These scripts (except the first one from Thomas) were slammed together very quickly and may very well break your editor, so use them at your own risk. There are things that could be better (like the way I parsed the color from the name) but it is what it is. It’s not like I’m going to get fired or anything…
///
/// Edit I have gone much further and created a MonoBehaviour with color properties and stuff that define the color for the header and removes the need for weird naming conventions, and had this script read those instead, but I haven’t actually used it in a project.
///
///     Late Edit
/// I cleaned up the color tag code a bit (if you want to consider regular expressions as ‘cleanup’). Also removed the ‘c=’ bit from the color tag because it’s not really necessary
/// </summary>
[InitializeOnLoad]
public static class HierarchySectionHeader
{
    #region Attributes

    static Color HEADER_BACKGROUND_COLOR = new Color(80f / 255f, 80f / 255f, 80f / 255f);

    static HierarchySectionHeader() => EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;

    static Regex _regex = new Regex("^(?<tag>//)?(?:\\[(?<color>#[0-9A-F]{6}){1}\\])?(?<title>.*)$", RegexOptions.IgnoreCase);


    #endregion Attributes


    #region Unity Methods

    /// <summary>
    /// Awake is called before the Start calls round
    /// </summary>



    /// <summary>
    /// Start is called before the first frame update
    /// </summary>



    /// <summary>
    /// Update is called once per frame
    /// </summary>


    #endregion Unity Methods
    

    #region My Custom Methods

    
    static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (gameObject == null) return;

        var match = _regex.Match(gameObject.name);

        // Did we match the regex? If not, ignore and treat like any other object
        if (!match.Success) return;
        // Does it have our tag? If not, ignore and treat like any other object
        if (string.IsNullOrWhiteSpace(match.Groups["tag"].Value)) return;

        // We have a 'header'. Try to parse color, if any
        var color = HEADER_BACKGROUND_COLOR;
        if (ColorUtility.TryParseHtmlString(match.Groups["color"].Value, out Color parsed))
            color = parsed;

        // Grab the title
        var title = match.Groups["title"].Value;

        // Draw the 'header'
        EditorGUI.DrawRect(selectionRect, color);
        EditorGUI.DropShadowLabel(selectionRect, title);
    }

    
    // /// <summary>
    // /// Original code from: Thomas Brush
    // /// </summary>
    // /// <param name="instanceID"></param>
    // /// <param name="selectionRect"></param>
    // static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    // {
    //     var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
    //     if (gameObject == null) return;
    //     if (!gameObject.name.StartsWith("//")) return;
    //
    //     EditorGUI.DrawRect(selectionRect, HEADER_BACKGROUND_COLOR);
    //     EditorGUI.DropShadowLabel(selectionRect, gameObject.name.Substring(2));
    // }

    #endregion My Custom Methods

}
