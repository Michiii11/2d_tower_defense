using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void StartGame(){
        GameState.Instance.playMode = PlayMode.PLAY;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
