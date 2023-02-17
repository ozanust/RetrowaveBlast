using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemGainFX : MonoBehaviour
{

    public void SetBool()
    {
        GetComponent<Animator>().SetBool("Play", false);
    }
}
