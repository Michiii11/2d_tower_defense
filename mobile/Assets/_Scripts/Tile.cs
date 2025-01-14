using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Tile : MonoBehaviour {
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _wall;

    public Rigidbody2D rb;

    public bool IsOccupied { get; private set; }

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

        // Ensure the Wall GameObject has a Collider2D component
        if (_wall != null && _wall.GetComponent<Collider2D>() == null)
        {
            _wall.AddComponent<BoxCollider2D>(); // Adjust to fit your object
        }

        // Ensure the Tile GameObject has a Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.bodyType = RigidbodyType2D.Static; // Set Rigidbody2D to Static
    }

    // Method to place a block on the tile
    public void PlaceBlock()
    {
        IsOccupied = true;
        _wall.SetActive(true);
        _wall.tag = "Block"; // Set the tag to "Block"
        rb.tag = "Block";
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