using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // µ¥Àý
    public static UIManager Instance;
    
    public GameObject ScorePanel;
    public GameObject GameOverPanel;
    public GameObject RankPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        EventHandler.GameOverEvent += OnGameOver;
    }

    private void OnDisable()
    {   
        EventHandler.GameOverEvent -= OnGameOver;
    }
    
    public void OnGameOver()
    {
        ScorePanel.SetActive(false);
        GameOverPanel.SetActive(true);
    }
    
    public void ShowRankPanel()
    {
        ScorePanel.SetActive(false);
        GameOverPanel.SetActive(false);
        RankPanel.SetActive(true);
    }
}
