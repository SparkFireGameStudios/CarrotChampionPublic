using System.Collections.Generic;
using Manager;
using PlayFab.ClientModels;
using UnityEngine;

public class RankPanel : MonoBehaviour
{
    public GameObject rankItemPrefab;
    public Transform rankContent;
    void Start()
    {
        // 获取最新数据
        PlayFabManager.Instance?.GetLeaderboardData();
        
        // 读取数据
        List<PlayerLeaderboardEntry> playerScores = GameManager.Instance?.GetPlayerScoresData();
        
        // 显示数据
        foreach (var playerLeaderboardEntry in playerScores)
        {
            GameObject rankItem = Instantiate(rankItemPrefab, rankContent);
            rankItem.GetComponent<RankItem>().SetData(playerLeaderboardEntry);
        }
        
    }
    
    public void OnClickRestart()
    {
        Debug.Log("[LOG] OnClickRestart()");
        MapManager.Instance?.ClearMap();
        TransitionManager.Instance?.Transition("Game_1");
        AudioManager.Instance?.PlayBgm();
    }
    
    public void OnClickHome()
    {
        Debug.Log("[LOG] OnClickHome()");
        MapManager.Instance?.ClearMap();
        TransitionManager.Instance?.Transition("Start");
        AudioManager.Instance?.PlayBgm();
    }
    
}
