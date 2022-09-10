using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    public Animator transition;
    public Button newGameButton;
    public Button quitButton;
    public Button optionsButton;
    bool buttonClicked = false;
    public Canvas tCanvas;
    public Canvas UICanvas;
    public AudioMixer musicMixer;
    public AudioMixer sfxMixer;

    private void Start()
    {
        musicMixer.SetFloat("Volume", PlayerPrefs.GetFloat("Music Volume"));
        sfxMixer.SetFloat("Volume", PlayerPrefs.GetFloat("SFX Volume"));
    }


    private void Awake()
    {
        PlayerPrefs.SetInt("Flag", 0);

        if (PlayerPrefs.GetInt("Opened") == 0)
        {
            PlayerPrefs.SetInt("SavedLevel", 2);
            PlayerPrefs.SetInt("Opened", 1);
        }
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("Flag", 0);
        PlayerPrefs.SetInt("Saved Level", 2);
        StartCoroutine(loadLevel(2));
    }
    public void Continue()
    {
        StartCoroutine(loadLevel(PlayerPrefs.GetInt("Saved Level")));
        SceneManager.LoadScene(PlayerPrefs.GetInt("Saved Level"));
    }
    public void Quit()
    {
        StartCoroutine(loadLevel(-1));

    }

    IEnumerator loadLevel(int index)
    {
        if (!buttonClicked)
        {
            Debug.Log("called");

            UICanvas.sortingOrder = 0;
            tCanvas.sortingOrder = 1;
            buttonClicked = true;
            quitButton.enabled = false;
            optionsButton.enabled = false;
            newGameButton.enabled = false;
            transition.SetTrigger("Start");
            yield return new WaitForSeconds(1f);
            if (index == -1)
            {
                Debug.Log("Quit");
                Application.Quit();
            }
            else
            {
                SceneManager.LoadScene(index);
            }
        }
    }
}
