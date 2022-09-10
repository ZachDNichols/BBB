using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    Animator anim;
    public AudioClip endSong;
    AudioSource music;
    bool started = false;

    const string scroll = "Scroll";

    // Start is called before the first frame update
    void Start()
    {
        music = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TrumpBoss.isGone && !started)
        {
            StartCoroutine(endGame());
            started = true;
        }
    }

    IEnumerator endGame()
    {
        anim.SetTrigger(scroll);
        yield return new WaitForSeconds(47f);
        LevelLoader.endLevel = true;
    }
}
