using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

	[SerializeField]
	private float m_bulletPower;

	[SerializeField]
	private SpriteRenderer m_bulletSpriteRenderer;

	[SerializeField]
	private Rigidbody2D m_bulletRigidbody;

	[SerializeField]
	private Collider2D m_bulletCollider;

    [SerializeField]
    private ParticleSystem m_bulletHitEffect;

    [SerializeField]
    private bool m_isActive = false;

	public void FireBullet (float firePower)
	{
        ActivateBullet();
        m_bulletRigidbody.AddForce (new Vector2 (0, 1.5f));
	}

	public float BulletPower {
		get{ return m_bulletPower; }
		set{ m_bulletPower = value; }
	}

	public SpriteRenderer BulletSpriteRenderer {
		get{ return m_bulletSpriteRenderer; }
	}

    public bool IsActive
    {
        get { return m_isActive; }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject hitObj = collider.gameObject;
        if (hitObj.tag == "Block")
        {
            CircleBlock hitBlock = hitObj.GetComponent<CircleBlock>();
            if (hitBlock.CurrentPoint <= 0)
                hitObj.tag = "DestroyedBlock";
            //hitBlock.CurrentPoint -= BulletPower;
            hitBlock.Hit(BulletPower);
            hitBlock.PlayHitAnimation();
            hitBlock.PlayHitEffect(this.gameObject.transform.position);

#if UNITY_ANDROID && !UNITY_EDITOR
            if (GameManager.Instance.IsVibrationOn)
                AndroidVibrator.Vibrate(50);
#endif

            Destroy();
        }
        else if (hitObj.name == "BulletDestroyer")
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        this.gameObject.SetActive(false);
        m_isActive = false;
    }

    public void ActivateBullet()
    {
        this.gameObject.SetActive(true);
        m_isActive = true;
    }
}
