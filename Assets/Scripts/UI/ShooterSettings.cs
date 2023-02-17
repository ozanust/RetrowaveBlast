using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShooterSettings : MonoBehaviour
{

    public enum MenuState
    {
        Closed = 0,
        Open = 1
    }

    [SerializeField]
    private MenuState m_menuState;

    [SerializeField]
    private RectTransform m_rectTransform;

    [SerializeField]
    private CanvasGroup m_canvasGroup;

    [SerializeField]
    private RectTransform m_canvasRT;

    [SerializeField]
    private Button m_confirmAndCloseButton;

    [SerializeField]
    private UnlockableShooter m_activeShooter;

    Coroutine c_menuOperations = null;
    Vector2 m_velocityRef;

    private Vector2 m_openPosition = new Vector2(50, -170);
    private Vector2 m_closedPosition = new Vector2(50, 80);

    public MenuState CurrentMenuState
    {
        get { return m_menuState; }
    }

    public UnlockableShooter ActiveShooterButton
    {
        get { return m_activeShooter; }
        set { m_activeShooter = value; }
    }

    void RegisterEventListeners()
    {
        m_confirmAndCloseButton.onClick.AddListener(StartMenuOperations);
    }

    void RemoveEventListeners()
    {
        m_confirmAndCloseButton.onClick.RemoveAllListeners();
    }

    IEnumerator CMenuOperations()
    {
        if (m_menuState == MenuState.Closed)
        {
            float timer = 0;
            while (m_canvasGroup.alpha < 1)
            {
                timer += 0.06f;
                m_canvasGroup.alpha = timer;
                yield return new WaitForEndOfFrame();
            }

            m_canvasGroup.blocksRaycasts = true;
            m_canvasGroup.alpha = 1;
            timer = 0.0f;
            m_menuState = MenuState.Open;
            RegisterEventListeners();
            c_menuOperations = null;
        }
        else
        {
            RemoveEventListeners();
            float timer = 0;
            while (m_canvasGroup.alpha > 0)
            {
                timer += 0.06f;
                m_canvasGroup.alpha = 1 - timer;
                yield return new WaitForEndOfFrame();
            }

            m_canvasGroup.blocksRaycasts = false;
            m_canvasGroup.alpha = 0;
            timer = 0.0f;
            m_menuState = MenuState.Closed;
            c_menuOperations = null;
        }
    }

    public void StartMenuOperations()
    {
        if (c_menuOperations == null)
            c_menuOperations = StartCoroutine(CMenuOperations());
    }

}
