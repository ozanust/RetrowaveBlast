using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockableManager : MonoManager<UnlockableManager> {

    [SerializeField]
    private bool m_dontDestroyOnLoad = true;

    [SerializeField]
    private List<ShooterItem> m_unlockableShooterList;

    [SerializeField]
    private List<BackgroundItem> m_unlockableBackgroundList;

    [SerializeField]
    private GridLayoutGroup m_shooterLayout;

    [SerializeField]
    private GridLayoutGroup m_backgroundLayout;

    [SerializeField]
    private UnlockableShooter m_unlockableShooterPrefab;

    [SerializeField]
    private UnlockableBackground m_unlokcableBackgroundPrefab;

    private Dictionary<int, ShooterItem> m_unlockableShooterDict = new Dictionary<int, ShooterItem>();
    private Dictionary<int, BackgroundItem> m_unlockableBackgroundDict = new Dictionary<int, BackgroundItem>();

    private void Awake()
    {
        if (m_dontDestroyOnLoad)
            DontDestroyOnLoad(this.gameObject);

        foreach (ShooterItem us in m_unlockableShooterList)
        {
            m_unlockableShooterDict.Add(us.ID, us);
            UnlockableShooter tempShooter = Instantiate(m_unlockableShooterPrefab, m_shooterLayout.transform);
            tempShooter.SetShooterProperties(us.ShooterStyle, us.LockedSprite, us.ShooterBulletStyle, us.ID, us.Price);
            if (GameManager.Instance.PlayerProfile.UnlockedShooterIDList.Contains(us.ID))
            {
                tempShooter.UnlockWithoutGoldSpending();
            }
        }

        foreach (BackgroundItem ub in m_unlockableBackgroundList)
        {
            m_unlockableBackgroundDict.Add(ub.ID, ub);
            UnlockableBackground tempBackground = Instantiate(m_unlokcableBackgroundPrefab, m_backgroundLayout.transform);
            tempBackground.SetBackgroundProperties(ub.Background, ub.ID, ub.Price);
            if (GameManager.Instance.PlayerProfile.UnlockedBackgroundIDList.Contains(ub.ID))
            {
                tempBackground.UnlockWithoutGoldSpending();
            }
        }
    }

    public List<ShooterItem> UnlockableShooters
    {
        get { return m_unlockableShooterList; }
    }

    public List<BackgroundItem> UnlockableBackgrounds
    {
        get{ return m_unlockableBackgroundList; }
    }

    public ShooterItem GetUnlockableShooterByID(int id)
    {
        return m_unlockableShooterDict[id];
    }

    public BackgroundItem GetUnlockableBackgroundByID(int id)
    {
        return m_unlockableBackgroundDict[id];
    }
}
