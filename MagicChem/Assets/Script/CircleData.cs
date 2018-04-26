using UnityEngine;
using System.Collections;

public class CircleDataSide
{
    public Direction Direction { set; get; }
    public bool Hole { set; get; }
}

public class CircleData : AbstractTileData
{
    CircleDataSide[] CircleDataSides;
    [SerializeField]
    private Direction[] HoleLocations = new Direction[2];

    private void Awake()
    {
        CircleDataSides = new CircleDataSide[4];
        CircleDataSides[0] = new CircleDataSide() { Direction = Direction.North, Hole = false };
        CircleDataSides[1] = new CircleDataSide() { Direction = Direction.East, Hole = false };
        CircleDataSides[2] = new CircleDataSide() { Direction = Direction.West, Hole = false };
        CircleDataSides[3] = new CircleDataSide() { Direction = Direction.South, Hole = false };
    }

    private void Start()
    {
        foreach(Direction hole in HoleLocations)
        {
            foreach(CircleDataSide side in CircleDataSides)
            {
                if(side.Direction == hole)
                {
                    if(side.Hole)
                        Debug.LogWarning(string.Format("GameObject: {0}\nScript: CircleData\nError: Duplicate hole direction", gameObject.name));
                    side.Hole = true;
                }
            }
        }
    }

    public void RotateClockwise()
    {
        transform.Rotate(Vector3.forward, -90);
        foreach(CircleDataSide side in CircleDataSides)
        {
            switch(side.Direction)
            {
                case Direction.North:
                    side.Direction = Direction.East;
                    break;
                case Direction.East:
                    side.Direction = Direction.South;
                    break;
                case Direction.South:
                    side.Direction = Direction.West;
                    break;
                case Direction.West:
                    side.Direction = Direction.North;
                    break;

            }
        }
    }

    public void RotateCounterClockwise()
    {
        transform.Rotate(Vector3.forward, 90);

        foreach (CircleDataSide side in CircleDataSides)
        {
            switch (side.Direction)
            {
                case Direction.North:
                    side.Direction = Direction.West;
                    break;
                case Direction.East:
                    side.Direction = Direction.North;
                    break;
                case Direction.South:
                    side.Direction = Direction.East;
                    break;
                case Direction.West:
                    side.Direction = Direction.South;
                    break;

            }
        }
    }


    
}
