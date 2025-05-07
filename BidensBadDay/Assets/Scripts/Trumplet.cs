using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trumplet : MonoBehaviour
{
    //Other variables
    [SerializeField]
    private float moveForce;
    public float Ttime;
    public float minTime = 1f;
    public float maxTime = 3f;
    bool isDead = false;
    bool bricked = false;

    //Animation Strings
    const string walk = "Walk";
    const string brick = "Brick";

    //Tags
    const string PLAYER = "Player";
    const string BROKEN = "Broken";

    //Unity Objects
    private Rigidbody2D body;
    private BoxCollider2D col;
    private SpriteRenderer sprite;
    private Animator anim;
    private Transform trans;


    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        trans = GetComponent<Transform>();
        StartCoroutine(moveEnemy());
    }



    IEnumerator moveEnemy()
    {
        while (!isDead)
        {
            float time = Ttime;
            while (time > 0)
            {
                if (!isDead)
                {
                    anim.SetBool(walk, true);
                    body.linearVelocity = new Vector2(moveForce, body.linearVelocity.y);
                    yield return null;
                    time = time - Time.deltaTime;
                }
                else
                    break;
            }
            anim.SetBool(walk, false);
            body.linearVelocity = Vector2.zero;
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            time = Ttime;
            
            while (time > 0)
            {
                if (!isDead)
                {
                    anim.SetBool(walk, true);
                    body.linearVelocity = new Vector2(-moveForce, body.linearVelocity.y);
                    yield return null;
                    time = time - Time.deltaTime;
                }
                else
                    break;
            }
            body.linearVelocity = Vector2.zero;
            anim.SetBool(walk, false);
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
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
        bool above = determineAngle(hit);

        switch (collision.gameObject.tag)
        {
            case PLAYER:
                if (above && !Player._isJumping)
                {
                    isDead = true;
                    StartCoroutine(Dead());
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case BROKEN:
                if (!bricked)
                {
                    StartCoroutine(brickKill());
                }
                break;
        }
    }

    IEnumerator brickKill()
    {
        yield return null;
        sprite.flipY = true;
        body.linearVelocity = Vector3.zero;
        trans.position = new Vector3(trans.position.x, trans.position.y + .168f);
        body.linearVelocity = Vector3.zero;
        isDead = true;
        bricked = true;
        col.enabled = false;
        yield return new WaitForSeconds(1.2f);
        Destroy(gameObject);
    }

    IEnumerator Dead()
    {
        yield return null;
        sprite.flipY = true;
        body.linearVelocity = Vector2.zero;
        col.enabled = false;
        yield return new WaitForSeconds(1.2f);
        Destroy(gameObject);
    }


}
