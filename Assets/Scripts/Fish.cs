using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public Sprite[] sprites;
    SpriteRenderer sr;
    Rigidbody2D rb;

    [SerializeField]
    float jumpTime;
    [SerializeField]
    float moveForce;

    const string player = "Player";

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(moveFish());
    }

    IEnumerator moveFish()
    {
        while (true)
        {
            rb.isKinematic = false;
            rb.velocity = Vector2.zero;
            float ttime = jumpTime;
            float timeToFall = 0f;
            while(ttime > 0)
            {
                rb.velocity = Vector2.up * moveForce;
                ttime -= Time.deltaTime;
                yield return null;
            }

            while (rb.velocity.y > 0f)
            {
                timeToFall += Time.deltaTime;
                yield return null;
            }
            
            sr.flipY = true;
            yield return new WaitForSeconds((jumpTime * 1.6f) - (timeToFall * .95f));
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(Random.Range(0, 3));
            sr.sprite = sprites[0];
            sr.flipY = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case (player):
                sr.sprite = sprites[1];
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case (player):
                sr.sprite = sprites[1];
                break;
        }
    }

}
