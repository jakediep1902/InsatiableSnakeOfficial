using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class Admod : MonoBehaviour
{
    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;
    public GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameController.Instance;
        MobileAds.Initialize(initStatus => { });

        this.RequestBanner();
    }
    private void RequestBanner()
    {
#if UNITY_ANDROID
        //string adUnitId = "ca-app-pub-3940256099942544/6300978111";//thu nghiem
        string adUnitId = "ca-app-pub-7260641755866450/2513620418";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }
    public void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";//thu nghiem
        //string adUnitId = "ca-app-pub-7260641755866450/1974015627";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif
        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Called when an ad request has successfully loaded.
        this.interstitial.OnAdLoaded += Interstitial_OnAdLoaded;
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);   
    }
    private void Interstitial_OnAdLoaded(object sender, System.EventArgs e)
    {
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
            Debug.Log($"Intersitial inside");
        }
    }

    public void CreateAndLoadRewardedAd()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";//thu nghiem
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            string adUnitId = "unexpected_platform";
#endif

        this.rewardedAd = new RewardedAd(adUnitId);

        this.rewardedAd.OnAdLoaded += RewardedAd_OnAdLoaded;
        this.rewardedAd.OnUserEarnedReward += RewardedAd_OnUserEarnedReward;
        this.rewardedAd.OnAdClosed += RewardedAd_OnAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    private void RewardedAd_OnAdClosed(object sender, System.EventArgs e)
    {
        //when user close ad
        gameController.ResumeGame();
        Debug.Log($"Reward on ad closed");
    }

    private void RewardedAd_OnUserEarnedReward(object sender, Reward e)
    {
        //reward user
        Debug.Log($"Reward on user earned reward");
    }
    private void RewardedAd_OnAdLoaded(object sender, System.EventArgs e)
    {
        rewardedAd.Show();
        gameController.PauseGame();
        Debug.Log($"Reward on ad loaded");
    }
}
