using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHUDController : MonoBehaviour {

    [SerializeField]
    private GameController m_gameController;

    [SerializeField]
    private Text m_scoreValueField;

    [SerializeField]
    private Text m_levelValueField;

    [SerializeField]
    private Text m_coinValueField;

    [SerializeField]
    private Text m_currentLevelValueField;

    [SerializeField]
    private Text m_nextLevelValueField;

    [SerializeField]
    private Image m_barHorFill;

    private float m_fillAmount = 0;
    private int m_totalBlockCount = 0;
    Player m_player;

    private Coroutine c_fillBarCoroutine = null;

    private void Awake()
    {
        m_player = GameManager.Instance.PlayerProfile;
    }

    private void Start()
    {
        m_totalBlockCount = m_gameController.TotalBlockCountToDestroy;
        UpdateLevel(m_player.Level);
        UpdateCoin();
    }

    private void Update()
    {
        UpdateCoin();
    }

    public void UpdateScore(int score)
    {
        m_scoreValueField.text = score.ToString();
    }

    public void UpdateLevel(int level)
    {
        m_levelValueField.text = (level + 1).ToString();
        m_currentLevelValueField.text = m_player.Level.ToString();
        m_nextLevelValueField.text = (m_player.Level + 1).ToString(); 
    }

    public void UpdateCoin()
    {
        m_coinValueField.text = m_player.Gold.ToString();
    }

    IEnumerator CFillBar(float fillAmount)
    {
        while(m_barHorFill.fillAmount < fillAmount)
        {
            m_barHorFill.fillAmount += 0.02f;
            yield return new WaitForEndOfFrame();
        }

        m_barHorFill.fillAmount = fillAmount;

        c_fillBarCoroutine = null;
    }

    public void FillBar(float fillAmount)
    {
        if (c_fillBarCoroutine == null)
            c_fillBarCoroutine = StartCoroutine(CFillBar(fillAmount));
    }

    public void ResetAllValues()
    {
        if(c_fillBarCoroutine != null)
        {
            StopCoroutine(c_fillBarCoroutine);
            c_fillBarCoroutine = null;
        }

        m_scoreValueField.text = "0";
        m_coinValueField.text = GameManager.Instance.PlayerProfile.Gold.ToString();
        m_levelValueField.text = GameManager.Instance.PlayerProfile.Level.ToString();
        m_fillAmount = 0;
        m_barHorFill.fillAmount = 0;
    }
}
