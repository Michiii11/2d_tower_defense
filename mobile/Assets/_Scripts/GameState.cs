using UnityEngine;
using TMPro;

public class GameState : MonoBehaviour
{
    public TextMeshProUGUI enemiesPassedThroughCounter;
    public TextMeshProUGUI coinsCounter;

    public PlayMode playMode = PlayMode.PLACE;
    public SelectedItem selectedItem = SelectedItem.WALL;
    private int _enemiesPassedThrough;
    private int _coins = 1000;

    public int EnemiesPassedThrough
    {
        get => _enemiesPassedThrough;
    }

    public void enemyPassedThrough(){
        _enemiesPassedThrough++;
        enemiesPassedThroughCounter.text = $"Gegner in der Schatzkammer: {EnemiesPassedThrough}";
    }

     public int Coins
    {
        get => _coins;
        set
        {
            _coins = value;
            coinsCounter.text = $"Münzen: {_coins}";
        }
    }

    private static GameState _instance;
    public static GameState Instance
    {
        get
        {
            if (_instance == null)
            {
                // Try to find an existing instance in the scene
                _instance = Object.FindAnyObjectByType<GameState>();

                // If none exists, create a new GameObject with the GameState component
                if (_instance == null)
                {
                    GameObject newGameState = new GameObject("GameState");
                    _instance = newGameState.AddComponent<GameState>();
                }
            }
            return _instance;
        }
        private set => _instance = value;
    }

    private void Awake()
    {
        // Ensure that there's only one instance of this class
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); // Destroy the duplicate GameObject
        }
        else
        {
            _instance = this;

            // Optionally, persist this object across scenes
            DontDestroyOnLoad(gameObject);
        }
    }

    public static void DrawDebugDot(Vector2 position)
    {
        // Define a small offset for the "dot"
        float dotSize = 0.1f;

        // Convert the Vector2 to Vector3 (with z = 0) for Debug.DrawLine
        Vector3 pos3D = new Vector3(position.x, position.y, 0);

        // Draw a tiny cross to represent the dot
        Debug.DrawLine(pos3D - Vector3.right * dotSize, pos3D + Vector3.right * dotSize, Color.red, 0.1f);
        Debug.DrawLine(pos3D - Vector3.up * dotSize, pos3D + Vector3.up * dotSize, Color.red, 0.1f);
    }
}


public enum PlayMode {
    PLACE,
    PLAY
}

public enum SelectedItem {
    WALL,
    JAGUAR,
    ELEPHANT
}