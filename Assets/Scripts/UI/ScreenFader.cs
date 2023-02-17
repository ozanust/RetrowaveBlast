using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour {

    [SerializeField]
    private Image m_fadeImage;

    [SerializeField]
    private float m_fadingSpeed = 0.03f;

    Coroutine c_fadeIn;
    Coroutine c_fadeOut;
    Coroutine c_passScene;

	IEnumerator CFadeIn(System.Action callBack)
    {
        float timer = 0;
        while(m_fadeImage.color.a < 1)
        {
            timer += m_fadingSpeed;
            m_fadeImage.color = new Color(m_fadeImage.color.r, m_fadeImage.color.g, m_fadeImage.color.b, timer);
            yield return new WaitForEndOfFrame();
        }

        callBack();
        c_fadeIn = null;
    }

    IEnumerator CFadeOut(System.Action callBack)
    {
        float timer = 0;
        while (m_fadeImage.color.a > 1)
        {
            timer += m_fadingSpeed;
            m_fadeImage.color = new Color(m_fadeImage.color.r, m_fadeImage.color.g, m_fadeImage.color.b, 1 - timer);
            yield return new WaitForEndOfFrame();
        }

        callBack();
        c_fadeOut = null;
    }

    IEnumerator CPassScene(System.Action callBack)
    {
        m_fadeImage.gameObject.SetActive(true);
        float timer = 0;
        while (m_fadeImage.color.a < 1)
        {
            timer += m_fadingSpeed;
            m_fadeImage.color = new Color(m_fadeImage.color.r, m_fadeImage.color.g, m_fadeImage.color.b, timer);
            yield return new WaitForEndOfFrame();
        }

        callBack();

        timer = 0;

        while (m_fadeImage.color.a > 0)
        {
            timer += m_fadingSpeed;
            m_fadeImage.color = new Color(m_fadeImage.color.r, m_fadeImage.color.g, m_fadeImage.color.b, 1 - timer);
            yield return new WaitForEndOfFrame();
        }

        m_fadeImage.gameObject.SetActive(false);
        c_passScene = null;
    }

    public void PassScene(System.Action callBack)
    {
        if (c_passScene == null)
            c_passScene = StartCoroutine(CPassScene(callBack));
    }

    public void FadeIn(System.Action callBack)
    {
        if (c_fadeIn == null)
            c_fadeIn = StartCoroutine(CFadeIn(callBack));
    }

    public void FadeOut(System.Action callBack)
    {
        if (c_fadeOut == null)
            c_fadeOut = StartCoroutine(CFadeOut(callBack));
    }
}
