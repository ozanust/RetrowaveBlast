using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : MonoBehaviour {
    public static AudioClip GOLD_PICK_UP;
    public static AudioClip SHOOT;
    public static AudioClip MUSIC;
    public static AudioClip MUSIC2;
    public static AudioClip MUSIC3;
    public static AudioClip MUSIC4;
    public static AudioClip MUSIC5;
    public static AudioClip[] MUSICS;
    public static AudioClip EXPLOSION;
    public AudioClip HIT;
    public AudioClip FAIL;
    public AudioClip WIN;

    private void Awake()
    {
        GOLD_PICK_UP = Resources.Load<AudioClip>("SoundFX/GemPickup");
        MUSIC = Resources.Load<AudioClip>("SoundFX/Music/Music1");
        MUSIC2 = Resources.Load<AudioClip>("SoundFX/Music/Music2");
        MUSIC3 = Resources.Load<AudioClip>("SoundFX/Music/Music3");
        MUSIC4 = Resources.Load<AudioClip>("SoundFX/Music/Music4");
        MUSIC5 = Resources.Load<AudioClip>("SoundFX/Music/Music5");
        SHOOT = Resources.Load<AudioClip>("SoundFX/Fire2");
        EXPLOSION = Resources.Load<AudioClip>("SoundFX/Explosion");

        MUSICS = new AudioClip[5] { MUSIC, MUSIC2, MUSIC3, MUSIC4, MUSIC5 };
    }
}
