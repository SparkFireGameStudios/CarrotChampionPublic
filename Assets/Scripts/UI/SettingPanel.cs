using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    public GameObject _objButtonMusicOpen;
    public GameObject _objButtonMusicClose;
    public Text _textMusic;
    
    public GameObject _objButtonSoundOpen;
    public GameObject _objButtonSoundClose;
    public Text _textSound;
    
    private bool _isMusicOpen = true;
    private bool _isSoundOpen = true;
    
    void Start()
    {
        Init();
    }

    void Init()
    {
        _isMusicOpen = PlayerPrefs.GetInt("MusicState") == 0;
        _isSoundOpen = PlayerPrefs.GetInt("SoundState") == 0;
        
        _objButtonMusicOpen.SetActive(_isMusicOpen);
        _objButtonMusicClose.SetActive(!_isMusicOpen);
        _textMusic.text = _isMusicOpen? "音乐：开": "音乐：关";
        
        _objButtonSoundOpen.SetActive(_isSoundOpen);
        _objButtonSoundClose.SetActive(!_isSoundOpen);
        _textSound.text = _isSoundOpen? "音效：开": "音效：关";
    }
    
    public void OnClickMusicButton()
    {
        _isMusicOpen = !_isMusicOpen;
        PlayerPrefs.SetInt("MusicState", _isMusicOpen ? 0 : 1);
        
        _objButtonMusicOpen.SetActive(_isMusicOpen);
        _objButtonMusicClose.SetActive(!_isMusicOpen);
        _textMusic.text = _isMusicOpen? "音乐：开": "音乐：关";
        AudioManager.Instance.OnClickButtonMusic(_isMusicOpen);
    }
    
    public void OnClickSoundButton()
    {
        _isSoundOpen = !_isSoundOpen;
        PlayerPrefs.SetInt("SoundState", _isSoundOpen ? 0 : 1);
        
        _objButtonSoundOpen.SetActive(_isSoundOpen);
        _objButtonSoundClose.SetActive(!_isSoundOpen);
        _textSound.text = _isSoundOpen? "音效：开": "音效：关";
        AudioManager.Instance.OnClickButtonSound(_isSoundOpen);
    }
    
    public void OnClickBackButton()
    {
        gameObject.SetActive(false);
        // 这里可以添加其他逻辑，比如保存设置等
    }
}
