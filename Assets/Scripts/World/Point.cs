using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Point 
{
    // Properties
    public int X { get; set; }
    public int Y { get; set; }

    // Constructors
    #region
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    #endregion

    // Operator Definitions
    #region
    public static bool operator ==(Point first, Point second)
    {
        return first.X == second.X && first.Y == second.Y;
    }
    public static bool operator !=(Point first, Point second)
    {
        return first.X != second.X || first.Y != second.Y;
    }
    public static Point operator -(Point x, Point y)
    {
        return new Point(x.X - y.X, x.Y - y.Y);
    }
    #endregion

}
