using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameOverMenu : MonoBehaviour
{
    private int lastLevel;
    private GameObject transition;
    private Animator anim;
    public GameObject video;
    VideoPlayer vplayer;

    

    private void Start()
    {
        lastLevel = Current.levelNum;
        transition = GameObject.Find("Crossfade");
        anim = transition.GetComponent<Animator>();
        vplayer = video.GetComponent<VideoPlayer>();

        float videoSound = PlayerPrefs.GetFloat("Music Volume");

        if (videoSound == 0)
        {
            videoSound = 1;
        }
        else
        {
            videoSound *= -1;
            videoSound = (1 / videoSound);
        }

        vplayer.SetDirectAudioVolume(0, videoSound);

    }


    public void TryAgain()
    {
        StartCoroutine(Transitioner(2));
    }

    public void MainMenu()
    {
        StartCoroutine(Transitioner(0));
    }

    private IEnumerator Transitioner(int levelIndex)
    {
        anim.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(levelIndex);
    }
    
    private IEnumerator goBack()
    {
        anim.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
    }

}
