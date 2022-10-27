using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Current : MonoBehaviour
{
    public static int levelNum;
    private Scene currentLvl;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        currentLvl = SceneManager.GetActiveScene();
        levelNum = currentLvl.buildIndex;
    }
}
