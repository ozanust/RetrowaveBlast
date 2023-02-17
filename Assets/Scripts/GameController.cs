using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class GameController : MonoBehaviour
{
    public enum GameState
    {
        Ready = 0,
        Stopped = 1,
        Paused = 2,
        Playing = 3,
        Finished = 4
    }


    #region Member Variables

    [SerializeField]
    private CircleBlock[] m_preDefinedBlockPool;

    [SerializeField]
    LevelConfig m_levelConfig;

    [SerializeField]
    private List<CircleBlock> m_runtimeBlockPool;

    [SerializeField]
    private GameObject m_bulletHitEffectPrefab;

    [SerializeField]
    private List<GameObject> m_bulletHitEffectPool = new List<GameObject>();

    [SerializeField]
    private List<CircleBlock> m_subBlockPool;

    [SerializeField]
    private Gold m_goldPrefab;

    [SerializeField]
    private List<Gold> m_goldList;

    [SerializeField]
    private GameManager m_gameManager;

    [SerializeField]
    private EndLevelPanel m_endLevelPanel;

    [SerializeField]
    private ShooterController m_shooterController;

    [SerializeField]
    public UIHUDController m_uiHudController;

    [SerializeField]
    public MainUIController m_mainUIController;

    [SerializeField]
    private BackgroundController m_bgController;

    [SerializeField]
    private BoxCollider2D m_leftBoundaryCollider;

    [SerializeField]
    private BoxCollider2D m_rightBoundaryCollider;

    [SerializeField]
    private GameState m_gameState;

    [SerializeField]
    private int m_activeBlockCount = 0;

    [SerializeField]
    private int m_destroyedBlockCount = 0;

    [SerializeField]
    private int m_totalBlockCountToDestroy = 0;

    [SerializeField]
    private int m_interstitialAdInterval = 5;

    private float m_leftBlockSpawnPoint = -6;
    private float m_rightBlockSpawnPoint = 6;
    private int m_score = 0;
    private int m_gold = 0;
    private int m_totalGoldOfCurrentLevel = 0;
    private bool m_isLevelFailed = false;
    private int m_maximumBlockTypeIndex = 0;
    private int m_gameFailCount = 0;

    Coroutine c_populateBlockPool = null;
    Coroutine c_throwBlocksOnGameArea = null;
    Coroutine c_gameFinishedSuccesfully = null;
    Coroutine c_onLevelFailed = null;
    Coroutine c_onContinue = null;

    private AudioManager m_audioManager;

    #endregion


    #region Getters/Setters

    public CircleBlock[] PredefinedBlockPool
    {
        //get { return m_preDefinedBlockPool; }
        set { m_preDefinedBlockPool = value; }
    }

    public int ActiveBlockCount
    {
        get { return m_activeBlockCount; }
        set { m_activeBlockCount = value; }
    }

    public int DestroyedBlockCount
    {
        get { return m_destroyedBlockCount; }
        set { m_destroyedBlockCount = value;
            m_uiHudController.FillBar((float)m_destroyedBlockCount / (float)TotalBlockCountToDestroy);
            if(m_destroyedBlockCount == m_totalBlockCountToDestroy)
            {
                ListenGameFinish();
            }
        }
    }

    public int TotalBlockCountToDestroy
    {
        get { return m_totalBlockCountToDestroy; }
    }

    public GameState CurrentGameState
    {
        get { return m_gameState; }
        set { m_gameState = value;
            m_mainUIController.DoMenuOperation(m_gameState);
        }
    }

    public int Gold
    {
        get { return m_gold; }
    }

    public int Score
    {
        get { return m_score; }
    }

    #endregion


    #region Initializers

    private void Awake()
    {
        if (m_gameManager == null)
            m_gameManager = GameManager.Instance;

        if (m_audioManager == null)
            m_audioManager = AudioManager.Instance;

        Init();
    }

    void Init()
    {
        m_gameState = GameState.Ready;
        SetBoundaryColliderPositions();
    }

    #endregion


    #region Finalizers

    private void OnDisable()
    {
        m_runtimeBlockPool.Clear();
        m_activeBlockCount = 0;
    }

    #endregion


    #region MonoBehaviour Methods

    private void Update()
    {
        if(Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    if (m_gameState == GameState.Ready)
                    {
                        //m_mainUIController.FadeOutMainBackground();
                        StartLevel();
                    }
                }
            }
        }else if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (m_gameState == GameState.Ready)
                {
                    //m_mainUIController.FadeOutMainBackground();
                    StartLevel();
                }
            }
        }
    }

    #endregion


    #region Coroutines

    IEnumerator CPopulateBlockPool()
    {
        List<CircleBlock> tempList = new List<CircleBlock>();
        for (int i = 0; i < m_levelConfig.BlockCountPerLevel[m_gameManager.CurrentLevel]; i++)
        {
            CircleBlock tempBlock = Instantiate(m_preDefinedBlockPool[Random.Range(0, m_maximumBlockTypeIndex + 1)], new Vector3(-10, 0, 5), Quaternion.identity);
            tempBlock.SetBasePoint(Random.Range(m_levelConfig.MinBlockPointPerLevel[m_gameManager.CurrentLevel], m_levelConfig.MaxBlockPointPerLevel[m_gameManager.CurrentLevel] + 1));
            tempBlock.BounceHeight = Random.Range(Constants.MIN_BOUNCE_HEIGHT, Constants.MAX_BOUNCE_HEIGHT);
            tempBlock.SceneGameController = this;

            bool tempRand = RandomBool();

            if (tempRand)
            {
                tempBlock.IsLeftBlock = true;
            }
            else
            {
                tempBlock.IsLeftBlock = false;
            }

            tempBlock.gameObject.transform.position = new Vector3((tempBlock.IsLeftBlock) ? m_leftBlockSpawnPoint : m_rightBlockSpawnPoint, tempBlock.BounceHeight, 5);

            tempList.Add(tempBlock);
        }

        List<CircleBlock> orderedPool = tempList.OrderBy(block => block.BasePoint).ToList();

        for(int i = 0; i < orderedPool.Count; i++)
        {
            m_runtimeBlockPool.Add(orderedPool[i]);
        }

        CalculateTotalBlockCount();
        AttachGoldsToBlocks();
        yield return new WaitForEndOfFrame();

        c_populateBlockPool = null;
    }

    IEnumerator CThrowBlocksOnGameArea()
    {
        for (int i = 0; i < m_runtimeBlockPool.Count; i++)
        {
            if (m_activeBlockCount >= Constants.MAX_BLOCK_COUNT_IN_GAMEPLAY_AREA)
                yield return new WaitUntil(() => m_activeBlockCount < Constants.MAX_BLOCK_COUNT_IN_GAMEPLAY_AREA);

            if (m_gameState != GameState.Playing)
                break;

            m_runtimeBlockPool[i].StartEntryToGameAreaAnimation(m_runtimeBlockPool[i].IsLeftBlock);
            yield return new WaitForSeconds(Random.Range(2, 6));
        }

        c_throwBlocksOnGameArea = null;
    }

    IEnumerator COnLevelSuccess()
    {
        m_audioManager.StopAll();
        yield return new WaitForEndOfFrame();
        print("Level Finished Successfully!");
        CurrentGameState = GameState.Finished;

        KillAllBlocks();
        DestroyGolds();
        m_activeBlockCount = 0;
        m_gameManager.CurrentLevel += 1;
        m_shooterController.DestroyAllBullets();
        m_totalBlockCountToDestroy = 0;
        m_destroyedBlockCount = 0;

        ActivateLevelEndPanel(true);

        UpdateUILevelText(m_gameManager.CurrentLevel);

        c_gameFinishedSuccesfully = null;
    }

    IEnumerator COnLevelFailed()
    {
        m_audioManager.PauseAll();
        //m_audioManager.CrossFadeGameMusicToMenuMusic();
        yield return new WaitForEndOfFrame();
        CurrentGameState = GameState.Paused;

        ActivateLevelEndPanel(false);

        c_onLevelFailed = null;
    }

    IEnumerator COnContinue()
    {
        AudioManager.Instance.UnPauseAll();
        CurrentGameState = GameState.Playing;

        float timeVel = 0;
        while (Time.timeScale < 1f)
        {
            Time.timeScale = Mathf.SmoothDamp(Time.timeScale, 1.01f, ref timeVel, 1, 10, 0.06f);
            m_audioManager.SetGameMusicPitch(Time.timeScale);

            yield return new WaitForEndOfFrame();
        }

        Time.timeScale = 1;
        m_audioManager.SetGameMusicPitch(1);

        c_onContinue = null;
    }

    #endregion


    #region Coroutine Callers

    void ThrowBlocksOnGameArea()
    {
        if (c_throwBlocksOnGameArea == null)
            c_throwBlocksOnGameArea = StartCoroutine(CThrowBlocksOnGameArea());
    }

    void ListenGameFinish()
    {
        if (c_gameFinishedSuccesfully == null)
            c_gameFinishedSuccesfully = StartCoroutine(COnLevelSuccess());
    }

    void PopulateBlockPool()
    {
        if (c_populateBlockPool == null)
            c_populateBlockPool = StartCoroutine(CPopulateBlockPool());
    }

    public void FailLevel()
    {
        if (c_onLevelFailed == null)
            c_onLevelFailed = StartCoroutine(COnLevelFailed());
    }

    #endregion


    #region Public Methods

    public void StartLevel()
    {
        if (m_mainUIController.General.CurrentMenuState == GeneralSettings.MenuState.Open)
            m_mainUIController.General.StartMenuOperations();

        m_bgController.SetBackground();

        m_audioManager.SetGameMusic();
        SetMaximumBlockIndex();
        //AudioManager.Instance.PlaySound(SoundLibrary.MUSIC);
        //m_audioManager.PlayGameMusic();
        m_audioManager.CrossFadeMenuMusicToGameMusic();
        m_totalGoldOfCurrentLevel = 0;
        m_totalBlockCountToDestroy = 0;
        m_destroyedBlockCount = 0;
        PopulateBlockPool();
        PopulateHitEffectPool();

        Time.timeScale = 1;
        m_shooterController.ActiveShooter.ResetShooter();
        CurrentGameState = GameState.Playing;
        ThrowBlocksOnGameArea();
    }

    public void EndLevel()
    {
        m_audioManager.StopAll();
        CurrentGameState = GameState.Finished;

        if(c_throwBlocksOnGameArea != null)
        {
            StopCoroutine(CThrowBlocksOnGameArea());
            c_throwBlocksOnGameArea = null;
        }
        
        KillAllBlocks();
        DestroyGolds();
        m_activeBlockCount = 0;
        m_shooterController.DestroyAllBullets();

        m_uiHudController.ResetAllValues();
        UpdateUILevelText(m_gameManager.CurrentLevel);

        Time.timeScale = 1;
        m_score = 0;
        m_gold = 0;
    }

    public void ContinueGame()
    {
        if (c_onContinue == null)
            c_onContinue = StartCoroutine(COnContinue());
    }

    public void UpdateScore(int score)
    {
        m_score += score;
        m_uiHudController.UpdateScore(m_score);
        if (m_score > m_gameManager.PlayerProfile.HighestScore)
        {
            m_gameManager.PlayerProfile.HighestScore = m_score;
        }
    }

    public void UpdateUILevelText(int level)
    {
        m_uiHudController.UpdateLevel(level);
    }

    public void AddSubBlockToSubBlockPool(CircleBlock subBlock)
    {
        m_subBlockPool.Add(subBlock);
    }

    #endregion


    #region Private Methods

    /// <summary>
    /// Activates the end level screen
    /// </summary>
    /// <param name="isLevelSuccess">True if level finished succesfully, otherwise false.</param>
    void ActivateLevelEndPanel(bool isLevelSuccess)
    {
        m_endLevelPanel.IsLevelSucceeded = isLevelSuccess;
        m_endLevelPanel.gameObject.SetActive(true);
        m_endLevelPanel.SetCoinValue(m_gold);
        m_endLevelPanel.SetScoreValue(m_score);
        m_endLevelPanel.SetLevelValue(m_gameManager.PlayerProfile.Level);
    }

    void KillAllBlocks()
    {
        for(int i = 0; i < m_runtimeBlockPool.Count; i++)
        {
            m_runtimeBlockPool[i].KillBlock();
        }

        for (int i = 0; i < m_subBlockPool.Count; i++)
        {
            m_subBlockPool[i].KillBlock();
        }

        m_runtimeBlockPool.Clear();
        m_subBlockPool.Clear();
    }

    void DestroyGolds()
    {
        for (int i = 0; i < m_goldList.Count; i++)
        {
            if (!m_goldList[i].gameObject.activeInHierarchy)
                Destroy(m_goldList[i].gameObject);
            else
                m_goldList[i].Destroy();
        }

        m_goldList.Clear();
    }

    void SetMaximumBlockIndex()
    {
        if (m_gameManager.PlayerProfile.Level < 5)
            m_maximumBlockTypeIndex = 1;
        else if (m_gameManager.PlayerProfile.Level >= 5)
            m_maximumBlockTypeIndex = 2;
    }

    void PopulateHitEffectPool()
    {
        for(int i = 0; i < m_gameManager.PlayerProfile.FireSpeed; i++)
        {
            GameObject tempEffect = Instantiate(m_bulletHitEffectPrefab, new Vector3(-10, -10, 0), Quaternion.identity);
            m_bulletHitEffectPool.Add(tempEffect);
        }
    }

    #endregion


    #region Utilities

    float RandomSelect(float rand1, float rand2)
    {
        int tempRand = Random.Range(0, 2);
        if (tempRand == 0)
            return rand1;
        else
            return rand2;
    }

    bool RandomBool()
    {
        int tempRand = Random.Range(0, 2);
        if (tempRand == 0)
            return true;
        else
            return false;
    }

    void CalculateTotalBlockCount()
    {
        for(int i = 0; i < m_runtimeBlockPool.Count; i++)
        {
            if(m_runtimeBlockPool[i].BlockSize == CircleBlock.Size.Small)
            {
                m_totalBlockCountToDestroy += 1;
            }else if(m_runtimeBlockPool[i].BlockSize == CircleBlock.Size.Medium)
            {
                m_totalBlockCountToDestroy += 3;
            }
            else if (m_runtimeBlockPool[i].BlockSize == CircleBlock.Size.Big)
            {
                m_totalBlockCountToDestroy += 7;
            }
            else if (m_runtimeBlockPool[i].BlockSize == CircleBlock.Size.Huge)
            {
                m_totalBlockCountToDestroy += 15;
            }
        }
    }

    int CalculateTotalGoldAmount()
    {
        float totalPoint = 0;
        for (int i = 0; i < m_runtimeBlockPool.Count; i++)
        {
            totalPoint += m_runtimeBlockPool[i].BasePoint;
        }

        m_totalGoldOfCurrentLevel = (int)(totalPoint / 10f);
        m_totalGoldOfCurrentLevel = m_totalGoldOfCurrentLevel * (int)GameManager.Instance.PlayerProfile.GoldDropMultiplier;
        return m_totalGoldOfCurrentLevel;
    }

    void AttachGoldsToBlocks()
    {
        /*int totalGold = CalculateTotalGoldAmount();
        int goldCount = totalGold / 5;*/

        foreach(CircleBlock block in m_runtimeBlockPool)
        {
            float rand = Random.Range(0f, 1f);
            if(rand >= 0.2f)
            {
                //TODO: Set a maximum gold count
                int goldCountRand = Random.Range(2, Mathf.Ceil(((float)m_gameManager.CurrentLevel * GameManager.Instance.PlayerProfile.GoldDropMultiplier / 5f)) > 4 ? (int)Mathf.Ceil((float)m_gameManager.CurrentLevel * GameManager.Instance.PlayerProfile.GoldDropMultiplier / 5f) : 4);

                for(int i = 0; i < goldCountRand; i++)
                {
                    Gold gold = Instantiate(m_goldPrefab, new Vector3(0, 0, 5), Quaternion.identity);
                    gold.transform.SetParent(block.transform);
                    gold.transform.localPosition = new Vector3(0, 0, 0);
                    gold.Value = (int)Mathf.Ceil(block.BasePoint / 3f);
                    block.AddGold(gold);
                    m_goldList.Add(gold);
                }
            }
        }
    }

    /*CircleBlock GetDesiredBlockTypeFromPool(CircleBlock[] blockPool)
    {

    }*/

    void SetBoundaryColliderPositions()
    {
        float cameraHalfHorizontalWidth = Camera.main.orthographicSize * (float)Screen.width / (float)Screen.height;
        m_leftBoundaryCollider.transform.position = new Vector3(-cameraHalfHorizontalWidth  - m_leftBoundaryCollider.size.x / 2f, 0, 5);
        m_rightBoundaryCollider.transform.position = new Vector3(cameraHalfHorizontalWidth + m_rightBoundaryCollider.size.x / 2f, 0, 5);
    }

    BlockLayer GetDeactiveLayer(BlockLayer[] blockLayerPool)
    {
        int layerID = 0;
        while (blockLayerPool[layerID].IsOccupied)
        {
            layerID++;
        }

        return blockLayerPool[layerID];
    }

    public GameObject GetNonActiveEffect()
    {
        int index = 0;
        while (m_bulletHitEffectPool[index].activeInHierarchy)
        {
            index++;
        }

        return m_bulletHitEffectPool[index];
    }

    public void GainGoldForLevel(int gold)
    {
        m_gold += gold;
    }

    #endregion

}
