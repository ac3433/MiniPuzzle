using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircleData : AbstractTileData
{
    [SerializeField]
    private Direction[] _holeLocations = new Direction[2];

    private CircleData _nextCircleDataOutput;

    [SerializeField]
    private GameObject _lightUpSprite;
    [SerializeField]
    private bool _lightUp;

    private void Start()
    {
        List<Direction> exist = new List<Direction>();

        foreach(Direction hole in _holeLocations)
        {
            if (exist.Contains(hole))
                Debug.LogError(string.Format("GameObject: {0}\nScript: CircleData\nError: Duplicate direction.", gameObject.name));
            else
                exist.Add(hole);
        }
    }

    public void RotateClockwise()
    {
        for(int i = 0; i < _holeLocations.Length; i++)
        {
            switch(_holeLocations[i])
            {
                case Direction.North:
                    _holeLocations[i] = Direction.East;
                    break;
                case Direction.East:
                    _holeLocations[i] = Direction.South;
                    break;
                case Direction.South:
                    _holeLocations[i] = Direction.West;
                    break;
                case Direction.West:
                    _holeLocations[i] = Direction.North;
                    break;

            }
        }
    }

    public void RotateCounterClockwise()
    {
        for (int i = 0; i < _holeLocations.Length; i++)
        {
            switch (_holeLocations[i])
            {
                case Direction.North:
                    _holeLocations[i] = Direction.West;
                    break;
                case Direction.East:
                    _holeLocations[i] = Direction.North;
                    break;
                case Direction.South:
                    _holeLocations[i] = Direction.East;
                    break;
                case Direction.West:
                    _holeLocations[i] = Direction.South;
                    break;

            }
        }
    }

    public bool LightOn()
    {
        return _lightUp;
    }

    public void TurnLightOff()
    {
        _lightUp = false;
        _lightUpSprite.SetActive(false);
    }

    public void TurnLightOn()
    {
        _lightUp = true;
        _lightUpSprite.SetActive(true);
    }

    public void SetNextCircleData(CircleData data)
    {
        _nextCircleDataOutput = data;
    }


    public void RemoveNextCircleData()
    {
        TurnLightOff();
        if (_nextCircleDataOutput != null)
        {
            _nextCircleDataOutput.RemoveNextCircleData();
            _nextCircleDataOutput = null;
        }

    }

    public bool MatchConnectingHole(Direction directionFrom)
    {
        Direction opposite = DirectionExtensions.GetOppositeDirection(directionFrom);
        foreach(Direction hole in _holeLocations)
        {
            if (hole == opposite)
                return true; 
        }

        return false;
    }

    public void CheckSurrounding()
    {
        Direction missedHole = Direction.None;

        foreach(Direction hole in _holeLocations)
        {
            Point targetPoint = DirectionExtensions.GetDirectionPoint(hole);
            GameObject side = GridLevelManager.Instance.GetCircleData(targetPoint.X + GetPoint().X, targetPoint.Y + GetPoint().Y);
            if (side != null)
            {
                CircleData sideData = side.GetComponent<CircleData>();
                if(sideData != null)
                {
                    if (sideData.MatchConnectingHole(hole))
                    {
                        if (sideData.LightOn())
                        {
                            sideData.SetNextCircleData(this);
                            TurnLightOn();
                            if (missedHole != Direction.None)
                            {
                                targetPoint = DirectionExtensions.GetDirectionPoint(missedHole);
                                side = GridLevelManager.Instance.GetCircleData(targetPoint.X + GetPoint().X, targetPoint.Y + GetPoint().Y);
                                sideData = side.GetComponent<CircleData>();
                                sideData.CheckSurrounding();
                            }
                        }
                        else if (LightOn())
                        {
                            sideData.CheckSurrounding();
                        }
                        else
                            missedHole = hole;
                    }
                }
            }
        }
    }

}
