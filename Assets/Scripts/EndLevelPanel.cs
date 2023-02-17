using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndLevelPanel : MonoBehaviour
{
    enum RewardedAdState
    {
        None = 0,
        SecondChance = 1,
        TripleCoin = 2
    }

    #region Second Chance UI Members

    [SerializeField]
    private GameObject m_secondChancePanel;

    [SerializeField]
    private Button m_secondChanceButton;

    [SerializeField]
    private Image m_secondChanceButtonStroke;

    [SerializeField]
    private Button m_skipSecondChanceButton;

    [SerializeField]
    private CanvasGroup m_secondChanceGroup;

    #endregion


    #region End Level UI Members

    [SerializeField]
    private CanvasGroup m_endLevelPanelGroup;

    [SerializeField]
    private GameObject m_endLevelPanel;

    [SerializeField]
    private Text m_scoreValueField;

    [SerializeField]
    private Text m_coinValueField;

    [SerializeField]
    private Text m_levelValueField;

    [SerializeField]
    private Text m_highestScoreValueField;

    [SerializeField]
    private Button m_getTripleCoinButton;

    [SerializeField]
    private Button m_dismissEndLevelPanelButton;

    [SerializeField]
    private Image m_endLevelPanelBackground;

    [SerializeField]
    private Image m_completedTextImage;

    [SerializeField]
    private Image m_failedTextImage;

    #endregion


    [SerializeField]
    private ScreenFader m_screenFader;

    [SerializeField]
    private bool m_isLevelSucceeded = false;

    [SerializeField]
    private GameController m_gameController;

    [SerializeField]
    private MainUIController m_mainUIController;

    [SerializeField]
    private int m_interstitialAdInterval = 10;

    public bool IsLevelSucceeded
    {
        get { return m_isLevelSucceeded; }
        set { m_isLevelSucceeded = value; }
    }

    bool m_isSecondChanceUsed = false;
    bool m_isSecondChanceRewarded = false;
    bool m_isTripleCoinRewarded = false;
    bool m_isReturnedFromAd = false;
    bool m_isAdOpened = false;

    Coroutine c_secondChanceTimer = null;
    Coroutine c_endLevelPanelSlideCoroutine = null;
    Coroutine c_skipSecondChanceCoroutine = null;
    private int m_gameFailCount = 0;

    private AudioManager m_audioManager;
    private GameManager m_gameManager;

    RewardBasedVideoAd m_ad;
    BannerView m_bannerAd;
    private RewardedAdState m_adState = RewardedAdState.None;

    #region Public Methods

    public void SetScoreValue(int score)
    {
        m_scoreValueField.text = score.ToString();
    }

    public void SetCoinValue(int coin)
    {
        m_coinValueField.text = coin.ToString();
    }

    public void SetLevelValue(int level)
    {
        m_levelValueField.text = level.ToString();
    }

    public void SetBestScoreValue(int score)
    {
        m_highestScoreValueField.text = score.ToString();
    }

    public void StartSecondChanceTimer()
    {
        if (c_secondChanceTimer == null)
            c_secondChanceTimer = StartCoroutine(CSecondChanceTimer());
    }

    #endregion


    #region Initializers

    private void Awake()
    {
        if (m_audioManager == null)
            m_audioManager = AudioManager.Instance;

        if (m_gameManager == null)
            m_gameManager = GameManager.Instance;

        m_ad = RewardBasedVideoAd.Instance;
        m_ad.OnAdClosed += HandleRewardedAdClosed;
        m_ad.OnAdOpening += HandleRewardBasedVideoOpened;
        //m_endLevelPanelBackground.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
    }

    private void OnEnable()
    {
        if (!IsLevelSucceeded && !m_isSecondChanceUsed)
        {
            StartSecondChanceTimer();
        }
        else
        {
            StartEndLevelPanelSlideIn();
        }

        if (IsLevelSucceeded)
            m_completedTextImage.gameObject.SetActive(true);
        else
            m_failedTextImage.gameObject.SetActive(true);

        AddEventListeners();

        SetBestScoreValue(m_gameManager.PlayerProfile.HighestScore);
    }

    private void AddEventListeners()
    {
        m_dismissEndLevelPanelButton.onClick.AddListener(DismissEndLevelPanel);
        m_skipSecondChanceButton.onClick.AddListener(SkipSecondChance);
        m_getTripleCoinButton.onClick.AddListener(StartTripleCoinAd);
        m_secondChanceButton.onClick.AddListener(StartSecondChanceAd);
    }

    #endregion


    #region Finalizers

    private void OnDisable()
    {
        if(m_bannerAd != null)
            m_bannerAd.Destroy();

        if(!GameManager.Instance.PlayerProfile.NoAdPurchase)
            AdManager.Instance.RequestBannerAd();

        m_completedTextImage.gameObject.SetActive(false);
        m_failedTextImage.gameObject.SetActive(false);
        m_secondChanceButtonStroke.fillAmount = 0.0f;
        m_isSecondChanceRewarded = false;
        m_isReturnedFromAd = false;
        m_getTripleCoinButton.interactable = true;
        m_endLevelPanelGroup.alpha = 0;
        //m_endLevelPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-400, 0);
        m_skipSecondChanceButton.gameObject.SetActive(false);
        RemoveEventListeners();
    }

    private void RemoveEventListeners()
    {
        m_dismissEndLevelPanelButton.onClick.RemoveAllListeners();
        m_skipSecondChanceButton.onClick.RemoveAllListeners();
        m_getTripleCoinButton.onClick.RemoveAllListeners();
        m_secondChanceButton.onClick.RemoveAllListeners();
    }

    #endregion


    #region Coroutines

    IEnumerator CSecondChanceTimer()
    {
        m_secondChancePanel.SetActive(true);
        float timeVel = 0;
        float alphaVel = 0;
        while(m_secondChanceGroup.alpha < 1)
        {
            Time.timeScale = Mathf.SmoothDamp(Time.timeScale, 0, ref timeVel, 1, 10, 0.06f);
            m_audioManager.SetGameMusicPitch(Time.timeScale);

            m_secondChanceGroup.alpha = Mathf.SmoothDamp(m_secondChanceGroup.alpha, 1.1f, ref alphaVel, 1, 10, 0.04f);

            yield return new WaitForEndOfFrame();
        }

        m_audioManager.SetGameMusicPitch(0);

        float strokeTimer = 0;
        while (m_secondChanceButtonStroke.fillAmount < 1)
        {
            strokeTimer += 0.02f;
            m_secondChanceButtonStroke.fillAmount = strokeTimer / 4f;

            if (!m_skipSecondChanceButton.IsActive() && strokeTimer >= 1f)
            {
                m_skipSecondChanceButton.gameObject.SetActive(true);
            }
            yield return new WaitForEndOfFrame();
        }

        if(m_secondChancePanel.activeInHierarchy)
            SkipSecondChance();

        c_secondChanceTimer = null;
    }

    IEnumerator CSkipSecondChance()
    {
        if (c_secondChanceTimer != null)
        {
            StopCoroutine(CSecondChanceTimer());
            c_secondChanceTimer = null;
        }

        m_secondChanceButton.interactable = false;

        float alphaVel = 0;
        while (m_secondChanceGroup.alpha > 0)
        {
            m_secondChanceGroup.alpha = Mathf.SmoothDamp(m_secondChanceGroup.alpha, -0.1f, ref alphaVel, 1, 10, 0.08f);

            yield return new WaitForEndOfFrame();
        }

        m_secondChanceGroup.alpha = 0;
        m_secondChanceButtonStroke.fillAmount = 0;

        //m_gameController.EndLevel();
        m_secondChancePanel.SetActive(false);
        m_secondChanceButton.interactable = true;

        c_skipSecondChanceCoroutine = null;

        StartEndLevelPanelSlideIn();
    }

    IEnumerator CEndLevelPanelSlideIn()
    {
        m_bannerAd = AdManager.Instance.ShowBannerAd();
        m_audioManager.CrossFadeGameMusicToMenuMusic();
        m_endLevelPanel.SetActive(true);
        //Time.timeScale = 0;
        RectTransform panelRect = m_endLevelPanel.GetComponent<RectTransform>();
        float timer = 0;

        while(m_endLevelPanelGroup.alpha < 1)
        {
            timer += 0.02f;
            m_endLevelPanelGroup.alpha = timer;
            yield return new WaitForEndOfFrame();
        }

        /*while (panelRect.anchoredPosition.x < 400)
        {
            timer += 16f;
            panelRect.anchoredPosition = new Vector2(timer, panelRect.anchoredPosition.y);
            yield return new WaitForEndOfFrame();
        }*/

        c_endLevelPanelSlideCoroutine = null;
    }

    #endregion

    private void StartEndLevelPanelSlideIn()
    {
        if (c_endLevelPanelSlideCoroutine == null)
            c_endLevelPanelSlideCoroutine = StartCoroutine(CEndLevelPanelSlideIn());
    }

    private void SkipSecondChance()
    {
        if (c_skipSecondChanceCoroutine == null)
            c_skipSecondChanceCoroutine = StartCoroutine(CSkipSecondChance());
    }

    private void StartTripleCoinAd()
    {
        //start ad
        AdManager.Instance.ShowRewardAd();
        //m_ad.OnAdRewarded += HandleUserEarnedTripleCoinReward;
        m_adState = RewardedAdState.TripleCoin;
    }

    private void StartSecondChanceAd()
    {
        //start ad
        /*CustomRewardAd videoAd = AdManager.Instance.ShowVideoAdFromLoadedPool();
        videoAd.Ad.OnUserEarnedReward += HandleUserEarnedReward;
        videoAd.Ad.OnAdClosed += HandleRewardedAdClosed;
        videoAd.Ad.OnAdFailedToShow += HandleRewardedAdFailedToShow;*/

        AdManager.Instance.ShowRewardAd();
        //m_ad.OnAdRewarded += HandleUserEarnedSecondChanceReward;
        m_adState = RewardedAdState.SecondChance;

        Time.timeScale = 0;
        //m_secondChancePanel.SetActive(false);
        StopCoroutine(CSecondChanceTimer());
        c_secondChanceTimer = null;
        m_isSecondChanceUsed = true;

        //OnVideoAdFinished();
    }

    private void OnVideoAdFinished()
    {
        m_secondChancePanel.SetActive(false);
        m_secondChanceGroup.alpha = 0;
        m_gameController.ContinueGame();
        GameManager.Instance.ActiveShooter.DeactiveColliderForAmountOfTime(3);
        this.gameObject.SetActive(false);
    }

    private void OnTripleCoinAdFinished()
    {
        GameManager.Instance.PlayerProfile.GainGold(m_gameController.Gold * 2);
        SetCoinValue(m_gameController.Gold * 3);
        m_getTripleCoinButton.interactable = false;
    }

    private void DoEndLevelOperations()
    {
        m_endLevelPanel.SetActive(false);
        m_gameController.EndLevel();

        if (IsLevelSucceeded)
        {
            m_isSecondChanceUsed = false;
            m_gameController.StartLevel();
        }
        else
        {
            m_gameFailCount++;
            //Debug.Log("Game Fail Count:" + m_gameFailCount);
            if (m_gameFailCount == m_interstitialAdInterval)
            {
                if (!GameManager.Instance.PlayerProfile.NoAdPurchase)
                    AdManager.Instance.ShowInterAd();
                m_gameFailCount = 0;
            }

            m_isSecondChanceUsed = false;
            m_gameController.CurrentGameState = GameController.GameState.Ready;
            //m_mainUIController.FadeInMainBackground();
        }

        this.gameObject.SetActive(false);
    }

    private void DismissEndLevelPanel()
    {
        m_screenFader.PassScene(DoEndLevelOperations);
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardBasedVideoOpened event received");

        if(m_adState == RewardedAdState.SecondChance)
            m_ad.OnAdRewarded += HandleUserEarnedSecondChanceReward;
        else if(m_adState == RewardedAdState.TripleCoin)
            m_ad.OnAdRewarded += HandleUserEarnedTripleCoinReward;
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        Debug.Log("HandleRewardedAdFailedToShow event received with message: " + args.Message);

        m_isReturnedFromAd = true;
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdClosed event received");
        Debug.Log(m_adState.ToString());
        m_isReturnedFromAd = true;
    }

    public void HandleUserEarnedSecondChanceReward(object sender, Reward args)
    {
        Debug.Log("HandleUserEarnedSecondChanceReward event received");
        m_isSecondChanceRewarded = true;
    }

    public void HandleUserEarnedTripleCoinReward(object sender, Reward args)
    {
        Debug.Log("HandleUserEarnedTripleCoinReward event received");
        m_isTripleCoinRewarded = true;
    }

    private void OnApplicationPause(bool pause)
    {
        if(m_isSecondChanceRewarded && m_isReturnedFromAd && !pause)
        {
            m_isSecondChanceRewarded = false;
            m_isReturnedFromAd = false;
            m_ad.OnAdRewarded -= HandleUserEarnedSecondChanceReward;
            OnVideoAdFinished();
        }

        if(m_isTripleCoinRewarded && m_isReturnedFromAd && !pause)
        {
            m_isTripleCoinRewarded = false;
            m_isReturnedFromAd = false;
            m_ad.OnAdRewarded -= HandleUserEarnedTripleCoinReward;
            OnTripleCoinAdFinished();
        }

        if (!m_isSecondChanceRewarded && !m_isTripleCoinRewarded && m_isReturnedFromAd && !pause)
        {
            m_isReturnedFromAd = false;

            if (m_adState == RewardedAdState.SecondChance)
            {
                m_ad.OnAdRewarded -= HandleUserEarnedSecondChanceReward;
                if (m_secondChancePanel.activeInHierarchy)
                    SkipSecondChance();
            }
            else if (m_adState == RewardedAdState.TripleCoin)
            {
                m_ad.OnAdRewarded -= HandleUserEarnedTripleCoinReward;

            }

            //m_adState = RewardedAdState.None;
        }
    }
}
