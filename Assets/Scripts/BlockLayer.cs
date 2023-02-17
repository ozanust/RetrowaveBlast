using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLayer : MonoBehaviour {

    [SerializeField]
    private int m_layer;

    [SerializeField]
    private float m_layerZ;

    [SerializeField]
    private bool m_isOccupied;

    [SerializeField]
    private BoxCollider2D[] m_boundaries;

    public int Layer
    {
        get { return m_layer; }
    }

    public float LayerZ
    {
        get { return m_layerZ; }
    }

    public bool IsOccupied
    {
        get { return m_isOccupied; }
    }

    public void ActivateLayer()
    {
        for(int i = 0; i < m_boundaries.Length; i++)
        {
            m_boundaries[i].enabled = true;
        }

        m_isOccupied = true;
    }

    public void DeactivateLayer()
    {
        for (int i = 0; i < m_boundaries.Length; i++)
        {
            m_boundaries[i].enabled = false;
        }

        m_isOccupied = false;
    }
}
