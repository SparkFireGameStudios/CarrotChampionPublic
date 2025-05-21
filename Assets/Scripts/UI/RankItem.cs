using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

public class RankItem : MonoBehaviour
{
    public GameObject rankImage1;
    public GameObject rankImage2;
    public GameObject rankImage3;
    public Text nameText;
    public Text scoreText;
  
    public void SetData(PlayerLeaderboardEntry playerLeaderboardEntry)
    {
        int rank = playerLeaderboardEntry.Position + 1;
        string name = playerLeaderboardEntry.DisplayName;
        int score = playerLeaderboardEntry.StatValue;

        // ???????
        rankImage1.SetActive(false);
        rankImage2.SetActive(false);
        rankImage3.SetActive(false);
        
        rankImage1.gameObject.SetActive(rank == 1);
        rankImage2.gameObject.SetActive(rank == 2);
        rankImage3.gameObject.SetActive(rank == 3);
        
        nameText.text = name;
        scoreText.text = score.ToString();

        if (name.Equals(PlayFabManager.Instance._displayName))
        {
            nameText.color = Color.yellow;
        }
    }
}
