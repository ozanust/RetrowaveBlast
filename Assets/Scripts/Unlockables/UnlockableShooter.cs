using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnlockableShooter : UnlockableItem {

    [SerializeField]
    private Sprite m_shooterStyle;

    [SerializeField]
    private Sprite m_shooterBulletStyle;

    private ShooterSettings m_shooterSettings;

    public Sprite ShooterStyle
    {
        get
        {
            return m_shooterStyle;
        }
    }

    public Sprite ShooterBulletStyle
    {
        get
        {
            return m_shooterBulletStyle;
        }
    }

    public int ID
    {
        get { return m_itemID; }
    }

    public GameObject SelectionOutline
    {
        get { return m_selectionOutline; }
    }

    public bool IsUnlocked
    {
        get { return m_isUnlocked; }
    }

    public void SetShooterProperties(Sprite shooterSprite, Sprite lockedSprite, Sprite bulletSprite, int id, int price)
    {
        m_shooterSettings = FindObjectOfType<ShooterSettings>();
        m_shooterStyle = shooterSprite;
        m_unlockedSprite = shooterSprite;
        m_itemImage.sprite = lockedSprite;
        m_shooterBulletStyle = bulletSprite;
        m_itemID = id;
        m_itemPrice = price;
        m_priceText.text = price.ToString();
    }

    public override void Unlock()
    {
        if (!m_isUnlocked && GameManager.Instance.PlayerProfile.Gold >= m_itemPrice)
        {
            m_isUnlocked = true;
            //to change question mark (placeholder) to unlocked ship image
            m_itemImage.sprite = m_unlockedSprite;
            GameManager.Instance.PlayerProfile.SpendGold(m_itemPrice);
            GameManager.Instance.PlayerProfile.UnlockedShooterIDList.Add(m_itemID);
            UseItem();
        }
        else if (m_isUnlocked)
        {
            UseItem();
        }
    }

    public void UnlockWithoutGoldSpending()
    {
        m_isUnlocked = true;
        m_itemImage.sprite = m_unlockedSprite;
    }

    private void UseItem()
    {
        if (m_shooterSettings.ActiveShooterButton != null)
            m_shooterSettings.ActiveShooterButton.DeUseItem();

        m_shooterSettings.ActiveShooterButton = this;
        m_isInUsing = true;
        m_selectionOutline.SetActive(true);
        GameManager.Instance.ActiveShooter.Style = m_shooterStyle;
        GameManager.Instance.ActiveShooter.BulletStyle = m_shooterBulletStyle;
        GameManager.Instance.PlayerProfile.SetSelectedShooterID(m_itemID);
    }

    private void DeUseItem()
    {
        m_isInUsing = false;
        m_selectionOutline.SetActive(false);
    }
}
