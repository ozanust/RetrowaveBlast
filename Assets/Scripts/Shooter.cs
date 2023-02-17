using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Shooter : MonoBehaviour
{
    public enum Cannon
    {
        Left = 0,
        Middle = 1,
        Right = 2
    }

	[SerializeField]
	private float m_shooterPower;

	[SerializeField]
	private int m_shooterFireSpeed;

	[SerializeField]
	private Sprite m_shooterStyle;

	[SerializeField]
	private Sprite m_shooterBulletStyle;

	[SerializeField]
	private Bullet m_bullet;

    [SerializeField]
    private GameObject m_leftCannonTip;

    [SerializeField]
    private GameObject m_middleCannonTip;

    [SerializeField]
    private GameObject m_rightCannonTip;

    [SerializeField]
    private GameController m_gameController;

    [SerializeField]
    private Rigidbody2D m_thisRigidbody;

    [SerializeField]
    private SpriteRenderer m_shooterRenderer;

    [SerializeField]
    private BoxCollider2D m_collider;

    [SerializeField]
    private GameObject m_forceField;

    [SerializeField]
    private Animator m_gemGainFXPrefab;

    [SerializeField]
    private Animator[] m_gemGainFXPool;

    private Vector3 m_refVel;
    private Coroutine c_deactiveColliderCoroutine = null;
    private GameManager m_gameManager;
    private AudioManager m_audioManager;

    public float Power {
		get { return m_shooterPower; }
        set { m_shooterPower = value; }
	}

	public int FireSpeed {
		get { return m_shooterFireSpeed; }
        set { m_shooterFireSpeed = value; }
	}

	public Sprite Style {
		get { return m_shooterStyle; }
        set { m_shooterStyle = value;
            m_shooterRenderer.sprite = m_shooterStyle;
        }
	}

	public Sprite BulletStyle {
		get { return m_shooterBulletStyle; }
        set { m_shooterBulletStyle = value;
            m_bullet.BulletSpriteRenderer.sprite = m_shooterBulletStyle;
        }
	}

	public Bullet BulletObj {
		get { return m_bullet; }
        set { m_bullet = value; }
	}

    public Vector3 LeftCannonTipPosition
    {
        get { return m_leftCannonTip.transform.position; }
    }

    public Vector3 MiddleCannonTipPosition
    {
        get { return m_middleCannonTip.transform.position; }
    }

    public Vector3 RightCannonTipPosition
    {
        get { return m_rightCannonTip.transform.position; }
    }

    public GameController ShooterGameController
    {
        set { m_gameController = value; }
    }

    void Awake ()
	{
        if (m_gameManager == null)
            m_gameManager = GameManager.Instance;

        if (m_audioManager == null)
            m_audioManager = AudioManager.Instance;

		if (m_shooterBulletStyle != null)
			m_bullet.BulletSpriteRenderer.sprite = m_shooterBulletStyle;

        if (m_shooterStyle != null)
            m_shooterRenderer.sprite = m_shooterStyle;
	}

	public void Fire (Bullet bullet, float firePower, Cannon cannon)
	{
        if(cannon == Cannon.Left)
            bullet.transform.position = LeftCannonTipPosition;
        else if(cannon == Cannon.Middle)
            bullet.transform.position = MiddleCannonTipPosition;
        else
            bullet.transform.position = RightCannonTipPosition;

        bullet.BulletSpriteRenderer.sprite = BulletStyle;
        bullet.FireBullet (firePower);

        //m_audioManager.PlaySound(SoundLibrary.SHOOT);
        m_audioManager.PlayFireSound();
	}

    public void ResetShooter()
    {
        m_thisRigidbody.velocity = new Vector2(0, 0);
        this.gameObject.transform.position = new Vector3(0, -5.6f, 5);

        //solve this for polishing (coroutine)
        //this.gameObject.transform.position = Vector3.SmoothDamp(this.gameObject.transform.position, new Vector3(0, -6, 5), ref m_refVel, 0.5f);
    }

    public void ResetShooterSpeed()
    {
        m_thisRigidbody.velocity = new Vector2(0, 0);
    }

    public void DeactiveColliderForAmountOfTime(float time)
    {
        if (c_deactiveColliderCoroutine == null)
            c_deactiveColliderCoroutine = StartCoroutine(CDeactiveBoxColliderFor(time));
    }

    IEnumerator CDeactiveBoxColliderFor(float time)
    {
        //activate visual shield for extra effect
        m_forceField.SetActive(true);
        m_collider.enabled = false;
        yield return new WaitForSeconds(time);
        m_collider.enabled = true;
        m_forceField.SetActive(false);

        c_deactiveColliderCoroutine = null;
    }

    private Animator GetNonPlayingGemGainFX()
    {
        int index = 0;
        while (m_gemGainFXPool[index].GetBool("Play"))
        {
            if (index < m_gemGainFXPool.Length - 1)
                index++;
            else
            {
                return null;
            }
        }

        return m_gemGainFXPool[index];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionObject = collision.gameObject;

        if (collisionObject.tag == "Block")
        {
            m_gameController.FailLevel();
        }
        else if (collisionObject.tag == "Gold")
        {
            int goldValue = collisionObject.GetComponent<Gold>().Value;
            m_gameManager.PlayerProfile.GainGold(goldValue);
            m_gameController.GainGoldForLevel(goldValue);
            m_audioManager.PlaySound(SoundLibrary.GOLD_PICK_UP, 0.5f);
            collisionObject.SetActive(false);
            Animator tempAnimator = GetNonPlayingGemGainFX();

            if (tempAnimator != null)
            {
                tempAnimator.SetBool("Play", true);
                tempAnimator.gameObject.GetComponent<TextMesh>().text = goldValue.ToString();
            }
        }
    }
}
