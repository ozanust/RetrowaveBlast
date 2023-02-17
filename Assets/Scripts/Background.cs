using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Background {

    [SerializeField]
    private Sprite m_bgSprite;

    [SerializeField]
    private Sprite m_unlockedSprite;

    [SerializeField]
    private float m_slideSpeed = 1.0f;

    [SerializeField]
    private GameObject[] m_subAnimationObjects;

    [SerializeField]
    private CircleBlock[] m_blockPrefabs;

    public Sprite BackgroundSprite
    {
        get { return m_bgSprite; }
    }

    public Sprite UnlockedSprite
    {
        get { return m_unlockedSprite; }
    }

    public float SlideSpeed
    {
        get { return m_slideSpeed; }
    }

    public GameObject[] SubAnimationObjects
    {
        get { return m_subAnimationObjects; }
    }

    public CircleBlock[] Blocks
    {
        get { return m_blockPrefabs; }
    }

    public void SetBackgroundSprite(Sprite sprite)
    {
        m_bgSprite = sprite;
    }
}
