using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class Admod : MonoBehaviour
{    
    private BannerView bannerView;
    private InterstitialAd _interstitialAd;
    private RewardedAd rewardedAd;
    public GameController gameController;
    public string AndroidBannerID       = "ca-app-pub-3940256099942544/6300978111";//thu nghiem
    public string IOSBannerID           = "ca-app-pub-3940256099942544/2934735716";//thu nghiem
    public string AndroidInterstitialID = "ca-app-pub-3940256099942544/1033173712";//thu nghiem
    public string IOSInterstitialID     = "ca-app-pub-3940256099942544/4411468910";//thu nghiem




    // Start is called before the first frame update
    void Start()
    {
        BoardCountDown.eventCountDone.AddListener(() => { ShowInterstitialAd(); });
        gameController = GameController.Instance;
        MobileAds.Initialize(initStatus => {
            RequestBanner();
            LoadInterstitialAd();
        });
        
    }   
    private void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = AndroidBannerID ;
#elif UNITY_IPHONE
            string adUnitId = IOSBannerID;
#else
        string adUnitId = "unexpected_platform";
#endif
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.TopLeft);

        // Create an empty ad request.
        AdRequest request = new AdRequest();

        // Load the banner with the request.
        bannerView.LoadAd(request);
    }
    public void LoadInterstitialAd()
    {
#if UNITY_ANDROID
        string _adUnitId = AndroidInterstitialID;
#elif UNITY_IPHONE
        string _adUnitId = IOSInterstitialID;
#else
        string _adUnitId = "unused";
#endif
        // Clean up the old ad before loading a new one.
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }
        Debug.Log("Loading the interstitial ad.");
        // create our request used to load the ad.
        var adRequest = new AdRequest();
        // send the request to load the ad.
        InterstitialAd.Load(_adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }
                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());
                _interstitialAd = ad;
                RegisterEventHandlers(_interstitialAd);
            });     
    }
    public void ShowInterstitialAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }
    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            _interstitialAd.Destroy();
            LoadInterstitialAd();
            Debug.Log("Interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
            LoadInterstitialAd();
        };
    }

  
}
