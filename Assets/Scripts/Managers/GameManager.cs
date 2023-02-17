using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoManager<GameManager>
{
    [SerializeField]
    private bool m_dontDestroyOnLoad = true;

    [SerializeField]
    private bool m_isVibrationOn;

    [SerializeField]
    private bool m_isAudioOn;

    [SerializeField]
    private Shooter m_activeShooter;

    [SerializeField]
    private BackgroundController m_backgroundController;

    [SerializeField]
    private Player m_player;

    //For debug
    [SerializeField]
    private Background m_activeBackground;

    public Shooter ActiveShooter
    {
        get { return m_activeShooter; }
        set { m_activeShooter = value; }
    }

    public int CurrentLevel
    {
        get { return PlayerProfile.Level; }
        set
        {
            PlayerProfile.Level = value;
            PlayerPrefs.SetInt("GameLevel", PlayerProfile.Level);
        }
    }

    public bool IsVibrationOn
    {
        get { return m_isVibrationOn; }
    }

    public bool IsAudioOn
    {
        get { return m_isAudioOn; }
    }

    public BackgroundController BackgroundController
    {
        get
        {
            return m_backgroundController;
        }

        set
        {
            m_backgroundController = value;
        }
    }

    public Player PlayerProfile
    {
        get
        {
            return m_player;
        }

        set
        {
            m_player = value;
        }
    }

    public Background ActiveBackground
    {
        get
        {
            return m_activeBackground;
        }

        set
        {
            m_activeBackground = value;
        }
    }

    private void Awake()
    {
        Init();

        if (m_dontDestroyOnLoad)
            DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            SceneManager.LoadScene(1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    override public void Init()
    {
        /*PlayerPrefs.DeleteKey("Player");
        PlayerPrefs.DeleteKey("GoldMultiplier");*/

        if (PlayerPrefs.HasKey("Player"))
            m_player = DataManager.Instance.LoadPlayerFromMemory();
        else
            m_player = new Player(10, 1, 1, new List<int>(), 0, 0, 0, new List<int>(), 0, 0, false);

        if (PlayerPrefs.HasKey("GoldMultiplier"))
            m_player.GoldDropMultiplier = PlayerPrefs.GetFloat("GoldMultiplier");

        if (PlayerPrefs.HasKey("Vibration"))
            m_isVibrationOn = PlayerPrefs.GetInt("Vibration") == 1 ? true : false;
        else
            m_isVibrationOn = true;

        if (PlayerPrefs.HasKey("Audio"))
            m_isAudioOn = PlayerPrefs.GetInt("Audio") == 1 ? true : false;
        else
            m_isAudioOn = true;

        if (PlayerPrefs.HasKey("Audio"))
            AudioListener.volume = PlayerPrefs.GetInt("Audio");
        else
            AudioListener.volume = 1;
    }

    /// <summary>
    /// Set vibration state.
    /// </summary>
    /// <param name="isOn">True if vibration is on, otherwise false.</param>
    public void SetVibration(bool isOn)
    {
        m_isVibrationOn = isOn;
        PlayerPrefs.SetInt("Vibration", m_isVibrationOn ? 1 : 0);
    }

    /// <summary>
    /// Set game audio state.
    /// </summary>
    /// <param name="isOn">True if audio is on, otherwise false.</param>
    public void SetAudio(bool isOn)
    {
        m_isAudioOn = isOn;
        PlayerPrefs.SetInt("Audio", m_isAudioOn ? 1 : 0);
        AudioListener.volume = PlayerPrefs.GetInt("Audio");
    }

    private void OnApplicationQuit()
    {
        DataManager.Instance.SavePlayer(m_player);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            DataManager.Instance.SavePlayer(m_player);
    }
}
