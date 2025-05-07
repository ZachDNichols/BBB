using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrumpBoss : MonoBehaviour
{
    public static bool start = false;
    bool dead = false;
    bool grounded = true;
    bool lookAtPlayer = true;
    bool isInvunerable = false;
    bool dying = false;
    public static bool isGone = false;

    public float leftCoordX = 698.85f;
    public float rightCoordX = 716.34f;
    float playerPos;
    float currentPos;
    [SerializeField]
    float moveForce = 10f;

    int action;
    int health = 6;

    //Unity Game
    Rigidbody2D rb;
    SpriteRenderer sr;
    private Transform playerTrans;
    public GameObject player;
    public GameObject warningTriangle;
    Transform triangleTrans;
    Animator triangleAnim;
    Transform trans;
    Animator anim;
    PolygonCollider2D col;
    public GameObject rightBarrier;
    public GameObject leftBarrier;
    public GameObject bossMusic;
    public GameObject leftEndBarrier;
    public GameObject rightEndBarrier;
    public GameObject endMusic;
    

    //Animation Triggers/Bool Variables
    string flash = "Flashing";
    string jump = "Jump";
    string run = "Run";
    string falling = "Falling";

    //GameObject Tags
    const string GROUND = "Ground";
    const string PLAYER = "Player";

    private void Awake()
    {
        start = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        triangleAnim = warningTriangle.GetComponent<Animator>();
        triangleTrans = warningTriangle.GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        playerTrans = player.GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector3.zero;
        col = GetComponent<PolygonCollider2D>();
        StartCoroutine(startCheck());
    }

    // Update is called once per frame
    void Update()
    {
        if (lookAtPlayer)
        {
            playerPos = playerTrans.position.x;
            currentPos = trans.position.x;

            if (playerPos > currentPos)
            {
                trans.transform.localScale = new Vector2(1, 1);
            }
            else if (playerPos < currentPos)
            {
                trans.transform.localScale = new Vector2(-1, 1);
            }
        }

        if(dead && !dying)
        {
            StopAllCoroutines();
            dying = true;
            StartCoroutine(Dead());
        }

    }

    IEnumerator startCheck()
    {
        while (!start)
        {
            yield return null;
        }
        triangleAnim.SetBool(flash, false);
        warningTriangle.SetActive(true);
        rb.bodyType = RigidbodyType2D.Dynamic;
        StartCoroutine(bossFight());
    }

    IEnumerator bossFight()
    {
        while (!dead)
        {
            if (!isInvunerable)
            {
                action = Random.Range(1, 3);
            }
            else
            {
                action = 2;
            }


            if (grounded)
            {
                //Jump Attack
                if (action == 1)
                {
                    anim.SetTrigger(jump);
                    yield return new WaitForSeconds(0.26f);
                    rb.bodyType = RigidbodyType2D.Kinematic;
                    rb.linearVelocity = new Vector2(0f, 30f);
                    yield return new WaitForSeconds(1.5f);
                    rb.linearVelocity = Vector2.zero;
                    trans.position = new Vector2(727f, 60f);
                    triangleAnim.SetBool(flash, true);
                    yield return new WaitForSeconds(2f);
                    trans.position = new Vector2(triangleTrans.position.x, 60f);
                    triangleAnim.SetBool(flash, false);
                    anim.SetBool(falling, true);
                    grounded = false;
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    rb.linearVelocity = new Vector2(0f, -50);
                }
                //Run Attack
                else if (action == 2)
                {
                    rb.bodyType = RigidbodyType2D.Kinematic;
                    lookAtPlayer = false;

                    float leftDelta = trans.position.x - leftCoordX;
                    float rightDelta = rightCoordX - trans.position.x;
                    
                    if (leftDelta > rightDelta)
                    {
                        if (moveForce > 0)
                        {
                            moveForce = -moveForce;
                        }
                    }

                    for (int i = 0; i <= 3; i++)
                    {
                        anim.SetBool(run, true);
                        rb.linearVelocity = new Vector2(moveForce, 0f);
                        //Going right
                        if (moveForce > 0)
                        {
                            trans.transform.localScale = new Vector2(1, 1);
                            while (trans.position.x < rightCoordX)
                            {
                                yield return null;
                            }
                            trans.transform.localScale = new Vector2(-1, 1);
                        }
                        //Going left
                        else if (moveForce < 0)
                        {
                            trans.transform.localScale = new Vector2(-1, 1);
                            while (trans.position.x > leftCoordX)
                            {
                                yield return null;
                            }
                            trans.transform.localScale = new Vector2(1, 1);
                        }
                        moveForce = -moveForce;
                        anim.SetBool(run, false);
                        rb.linearVelocity = Vector2.zero;
                        
                        yield return new WaitForSeconds(0.5f);
                    }
                    lookAtPlayer = true;
                    anim.SetBool(run, false);
                    rb.linearVelocity = Vector2.zero;
                    yield return new WaitForSeconds(1f);


                }
                //In case something breaks
                else
                {
                    yield return null;
                }
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
            yield return null;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 hit = collision.contacts[0].normal;
        bool above = determineAngle(hit);

        switch (collision.gameObject.tag)
        {
            case GROUND:
                if (!grounded)
                {
                    rb.linearVelocity = Vector2.zero;
                    grounded = true;
                    anim.SetBool(falling, false);
                }
                break;
            case PLAYER:
                if (above && !Player._isJumping)
                {
                    health--;
                    StartCoroutine(takeHealth());
                }
                break;

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

    IEnumerator takeHealth()
    {
        if (health > 0)
        {
            int invincibleLayer = 7;
            int bossLayer = 8;

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
            gameObject.layer = bossLayer;
        }
        else if (health <= 0)
        {
            dead = true;
        }
    }

    IEnumerator Dead()
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Dynamic;
        gameObject.layer = 8;
        sr.flipY = true;
        col.isTrigger = true;
        Destroy(rightBarrier);
        Destroy(leftBarrier);
        Destroy(bossMusic);
        isGone = true;
        endMusic.SetActive(true);
        leftEndBarrier.SetActive(true);
        rightEndBarrier.SetActive(true);
        yield return new WaitForSeconds(7f);
        Destroy(gameObject);
        StopAllCoroutines();
    }

}
