using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenuLoader : MonoBehaviour
{
    public Animator transition;
    public Button continueButton;
    public Button newGameButton;
    public Button quitButton;
    public Button optionsButton;
    public TMP_Text buttonText;
    bool buttonClicked = false;
    public Canvas tCanvas;
    public Canvas UICanvas;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("Opened") == 0)
        {
            PlayerPrefs.SetInt("SavedLevel", 2);
            PlayerPrefs.SetInt("Opened", 1);
        }

        if (PlayerPrefs.GetInt("Saved Level") <= 2)
        {
            continueButton.interactable = false;
            buttonText.color = Color.grey;
        }
        else
        {
            continueButton.interactable = true;
        }
    }

    public void NewGame()
    {
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
            UICanvas.sortingOrder = 0;
            tCanvas.sortingOrder = 1;
            buttonClicked = true;
            continueButton.enabled = false;
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
