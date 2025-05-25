using Manager;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using UnityEngine.InputSystem;

namespace UI
{
    public class StartPanel : MonoBehaviour
    {
        public GameObject _objBtnStartGame;
        public GameObject _objName;
        public GameObject _objSetName;

        public InputField _inputFieldName;
        public Button _btnSetName;
        public Text _name;

        public GameObject _settingPanel;
        
        private void OnEnable()
        {
            Core.Log("StartPanel OnEnable");
            Init();
        }

        public void Init()
        {
            _btnSetName.onClick.AddListener(OnClickSetName);
            RefreshDisplayName();
            _settingPanel.SetActive(false);
        }
        
        public void RefreshDisplayName()
        {
            if (string.IsNullOrEmpty(PlayFabManager.Instance._displayName))
            {
                _objSetName.SetActive(true);
                _objName.SetActive(false);
                _objBtnStartGame.SetActive(false);
            }
            else
            {
                _objSetName.SetActive(false);
                _objName.SetActive(true);
                _objBtnStartGame.SetActive(true);
                _name.text = PlayFabManager.Instance._displayName;
            }
        }

        private void OnClickSetName()
        {
            if (string.IsNullOrEmpty(_inputFieldName.text))
            {
                Debug.Log("Name is empty");
                return;
            }
            PlayFabManager.Instance.SubmitDisplayName(_inputFieldName.text);
        }

        public void OnClickStart()
        {
            TransitionManager.Instance.Transition("Game_1");
            AudioManager.Instance.PlayBgm();
        }

        public void OnClickSetting()
        {
            _settingPanel.SetActive(true);
        }
        
        private void OnDisplayNameUpdateSuccess(UpdateUserTitleDisplayNameResult obj)
        {
            Debug.Log("[LOG] StartPanel 成功更新DisplayName!");
            string _displayName = obj.DisplayName;
        }
    }
}
