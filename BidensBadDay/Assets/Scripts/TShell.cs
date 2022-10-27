using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TShell : MonoBehaviour
{
    public bool playerOn;
    private bool waiting;
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOn = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOn = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!waiting)
            {
                StartCoroutine(QuickWait());
            }
        }
    }

    IEnumerator QuickWait()
    {
        waiting = true;
        yield return new WaitForSeconds(0.5f);
        playerOn = false;
        waiting = false;
    }

}
