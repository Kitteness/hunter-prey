using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdsManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    private string adUnitSuffix;
    [SerializeField] private LifeManager lifeManager;

    public void Awake()
    {
#if UNITY_IOS
        adUnitSuffix = "_iOS";
#elif UNITY_ANDROID
        adUnitSuffix = "_Android";
#elif UNITY_EDITOR
        adUnitSuffix = "_Android";
#endif
    }

    public void LoadAd(string adUnitPrefix)
    {
        string adUnitId = adUnitPrefix + adUnitSuffix;
        Advertisement.Load(adUnitId, this);
    }

    public void ShowAd(string adUnitPrefix)
    {
        string adUnitId = adUnitPrefix + adUnitSuffix;
        Advertisement.Show(adUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"{placementId} Advertisement is loaded");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogWarning($"{placementId} failed to load: {error} - {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogWarning($"Failed to show {placementId} ad: {error} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log($"Begin showing {placementId} ad");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log($"{placementId} ad was clicked");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log($"{placementId} ad finished");

        switch (placementId)
        {
            case "Rewarded_Android":
                if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
                {
                    Debug.Log($"You watched the ad. Have 1 Life!");
                    AnalyticsManager.Instance.adViewEvent("Rewarded");
                    lifeManager.GainLife(1);
                }
                break;
        }
    }
}
