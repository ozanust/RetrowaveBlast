using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralSettings : MonoBehaviour
{

    public enum MenuState
    {
        Closed = 0,
        Open = 1
    }

    [SerializeField]
    private Toggle m_vibrationOnOffButton;

    [SerializeField]
    private Toggle m_audioOnOffButton;

    [SerializeField]
    private Button m_privacyPolicyButton;

    [SerializeField]
    private MenuState m_menuState;

    [SerializeField]
    private RectTransform m_rectTransform;

    [SerializeField]
    private GameObject m_privacyPolicy;

    Coroutine c_menuOperations = null;
    Vector2 m_velocityRef;

    private Vector2 m_openPosition = new Vector2(75, -300);
    private Vector2 m_closedPosition = new Vector2(75, 190);
    private float m_lerpTimer = 0.0f;

    public MenuState CurrentMenuState
    {
        get { return m_menuState; }
    }

    private void Start()
    {
        m_audioOnOffButton.isOn = GameManager.Instance.IsAudioOn;
        m_vibrationOnOffButton.isOn = GameManager.Instance.IsVibrationOn;
    }

    void RegisterEventListeners()
    {
        m_audioOnOffButton.onValueChanged.AddListener(AudioOnOff);
        m_vibrationOnOffButton.onValueChanged.AddListener(VibrationOnOff);
        m_privacyPolicyButton.onClick.AddListener(PrivacyPolicyOnOff);
    }

    void RemoveEventListeners()
    {
        m_audioOnOffButton.onValueChanged.RemoveAllListeners();
        m_vibrationOnOffButton.onValueChanged.RemoveAllListeners();
        m_privacyPolicyButton.onClick.RemoveAllListeners();
    }

    void AudioOnOff(bool isOn)
    {
        GameManager.Instance.SetAudio(isOn);
    }

    void VibrationOnOff(bool isOn)
    {
        GameManager.Instance.SetVibration(isOn);
    }

    void PrivacyPolicyOnOff()
    {
        if (m_privacyPolicy.activeInHierarchy)
            m_privacyPolicy.SetActive(false);
        else
            m_privacyPolicy.SetActive(true);
    }

    IEnumerator CMenuOperations()
    {
        if (m_menuState == MenuState.Closed)
        {
            while (m_rectTransform.anchoredPosition.y >= m_openPosition.y + 1)
            {
                m_lerpTimer += 0.04f;
                m_rectTransform.anchoredPosition = Vector2.Lerp(m_closedPosition, m_openPosition, Mathf.SmoothStep(0.0f, 1.0f, m_lerpTimer));
                yield return new WaitForEndOfFrame();
            }

            m_lerpTimer = 0.0f;
            m_menuState = MenuState.Open;
            RegisterEventListeners();
            c_menuOperations = null;
        }
        else
        {
            RemoveEventListeners();
            while (m_rectTransform.anchoredPosition.y < m_closedPosition.y - 1)
            {
                m_lerpTimer += 0.04f;
                m_rectTransform.anchoredPosition = Vector2.Lerp(m_openPosition, m_closedPosition, Mathf.SmoothStep(0.0f, 1.0f, m_lerpTimer));
                yield return new WaitForEndOfFrame();
            }

            m_lerpTimer = 0.0f;
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
