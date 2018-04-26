using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField]
    private GameObject[] _tile;

    [SerializeField]
    private GameObject _startPosition;

    [SerializeField]
    [Tooltip("It will create a perfect sqaure grid based on the number. Ex. 8 will create 8x8 grid.")]
    [Range(2, 8)]
    private byte _gridSize = 2;

    private GameObject[,] _grid;
    private GameObject[,] _circleNode;

    void Start () {
        if (_tile == null)
        {
            Debug.LogError(string.Format("GameObject: {0}\nScript: LevelManager\nError: Missing tile game object.", gameObject.name));
            return;
        }

        if (_startPosition == null)
        {
            Debug.LogError(string.Format("GameObject: {0}\nScript: LevelManager\nError: Missing start position game object.", gameObject.name));
            return;
        }

        CreateInteractableTile();

        _circleNode = new GameObject[_gridSize, _gridSize];
    }

    private void CreateInteractableTile()
    {
        _grid = new GameObject[_gridSize, _gridSize];

        for (int y = 0; y < _gridSize; y++)
        {
            for (int x = 0; x < _gridSize; x++)
            {
                if (_grid[x, y] == null)
                    PlaceTile(x, y, 0);
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
        newTile.transform.parent = _startPosition.transform;
        newTile.transform.position = new Vector3(_startPosition.transform.position.x + (TileSize() * x), _startPosition.transform.position.y - (TileSize() * y));
        newTile.name = string.Format("Tile {0}x{1}", x, y);

        _grid[x, y] = newTile;
    }
}
