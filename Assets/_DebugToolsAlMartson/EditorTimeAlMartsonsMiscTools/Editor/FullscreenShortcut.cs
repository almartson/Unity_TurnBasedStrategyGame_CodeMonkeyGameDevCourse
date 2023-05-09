using UnityEditor;
using UnityEngine;

/// <summary>
/// Is there a keyboard shortcut to maximize the Game window in Unity in Play Mode? <br/>
/// [ https://stackoverflow.com/questions/54431635/is-there-a-keyboard-shortcut-to-maximize-the-game-window-in-unity-in-play-mode#:~:text=Ctrl%20%2B%20Space%20maximizes%20most%20windows,2018%20when%20in%20edit%20mode. ] <br/>
/// More information here:   https://support.unity.com/hc/en-us/articles/210719486-Enter-Play-Mode-with-F5-key<br/>
/// </summary>
[InitializeOnLoad]
static class FullscreenShortcut
{

#if UNITY_EDITOR

    static FullscreenShortcut()
    {
        EditorApplication.update += Update;
    }


    static void Update()
    {
        if (EditorApplication.isPlaying && ShouldToggleMaximize())
        {
            EditorWindow.focusedWindow.maximized = !EditorWindow.focusedWindow.maximized;
        }
    }

    /// <summary>
    /// This is the Keyboard Shortcut to Maximize the current focused Window.
    /// </summary>
    /// <returns></returns>
    private static bool ShouldToggleMaximize()
    {
        return Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Space);
    }

    
    // /// <summary>
    // /// This is the Keyboard Shortcut.
    // /// </summary>
    // /// <returns></returns>
    // private static bool ShouldOpenFileWithAKey()
    // {
    //     return Input.GetKey(KeyCode.RightControl) && Input.GetKey(KeyCode.O);   //Input.GetKeyDown(KeyCode.Return);
    // }
    
    
    // [OnOpenAsset]
    // public static bool OnOpenAsset(int instanceId, int line)
    // {
    //     UnityEngine.Object obj = EditorUtility.InstanceIDToObject(instanceId);
    //
    //     if (obj is (MonoBehaviour or MonoScript))
    //     {
    //         
    //         Debug.Log("Script Asset Opened");
    //         return true;
    //     }
    //
    //     return false;
    // }
    
    // [MenuItem("AssetDatabase/Manually Check Textures")]
    // static void OpenAssetExample()
    // {
    //     for (var i = 0; i < 10; i++)
    //     {
    //         var texturePath = AssetDatabase.LoadMainAssetAtPath($"Assets/Textures/GroundTexture{i}.jpg");
    //         if(!EditorUtility.DisplayDialog($"Open next texture", $"Open GroundTexture{i}.jpg texture?", "Yes", "Cancel"))
    //             break;
    //         AssetDatabase.OpenAsset(texturePath);
    //     }
    //
    //     var texturePath = AssetDatabase.OpenAsset(0);
    //
    // }
#endif

}