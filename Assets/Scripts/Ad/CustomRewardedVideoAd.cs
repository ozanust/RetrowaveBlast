using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class CustomRewardedVideoAd {

    public enum AdState
    {
        Loaded = 0,
        FailedToLoad = 1,
        Opening = 2,
        Started = 3,
        UserRewarded = 4,
        Closed = 5,
        LeftApp = 6
    }

    private RewardBasedVideoAd m_rewardBasedVideoAd;
    private string m_videoAdID;
    private AdState m_adState;
    private bool m_isLoaded =  false;
    private bool m_isFailed = false;

    public CustomRewardedVideoAd(string adID)
    {
        this.m_videoAdID = adID;
        AddEvents();
        RequestRewarBasedVideo();
    }

    public RewardBasedVideoAd Ad
    {
        get { return m_rewardBasedVideoAd; }
    }

    public AdState State
    {
        get { return m_adState; }
    }

    public CustomRewardedVideoAd Show()
    {
        m_rewardBasedVideoAd.Show();
        return this;
    }

    public bool IsLoaded
    {
        get { return m_rewardBasedVideoAd.IsLoaded(); }
    }

    public bool IsFailed
    {
        get { return m_isFailed; }
    }

    private void AddEvents()
    {
        m_rewardBasedVideoAd = RewardBasedVideoAd.Instance;

        // Called when an ad request has successfully loaded.
        m_rewardBasedVideoAd.OnAdLoaded += HandleRewardBasedVideoLoaded;
        // Called when an ad request failed to load.
        m_rewardBasedVideoAd.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        // Called when an ad is shown.
        m_rewardBasedVideoAd.OnAdOpening += HandleRewardBasedVideoOpened;
        // Called when the ad starts to play.
        m_rewardBasedVideoAd.OnAdStarted += HandleRewardBasedVideoStarted;
        // Called when the user should be rewarded for watching a video.
        m_rewardBasedVideoAd.OnAdRewarded += HandleRewardBasedVideoRewarded;
        // Called when the ad is closed.
        m_rewardBasedVideoAd.OnAdClosed += HandleRewardBasedVideoClosed;
        // Called when the ad click caused the user to leave the application.
        m_rewardBasedVideoAd.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;
    }

    private void RequestRewarBasedVideo()
    {
        AdRequest request = new AdRequest.Builder().Build();
        m_rewardBasedVideoAd.LoadAd(request, m_videoAdID);
    }


    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
        m_adState = AdState.Loaded;
        m_isLoaded = true;
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardBasedVideoFailedToLoad event received with message: "
                             + args.Message);
        m_adState = AdState.FailedToLoad;
        m_isFailed = true;
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
        m_adState = AdState.Opening;
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
        m_adState = AdState.Started;
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
        m_adState = AdState.Closed;
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardBasedVideoRewarded event received for "
                        + amount.ToString() + " " + type);

        m_adState = AdState.UserRewarded;
    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
        m_adState = AdState.LeftApp;
    }
}
