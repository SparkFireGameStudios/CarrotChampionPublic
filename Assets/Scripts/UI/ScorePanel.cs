using Manager;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ScorePanel : MonoBehaviour
    {
        public Text scoreText;
        private void OnEnable()
        {
            EventHandler.GetPointEvent += UpdateScore;
        }
        
        private void OnDisable()
        {
            EventHandler.GetPointEvent -= UpdateScore;
        }

        private void UpdateScore(int obj)
        {
            scoreText.text = $"SCORE: {obj}";   
        }

        public void OnClickBack()
        {
            MapManager.Instance?.ClearMap();
            TransitionManager.Instance?.Transition("Start");
            AudioManager.Instance?.PlayBgm();
        }
        
    }
}