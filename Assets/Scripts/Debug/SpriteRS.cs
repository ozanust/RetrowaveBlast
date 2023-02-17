using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRS : MonoBehaviour {

    [SerializeField]
    private SpriteRenderer m_sprite;

	// Use this for initialization
	void Start () {
        print("X Size: " + m_sprite.bounds.size.x);
        m_sprite.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        print("X Size: " + m_sprite.bounds.size.x);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
