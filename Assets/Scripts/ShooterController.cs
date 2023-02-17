using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    [SerializeField]
    private Shooter m_shooter;

    [SerializeField]
    private GameManager m_gameManager;

    [SerializeField]
    private List<Bullet> m_bulletPool;

    [SerializeField]
    private GameController m_gameController;

    private Vector3 m_currentVelocity;
    float m_smoothTime = 0.08f;

    float m_fireTimer = 1.1f;
    float m_fireInterval = 0.0f;

    int maximumPoolLenght = 100;
    int minimumPoolLenght = 15;

    int poolLenght = 0;

    public Shooter ActiveShooter
    {
        get { return m_shooter; }
    }

    void Start()
    {
        if (m_gameManager == null)
            m_gameManager = GameManager.Instance;

        m_gameManager.ActiveShooter = m_shooter;
        m_shooter.FireSpeed = m_gameManager.PlayerProfile.FireSpeed;
        m_shooter.Power = m_gameManager.PlayerProfile.BulletPower;
        m_shooter.Style = UnlockableManager.Instance.GetUnlockableShooterByID(m_gameManager.PlayerProfile.SelectedShooterID).ShooterStyle;
        m_shooter.BulletStyle = UnlockableManager.Instance.GetUnlockableShooterByID(m_gameManager.PlayerProfile.SelectedShooterID).ShooterBulletStyle;
        m_shooter.ShooterGameController = m_gameController;
        m_fireInterval = 1f / m_shooter.FireSpeed;

        if (m_shooter.FireSpeed < minimumPoolLenght)
            poolLenght = minimumPoolLenght;
        else if (m_shooter.FireSpeed >= minimumPoolLenght && m_shooter.FireSpeed < maximumPoolLenght)
            poolLenght = Mathf.FloorToInt(m_shooter.FireSpeed * 1.2f);
        else
            poolLenght = maximumPoolLenght;

        StartCoroutine(PopulateBulletPool());
    }

    void Update()
    {
        if(m_gameController.CurrentGameState == GameController.GameState.Playing)
            MoveShooter();
    }

    void MoveShooter()
    {
        if (Input.touchCount > 0)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            m_shooter.transform.position = Vector3.SmoothDamp(m_shooter.transform.position, new Vector3(worldPosition.x, m_shooter.transform.position.y, m_shooter.transform.position.z), ref m_currentVelocity, m_smoothTime);

            if (m_fireTimer < 1f / m_shooter.FireSpeed)
            {
                m_fireTimer += Time.deltaTime;
            }
            else
            {
                m_fireTimer = 0;

                if(m_shooter.FireSpeed < 35)
                {
                    m_shooter.Fire(GetDeactiveBullet(m_bulletPool), m_shooter.Power, Shooter.Cannon.Middle);
                }
                else if (m_shooter.FireSpeed >= 35 && m_shooter.FireSpeed < 70)
                {
                    m_shooter.Fire(GetDeactiveBullet(m_bulletPool), m_shooter.Power, Shooter.Cannon.Left);
                    m_shooter.Fire(GetDeactiveBullet(m_bulletPool), m_shooter.Power, Shooter.Cannon.Right);
                }
                else
                {
                    m_shooter.Fire(GetDeactiveBullet(m_bulletPool), m_shooter.Power, Shooter.Cannon.Left);
                    m_shooter.Fire(GetDeactiveBullet(m_bulletPool), m_shooter.Power, Shooter.Cannon.Middle);
                    m_shooter.Fire(GetDeactiveBullet(m_bulletPool), m_shooter.Power, Shooter.Cannon.Right);
                }
            }

        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m_shooter.transform.position = Vector3.SmoothDamp(m_shooter.transform.position, new Vector3(worldPosition.x, m_shooter.transform.position.y, m_shooter.transform.position.z), ref m_currentVelocity, m_smoothTime);

            if (m_fireTimer < 1f / m_shooter.FireSpeed)
            {
                m_fireTimer += Time.deltaTime;
            }
            else
            {
                if (m_shooter.FireSpeed < 35)
                {
                    m_shooter.Fire(GetDeactiveBullet(m_bulletPool), m_shooter.Power, Shooter.Cannon.Middle);
                    m_fireTimer = 0;
                }
                else if (m_shooter.FireSpeed >= 35 && m_shooter.FireSpeed < 70)
                {
                    m_shooter.Fire(GetDeactiveBullet(m_bulletPool), m_shooter.Power, Shooter.Cannon.Left);
                    m_shooter.Fire(GetDeactiveBullet(m_bulletPool), m_shooter.Power, Shooter.Cannon.Right);
                    m_fireTimer = -m_fireInterval;
                }
                else
                {
                    m_shooter.Fire(GetDeactiveBullet(m_bulletPool), m_shooter.Power, Shooter.Cannon.Left);
                    m_shooter.Fire(GetDeactiveBullet(m_bulletPool), m_shooter.Power, Shooter.Cannon.Middle);
                    m_shooter.Fire(GetDeactiveBullet(m_bulletPool), m_shooter.Power, Shooter.Cannon.Right);
                    m_fireTimer = -(2 * m_fireInterval);
                }
            }

        }

        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                m_fireTimer = 1.1f;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_fireTimer = 1.1f;
        }
    }

    public void ResetShooter()
    {
        m_shooter.ResetShooter();
    }

    IEnumerator PopulateBulletPool()
    {
        for(int i = 0; i < poolLenght; i++)
        {
            Bullet tempBullet = Instantiate(m_shooter.BulletObj, new Vector3(-10,-10,0), Quaternion.identity);
            tempBullet.BulletPower = m_shooter.Power;
            tempBullet.BulletSpriteRenderer.sprite = m_shooter.BulletStyle;

            m_bulletPool.Add(tempBullet);
        }
        yield return new WaitForEndOfFrame();
    }

    public void DestroyAllBullets()
    {
        for(int i = 0; i < m_bulletPool.Count; i++)
        {
            if(m_bulletPool[i].IsActive)
                m_bulletPool[i].Destroy();
        }
    }

    Bullet GetDeactiveBullet(List<Bullet> bulletPool)
    {
        int bulletID = 0;
        while (bulletPool[bulletID].IsActive)
        {
            bulletID++;
        }

        return bulletPool[bulletID];
    }

}
