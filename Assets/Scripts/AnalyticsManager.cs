using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;
    private bool _isInitialized = false;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public async void Start()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection(); // consent to collect given
        //AnalyticsService.Instance.StopDataCollection(); // consent revoked
        _isInitialized = true;
    }

    public void lifePurchaseEvent(int livesPurchased)
    {
        if (!_isInitialized)
        {
            return;
        }

        CustomEvent myLifePurchaseEvent = new CustomEvent("lifePurchaseEvent")
        {
            {"livesPurchased", livesPurchased }
        };

        AnalyticsService.Instance.RecordEvent(myLifePurchaseEvent);
        AnalyticsService.Instance.Flush();
    }

    public void adViewEvent(string adTypeViewed)
    {
        if (!_isInitialized)
        {
            return;
        }

        CustomEvent myAdViewEvent = new CustomEvent("adViewEvent")
        {
            {"adTypeViewed", adTypeViewed}
        };

        AnalyticsService.Instance.RecordEvent(myAdViewEvent);
        AnalyticsService.Instance.Flush();
    }

    public void playerCaptureEvent(float positionX, float positionY, float positionZ, string currentLevel)
    {
        if (!_isInitialized)
        {
            return;
        }

        CustomEvent myplayerCaptureEvent = new CustomEvent("playerCaptureEvent")
        {
            {"positionX", positionX },
            {"positionY", positionY },
            {"positionZ", positionZ },
            {"currentLevel", currentLevel }
        };

        AnalyticsService.Instance.RecordEvent(myplayerCaptureEvent);
        AnalyticsService.Instance.Flush();
    }
}
