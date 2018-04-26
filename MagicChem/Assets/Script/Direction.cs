using UnityEngine;
using System.Collections;

public enum Direction
{
    None, North, East,South,West
}

public class DirectionMethod
{
    Point north = new Point() { X = 0, Y = 1 };
    Point south = new Point() { X = 0, Y = -1 };
    Point east = new Point() { X = 1, Y = 0 };
    Point west = new Point() { X = -1, Y = 0 };
    Point none = new Point() { X = 0, Y = 0 };

    public Point GetDirectionPoint(Direction dir)
    {
        switch (dir)
        {
            case Direction.North:
                return north;
            case Direction.East:
                return east;
            case Direction.West:
                return west;
            case Direction.South:
                return south;
            default:
                return none;
        }
    }
}
