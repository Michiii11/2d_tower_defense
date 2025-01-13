using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }

    public PlayMode playMode = PlayMode.PLACE;

    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
        
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this;
            Debug.Log("init");
        }
    }
}

public enum PlayMode {
    PLACE,
    PLAY
}