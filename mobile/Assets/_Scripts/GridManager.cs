using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _cam;
    private Dictionary<Vector2, Tile> _tiles;  // Lookup by grid coordinates
    [SerializeField] private int _width, _height;
    [SerializeField] private int _offsetLeft = 0, _offsetBottom = 0;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();

        for (int x = _offsetLeft; x < _width; x++)
        {
            for (int y = _offsetBottom; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y, 0), Quaternion.identity); // Set Z position to 0
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);

                // Store tile in the dictionary
                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        // Adjust the camera position and size
        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
        float aspectRatio = 9f / 16f;
        float gridAspectRatio = (float)_width / _height;
        if (gridAspectRatio > aspectRatio)
        {
            _cam.GetComponent<Camera>().orthographicSize = _width / 2f / aspectRatio;
        }
        else
        {
            _cam.GetComponent<Camera>().orthographicSize = _height / 2f;
        }
    }

    /// <summary>
    /// Get a tile at the specified grid coordinates.
    /// </summary>
    public Tile GetTileByCoordinates(int x, int y)
    {
        Vector2 gridPosition = new Vector2(x, y);

        if (_tiles.TryGetValue(gridPosition, out Tile tile))
        {
            return tile;
        }

        Debug.LogWarning($"No tile found at coordinates ({x}, {y})");
        return null;
    }

    /// <summary>
    /// Get the grid coordinates of a given tile.
    /// </summary>
    public Vector2? GetCoordinatesOfTile(Tile tile)
    {
        foreach (var kvp in _tiles)
        {
            if (kvp.Value == tile)
            {
                return kvp.Key; // Return the grid coordinates if the tile matches
            }
        }

        Debug.LogWarning("Tile not found in the grid.");
        return null; // Return null if the tile is not found
    }

    /// <summary>
    /// Get the tile at the specified world position.
    /// </summary>
    public Tile GetTileAtWorldPosition(Vector3 worldPosition)
    {
        // Convert world position to grid position
        Vector2 gridPosition = new Vector2(Mathf.FloorToInt(worldPosition.x), Mathf.FloorToInt(worldPosition.y));

        // Retrieve tile from the grid using the grid position
        if (_tiles.TryGetValue(gridPosition, out Tile tile))
        {
            return tile;
        }

        Debug.LogWarning($"No tile found at world position {worldPosition}");
        return null;
    }
}