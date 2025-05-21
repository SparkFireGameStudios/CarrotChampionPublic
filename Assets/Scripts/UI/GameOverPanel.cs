using Manager;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    
    public void OnClickRestart()
    {
        Debug.Log("[LOG] OnClickRestart()");
        MapManager.Instance?.ClearMap();
        TransitionManager.Instance?.Transition("Game_1");
        AudioManager.Instance?.PlayBgm();
    }

    public void OnClickRank()
    {
        Debug.Log("[LOG] OnClickRank()");
        UIManager.Instance.ShowRankPanel();
    }

    public void OnClickHome()
    {
        Debug.Log("[LOG] OnClickHome()");
        MapManager.Instance?.ClearMap();
        TransitionManager.Instance?.Transition("Start");
        AudioManager.Instance?.PlayBgm();
    }
}
