using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item
{
    [SerializeField]
    protected int m_itemPrice;

    [SerializeField]
    protected int m_itemID;

    [SerializeField]
    protected Sprite m_lockedSprite;

    [SerializeField]
    protected Sprite m_unlockedSprite;

    [SerializeField]
    protected bool m_isUnlocked = false;
}
