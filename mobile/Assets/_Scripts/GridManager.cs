using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _cam;
    private Dictionary<Vector2, Tile> _tiles;
    [SerializeField] private int _width, _height;

    void Start()
    {
        GenerateGrid();

        Debug.Log("test");
    }

    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y, 0), Quaternion.identity); // Set Z position to 0
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);

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
}