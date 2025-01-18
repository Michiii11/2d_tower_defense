using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Transform _cam;

    [SerializeField] private int _width, _height;
    [SerializeField] private int _offsetLeft = 0, _offsetBottom = 0;

    private Dictionary<Vector2, Tile> _tiles;  // Lookup by grid coordinates
    private Dictionary<int, Enemy> _enemies = new Dictionary<int, Enemy>();

    void Start()
    {
        GenerateGrid();

        Debug.Log("Tile 3 3: " + GetTileAtWorldPosition(new Vector3(3,3,0)));
        
        //
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

    public void StartWave(){
        StartCoroutine(SpawnEnemiesForDuration(60f));
    }

    IEnumerator SpawnEnemiesForDuration(float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(1f); // Wait for 1 second
            elapsedTime += 1f;
        }
    }

    public void SpawnEnemy(){
        if(GameState.Instance.playMode != PlayMode.PLAY){
            return;
        }

        System.Random random = new System.Random();

        var spawnedEnemy = Instantiate(_enemyPrefab, new Vector3(random.Next(1, 9), -1f, 0), Quaternion.identity);
        spawnedEnemy.name = $"Enemy {_enemies.Count}";

        spawnedEnemy.Init(this);

        _enemies[_enemies.Count] = spawnedEnemy;
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

        //Debug.LogWarning($"No tile found at coordinates ({x}, {y})");
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

        //Debug.LogWarning("Tile not found in the grid.");
        return null; // Return null if the tile is not found
    }

    /// <summary>
    /// Get the tile at the specified world position.
    /// </summary>
    public Tile GetTileAtWorldPosition(Vector3 worldPosition)
    {
        // Convert world position to grid position
        Vector2 gridPosition = new Vector2(Mathf.RoundToInt(worldPosition.x), Mathf.RoundToInt(worldPosition.y));

        // Retrieve tile from the grid using the grid position
        if (_tiles.TryGetValue(gridPosition, out Tile tile))
        {
            return tile;
        }

        //Debug.LogWarning($"No tile found at world position {worldPosition}");
        return null;
    }
}