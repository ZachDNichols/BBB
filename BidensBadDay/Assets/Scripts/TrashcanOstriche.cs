using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashcanOstriche : MonoBehaviour
{
    Animator anim;
    public AudioClip[] sfx;
    AudioSource audioSource;
    [SerializeField]
    GameObject trashcan;

    private bool isOn;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        StartCoroutine(moveOstriche());
    }

    private void Update()
    {
        isOn = trashcan.GetComponent<TShell>().playerOn;
    }

    IEnumerator moveOstriche()
    {
        while (true)
        {
            if (!isOn)
            {
                anim.SetBool("Move", true);
                yield return new WaitForSeconds(.4f);
                audioSource.PlayOneShot(sfx[0]);
                yield return new WaitForSeconds(1.95f);
                audioSource.PlayOneShot(sfx[1]);
                yield return new WaitForSeconds(.78f);
                yield return new WaitForSeconds(.1f);
                anim.SetBool("Move", false);
                yield return new WaitForSeconds(Random.Range(0, 3f));
            }
            yield return null;
        }
    }

}
