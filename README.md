# UnityAdsManager
Very simple and basic integration unity ads for your game.

# How to Use
First you must setup basic service Unity.
If you done with it, you can show the ads just with call the code :

Show Banner :
- AdsManager.Instance.RequestBanner();
Hide Banner :
- AdsManager.Instance.DestroyBanner();

Show Interstitial :
- AdsManager.Instance.RequestInterstitial();

Reward Based Ad :
- First you should init the reward based video with AdManager.Instance.RequestRewardBasedVideo();
- To show the ads : AdManager.Instance.ShowRewardBasedAd();
