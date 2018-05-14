using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct NodeSize
{
    public GameObject Node;
    public GameObject InactiveNode;
    public byte Size;
}
[Serializable]
public class SpecialNode
{
    public GameObject Node;
    public int X;
    public int Y;
    public Direction Direction;
}

public class GridLevelManager : MonoBehaviour {

    #region Singleton
    private static GridLevelManager _instance;
    //Used only once to ensure when one thread have access to create the instance
    private static readonly object _Lock = new object();

    public static GridLevelManager Instance
    {
        get
        {
            //thread safe!
            lock (_Lock)
            {
                if (_instance != null)
                    return _instance;
                GridLevelManager[] instances = FindObjectsOfType<GridLevelManager>();
                //see if there are any already more instance of this
                if (instances.Length > 0)
                {
                    //yay only 1 instance so give it back
                    if (instances.Length == 1)
                        return _instance = instances[0];

                    //remove all other instance of it other than the 1st one
                    for (int i = 1; i < instances.Length; i++)
                        Destroy(instances[i]);
                    return _instance = instances[0];
                }

                GameObject manage = new GameObject("GridLevelManager");
                manage.AddComponent<GameController>();

                return _instance = manage.GetComponent<GridLevelManager>();
            }
        }
    }
    #endregion

    [Header("Grid Info")]
    [SerializeField]
    private GameObject[] _tile;
    [SerializeField]
    private GameObject _startGridPosition;

    [SerializeField]
    [Tooltip("It will create a perfect sqaure grid based on the number. Ex. 8 will create 8x8 grid.")]
    [Range(2, 8)]
    private byte _gridSize = 2;

    [SerializeField]
    private SpecialNode _startNode;
    [SerializeField]
    private SpecialNode _endNode;

    [SerializeField]
    [Tooltip("Add spacing between tiles.")]
    [Range(1, 3)]
    private float _tileOffset = 1;

    [Header("Node Info")]
    [SerializeField]
    private GameObject _startNodePosition;
    [SerializeField]
    private NodeSize[] _circleNodeSize;
    [SerializeField]
    [Tooltip("Add spacing between node.")]
    [Range(1, 3)]
    private float _nodeOffset = 1;

    private GameObject[,] _grid;
    private GameObject[,] _circleNode;

    void Start () {
        if (_tile == null || _tile.Length < 1)
        {
            Debug.LogError(string.Format("GameObject: {0}\nScript: LevelManager\nError: Missing tile game object.", gameObject.name));
            return;
        }

        if (_startGridPosition == null)
        {
            Debug.LogError(string.Format("GameObject: {0}\nScript: LevelManager\nError: Missing start tile position game object.", gameObject.name));
            return;
        }
        if (_circleNodeSize == null || _circleNodeSize.Length < 1)
        {
            Debug.LogError(string.Format("GameObject: {0}\nScript: LevelManager\nError: Missing circle node game object.", gameObject.name));
            return;
        }

        if (_startNodePosition == null)
        {
            Debug.LogError(string.Format("GameObject: {0}\nScript: LevelManager\nError: Missing start node position game object.", gameObject.name));
            return;
        }
        //invisible padding for the start and end
        _gridSize++;
        _circleNode = new GameObject[_gridSize+1, _gridSize+1];

        CreateInteractableTile();
        CreateSpecialTile();
        CreateCircleNode();

        
    }

    private void CreateInteractableTile()
    {
        _grid = new GameObject[_gridSize, _gridSize];

        for (int y = 1; y < _gridSize; y++)
        {
            for (int x = 1; x < _gridSize; x++)
            {
                if (_grid[x, y] == null)
                    PlaceTile(x, y, 0);
            }
        }

    }

    //add validation later.
    private void CreateSpecialTile()
    {
        GameObject startNode = Instantiate(_startNode.Node);
        startNode.transform.parent = _startGridPosition.transform;
        startNode.transform.localPosition = new Vector3((TileSize() * _startNode.X * _nodeOffset), -(TileSize() * _startNode.Y * _nodeOffset), 1);
        _circleNode[_startNode.X, _startNode.Y] = startNode;
        startNode.name = _startNode.Node.name;

        GameObject endNode = Instantiate(_endNode.Node);
        endNode.transform.parent = _startGridPosition.transform;
        endNode.transform.localPosition = new Vector3((TileSize() * _endNode.X * _nodeOffset), -(TileSize() * _endNode.Y * _nodeOffset), 1);
        _circleNode[_endNode.X, _endNode.Y] = endNode;
        endNode.name = _endNode.Node.name;

    }

    //could be refactor
    private void CreateCircleNode()
    {
        for(int i = 0; i < _circleNodeSize.Length; i++)
        {
            GameObject inactiveNode = Instantiate(_circleNodeSize[i].InactiveNode);
            inactiveNode.transform.parent = _startNodePosition.transform;
            inactiveNode.transform.localPosition = new Vector3(0, -(TileSize() * i * _nodeOffset), 0);
            inactiveNode.name = _circleNodeSize[i].InactiveNode.name;

            for (int j = 0; j < _circleNodeSize[i].Size; j++)
            {
                GameObject newNode = Instantiate(_circleNodeSize[i].Node);
                newNode.transform.parent = _startNodePosition.transform;
                newNode.transform.localPosition = new Vector3(0, -(TileSize() * i * _nodeOffset), 0);
                newNode.name = _circleNodeSize[i].Node.name;
            }
            
        }
    }

    public byte GridSize() { return _gridSize; }

    public float TileSize() { return _tile[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }

    public void PlaceTile(int x, int y, int tileID)
    {
        if (x < 0 || x > _gridSize)
        {
            Debug.LogError(string.Format("GameObject: {0}\nScript: LevelManager\nError: X coordinate is out of bound.", gameObject.name));
            return;
        }

        if (y < 0 || y > _gridSize)
        {
            Debug.LogError(string.Format("GameObject: {0}\nScript: LevelManager\nError: Y coordinate is out of bound.", gameObject.name));
            return;
        }

        if (tileID < 0 || tileID > _tile.Length)
        {
            Debug.LogError(string.Format("GameObject: {0}\nScript: LevelManager\nError: tile id is out of bound.", gameObject.name));
            return;
        }

        GameObject newTile = Instantiate(_tile[tileID]);
        newTile.transform.parent = _startGridPosition.transform;
        newTile.transform.position = new Vector3(_startGridPosition.transform.position.x + (TileSize() * x) * _tileOffset, _startGridPosition.transform.position.y - (TileSize() * y) * _tileOffset);
        newTile.name = string.Format("Tile {0}x{1}", x, y);
        TileData tileData = newTile.GetComponent<TileData>();
        tileData.SetPoint(x, y);

        _grid[x, y] = newTile;
    }

    public GameObject GetCircleData(Point p)
    {
        return _circleNode[p.X, p.Y];
    }

    public GameObject GetCircleData(int x, int y)
    {
        if (x >= 0 && x < _gridSize && y >= 0 && y < _gridSize)
            return _circleNode[x, y];
        return null;
    }

    public void SetCircleData(GameObject node, Point p)
    {
        _circleNode[p.X, p.Y] = node;
    }

    public void PrintMap()
    {
        string msg = "";
        for(int i = 0; i < _gridSize + 1; i++)
        {
            for(int j = 0; j < _gridSize + 1; j++)
            {
                if (_circleNode[j, i] != null)
                    msg = string.Concat(msg, "1\t");
                else
                    msg = string.Concat(msg, "0\t");
            }
            msg = string.Concat(msg, "\n");
        }
        Debug.Log(msg);
    }
}
