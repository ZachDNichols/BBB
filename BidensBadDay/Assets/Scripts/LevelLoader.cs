using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private void Awake()
    {
        transition.ResetTrigger("Start");
        transition.ResetTrigger("End");
    }

    public Animator transition;
    public static bool endLevel = false;

    private void Update()
    {
        if (Health.health == 0)
            StartCoroutine(LoadGameOver());
        if (endLevel)
        {
            StartCoroutine(LoadLevel(0));
        }
    }

    public IEnumerator LoadGameOver()
    {
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(LoadLevel(1));
    }

    public IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        endLevel = false;
        SceneManager.LoadScene(levelIndex);
    }

}
