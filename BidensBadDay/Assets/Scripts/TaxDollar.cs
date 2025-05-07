using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxDollar : MonoBehaviour
{
    Rigidbody2D body;
    Transform trans;
    Animator anim;
    BoxCollider2D col;
    bool wingless = false;
    bool isDead = false;
    bool deadRan = false;
    SpriteRenderer sr;

    [SerializeField]
    float moveForce = 8f;
    [SerializeField]
    float jumpForce = 12f;
    [SerializeField]
    float jumpTime = 1f;

    const string player = "Player";
    const string ground = "Ground";

    const string wing = "Wingless";
    const string dead = "Dead";
    bool isInvunerable = false;
    bool grounded = false;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(moveBat());
    }

    private void Update()
    {
        if (isDead)
        {
            if (!deadRan)
            {
                StopAllCoroutines();
                deadRan = true;
                StartCoroutine(deadShow());
            }
        }
    }

    IEnumerator moveBat()
    {
        while (!isDead)
        {
            if (wingless)
            {
                if (grounded)
                {
                    body.linearVelocity = Vector2.zero;
                    float jt = jumpTime;
                    grounded = false;
                    while (jt > 0)
                    {
                        body.AddForce(Vector2.up * jumpForce);
                        body.linearVelocity = new Vector2(Random.Range(moveForce / 2, moveForce), body.linearVelocity.y);
                        jt = -Time.deltaTime;
                    }
                    moveForce = moveForce * -1;
                }
                else
                {
                    yield return null;
                }
           
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(0,2f));
                body.linearVelocity = new Vector2(moveForce, -moveForce);
                yield return new WaitForSeconds(1f);
                body.linearVelocity = new Vector2(moveForce / 2, 0f);
                yield return new WaitForSeconds(1f);
                body.linearVelocity = new Vector2(moveForce, moveForce);
                yield return new WaitForSeconds(1f);
                body.linearVelocity = Vector2.zero;
                yield return new WaitForSeconds(Random.Range(0,2f));
                body.linearVelocity = new Vector2(-moveForce, -moveForce);
                yield return new WaitForSeconds(1f);
                body.linearVelocity = new Vector2(-moveForce / 2, 0f);
                yield return new WaitForSeconds(1f);
                body.linearVelocity = new Vector2(-moveForce, moveForce);
                yield return new WaitForSeconds(1f);
                body.linearVelocity = Vector2.zero;
            }
            yield return null;
        }
        yield return null;
    }

    IEnumerator deadShow()
    {
        body.linearVelocity = Vector2.zero;
        col.enabled = false;
        yield return new WaitForSeconds(4f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionObject = collision.gameObject;
        Vector3 hit = collision.contacts[0].normal;
        bool above = determineAngle(hit, 180);

        switch (collisionObject.tag)
        {
            case player:
                if (above && !Player._isJumping && !isInvunerable)
                {
                    if (wingless)
                    {
                        anim.SetBool(dead, true);
                        isDead = true;
                    }
                    else
                    {
                        StopAllCoroutines();
                        body.bodyType = RigidbodyType2D.Dynamic;
                        anim.SetBool(wing, true);
                        wingless = true;
                        StartCoroutine(invicibility());
                    }
                }
                break;
            case ground:
                grounded = true;
                break;
        }
    }

    IEnumerator invicibility()
    {
        if (Health.health > 0)
        {
            int invincibleLayer = 7;
            int playerLayer = 6;

            gameObject.layer = invincibleLayer;
            isInvunerable = true;
            for (int i = 0; i < 8; i++)
            {
                sr.enabled = false;
                yield return new WaitForSeconds(0.1f);
                sr.enabled = true;
                yield return new WaitForSeconds(0.1f);
            }
            isInvunerable = false;
            gameObject.layer = playerLayer;
            StartCoroutine(moveBat());
        }
    }

    bool determineAngle(Vector3 hit, float a)
    {
        bool above = false;
        float angle = Vector3.Angle(hit, Vector3.up);

        if (Mathf.Approximately(angle, a))
        {
            above = true;
        }
        return above;
    }

}
