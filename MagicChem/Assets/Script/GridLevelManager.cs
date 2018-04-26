using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLevelManager : MonoBehaviour {

    [SerializeField]
    private GameObject[] _tile;

    [SerializeField]
    private GameObject _startPosition;

    [SerializeField]
    [Tooltip("It will create a perfect sqaure grid based on the number. Ex. 8 will create 8x8 grid.")]
    [Range(2, 8)]
    private byte _gridSize = 2;

    private GameObject[,] _grid;

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
