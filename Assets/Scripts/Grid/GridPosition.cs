
public struct GridPosition
{
    
    public int x;
    public int z;


    
    #region Constructor

    public GridPosition(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    #endregion Constructor

    
    #region Methods

    /// <summary>
    /// Prints this Object's <code>GridPosition</code> data.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        //////Original: return base.ToString();
        //
        // 1- Our Custom Solution 1:
        //
        // return "x: " + x + "; z: " + z;
        //
        // 2- Alternative C# Syntax (String Interpolation):
        //
        ///// Original: return $"x: {x}; z: {z}";
        // 2.1-
        // return $"(x, y, z) = ( {x}, 0, {z} )";
        // 2.2-
        //
        return $"( {x}, 0, {z} )";
    }
    
    /// <summary>
    /// Sets all parameters (x, z): together.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    public void SetXZ(int x, int z)
    {
        this.x = x;
        this.z = z;
    }


    #endregion Methods
    
}