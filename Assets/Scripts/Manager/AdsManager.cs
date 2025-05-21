using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : SingletonMonobehaviour<AdsManager>, IUnityAdsInitializationListener
{
        
    [SerializeField] private string _androidGameId;
    [SerializeField] private string _iOSGameId;
    [SerializeField] private bool _testMode = true;
    private string _gameId;

    protected override void Awake()
    {
        base.Awake();
        InitAds();
    }

    private void InitAds()
    {
#if UNITY_IOS
        _gameId = _iOSGameId;
#elif UNITY_ANDROID
        _gameId = _androidGameId;
#else
        _gameId = _androidGameId;
#endif
        if(Advertisement.isSupported&& !Advertisement.isInitialized)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
        }
        else if (Advertisement.isSupported && Advertisement.isInitialized)
        {
            Debug.Log("[LOG] UnityAds 已经初始化.");
        }
        else
        {
            Debug.LogError("Unity Ads is not supported on this platform.");
        }
        
    }

    public void OnInitializationComplete()
    {
        Debug.Log("[LOG] UnityAds 初始化完成.");
        // Load ads or show ads here
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Unity Ads initialization failed: {error.ToString()} - {message}");
        // Handle initialization failure here
    }
}