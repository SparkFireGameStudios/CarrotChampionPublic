using Manager;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    private void OnEnable()
    {
        int score = GameManager.Instance.GetScore();
        _scoreText.text = $"Score: {score}";
    }
        
    private void OnDisable()
    {
    }

    
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
