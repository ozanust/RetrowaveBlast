using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShooterItem : Item {

    [SerializeField]
    private Sprite m_shooterStyle;

    [SerializeField]
    private Sprite m_shooterBulletStyle;

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

    public Sprite LockedSprite
    {
        get { return m_lockedSprite; }
    }

    public int ID
    {
        get { return m_itemID; }
    }

    public int Price
    {
        get { return m_itemPrice; }
    }

    public void SetUnlockState(bool isUnlocked)
    {
        m_isUnlocked = isUnlocked;
    }
}
