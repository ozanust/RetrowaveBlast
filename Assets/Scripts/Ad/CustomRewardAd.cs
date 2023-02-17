using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class CustomRewardAd : MonoBehaviour {

    private RewardedAd m_rewardedAd;

    private string m_adID;
    private bool m_isFailed = false;
    private bool m_isFailedToShow = false;
    private bool m_isRewarded = false;

    public CustomRewardAd(string adID)
    {
        m_adID = adID;
        m_rewardedAd = new RewardedAd(m_adID);
        RequestRewarAd();
    }

    public CustomRewardAd Show()
    {
        m_rewardedAd.Show();
        return this;
    }

    public RewardedAd Ad
    {
        get { return m_rewardedAd; }
    }

    public bool IsLoaded
    {
        get { return m_rewardedAd.IsLoaded(); }
    }

    public bool IsFailed
    {
        get { return m_isFailed; }
    }

    public bool IsFailedToShow
    {
        get { return m_isFailedToShow; }
    }

    public bool IsRewarded
    {
        get { return m_isRewarded; }
    }

    private void AdEvents()
    {
        m_rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        m_rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        m_rewardedAd.OnAdOpening += HandleRewardedAdOpening;
    }

    private void RequestRewarAd()
    {
        AdRequest request = new AdRequest.Builder().Build();
        m_rewardedAd.LoadAd(request);
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);
        m_isFailed = true;
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);

        m_isFailedToShow = true;
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);

        m_isRewarded = true;
    }
}
