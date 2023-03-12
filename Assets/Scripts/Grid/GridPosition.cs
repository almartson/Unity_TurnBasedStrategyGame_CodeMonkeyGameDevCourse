using System;

/// <summary>
/// Mathematical 2-dimensional (x, z) Vector that contains Positional Data. <br />
/// Reference: Check this project's UML Class Diagram. <br />
/// </summary>
[System.Serializable]
public struct GridPosition : IEquatable<GridPosition>
{
    #region Attributes
    
    public int x;
    public int z;

    #endregion Attributes

    
    #region Constructor

    public GridPosition(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    #endregion Constructor

    
    #region Methods

    #region Misc

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

    #endregion Misc

    
    #region Getters and Setters
    
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
    
    #endregion Getters and Setters
    
    
    #region Equality. Automatically, Generated Metods

    public bool Equals(GridPosition other)
    {
        return this == other;
    }

    public override bool Equals(object obj)
    {
        return obj is GridPosition other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, z);
    }
    
    #endregion Equality. Automatically, Generated Metods

    
    #region == and != Operators
    
    /// <summary>
    /// Override Definition of the == Operator for this Struct. 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator ==(GridPosition a, GridPosition b)
    {
        return a.x == b.x && a.z == b.z;
    }

    /// <summary>
    /// Override Definition of the != Operator for this Struct. 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return !(a == b);
    }

    #endregion == and != Operators
    

    #region + and - Operators

    /// <summary>
    /// Override Definition of the + Operator for this Struct. 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static GridPosition operator +(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x + b.x, a.z + b.z);
    }
    
    /// <summary>
    /// Override Definition of the - Operator for this Struct. 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static GridPosition operator -(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x - b.x, a.z - b.z);
    }
    
    #endregion + and - Operators

    #endregion Methods
    
}