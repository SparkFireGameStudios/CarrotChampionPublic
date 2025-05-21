using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    public List<PlayerLeaderboardEntry> _playerScores = new List<PlayerLeaderboardEntry>();

    private int _score;
    private string _dataPath;

    // 是否使用PlayFab的分数
    public bool _isGlobalScore = false;

    protected override void Awake()
    {
        base.Awake();
        // 本地存储的地址
        _dataPath = Application.persistentDataPath + "/rank.json";

        _playerScores = GetPlayerScoresData();

    }

    public void OnEnable()
    {
        EventHandler.GetPointEvent += OnGetPoint;
        EventHandler.GameOverEvent += OnGameOver;
    }

    public void OnDisable()
    {
        EventHandler.GetPointEvent -= OnGetPoint;
        EventHandler.GameOverEvent -= OnGameOver;
    }

    void Start()
    {
        _score = 0;
    }

    // 读取保存的数据记录
    public List<PlayerLeaderboardEntry> GetPlayerScoresData()
    {
        if (_isGlobalScore)
        {
            return PlayFabManager.Instance._scoreList;
        }
        else
        {
            if (File.Exists(_dataPath))
            {
                string json = File.ReadAllText(_dataPath);
                return JsonConvert.DeserializeObject<List<PlayerLeaderboardEntry>>(json);
            }
        }

        return new List<PlayerLeaderboardEntry>();
    }

    private void OnGetPoint(int score)
    {
        _score = score;
    }

    // 是否发送分数到PlayFab
    private void SendScore(int score)
    {
        if (_isGlobalScore)
        {
            // 发送数据到PlayFab
            PlayFabManager.Instance.SendLeaderboard(_score);
        }
        else
        {
            Debug.Log("[LOG] 游戏结束 得分:" + score);
            // if (_playerScores.Contains(_score))
            // {
            //     return;
            // }

            // 保存本次得分数据到json
            PlayerLeaderboardEntry _score = new PlayerLeaderboardEntry
            {
                DisplayName = PlayFabManager.Instance._displayName,
                StatValue = score,
            };
            _playerScores.Add(_score);

            _playerScores.Sort(
                (x, y) => x.StatValue.CompareTo(y.StatValue));
            _playerScores.Reverse();

            // 将数据写入json
            string json = JsonConvert.SerializeObject(_playerScores);
            File.WriteAllText(_dataPath, json);
        }
    }

    private void OnGameOver()
    {
        SendScore(_score);
        Debug.Log("[LOG] 游戏结束 得分:" + _score);
        
        MapManager.Instance?.ResetPosition();
    }
}