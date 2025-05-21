using UnityEngine;
using UnityEngine.Audio;

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
        
        // …Ë÷√“Ù–ß
        SetBgm();
        PlayBgm();
        // …Ë÷√Ã¯‘æ“Ù–ß
        SetJumpFx();
    }
    
    private void OnDisable()
    {
        EventHandler.GameOverEvent -= OnGameOverEvent;
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
}
