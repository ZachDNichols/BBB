using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    AudioSource music;
    private bool playedFirstTime = false;

    // Start is called before the first frame update
    void Start()
    {
        music = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (PauseMenu.isPaused) 
        {
            music.Pause();
        }
        if (!music.isPlaying)
        {
            if (!playedFirstTime)
            {
                music.Play();
                playedFirstTime = true;
            }
            else
            {
                music.time = 7.40732f;
                music.Play();
            }
        }
    }
}
