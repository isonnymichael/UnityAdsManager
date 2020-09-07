using UnityEngine;
using UnityEngine.Monetization;
using UnityEngine.Advertisements;
using System.Collections;


public class AdManager : MonoBehaviour {

    public string appId;
    public string bannerId;
    public string interstitialId;
    public string rewardedVideoId;
    public bool testMode;
    
    public bool IsRewardedVideoReady { get; set; }
    public bool statusAds = true;

    private int countInterstitial;

    public static AdManager Instance;

    private void Awake()
    {
        if (Instance!=null)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Monetization.Initialize(appId, testMode);
        Advertisement.Initialize(appId, testMode);

    }

    IEnumerator WaitForAd()
    {
        while (!Monetization.IsReady(appId))
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
        if (Monetization.IsReady(interstitialId))
        {
            ShowAdPlacementContent ad = null;
            ad = Monetization.GetPlacementContent(interstitialId) as ShowAdPlacementContent;

            if (ad != null)
            {
                if(statusAds)
                {
                    ad.Show();
                }
                
            }
        }
    }

    public void RequestRewardBasedVideo()
    {
        if (Monetization.IsReady(rewardedVideoId))
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
        ShowAdCallbacks options = new ShowAdCallbacks();
        options.finishCallback = HandleShowResult;
        ShowAdPlacementContent ad = Monetization.GetPlacementContent(rewardedVideoId) as ShowAdPlacementContent;
        ad.Show(options);
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

    void HandleShowResult(UnityEngine.Monetization.ShowResult result)
    {
        if (result == UnityEngine.Monetization.ShowResult.Finished)
        {
            // Give Reward
            IsRewardedVideoReady = false;
            RequestRewardBasedVideo();
        }

        else if (result == UnityEngine.Monetization.ShowResult.Skipped)
        {
            // Ads is skipped dont give reward
            IsRewardedVideoReady = false;
            this.RequestRewardBasedVideo();
        }
        else if (result == UnityEngine.Monetization.ShowResult.Failed)
        {
            Debug.LogError("Video failed to show");
        }
    }

}
