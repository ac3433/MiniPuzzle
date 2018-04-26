using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircleData : AbstractTileData
{
    [SerializeField]
    private Direction[] HoleLocations = new Direction[2];

    private void Start()
    {
        List<Direction> exist = new List<Direction>();

        foreach(Direction hole in HoleLocations)
        {
            if (exist.Contains(hole))
                Debug.LogError(string.Format("GameObject: {0}\nScript: CircleData\nError: Duplicate direction.", gameObject.name));
            else
                exist.Add(hole);
        }
    }

    public void RotateClockwise()
    {
        transform.Rotate(Vector3.forward, -90);
        for(int i = 0; i < HoleLocations.Length; i++)
        {
            switch(HoleLocations[i])
            {
                case Direction.North:
                    HoleLocations[i] = Direction.East;
                    break;
                case Direction.East:
                    HoleLocations[i] = Direction.South;
                    break;
                case Direction.South:
                    HoleLocations[i] = Direction.West;
                    break;
                case Direction.West:
                    HoleLocations[i] = Direction.North;
                    break;

            }
        }
    }

    public void RotateCounterClockwise()
    {
        transform.Rotate(Vector3.forward, 90);

        for (int i = 0; i < HoleLocations.Length; i++)
        {
            switch (HoleLocations[i])
            {
                case Direction.North:
                    HoleLocations[i] = Direction.West;
                    break;
                case Direction.East:
                    HoleLocations[i] = Direction.North;
                    break;
                case Direction.South:
                    HoleLocations[i] = Direction.East;
                    break;
                case Direction.West:
                    HoleLocations[i] = Direction.South;
                    break;

            }
        }
    }

}
