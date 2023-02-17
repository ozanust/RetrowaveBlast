using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{

    [SerializeField]
    private int m_fireSpeed;

    [SerializeField]
    private float m_bulletPower;

    [SerializeField]
    private float m_goldDropMultiplier = 1.0f;

    [SerializeField]
    private List<int> m_unlockedShooterIDList;

    [SerializeField]
    private int m_gold;

    [SerializeField]
    private int m_level;

    [SerializeField]
    private int m_highestScore;

    [SerializeField]
    private List<int> m_unlockedBackgroundIDList;

    [SerializeField]
    private bool m_noAdPurchase;

    //this is for next time playing the game.
    [SerializeField]
    private int m_selectedShooterID;

    [SerializeField]
    private int m_selectedBackgroundID;


    public Player(int fireSpeed, float bulletPower, float goldDropMulitplier, List<int> unlockedShooterIDList, int gold, int level, int highestScore, List<int> unlockedBackgroundIDList, int selectedShooterID, int selectedBackgroundID, bool noAdPurchase)
    {
        this.m_fireSpeed = fireSpeed;
        this.m_bulletPower = bulletPower;
        this.m_goldDropMultiplier = goldDropMulitplier;
        this.m_unlockedShooterIDList = unlockedShooterIDList;
        this.m_gold = gold;
        this.m_level = level;
        this.m_highestScore = highestScore;
        this.m_unlockedBackgroundIDList = unlockedBackgroundIDList;
        this.m_selectedShooterID = selectedShooterID;
        this.m_selectedBackgroundID = selectedBackgroundID;
        this.m_noAdPurchase = noAdPurchase;
    }


    public int FireSpeed
    {
        get { return m_fireSpeed; }
    }

    public float BulletPower
    {
        get { return m_bulletPower; }
    }

    public float GoldDropMultiplier
    {
        get { return m_goldDropMultiplier; }
        set { m_goldDropMultiplier = value; }
    }

    public List<int> UnlockedShooterIDList
    {
        get { return m_unlockedShooterIDList; }
    }

    public List<int> UnlockedBackgroundIDList
    {
        get { return m_unlockedBackgroundIDList; }
    }

    public int Gold
    {
        get { return m_gold; }
    }

    public int Level
    {
        get { return m_level; }
        set
        {
            m_level = value;
            PlayerPrefs.SetInt("GameLevel", m_level);
        }
    }

    public int HighestScore
    {
        get { return m_highestScore; }
        set
        {
            m_highestScore = value;
            PlayerPrefs.SetInt("HighestScore", m_highestScore);
        }
    }

    public int SelectedShooterID
    {
        get { return m_selectedShooterID; }
    }

    public int SelectedBackgroundID
    {
        get { return m_selectedBackgroundID; }
    }

    public bool NoAdPurchase
    {
        get { return m_noAdPurchase; }
    }

    public void GainGold(int gold)
    {
        m_gold += gold;
        PlayerPrefs.SetInt("Gold", gold);
    }

    public void SpendGold(int gold)
    {
        m_gold -= gold;
        PlayerPrefs.SetInt("Gold", gold);
    }

    public void IncreaseFireSpeed(int increment)
    {
        m_fireSpeed += increment;
    }

    public void IncreaseFirePower(float increment)
    {
        m_bulletPower += increment;
    }

    public void IncreaseGoldMultiplier(float increment)
    {
        m_goldDropMultiplier += increment;
    }

    public void SetSelectedShooterID(int id)
    {
        m_selectedShooterID = id;
        PlayerPrefs.SetInt("SelectedShooterID", id);
    }

    public void SetSelectedBackgroundID(int id)
    {
        m_selectedBackgroundID = id;
        PlayerPrefs.SetInt("SelectedBackgroundID", id);
    }

    public void SetNoAdState(bool isPurchased)
    {
        m_noAdPurchase = isPurchased;
    }
}
