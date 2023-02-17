using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{

    [SerializeField]
    private Background m_selectedBackground;

    [SerializeField]
    private SpriteRenderer m_currentSprite;

    [SerializeField]
    private SpriteRenderer m_nextSprite;

    [SerializeField]
    private float m_bgSlideSpeed;

    private float m_cameraHeight;

    private GameManager m_gameManager;

    private void Start()
    {
        if (m_gameManager == null)
            m_gameManager = GameManager.Instance;

        m_cameraHeight = Camera.main.orthographicSize * 2f;
        SetBackground();
    }

    private void Update()
    {
        SpriteSlide(m_currentSprite);
        SpriteSlide(m_nextSprite);
    }

    void Resize(SpriteRenderer spriteRenderer)
    {
        SpriteRenderer sr = spriteRenderer;
        spriteRenderer.transform.localScale = new Vector3(1, 1, 1);

        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        Vector3 imgScale = new Vector3(1f, 1f, 1f);

        imgScale.x = worldScreenWidth / width;
        imgScale.y = imgScale.x;

        spriteRenderer.transform.localScale = imgScale;
    }

    private void SpriteSlide(SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer.transform.position.y <= ((-spriteRenderer.bounds.size.y / 2f) - (m_cameraHeight / 2f)))
        {
            spriteRenderer.transform.position = new Vector3(0, spriteRenderer.transform.position.y + 2 * spriteRenderer.bounds.size.y, 10);
        }

        spriteRenderer.transform.Translate(Vector3.down * Time.deltaTime * m_bgSlideSpeed);
    }

    public void SetBackground()
    {
        if (m_gameManager.PlayerProfile.UnlockedBackgroundIDList != null && m_gameManager.PlayerProfile.UnlockedBackgroundIDList.Count > 0)
            SetBackground(UnlockableManager.Instance.GetUnlockableBackgroundByID(m_gameManager.PlayerProfile.UnlockedBackgroundIDList[Random.Range(0, m_gameManager.PlayerProfile.UnlockedBackgroundIDList.Count)]).Background);
        else
            SetBackground(UnlockableManager.Instance.GetUnlockableBackgroundByID(0).Background);
    }

    public void SetBackground(Background background)
    {
        m_selectedBackground = background;
        m_bgSlideSpeed = m_selectedBackground.SlideSpeed;
        m_currentSprite.sprite = m_selectedBackground.BackgroundSprite;
        m_nextSprite.sprite = m_selectedBackground.BackgroundSprite;
        Resize(m_currentSprite);
        Resize(m_nextSprite);
        m_currentSprite.transform.position = new Vector3(0, (m_currentSprite.bounds.size.y / 2f) - (m_cameraHeight / 2f), 10);
        m_nextSprite.transform.position = new Vector3(0, (m_nextSprite.bounds.size.y * 1.5f) - (m_cameraHeight / 2f), 10);

        if(background.Blocks != null)
            FindObjectOfType<GameController>().PredefinedBlockPool = background.Blocks;
    }
}
