using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public static bool isPaused = false;
    public GameObject pauseMenu;
    public Animator transition;
    public AudioMixer musicMixer;
    public AudioMixer sfxMixer;
    public GameObject pauseBG;
    public GameObject music;

    private void Start()
    {
        musicMixer.SetFloat("Volume", PlayerPrefs.GetFloat("Music Volume"));
        sfxMixer.SetFloat("Volume", PlayerPrefs.GetFloat("SFX Volume"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Options.onScreen)
            {
                pauseMenu.SetActive(true);
            }
            else if (!Options.onScreen)
            {
                if (isPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
    }

    public void Resume()
    {
        musicMixer.SetFloat("Volume", PlayerPrefs.GetFloat("Music Volume"));
        sfxMixer.SetFloat("Volume", PlayerPrefs.GetFloat("SFX Volume"));
        isPaused = false;
        pauseMenu.SetActive(false);
        pauseBG.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        musicMixer.SetFloat("Volume", -80f);
        sfxMixer.SetFloat("Volume", -80f);
        pauseMenu.SetActive(true);
        pauseBG.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void QuitToMenu()
    {
        StartCoroutine(QTM());
    }

    IEnumerator QTM()
    {
        Resume();
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
    }
}
