using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
 
public class Tile : MonoBehaviour {
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _wall;
    [SerializeField] private GameObject _jaguar;
    [SerializeField] private GameObject _elephant;

    public Rigidbody2D rb;

    public TileType tileType { get; private set; }

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

    void OnMouseEnter() {
        _highlight.SetActive(true);
    }
 
    void OnMouseExit() {
        _highlight.SetActive(false);
    }

    void OnMouseDown(){
        if (EventSystem.current.IsPointerOverGameObject()){
            return;
        }

        if(GameState.Instance.playMode == PlayMode.PLACE){
            switch(GameState.Instance.selectedItem){
                case SelectedItem.WALL:
                    if(tileType == TileType.WALL)
                        RemoveWall();
                    else
                        PlaceWall();
                    break;
                case SelectedItem.JAGUAR:
                    if(tileType == TileType.JAGUAR){
                        _jaguar.SetActive(false);
                        tileType = TileType.EMPTY;
                    }
                    else{
                        _jaguar.SetActive(true);
                        tileType = TileType.JAGUAR;
                    }
                    break;
                case SelectedItem.ELEPHANT:
                    if(tileType == TileType.ELEPHANT){
                        _elephant.SetActive(false);
                        tileType = TileType.EMPTY;
                    }
                    else{
                        _elephant.SetActive(true);
                        tileType = TileType.ELEPHANT;
                    }
                    break;
            }
        }
    }

    public void PlaceWall()
    {
        tileType = TileType.WALL;
        _wall.SetActive(true);
        _wall.tag = "Block"; // Set the tag to "Block"
        rb.tag = "Block";
        transform.position = new Vector3(transform.position.x, transform.position.y, -1); // Set Z position to -1
        _wall.GetComponent<Collider2D>().enabled = true; // Enable collider for the block to interact with the enemy
        _wall.GetComponent<Collider2D>().isTrigger = false; // Disable trigger mode to block the enemy
    }

    public void RemoveWall()
    {
        tileType = TileType.EMPTY; // Set to default or another appropriate type
        _wall.SetActive(false);
        _wall.tag = "Untagged"; // Reset the tag
        rb.tag = "Untagged";
        transform.position = new Vector3(transform.position.x, transform.position.y, 0); // Reset Z position
        _wall.GetComponent<Collider2D>().enabled = false; // Disable collider
        _wall.GetComponent<Collider2D>().isTrigger = true; // Set trigger mode back to default
    }
}

public enum TileType
{
    EMPTY,
    WALL,
    JAGUAR,
    ELEPHANT
}