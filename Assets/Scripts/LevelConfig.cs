using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConfig : MonoBehaviour
{

    private int[] m_minBlockPointPerLevel; /*= { 1, 1, 1, 2, 2, 3, 4, 5, 6, 7, 8, 9, 9, 10, 11, 12, 13, 14, 16, 18, 22, 26, 32, 40, 50, 60, 70, 85, 100, 100, 120, 140, 140 };*/
    private int[] m_maxBlockPointPerLevel; /*= { 2, 2, 3, 4, 4, 5, 6, 7, 8, 10, 12, 14, 14, 15, 16, 18, 22, 26, 30, 34, 40, 44 , 50, 60, 75, 85, 100, 120, 140, 140, 160, 180, 180};*/
    private int[] m_blockCountPerLevel; /*= { 2, 2, 3, 3, 4, 4, 5, 6, 7, 7, 8, 9, 9, 9, 10, 11, 11, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 14 };*/
    private int[] m_predefinedBlockCount = { 2, 2, 3, 3, 4, 4, 5, 6, 7, 7, 8, 9, 9, 9, 10, 11, 11, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 14 };

    private void Start()
    {
        GenerateLevels(500);
    }

    void GenerateLevels(int levelCount)
    {
        float minPoint = 1f;
        float maxPoint = 3.5f;

        float incrementRate = 1.1f;

        m_minBlockPointPerLevel = new int[levelCount];
        m_maxBlockPointPerLevel = new int[levelCount];
        m_blockCountPerLevel = new int[levelCount];

        for (int i = 0; i < levelCount; i++)
        {

            if (i < 20)
            {
                if (incrementRate < 1.4f)
                    incrementRate += 0.005f;
            }
            else
            {
                if (incrementRate > 1.07f)
                    incrementRate -= 0.015f;
            }

            minPoint *= incrementRate;
            maxPoint *= incrementRate;


            if (i < 33)
                m_blockCountPerLevel[i] = m_predefinedBlockCount[i];
            else
                m_blockCountPerLevel[i] = m_blockCountPerLevel[i - 1] + ((i % 10) == 0 ? 1 : 0);

            m_minBlockPointPerLevel[i] = Mathf.FloorToInt(minPoint);
            m_maxBlockPointPerLevel[i] = Mathf.FloorToInt(maxPoint);

            /*print(i + "th level min block point: " + Mathf.FloorToInt(minPoint) + " | Increment rate: " + incrementRate);
            print(i + "th level max block point: " + Mathf.FloorToInt(maxPoint) + " | Increment rate: " + incrementRate);
            print(m_blockCountPerLevel[i]);*/
        }
    }

    public int[] MinBlockPointPerLevel
    {
        get { return m_minBlockPointPerLevel; }
    }

    public int[] MaxBlockPointPerLevel
    {
        get { return m_maxBlockPointPerLevel; }
    }

    public int[] BlockCountPerLevel
    {
        get { return m_blockCountPerLevel; }
    }
}
