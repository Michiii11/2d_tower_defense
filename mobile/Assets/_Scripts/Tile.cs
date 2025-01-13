using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Tile : MonoBehaviour {
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _wall;

    public bool IsOccupied { get; private set; }

    private Rigidbody2D rb;

    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    private void Awake()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0); // Ensure the correct Z position

        Collider2D tileCollider = GetComponent<Collider2D>();
        if (tileCollider != null)
        {
            tileCollider.isTrigger = true;
        }
    }

    // Method to place a block on the tile
    public void PlaceBlock()
    {
        IsOccupied = true;
        _wall.SetActive(true);
        transform.position = new Vector3(transform.position.x, transform.position.y, -1); // Set Z position to -1
        _wall.GetComponent<Collider2D>().enabled = true; // Enable collider for the block to interact with the enemy
        _wall.GetComponent<Collider2D>().isTrigger = false; // Disable trigger mode to block the enemy
    }

    void OnMouseEnter() {
        _highlight.SetActive(true);
    }
 
    void OnMouseExit()
    {
        _highlight.SetActive(false);
    }

    void OnMouseDown(){
        if(GameState.Instance.playMode == PlayMode.PLACE){
            PlaceBlock();
        }
    }
}