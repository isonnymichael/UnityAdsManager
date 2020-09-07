using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;


public class UnityAdsManager : MonoBehaviour, IUnityAdsListener
{

    public string appId;
    public string bannerId;
    public string interstitialId;
    public string rewardedVideoId;
    public bool testMode;

    public bool IsRewardedVideoReady { get; set; }
    public bool statusAds = true;

    private int countInterstitial;

    public static UnityAdsManager Instance;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Advertisement.Initialize(appId, testMode);
        Advertisement.AddListener(this);
    }

    IEnumerator WaitForAd()
    {
        while (!Advertisement.IsReady(appId))
        {
            yield return null;
        }
    }

    public void RequestBanner()
    {
        if (statusAds)
        {
            ShowBanner();
        }
    }

    public void RequestInterstitial()
    {
        if (Advertisement.IsReady(interstitialId))
        {
            Advertisement.Show(interstitialId);
        }
    }

    public void RequestRewardBasedVideo()
    {
        if (Advertisement.IsReady(rewardedVideoId))
        {
            IsRewardedVideoReady = true;
        }
    }

    IEnumerator ShowBannerWhenReady()
    {
        while (!Advertisement.IsReady("banner"))
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.Show(bannerId);
    }

    public void ShowRewardBasedAd()
    {
        if (statusAds)
        {
            Advertisement.Show(rewardedVideoId);
        }

    }


    public void ShowBanner()
    {
        if (statusAds)
        {
            StartCoroutine(ShowBannerWhenReady());
        }

    }

    public void DestroyBanner()
    {
        Advertisement.Banner.Hide();
    }

    public void HideBanner()
    {
        Advertisement.Banner.Hide();
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            IsRewardedVideoReady = false;
            RequestRewardBasedVideo();
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
            IsRewardedVideoReady = false;
            this.RequestRewardBasedVideo();
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, show the ad:
        if (placementId == rewardedVideoId)
        {
            // Optional actions to take when the placement becomes ready(For example, enable the rewarded ads button)
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }

}
