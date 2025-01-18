using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GridManager gridManager;

    public void StartGame(){
        GameState.Instance.playMode = PlayMode.PLAY;
        gridManager.StartWave();
    }

    public void SelectWall(){
        GameState.Instance.selectedItem = SelectedItem.WALL;
    }

    public void SelectJaguar(){
        GameState.Instance.selectedItem = SelectedItem.JAGUAR;
    }

    public void SelectElephant(){
        GameState.Instance.selectedItem = SelectedItem.ELEPHANT;
    }
}
