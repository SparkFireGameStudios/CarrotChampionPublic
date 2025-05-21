using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

/// <summary>
/// 看广告领奖按钮
/// </summary>
public class RewardAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    // 看广告按钮
    [SerializeField] private Button _button;
    // android 广告ID
    [SerializeField] private string _androidAdUnitId = "Rewarded_Android";
    // ios 广告ID
    [SerializeField] private string _iOSAdUnitId = "Rewarded_iOS";
    // 当前使用的广告ID
    private string _adUnitId = null;

    /// <summary>
    /// 初始化广告id，并且禁用按钮点击
    /// </summary>
    private void Awake()
    {
#if Unity_IOS
            _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
            _adUnitId = _androidAdUnitId;
#else
        _adUnitId = _androidAdUnitId;
#endif
        _button.interactable = false;
    }

    // 界面激活时加载广告
    private void OnEnable()
    {
        LoadAd();
    }

    // 开始加载广告
    private void LoadAd()
    {
        Debug.Log("加载广告: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    // 当广告加载成功时调用
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("广告加载完成: " + adUnitId);
        if (adUnitId.Equals(_adUnitId))
        {
            _button.onClick.AddListener(ShowAd);
            _button.interactable = true;
        }
    }
    
    // 播放广告 禁用按钮
    private void ShowAd()
    {
        _button.interactable = false;
        Advertisement.Show(_adUnitId,this);
    }

    // 当广告加载失败时调用
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"广告加载失败 : {adUnitId} - {error.ToString()} - {message}");
        // Handle ad loading failure here
    }

    // 当广告播放失败时调用
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    { 
        Debug.LogError($"广告播放失败 : {adUnitId} - {error.ToString()} - {message}");
        // Handle ad showing failure here
        _button.interactable = true;
    }

    // 当广告开始播放时调用
    public void OnUnityAdsShowStart(string adUnitId)
    {
        Debug.Log("广告开始播放 : " + adUnitId);
        // Handle ad start event here
    }

    // 当点击广告时调用
    public void OnUnityAdsShowClick(string adUnitId)
    {
        Debug.Log("广告点击 : " + adUnitId);
        // Handle ad click event here
    }

    // 当广告播放完成时调用，用于发奖励
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if(adUnitId.Equals(_adUnitId)&& showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
           Debug.Log("广告播放完成 : " + adUnitId);
        }
    }

    private void OnDestroy()
    {
        if (_button != null)
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}