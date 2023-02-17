using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleBlock : MonoBehaviour
{

	public enum BlockType
	{
		Breakable = 0,
		Solid = 1
	}

	public enum Size
	{
		Small = 0,
		Medium = 1,
		Big = 2,
		Huge = 3
	}

    [SerializeField]
    private AudioManager m_audioManager;

	[SerializeField]
	private Sprite m_blockSprite;

	[SerializeField]
	private TextMesh m_pointText;

	[SerializeField]
	private BlockType m_blockType;

	[SerializeField]
	private Size m_size;

	[SerializeField]
	private CircleBlock[] m_subBlocks;

	[SerializeField]
	private float m_basePoint;

	[SerializeField]
	private SpriteRenderer m_blockRenderer;

	[SerializeField]
	private Collider2D m_blockCollider;

    [SerializeField]
    private PhysicsMaterial2D m_defaultPhysicsMaterial;

    [SerializeField]
    private PhysicsMaterial2D m_bouncyPhysicsMaterial;

    [SerializeField]
	private Rigidbody2D m_blockRigidbody;

	[SerializeField]
	private GameController m_gameController;

    [SerializeField]
    private Animator m_spriteAnimator;

    [SerializeField]
    private ParticleSystem m_explosionEffect;

    [SerializeField]
    private Color m_explosionColor;

    [SerializeField]
    private List<Gold> m_goldList;

	[SerializeField]
	private int m_layerID;

	private float m_currentPoint;
	private float m_bounceHeight;

	private float m_leftDropPoint;
	private float m_rightDropPoint;

    bool m_isDestroyed = false;

    [SerializeField]
    private Rigidbody2D[] m_subBlockRigidbodies;

    Coroutine c_startEntryAnimation = null;

	[SerializeField]
	private bool m_isLeftBlock = false;

	public BlockType Block_Type {
		get{ return m_blockType; }
	}

	public Size BlockSize {
		get{ return m_size; }
	}

	public CircleBlock[] SubBlocks {
		get{ return m_subBlocks; }
	}

	public float BasePoint {
		get{ return m_basePoint; }
		set{ m_basePoint = value; }
	}

    public Collider2D Collider
    {
        get { return m_blockCollider; }
    }

	public float CurrentPoint {
		get{ return m_currentPoint; }
		set {
			m_currentPoint = value;

            if (m_currentPoint <= 0 && !m_isDestroyed)
                DestroyBlock();

            m_pointText.text = Mathf.CeilToInt(m_currentPoint).ToString("F0");
            if (m_currentPoint <= 20)
                m_blockRenderer.color = Constants.UNDER_20_COLOR;
            else if (m_currentPoint > 20 && m_currentPoint < 60)
                m_blockRenderer.color = Constants.BETWEEN_20_60_COLOR;
            else if (m_currentPoint > 60 && m_currentPoint < 100)
                m_blockRenderer.color = Constants.BETWEEN_60_100_COLOR;
            else if (m_currentPoint > 100 && m_currentPoint < 160)
                m_blockRenderer.color = Constants.BETWEEN_100_160_COLOR;
            else if (m_currentPoint > 160 && m_currentPoint < 220)
                m_blockRenderer.color = Constants.BETWEEN_160_220_COLOR;
            else if (m_currentPoint > 220 && m_currentPoint < 300)
                m_blockRenderer.color = Constants.BETWEEN_220_300_COLOR;
            else if (m_currentPoint > 300 && m_currentPoint < 400)
                m_blockRenderer.color = Constants.BETWEEN_300_400_COLOR;
            else
                m_blockRenderer.color = Constants.BETWEEN_400_500_COLOR;
		}
	}

	public float BounceHeight {
		get{ return m_bounceHeight; }
		set { m_bounceHeight = value; }
	}

	public bool IsLeftBlock {
		get { return m_isLeftBlock; }
		set { m_isLeftBlock = value; }
	}

	public GameController SceneGameController {
		get { return m_gameController; }
		set { m_gameController = value; }
	}

	public int LayerID {
		get { return m_layerID; }
		set { m_layerID = value; }
	}

    private void Awake ()
	{
        if (m_audioManager == null)
            m_audioManager = AudioManager.Instance;

        m_subBlockRigidbodies = new Rigidbody2D[m_subBlocks.Length];
        for(int i = 0; i < m_subBlocks.Length; i++)
        {
            m_subBlockRigidbodies[i] = m_subBlocks[i].gameObject.GetComponent<Rigidbody2D>();
        }

        if (m_spriteAnimator == null)
            m_spriteAnimator = GetComponentInChildren<Animator>();

		if (BlockSize == Size.Small) {
			m_leftDropPoint = Constants.LEFT_BARRIER_POSITION + (Constants.SMALL_BLOCK_SIZE_MULTIPLIER / 10f);
			m_rightDropPoint = Constants.RIGHT_BARRIER_POSITION - (Constants.SMALL_BLOCK_SIZE_MULTIPLIER / 10f);
		} else if (BlockSize == Size.Medium) {
			m_leftDropPoint = Constants.LEFT_BARRIER_POSITION + (Constants.MEDIUM_BLOCK_SIZE_MULTIPLIER / 10f);
			m_rightDropPoint = Constants.RIGHT_BARRIER_POSITION - (Constants.MEDIUM_BLOCK_SIZE_MULTIPLIER / 10f);
		} else if (BlockSize == Size.Big) {
			m_leftDropPoint = Constants.LEFT_BARRIER_POSITION + (Constants.BIG_BLOCK_SIZE_MULTIPLIER / 10f);
			m_rightDropPoint = Constants.RIGHT_BARRIER_POSITION - (Constants.BIG_BLOCK_SIZE_MULTIPLIER / 10f);
		} else if (BlockSize == Size.Huge) {
			m_leftDropPoint = Constants.LEFT_BARRIER_POSITION + (Constants.HUGE_BLOCK_SIZE_MULTIPLIER / 10f);
			m_rightDropPoint = Constants.RIGHT_BARRIER_POSITION - (Constants.HUGE_BLOCK_SIZE_MULTIPLIER / 10f);
		}

        ParticleSystem.MainModule psMain = m_explosionEffect.main;
        psMain.startColor = m_explosionColor;
	}

	public IEnumerator CStartEntryToGameAreaAnimation (bool isFromLeft)
	{
		bool reachedDestination = false;

		if (isFromLeft) {
			while (!reachedDestination) {
				this.gameObject.transform.Translate (new Vector3 (1, 0, 0) * Time.deltaTime * 2);
				if (this.gameObject.transform.position.x > m_leftDropPoint)
					reachedDestination = true;
				yield return new WaitForFixedUpdate ();
			}
		} else {
			while (!reachedDestination) {
				this.gameObject.transform.Translate (new Vector3 (-1, 0, 0) * Time.deltaTime * 2);
				if (this.gameObject.transform.position.x < m_rightDropPoint)
					reachedDestination = true;
				yield return new WaitForFixedUpdate ();
			}
		}

		ActivateBlock (isFromLeft);

        c_startEntryAnimation = null;
	}

    public void StartEntryToGameAreaAnimation(bool isFromLeft)
    {
        if (this.gameObject.activeInHierarchy && c_startEntryAnimation == null)
            c_startEntryAnimation = StartCoroutine(CStartEntryToGameAreaAnimation(isFromLeft));
    }

    void ActivateBlock (bool isFromLeft)
	{
		m_blockCollider.enabled = true;
		m_blockRigidbody.gravityScale = 1;

		if (isFromLeft)
			m_blockRigidbody.AddForce (new Vector2 (1, 0), ForceMode2D.Impulse);
		else
			m_blockRigidbody.AddForce (new Vector2 (-1, 0), ForceMode2D.Impulse);

        m_blockRigidbody.AddTorque(Random.Range(-10,10));

		m_gameController.ActiveBlockCount++;
		//m_gameController.BlockLayerPool [m_layerID].ActivateLayer ();
	}

	void DestroyBlock ()
	{
        //m_gameController.BlockLayerPool [m_layerID].DeactivateLayer ();
        m_audioManager.PlaySound(SoundLibrary.EXPLOSION, 0.5f);
        m_isDestroyed = true;
        m_explosionEffect.transform.SetParent(null);
        m_explosionEffect.gameObject.SetActive(true);
        switch (BlockSize)
        {
            case Size.Small:
                m_gameController.ActiveBlockCount--;
                break;
            case Size.Medium:
                m_gameController.ActiveBlockCount++;
                break;
            case Size.Big:
                m_gameController.ActiveBlockCount++;
                break;
            case Size.Huge:
                m_gameController.ActiveBlockCount++;
                break;
        }

        m_gameController.DestroyedBlockCount += 1;
        //m_gameController.UpdateScore((int)BasePoint);

		if (m_subBlocks.Length > 0) {
            m_subBlocks[0].transform.SetParent(null);
            m_subBlocks[0].gameObject.SetActive(true);
            m_subBlockRigidbodies[0].AddForce(new Vector2(-1, 1), ForceMode2D.Impulse);
            m_subBlockRigidbodies[0].AddTorque(Random.Range(-10, 10));

            m_subBlocks[1].transform.SetParent(null);
            m_subBlocks[1].gameObject.SetActive(true);
            m_subBlockRigidbodies[1].AddForce(new Vector2(1, 1), ForceMode2D.Impulse);
            m_subBlockRigidbodies[1].AddTorque(Random.Range(-10, 10));

            for (int i = 0; i < m_subBlocks.Length; i++)
            {
                m_subBlocks[i].SetBasePoint(Mathf.CeilToInt(BasePoint * 0.8f));
                m_subBlocks[i].SceneGameController = m_gameController;
                m_gameController.AddSubBlockToSubBlockPool(m_subBlocks[i]);

                if(gameObject.transform.position.y < Constants.MIN_BOUNCE_HEIGHT)
                {
                    m_bouncyPhysicsMaterial.bounciness = Random.Range(1.1f, 1.6f);
                    m_subBlocks[i].Collider.sharedMaterial = m_bouncyPhysicsMaterial;
                }
            }
        }

        if(m_goldList.Count > 0)
        {
            for(int i = 0; i < m_goldList.Count; i++)
            {
                m_goldList[i].transform.SetParent(null);
                m_goldList[i].gameObject.SetActive(true);
                m_goldList[i].transform.eulerAngles = new Vector3(0, 0, 0);
                m_goldList[i].GoldRigidbody.AddForce(new Vector2(Random.Range(-5, 5), Random.Range(5, 10)), ForceMode2D.Impulse);
            }
        }

        m_gameController.m_uiHudController.FillBar((float)m_gameController.DestroyedBlockCount / (float)m_gameController.TotalBlockCountToDestroy);

        if (GameManager.Instance.IsVibrationOn)
            Handheld.Vibrate();

        this.gameObject.SetActive(false);
    }

    public void KillBlock()
    {
        Destroy(this.gameObject);
    }

    public void SetBasePoint(float point)
    {
        BasePoint = point;
        SetCurrentPoint(BasePoint);
    }

    public void SetCurrentPoint(float point)
    {
        CurrentPoint = point;
    }

    public void AddGold(Gold gold)
    {
        if (m_goldList == null)
            m_goldList = new List<Gold>();

        m_goldList.Add(gold);
    }

    public void PlayHitAnimation()
    {
        if (m_spriteAnimator.isActiveAndEnabled)
        {
            if (!m_spriteAnimator.GetBool("Hit"))
                m_spriteAnimator.SetTrigger("Hit");
        }
    }

    public void PlayHitEffect(Vector3 hitPoint)
    {
        GameObject hitEffect = m_gameController.GetNonActiveEffect();
        hitEffect.transform.position = this.gameObject.transform.position;
        hitEffect.transform.LookAt(hitPoint);
        hitEffect.transform.position = hitPoint;
        ParticleSystem.MainModule main = hitEffect.GetComponent<ParticleSystem>().main;
        main.startColor = m_explosionColor;
        hitEffect.SetActive(true);
        m_audioManager.PlayHitSound();
    }

    public void Hit(float hitAmount)
    {
        CurrentPoint -= hitAmount;
        m_gameController.UpdateScore((int)hitAmount);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_blockCollider.sharedMaterial == m_bouncyPhysicsMaterial && collision.gameObject.tag == "Ground")
            m_blockCollider.sharedMaterial = m_defaultPhysicsMaterial;
    }
}
