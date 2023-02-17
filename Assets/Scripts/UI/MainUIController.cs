using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour
{

    [SerializeField]
    private UIHUDController m_uiHudController;

    [SerializeField]
    private GameController m_gameController;

    [SerializeField]
    private Button m_gameStartButton;

    [SerializeField]
    private Button m_generalSettingsButton;

    [SerializeField]
    private Button m_backgroundSettingsButton;

    [SerializeField]
    private Button m_shooterSettingsButton;

    [SerializeField]
    private Button m_consumableSettingsButton;

    [SerializeField]
    private GeneralSettings m_settings;

    [SerializeField]
    private BackgroundSettings m_bgSettings;

    [SerializeField]
    private ShooterSettings m_shooterSettings;

    [SerializeField]
    private ConsumableSettings m_consumableSettings;

    [SerializeField]
    private GameObject m_upgradeSettingsPanel;

    [SerializeField]
    private GameObject m_mainUIPanel;

    [SerializeField]
    private CanvasGroup m_mainUIPanelGroup;

    [SerializeField]
    private Image m_mainBG;

    [SerializeField]
    private Text m_startText;

    private Coroutine c_fadeOutMainBackground = null;
    private Coroutine c_fadeInMainBackground = null;
    private Coroutine c_fadeOutMainScreen = null;
    private Coroutine c_fadeInMainScreen = null;

    public GeneralSettings General
    {
        get { return m_settings; }
    }

    private void Awake()
    {
        RegisterEventListeners();
    }

    private void OnDisable()
    {
        c_fadeOutMainBackground = null;
        c_fadeInMainBackground = null;
        StopAllCoroutines();
    }

    /*private void Update()
    {
        if (m_mainUIPanel.activeInHierarchy && m_gameController.CurrentGameState == GameController.GameState.Playing)
        {
            //m_mainUIPanel.SetActive(false);
            //m_upgradeSettingsPanel.SetActive(false);
            m_uiHudController.gameObject.SetActive(true);
        }
        else if (!m_mainUIPanel.activeInHierarchy && (m_gameController.CurrentGameState == GameController.GameState.Finished || m_gameController.CurrentGameState == GameController.GameState.Ready || m_gameController.CurrentGameState == GameController.GameState.Stopped))
        {
            //m_mainUIPanel.SetActive(true);
            //m_upgradeSettingsPanel.SetActive(true);
            m_uiHudController.gameObject.SetActive(false);
        }else if(!m_mainUIPanel.activeInHierarchy && m_gameController.CurrentGameState == GameController.GameState.Paused)
        {
            //m_mainUIPanel.SetActive(true);
            //m_upgradeSettingsPanel.SetActive(true);
            m_uiHudController.gameObject.SetActive(false);
        }
    }*/

    public void DoMenuOperation(GameController.GameState gameState)
    {
        if (gameState == GameController.GameState.Playing)
        {
            m_uiHudController.gameObject.SetActive(true);
            FadeOutMainScreen();
        }
        else if (gameState == GameController.GameState.Ready)
        {
            FadeInMainScreen();
        }

        if(gameState != GameController.GameState.Playing)
            m_uiHudController.gameObject.SetActive(false);
    }

    public void FadeOutMainBackground()
    {
        if (c_fadeOutMainBackground == null)
            c_fadeOutMainBackground = StartCoroutine(CFadeOutMainBackground());
    }

    public void FadeInMainBackground()
    {
        if (c_fadeInMainBackground == null)
            c_fadeInMainBackground = StartCoroutine(CFadeInMainBackground());
    }

    public void FadeOutMainScreen()
    {
        if (c_fadeOutMainScreen == null)
            c_fadeOutMainScreen = StartCoroutine(CFadeOutMainScreen());
    }

    public void FadeInMainScreen()
    {
        if (c_fadeInMainScreen == null)
            c_fadeInMainScreen = StartCoroutine(CFadeInMainScreen());
    }

    IEnumerator CFadeOutMainScreen()
    {
        if (c_fadeInMainScreen != null)
            yield return new WaitUntil(() => c_fadeInMainScreen == null);

        float timer = 0;
        while(m_mainUIPanelGroup.alpha > 0)
        {
            timer += 0.02f;
            m_mainUIPanelGroup.alpha = 1 - timer;
            yield return new WaitForEndOfFrame();
        }

        m_mainUIPanelGroup.gameObject.SetActive(false);

        c_fadeOutMainScreen = null;
    }

    IEnumerator CFadeInMainScreen()
    {
        m_mainUIPanelGroup.gameObject.SetActive(true);

        float timer = 0;
        while (m_mainUIPanelGroup.alpha < 1)
        {
            timer += 0.02f;
            m_mainUIPanelGroup.alpha = timer;
            yield return new WaitForEndOfFrame();
        }

        c_fadeInMainScreen = null;
    }

    IEnumerator CFadeOutMainBackground()
    {
        float timer = 0;
        while(m_mainBG.color.a > 0)
        {
            timer += Time.deltaTime;
            m_mainBG.color = Color.Lerp(m_mainBG.color, new Color(m_mainBG.color.r, m_mainBG.color.g, m_mainBG.color.b, 0), timer);
            m_startText.color = Color.Lerp(m_startText.color, new Color(m_startText.color.r, m_startText.color.g, m_startText.color.b, 0), timer);
            yield return new WaitForEndOfFrame();
        }

        m_mainBG.color = new Color(m_mainBG.color.r, m_mainBG.color.g, m_mainBG.color.b, 0);
        m_startText.color = new Color(m_startText.color.r, m_startText.color.g, m_startText.color.b, 0);

        c_fadeOutMainBackground = null;
    }

    IEnumerator CFadeInMainBackground()
    {
        m_mainBG.raycastTarget = true;
        float timer = 0;
        while (m_mainBG.color.a < 1)
        {
            timer += Time.deltaTime;
            m_mainBG.color = Color.Lerp(m_mainBG.color, new Color(m_mainBG.color.r, m_mainBG.color.g, m_mainBG.color.b, 1), timer);
            m_startText.color = Color.Lerp(m_startText.color, new Color(m_startText.color.r, m_startText.color.g, m_startText.color.b, 1), timer);
            yield return new WaitForEndOfFrame();
        }

        m_mainBG.color = new Color(m_mainBG.color.r, m_mainBG.color.g, m_mainBG.color.b, 1);
        m_startText.color = new Color(m_startText.color.r, m_startText.color.g, m_startText.color.b, 1);
        m_mainBG.raycastTarget = false;

        c_fadeInMainBackground = null;
    }

    IEnumerator CScreenFade()
    {
        yield return null;
    }

    void RegisterEventListeners()
    {
        m_generalSettingsButton.onClick.AddListener(OnOffGeneralMenu);
        m_backgroundSettingsButton.onClick.AddListener(OnOffBackgroundsMenu);
        m_shooterSettingsButton.onClick.AddListener(OnOffShootersMenu);
        m_consumableSettingsButton.onClick.AddListener(OnOffConsumablesMenu);

    }

    void OnOffGeneralMenu()
    {
        m_settings.StartMenuOperations();
    }

    void OnOffBackgroundsMenu()
    {
        m_bgSettings.StartMenuOperations();
    }

    void OnOffShootersMenu()
    {
        m_shooterSettings.StartMenuOperations();
    }

    void OnOffConsumablesMenu()
    {
        m_consumableSettings.StartMenuOperations();
    }

}
