using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BackgroundItem : Item {

    [SerializeField]
    private Background m_background;

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

    public int Price
    {
        get { return m_itemPrice; }
    }
}
