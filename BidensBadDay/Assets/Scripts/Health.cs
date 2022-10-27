using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update

    private static int ahealth;

    public static int health
        {
           get { return ahealth; }
           set 
           { 
            if (value < 3 || value >= 0)
            {
                ahealth = value;
            }
            else if (value < 3)
            {
                ahealth = 3;
            }
            else
            {
                ahealth = 0;
            }

           }
        }


    [SerializeField]
    private Image[] hearts;

    [SerializeField]
    private Sprite fullHeart;
    [SerializeField]
    private Sprite emptyHeart;


    void Awake()
    {
        health = 3;
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
    }
    private void CheckHealth()
    {
        switch (health)
        {
            case 0:
                hearts[0].sprite = emptyHeart;
                hearts[1].sprite = emptyHeart;
                hearts[2].sprite = emptyHeart;
                break;
            case 1:
                hearts[0].sprite = fullHeart;
                hearts[1].sprite = emptyHeart;
                hearts[2].sprite = emptyHeart;
                break;
            case 2:
                hearts[0].sprite = fullHeart;
                hearts[1].sprite = fullHeart;
                hearts[2].sprite = emptyHeart;
                break;
            case 3:
                hearts[0].sprite = fullHeart;
                hearts[1].sprite = fullHeart;
                hearts[2].sprite = fullHeart;
                break;
            default:
                health = 3;
                break;
        }
    }
}
