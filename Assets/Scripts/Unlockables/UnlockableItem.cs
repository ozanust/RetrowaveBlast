using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
abstract public class UnlockableItem : MonoBehaviour {

    [SerializeField]
    protected int m_itemPrice;

    [SerializeField]
    protected Text m_priceText;

    [SerializeField]
    protected int m_itemID;

    [SerializeField]
    protected Image m_itemImage;

    [SerializeField]
    protected Sprite m_lockedSprite;

    [SerializeField]
    protected Sprite m_unlockedSprite;

    [SerializeField]
    protected GameObject m_selectionOutline;

    [SerializeField]
    protected bool m_isUnlocked = false;

    [SerializeField]
    protected bool m_isInUsing = false;

    abstract public void Unlock();
}
