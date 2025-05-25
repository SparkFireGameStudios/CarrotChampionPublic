using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UI;
using UnityEngine;

public class PlayFabManager : SingletonMonobehaviour<PlayFabManager>
{
    public List<PlayerLeaderboardEntry> _scoreList = new List<PlayerLeaderboardEntry>();

    // 玩家展示的名字
    public string _displayName;
    
    [SerializeField]
    private StartPanel _startPanel;

    protected override void Awake()
    {
        base.Awake();
        
        if (_startPanel == null)
        {
            _startPanel = FindObjectOfType<StartPanel>();
        }
    }

    private void Start()
    {
        Login();
    }

    #region PlayFab Login

    private void Login()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId)){
            Debug.LogError("PlayFab TitleId is not set. Please set it in the PlayFab Editor.");
            return;
        }
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
        };
        // 获取玩家信息上传的参数
        request.InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
        {
            GetPlayerProfile = true,
            GetUserAccountInfo = true,
            GetUserData = true,
            GetUserInventory = true,
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnFailure);
        Debug.Log("[LOG] PlayFab 发送登录请求. "+
            "CustomId: " + request.CustomId +
            " TitleId: " + PlayFabSettings.staticSettings.TitleId);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("[LOG] 恭喜! 你成功登录了 PlayFab!");
        if(result.InfoResultPayload.PlayerProfile != null)
        {
            // 获取返回的玩家名字
            _displayName = result.InfoResultPayload.PlayerProfile.DisplayName;
            Debug.Log("[LOG] 玩家DisplayName: " + _displayName);
            _startPanel.RefreshDisplayName();
        }
        else
        {
            Debug.Log("[LOG] 玩家DisplayName: " + "未设置");
        }
        _startPanel.RefreshDisplayName();
    }
    
    #endregion

    #region Leadboard Update
    
    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "LevelScore_1",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdateSuccess, OnFailure);
        Debug.Log("[LOG] PlayFab 发送排行榜数据请求. "+
            "StatisticName: " + request.Statistics[0].StatisticName +
            " Value: " + request.Statistics[0].Value);
    }

    private void OnLeaderboardUpdateSuccess(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("[LOG] 成功更新排行榜数据!");
        GetLeaderboardData();
    }

    // 获取排行榜数据
    public void GetLeaderboardData()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "LevelScore_1",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboardSuccess, OnFailure);
    }

    private void OnGetLeaderboardSuccess(GetLeaderboardResult result)
    {
        _scoreList = new List<PlayerLeaderboardEntry>();
        Debug.Log("[LOG] 成功获取排行榜数据!");
        foreach (var item in result.Leaderboard)
        {
            Debug.Log($"Rank: {item.Position}, Player: {item.DisplayName}, Score: {item.StatValue}");
            _scoreList.Add(item);
        }
        
    }

    #endregion

    #region DisplayName

    public void SubmitDisplayName(string displayName)
    {
        Debug.Log("[LOG] PlayFab 发送DisplayName请求. "+
            "DisplayName: " + displayName);
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = displayName
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdateSuccess, OnFailure);
    }

    private void OnDisplayNameUpdateSuccess(UpdateUserTitleDisplayNameResult obj)
    {
        Debug.Log("[LOG] 成功更新DisplayName!");
        _displayName = obj.DisplayName;
        _startPanel.RefreshDisplayName();
    }

    #endregion
    
    private void OnFailure(PlayFabError error)
    {
        Debug.LogError("PlayFabManager OnFailure: " + error.GenerateErrorReport());
    }
}
