using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSettings : MonoBehaviour {

    [SerializeField]
    private Button m_fireSpeedUpgradeButton;

    [SerializeField]
    private Text m_fireSpeedPriceText;

    [SerializeField]
    private Button m_firePowerUpgradeButton;

    [SerializeField]
    private Text m_firePowerPriceText;

    [SerializeField]
    private Button m_goldMultiplierUpgradeButton;

    [SerializeField]
    private Text m_goldMultiplierPriceText;

    [SerializeField]
    private Text m_fireSpeedNextUpgradeValue;

    [SerializeField]
    private Text m_firePowerNextUpgradeValue;

    [SerializeField]
    private Text m_goldMultiplierNextUpgradeValue;

    private int[] m_fireSpeedUpgradePrices;
    private int[] m_firePowerUpgradePrices;
    private int[] m_goldMultiplierUpgradePrices;

    private int m_currentFireSpeed, m_currentFirePower, m_currentGoldMultiplier;

    private GameManager m_gameManager;

    private void Awake()
    {
        if (m_gameManager == null)
            m_gameManager = GameManager.Instance;

        m_fireSpeedUpgradePrices = MakePriceList(5, 1.15f);
        m_firePowerUpgradePrices = MakePriceList(10, 1.1f);
        m_goldMultiplierUpgradePrices = MakePriceList(10, 1.4f);

        m_currentFireSpeed = m_gameManager.PlayerProfile.FireSpeed;
        m_currentFirePower = Mathf.CeilToInt(m_gameManager.PlayerProfile.BulletPower / 0.1f) - 10;
        m_currentGoldMultiplier = Mathf.CeilToInt(m_gameManager.PlayerProfile.GoldDropMultiplier / 0.1f) - 10;
    }

    private void Start()
    {
        m_fireSpeedPriceText.text = m_fireSpeedUpgradePrices[m_currentFireSpeed].ToString();
        m_firePowerPriceText.text = m_firePowerUpgradePrices[m_currentFirePower].ToString();
        m_goldMultiplierPriceText.text = m_goldMultiplierUpgradePrices[m_currentGoldMultiplier].ToString();
        m_firePowerNextUpgradeValue.text = (1 + (m_currentFirePower * 0.1f + 0.1f)).ToString();
        m_fireSpeedNextUpgradeValue.text = (m_currentFireSpeed + 1).ToString();
        m_goldMultiplierNextUpgradeValue.text = (1 + (m_currentGoldMultiplier * 0.1f + 0.1f)).ToString();
    }

    private void OnEnable()
    {
        RegisterEventHandlers();

        if (m_gameManager.PlayerProfile.Gold < m_fireSpeedUpgradePrices[m_currentFireSpeed])
        {
            m_fireSpeedUpgradeButton.interactable = false;
        }
        else
        {
            m_fireSpeedUpgradeButton.interactable = true;
        }

        if (m_gameManager.PlayerProfile.Gold < m_firePowerUpgradePrices[m_currentFirePower])
        {
            m_firePowerUpgradeButton.interactable = false;
        }
        else
        {
            m_firePowerUpgradeButton.interactable = true;
        }

        if (m_gameManager.PlayerProfile.Gold < m_goldMultiplierUpgradePrices[m_currentGoldMultiplier])
        {
            m_goldMultiplierUpgradeButton.interactable = false;
        }
        else
        {
            m_goldMultiplierUpgradeButton.interactable = true;
        }
    }

    private void OnDisable()
    {
        RemoveEventHandlers();
    }

    private void RegisterEventHandlers()
    {
        m_fireSpeedUpgradeButton.onClick.AddListener(OnClickUpgradeFireSpeedButton);
        m_firePowerUpgradeButton.onClick.AddListener(OnClickUpgradeFirePowerButton);
        m_goldMultiplierUpgradeButton.onClick.AddListener(OnClickUpgradeGoldMultiplierButton);
    }

    private void RemoveEventHandlers()
    {
        m_fireSpeedUpgradeButton.onClick.RemoveAllListeners();
        m_firePowerUpgradeButton.onClick.RemoveAllListeners();
        m_goldMultiplierUpgradeButton.onClick.RemoveAllListeners();
    }

    private int[] MakePriceList(int initialPrice, float priceIncrementRate)
    {
        int[] upgradePrices = new int[1000];
        upgradePrices[0] = initialPrice;

        for(int i = 1; i < upgradePrices.Length; i++)
        {
            upgradePrices[i] = Mathf.CeilToInt(upgradePrices[i - 1] * priceIncrementRate);
        }

        return upgradePrices;
    }

    private void OnClickUpgradeFireSpeedButton()
    {
        m_gameManager.PlayerProfile.SpendGold(m_fireSpeedUpgradePrices[m_currentFireSpeed]);
        m_gameManager.PlayerProfile.IncreaseFireSpeed(1);
        m_gameManager.ActiveShooter.FireSpeed = m_gameManager.PlayerProfile.FireSpeed;
        m_currentFireSpeed += 1;
        m_fireSpeedNextUpgradeValue.text = (m_currentFireSpeed + 1).ToString();
        m_fireSpeedPriceText.text = m_fireSpeedUpgradePrices[m_currentFireSpeed].ToString();
        m_gameManager.ActiveShooter.FireSpeed = m_gameManager.PlayerProfile.FireSpeed;

        if (m_gameManager.PlayerProfile.Gold < m_fireSpeedUpgradePrices[m_currentFireSpeed])
        {
            m_fireSpeedUpgradeButton.interactable = false;
        }

        if (m_gameManager.PlayerProfile.Gold < m_firePowerUpgradePrices[m_currentFirePower])
        {
            m_firePowerUpgradeButton.interactable = false;
        }

        if (m_gameManager.PlayerProfile.Gold < m_goldMultiplierUpgradePrices[m_currentGoldMultiplier])
        {
            m_goldMultiplierUpgradeButton.interactable = false;
        }
    }

    private void OnClickUpgradeFirePowerButton()
    {
        m_gameManager.PlayerProfile.SpendGold(m_firePowerUpgradePrices[m_currentFirePower]);
        m_gameManager.PlayerProfile.IncreaseFirePower(0.1f);
        m_gameManager.ActiveShooter.Power = m_gameManager.PlayerProfile.BulletPower;
        m_currentFirePower += 1;
        m_firePowerNextUpgradeValue.text = (1 + (m_currentFirePower * 0.1f + 0.1f)).ToString();
        m_firePowerPriceText.text = m_firePowerUpgradePrices[m_currentFirePower].ToString();
        m_gameManager.ActiveShooter.Power = m_gameManager.PlayerProfile.BulletPower;

        if (m_gameManager.PlayerProfile.Gold < m_fireSpeedUpgradePrices[m_currentFireSpeed])
        {
            m_fireSpeedUpgradeButton.interactable = false;
        }

        if (m_gameManager.PlayerProfile.Gold < m_firePowerUpgradePrices[m_currentFirePower])
        {
            m_firePowerUpgradeButton.interactable = false;
        }

        if (m_gameManager.PlayerProfile.Gold < m_goldMultiplierUpgradePrices[m_currentGoldMultiplier])
        {
            m_goldMultiplierUpgradeButton.interactable = false;
        }
    }

    private void OnClickUpgradeGoldMultiplierButton()
    {
        m_gameManager.PlayerProfile.SpendGold(m_goldMultiplierUpgradePrices[m_currentGoldMultiplier]);
        m_gameManager.PlayerProfile.IncreaseGoldMultiplier(0.1f);
        m_currentGoldMultiplier += 1;
        m_goldMultiplierNextUpgradeValue.text = (1 + (m_currentGoldMultiplier * 0.1f + 0.1f)).ToString();
        m_goldMultiplierPriceText.text = m_goldMultiplierUpgradePrices[m_currentGoldMultiplier].ToString();
        PlayerPrefs.SetFloat("GoldMultiplier", m_gameManager.PlayerProfile.GoldDropMultiplier);

        if (m_gameManager.PlayerProfile.Gold < m_fireSpeedUpgradePrices[m_currentFireSpeed])
        {
            m_fireSpeedUpgradeButton.interactable = false;
        }

        if (m_gameManager.PlayerProfile.Gold < m_firePowerUpgradePrices[m_currentFirePower])
        {
            m_firePowerUpgradeButton.interactable = false;
        }

        if (m_gameManager.PlayerProfile.Gold < m_goldMultiplierUpgradePrices[m_currentGoldMultiplier])
        {
            m_goldMultiplierUpgradeButton.interactable = false;
        }
    }
}
