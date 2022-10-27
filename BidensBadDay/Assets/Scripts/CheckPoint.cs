using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    public static int totalFlags;
    public static int flagsCleared = 0;
    private SpriteRenderer sr;
    public Sprite greenFlag;
    public Sprite redFlag;
    [SerializeField]
    int flagNumber = 1;
    public Transform playerPos;
    private Transform trans;
    bool flagChecked = false;

    const string player = "Player";

    void Awake()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 2:
                totalFlags = 2;
                break;
            default:
                totalFlags = 0;
                break;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        trans = gameObject.GetComponent<Transform>();

        flagsCleared = PlayerPrefs.GetInt("Flag");

        if (flagsCleared >= flagNumber)
        {
            sr.sprite = greenFlag;
            flagChecked = true;
            if(flagsCleared == flagNumber)
            {
                playerPos.position = trans.position;
            }
        }
        else
        {
            sr.sprite = redFlag;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case player:
                sr.sprite = greenFlag;
                PlayerPrefs.SetInt("Flag", flagNumber);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case player:
                if (!flagChecked)
                {
                    flagsCleared++;
                    sr.sprite = greenFlag;
                    PlayerPrefs.SetInt("Flag", flagsCleared);
                }
                break;
        }
    }

}
