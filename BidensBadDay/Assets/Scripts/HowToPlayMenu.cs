using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayMenu : MonoBehaviour
{
    public Image image;
    public Sprite[] images;
    public Button rightButton;
    public Button leftButton;
    private int imageIndex = 0;

    private void Update()
    {
        image.sprite = images[imageIndex];

        if(imageIndex > 2)
        {
            imageIndex = 2;
        }
        
        if (imageIndex < 2)
        {
            rightButton.interactable = true;
        }
        else
        {
            rightButton.interactable = false;
        }

        if (imageIndex > 0)
        {
            leftButton.interactable = true;
        }
        else
        {
            leftButton.interactable = false;
        }
    }

    public void nextImage()
    {
        imageIndex++;
    }

    public void lastImage()
    {
        imageIndex--;
    }

}
