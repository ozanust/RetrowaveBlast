using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour {

    [SerializeField]
    private int m_value;

    [SerializeField]
    private SpriteRenderer m_image;

    [SerializeField]
    private Rigidbody2D m_rigidbody;

    [SerializeField]
    private BoxCollider2D m_collider;

    private float m_selfDestroyTimer = 5f;
    private float m_fadeOutTimer = 1f;
    private Coroutine c_selfDestroyTimerCoroutine = null;
    private Coroutine c_fadeOutTimerCoroutine = null;

    public int Value
    {
        get { return m_value; }
        set { m_value = value; }
    }

    public Rigidbody2D GoldRigidbody
    {
        get { return m_rigidbody; }
    }

    private void Awake()
    {
        if (m_value > 0 && m_value < 3)
            m_image.color = Color.blue;
        else if (m_value >= 3 && m_value < 7)
            m_image.color = Color.cyan;
        else if (m_value >= 7 && m_value < 12)
            m_image.color = Color.green;
        else if (m_value >= 12 && m_value < 20)
            m_image.color = Color.magenta;
        else if (m_value >= 20 && m_value < 35)
            m_image.color = Color.red;
        else
            m_image.color = Color.yellow;

        if (c_selfDestroyTimerCoroutine == null)
            c_selfDestroyTimerCoroutine = StartCoroutine(CStartSelfDestroyTimer());
    }

    IEnumerator CStartSelfDestroyTimer()
    {
        float timer = 0.0f;
        while(timer < m_selfDestroyTimer)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        timer = m_selfDestroyTimer;

        if (c_fadeOutTimerCoroutine == null)
            c_fadeOutTimerCoroutine = StartCoroutine(CFadeOut(false));

        c_selfDestroyTimerCoroutine = null;
    }

    IEnumerator CFadeOut(bool destroyAfterFadeOut)
    {
        while (m_image.color.a > 0)
        {
            m_image.color = new Color(m_image.color.r, m_image.color.g, m_image.color.b, m_image.color.a - 0.05f);
            yield return new WaitForEndOfFrame();
        }

        gameObject.SetActive(false);

        if (destroyAfterFadeOut)
            Destroy(this.gameObject);

        c_fadeOutTimerCoroutine = null;
    }

    public void Destroy()
    {
        StopAllCoroutines();
        StartCoroutine(CFadeOut(true));
    }
}
