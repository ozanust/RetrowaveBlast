using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnlockableBackground : UnlockableItem
{
    [SerializeField]
    private Background m_background;

    private BackgroundSettings m_backgroundSettings;

    public Background Background
    {
        get
        {
            return m_background;
        }
    }

    public int ID
    {
        get { return m_itemID; }
    }

    public void SetBackgroundProperties(Background background, int id, int price)
    {
        m_backgroundSettings = FindObjectOfType<BackgroundSettings>();
        m_background = background;
        m_unlockedSprite = background.UnlockedSprite;
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
            GameManager.Instance.PlayerProfile.UnlockedBackgroundIDList.Add(m_itemID);
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
        if (m_backgroundSettings.ActiveBackgroundButton != null)
            m_backgroundSettings.ActiveBackgroundButton.DeUseItem();

        m_isInUsing = true;
        m_selectionOutline.SetActive(true);
        FindObjectOfType<BackgroundController>().SetBackground(m_background);
        GameManager.Instance.ActiveBackground = m_background;
        GameManager.Instance.PlayerProfile.SetSelectedBackgroundID(m_itemID);
    }

    private void DeUseItem()
    {
        m_isInUsing = false;
        m_selectionOutline.SetActive(false);
    }
}
