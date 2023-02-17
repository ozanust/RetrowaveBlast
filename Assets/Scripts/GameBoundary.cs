using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoundary : MonoBehaviour {

    [SerializeField]
    private Collider2D m_collider;

    [SerializeField]
    private ParticleSystem m_wallHitEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Block")
            m_wallHitEffect.Play();
    }
}
