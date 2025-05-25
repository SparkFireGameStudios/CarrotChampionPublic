using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : SingletonMonobehaviour<AudioManager>
{
    [Header("AudioMixer")] 
    public AudioMixer audioMixer;
    [Header("±≥æ∞“Ù¿÷")]
    public AudioSource bgmSource;
    public AudioSource fxSource;
    [Header("“Ù–ß")]
    public AudioClip bgmClip;
    public AudioClip jumpClip;
    public AudioClip deadClip;

    private void OnEnable()
    {
        EventHandler.GameOverEvent += OnGameOverEvent;
        EventHandler.JumpEvent += OnJumpEvent;
        
        // …Ë÷√“Ù–ß
        SetBgm();
        PlayBgm();
        // …Ë÷√Ã¯‘æ“Ù–ß
        SetJumpFx();
    }
    
    private void OnDisable()
    {
        EventHandler.GameOverEvent -= OnGameOverEvent;
        EventHandler.JumpEvent -= OnJumpEvent;
    }

    public void SetBgm()
    {
        bgmSource.clip = bgmClip;
    }

    public void SetJumpFx()
    {
        fxSource.clip = jumpClip;
    }
    
    public void PlayJumpFx()
    {
        fxSource.Play();
    }
    
    public void PlayBgm()
    {
        if (!bgmSource.isPlaying)
        {
            bgmSource.Play();
        }
    }
    
    private void OnGameOverEvent()
    {
        bgmSource.Stop();
        fxSource.PlayOneShot(deadClip);
    }

    public void OnClickToggleAudio(bool isOn)
    {
        if(isOn)
            audioMixer.SetFloat("MasterVolume", 0);
        else
            audioMixer.SetFloat("MasterVolume", -80);
    }
    
    public void OnClickButtonMusic(bool isOn)
    {
        if (isOn)
        {
            audioMixer.SetFloat("BGM", 0);
        }
        else
        {
            audioMixer.SetFloat("BGM", -80);
        }
    }
    
    public void OnClickButtonSound(bool isOn)
    {
        if (isOn)
        {
            audioMixer.SetFloat("FX", 0);
        }
        else
        {
            audioMixer.SetFloat("FX", -80);
        }
    }
    
    private void OnJumpEvent()
    {
        PlayJumpFx();
    }
}
