using UnityEngine;

/// <summary>
/// This class works as the code behind the Prefab, which is the Visual Representation of: The 'Available Places' (i.e.: Squares/Cells in the Grid System), to move this Turn in the Game.
/// </summary>
public class GridSystemVisualSingle : MonoBehaviour
{

    #region Attributes

    [Tooltip("Mesh Renderer of this Visual 3D Quad, for Enabling and Disabling its Visibility via Code")]
    [SerializeField]
    private MeshRenderer _meshRenderer;


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
    
    /// <summary>
    /// Shows (sets as VISIBLE) the referenced 3D Mesh. 
    /// </summary>
    public void Show()
    {
        _meshRenderer.enabled = true;
    }
    
    /// <summary>
    /// Hides (sets as INVISIBLE) the referenced 3D Mesh. 
    /// </summary>
    public void Hide()
    {
        _meshRenderer.enabled = false;
    }

    #endregion My Custom Methods

}
