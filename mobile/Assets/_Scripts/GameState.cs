using UnityEngine;

public class GameState : MonoBehaviour
{
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

    public PlayMode playMode = PlayMode.PLACE;

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
}


public enum PlayMode {
    PLACE,
    PLAY
}