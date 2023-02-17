using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Purchasing;

public class AdManager : MonoManager<AdManager> {

    [SerializeField]
    private string m_appID = "ca-app-pub-4494318679381284~1593810512";

    [SerializeField]
    private string m_bannerID = "ca-app-pub-4494318679381284/8640693883";

    [SerializeField]
    private string m_interAddID = "ca-app-pub-4494318679381284/3045303619";

    [SerializeField]
    private string m_getTripleGemAdID = "ca-app-pub-4494318679381284/3162043052";

    [SerializeField]
    private string m_secondChanceAdID = "ca-app-pub-4494318679381284/5479895260";

    [SerializeField]
    private string m_testVideoAd = "ca-app-pub-3940256099942544/5224354917";

    [SerializeField]
    private string m_testBannerAd = "ca-app-pub-3940256099942544/6300978111";

    [SerializeField]
    private string m_testInterAd = "ca-app-pub-3940256099942544/1033173712";

    private RewardBasedVideoAd m_rewardBasedVideoAd;
    private BannerView m_bannerAd;
    private InterstitialAd m_interAd;

    int m_videoAdLoadTry = 0;
    int m_bannerAdLoadTry = 0;
    int m_interAdLoadTry = 0;

    bool m_isBannerAdLoaded = false;
    bool m_isInterAdLoaded = false;
    bool m_isRewardAdLoaded = false;

    public bool IsBannerLoaded
    {
        get { return m_isBannerAdLoaded; }
    }

    public bool IsInterAdLoaded
    {
        get { return m_isInterAdLoaded; }
    }

    public bool IsRewardAdLoaded
    {
        get { return m_isRewardAdLoaded; }
    }

    private void Start()
    {
            MobileAds.Initialize(m_appID);

            m_rewardBasedVideoAd = RewardBasedVideoAd.Instance;

            m_rewardBasedVideoAd.OnAdLoaded += HandleRewardBasedVideoLoaded;
            m_rewardBasedVideoAd.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
            m_rewardBasedVideoAd.OnAdOpening += HandleRewardBasedVideoOpened;
            m_rewardBasedVideoAd.OnAdStarted += HandleRewardBasedVideoStarted;
            m_rewardBasedVideoAd.OnAdClosed += HandleRewardBasedVideoClosed;
            m_rewardBasedVideoAd.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;

            RequestRewarBasedVideo();

        if (!GameManager.Instance.PlayerProfile.NoAdPurchase)
        {
            RequestBannerAd();
            RequestInterstitialAd();
        }
    }

    public void RequestBannerAd()
    {
        m_bannerAd = new BannerView(m_bannerID, AdSize.Banner, AdPosition.Bottom);

        m_bannerAd.OnAdLoaded += HandleOnAdLoaded;
        m_bannerAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        m_bannerAd.OnAdOpening += HandleOnAdOpened;
        m_bannerAd.OnAdClosed += HandleOnAdClosed;
        m_bannerAd.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        AdRequest request = new AdRequest.Builder().Build();
        m_bannerAd.LoadAd(request);
    }

    private void RequestInterstitialAd()
    {
        m_interAd = new InterstitialAd(m_interAddID);

        m_interAd.OnAdLoaded += HandleOnInterAdLoaded;
        m_interAd.OnAdFailedToLoad += HandleOnInterAdFailedToLoad;
        m_interAd.OnAdOpening += HandleOnInterAdOpened;
        m_interAd.OnAdClosed += HandleOnInterAdClosed;
        m_interAd.OnAdLeavingApplication += HandleOnInterAdLeavingApplication;

        AdRequest request = new AdRequest.Builder().Build();
        m_interAd.LoadAd(request);
    }

    private void RequestRewarBasedVideo()
    {
        AdRequest request = new AdRequest.Builder().Build();
        m_rewardBasedVideoAd.LoadAd(request, m_getTripleGemAdID);
    }

    public BannerView ShowBannerAd()
    {
        m_bannerAd.Show();
        return m_bannerAd;
    }

    public InterstitialAd ShowInterAd()
    {
        m_interAd.Show();
        return m_interAd;
    }

    public RewardBasedVideoAd ShowRewardAd()
    {
        m_rewardBasedVideoAd.Show();
        return m_rewardBasedVideoAd;
    }

    /*private void Update()
    {
        if(m_isLoadedAdPoolFull && m_loadedCustomRewardAdPool.Count < 3)
        {
            StartCoroutine(CCreateAndLoadRewardAd());
            m_isLoadedAdPoolFull = false;
        }
    }

    public CustomRewardAd ShowVideoAdFromLoadedPool()
    {
        CustomRewardAd ad = m_loadedCustomRewardAdPool[0];
        m_loadedCustomRewardAdPool.Remove(m_loadedCustomRewardAdPool[0].Show());
        return ad;
    }

    IEnumerator CCreateAndLoadRewardAd()
    {
        while (m_loadedCustomRewardAdPool.Count < 3)
        {
            CustomRewardAd ad = new CustomRewardAd(m_testVideoAd);
            yield return new WaitUntil(() => (ad.IsLoaded || ad.IsFailed));

            if (ad.IsLoaded)
                m_loadedCustomRewardAdPool.Add(ad);
        }

        m_isLoadedAdPoolFull = true;
    }*/

    #region Reward Ad Handler Region

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
        m_videoAdLoadTry = 0;
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        /*MonoBehaviour.print(
            "HandleRewardBasedVideoFailedToLoad event received with message: "
                             + args.Message);*/

        m_videoAdLoadTry++;

        if(m_videoAdLoadTry < 10)
            RequestRewarBasedVideo();
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
        RequestRewarBasedVideo();
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        /*MonoBehaviour.print(
            "HandleRewardBasedVideoRewarded event received for "
                        + amount.ToString() + " " + type);*/
    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
    }

    #endregion

    #region Banner Ad Handler Region

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        //Debug.Log("HandleBannerAdLoaded event received");
        m_bannerAdLoadTry = 0;
        m_bannerAd.Hide();
        m_isBannerAdLoaded = true;
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //Debug.Log("HandleBannerAdFailed event received");
        m_bannerAdLoadTry++;
        m_isBannerAdLoaded = false;

        if(m_bannerAdLoadTry < 10)
            RequestBannerAd();
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        m_bannerAd.Destroy();
        RequestBannerAd();
        m_isBannerAdLoaded = false;
        //MonoBehaviour.print("HandleAdClosed event received");
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

    #endregion

    #region Interstitial Ad Handler Region

    public void HandleOnInterAdLoaded(object sender, EventArgs args)
    {
        //Debug.Log("HandleInterAdLoaded event received");
    }

    public void HandleOnInterAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //Debug.Log("HandleInterAdFailedToLoad");
        m_interAdLoadTry++;
        if (m_interAdLoadTry < 10)
            RequestInterstitialAd();
    }

    public void HandleOnInterAdOpened(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnInterAdClosed(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleAdClosed event received");
        m_interAd.Destroy();
        RequestInterstitialAd();
    }

    public void HandleOnInterAdLeavingApplication(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

    #endregion
}
