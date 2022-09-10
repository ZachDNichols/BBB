using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sam : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float moveTime;
    Rigidbody2D rb;
    Animator anim;
    BoxCollider2D col;
    [SerializeField]
    Animator tong;
    [SerializeField]
    bool tongue = true;
    bool deadPlayed;

    private const string tout = "Out";
    private const string tin = "In";
    private const string thrust = "Thrust";
    private const string dead = "Dead";
    private const string stay = "Stay";

    private const string PLAYER = "Player";

    private bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        StartCoroutine(jetPack());
    }

    private void Update()
    {
        if (!deadPlayed)
        {
            if (isDead)
            {
                StopAllCoroutines();
                deadPlayed = true;
                StartCoroutine(deadShow());
            }
        }

    }

    IEnumerator jetPack()
    {
        while (!isDead)
        {
            rb.velocity = new Vector2(rb.velocity.x, speed);
            anim.SetBool(thrust, true);
            yield return new WaitForSeconds(moveTime / 2);
            anim.SetBool(thrust, false);
            rb.velocity = Vector2.zero;

            if (tongue)
            {
                //Sets frog mouth open
                anim.SetBool(tout, true);
                yield return new WaitForSeconds(0.03f);
                //Tongue comes out
                tong.SetBool(tout, true);
                anim.SetBool(tout, false);
                yield return new WaitForSeconds(0.23f);
                tong.SetBool(stay, true);
                yield return new WaitForSeconds(Random.Range(0.5f, 1f));
                //Brings tongue back in
                tong.SetBool(tin, true);
                tong.SetBool(tout, false);
                yield return new WaitForSeconds(0.31f);
                tong.SetBool(stay, false);
                //Brings the sprite back into the frog
                tong.SetBool(tin, false);
                yield return new WaitForSeconds(0.01f);
                //Closes the mouth
                anim.SetBool(tout, false);
                anim.SetBool(tin, true);
                yield return new WaitForSeconds(0.03f);
                anim.SetBool(tin, false);
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(0, 1f));
            }

            rb.velocity = new Vector2(rb.velocity.x, speed);
            anim.SetBool(thrust, true);
            yield return new WaitForSeconds(moveTime / 2);
            anim.SetBool(thrust, false);
            rb.velocity = Vector2.zero;

            yield return new WaitForSeconds(1f);

            rb.velocity = new Vector2(rb.velocity.x, -speed);
            anim.SetBool(thrust, true);
            yield return new WaitForSeconds(moveTime / 2);
            anim.SetBool(thrust, false);
            rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(0.5f);

            if (tongue)
            {
                //Sets frog mouth open
                anim.SetBool(tout, true);
                yield return new WaitForSeconds(0.03f);
                //Tongue comes out
                tong.SetBool(tout, true);
                anim.SetBool(tout, false);
                yield return new WaitForSeconds(0.23f);
                tong.SetBool(stay, true);
                yield return new WaitForSeconds(Random.Range(0.5f, 1f));
                //Brings tongue back in
                tong.SetBool(tin, true);
                tong.SetBool(tout, false);
                yield return new WaitForSeconds(0.31f);
                tong.SetBool(stay, false);
                //Brings the sprite back into the frog
                tong.SetBool(tin, false);
                yield return new WaitForSeconds(0.01f);
                //Closes the mouth
                anim.SetBool(tout, false);
                anim.SetBool(tin, true);
                yield return new WaitForSeconds(0.03f);
                anim.SetBool(tin, false);
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(0, 1f));
            }

            rb.velocity = new Vector2(rb.velocity.x, -speed);
            anim.SetBool(thrust, true);
            yield return new WaitForSeconds(moveTime / 2);
            anim.SetBool(thrust, false);
            rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(Random.Range(0, 1f));

        }
    }

    bool determineAngle(Vector3 hit)
    {
        bool above = false;
        float angle = Vector3.Angle(hit, Vector3.up);

        if (Mathf.Approximately(angle, 180))
        {
            above = true;
        }

        return above;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 hit = collision.contacts[0].normal;

        switch (collision.gameObject.tag)
        {
            case PLAYER:
                if (determineAngle(hit) && !Player._isJumping)
                {
                    isDead = true;
                }
                break;
        }



    }


    IEnumerator deadShow() 
    {
        rb.velocity = Vector3.zero;
        tong.SetBool(stay, false);
        rb.isKinematic = false;
        anim.SetBool(tin, false);
        anim.SetBool(tin, false);
        anim.SetBool(thrust, false);
        tong.SetBool(tout, false);
        tong.SetBool(tin, false);
        anim.SetBool(dead, true);
        yield return new WaitForSeconds(6f);
        Destroy(gameObject);
    }

}
