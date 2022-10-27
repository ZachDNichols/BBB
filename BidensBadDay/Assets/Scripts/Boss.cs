using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Boss : MonoBehaviour
{
    Transform trans;
    Transform pTrans;
    SpriteRenderer sr;
    public GameObject player;
    int bossHealth = 5;
    bool isDead = false;
    bool isInvunerable = false;
    float jumpDelta = .85f;
    bool left = true;
    bool right = false;
    Rigidbody2D rb;
    float jumpForce = 10;
    float moveForce = 8;
    Animator anim;
    BoxCollider2D col;
    public static bool start = false;

    float bossScaleX = 0.1205093f;
    float bossScaleY = 0.1205093f;
    int attackTime;
    bool isGrounded = true;
    bool startedDead = false;

    public PlayableDirector end;

    public GameObject bbL;
    public GameObject bbR;
    public GameObject health;


    //Animator Stuff
    string jump = "Jump";
    string idle = "Idle";
    string croak = "Croak";

    private float playerScaleX = -0.0192f;
    private float playerScaleY = 0.01926368f;

    public Animator transition;


    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(7, 8, true);
        Physics.IgnoreLayerCollision(8, 7, true);
        attackTime = bossHealth - 2;
        trans = GetComponent<Transform>();
        trans.position = new Vector2(334.57f, 6.38f);
        pTrans = player.GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        
        StartCoroutine(boss());
    }

    // Update is called once per frame
    void Update()
    {
        dead();
        if (left)
        {
            trans.transform.localScale = new Vector2(bossScaleX, bossScaleY);
        }
        if (right)
        {
            trans.transform.localScale = new Vector2(-bossScaleX, bossScaleY);
        }
        if (bossHealth < 1)
        {
            attackTime = 1;
        }

    }

    void dead()
    {
        if (isDead)
        {
            if (!startedDead)
            {
                StartCoroutine(deadShow());
                startedDead = true;
            }
        }
    }

    IEnumerator deadShow()
    {
        Player.canMove = false;
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        Destroy(bbR);
        Destroy(bbL);
        Destroy(health);
        pTrans.transform.localScale = new Vector2(playerScaleX, playerScaleY);
        end.Play();
        transition.SetTrigger("End");
        transition.ResetTrigger("Start");
        yield return new WaitForSeconds(11.82f);
        LevelLoader.endLevel = true;
    }

    IEnumerator boss()
    {
        while (!isDead)
        {
            if (start)
            {
                yield return new WaitForSeconds(Random.Range(1, attackTime));
                anim.SetBool(idle, false);
                anim.SetTrigger(croak);
                yield return new WaitForSeconds(1.1f);
                anim.ResetTrigger(croak);
                if (left)
                {
                    anim.SetBool(jump, true);
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    rb.velocity = new Vector2(Random.Range(-1, -moveForce), rb.velocity.y);
                    isGrounded = false;
                    while (!isGrounded)
                    {
                        yield return null;
                    }
                    right = true;
                    left = false;
                    anim.SetBool(jump, false);
                }
                else if (right)
                {
                    anim.SetBool(jump, true);
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    rb.velocity = new Vector2(Random.Range(1, moveForce), rb.velocity.y);
                    isGrounded = false;
                    while (!isGrounded)
                    {
                        yield return null;
                    }
                    right = false;
                    left = true;
                    anim.SetBool(jump, false);
                }
                anim.SetBool(idle, true);
            }
            else
                yield return null;
        }
        


    }

    IEnumerator invicibility()
    {
        if (Health.health > 0)
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


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Feet"))
        {
            float delta = 0;
            if ((pTrans.position.y < 0 && trans.position.y > 0) || (trans.position.y < 0 && pTrans.position.y > 0))
            {
                delta = pTrans.position.y + trans.position.y;
            }
            else
            {
                delta = pTrans.position.y - trans.position.y;
            }

            if (delta > jumpDelta && !isInvunerable)
            {
                bossHealth--;
                if (bossHealth <= 0)
                {
                    isDead = true;
                }
                StartCoroutine(invicibility());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Feet"))
        {
            float delta = 0;
            if ((pTrans.position.y < 0 && trans.position.y > 0) || (trans.position.y < 0 && pTrans.position.y > 0))
            {
                delta = pTrans.position.y + trans.position.y;
            }
            else
            {
                delta = pTrans.position.y - trans.position.y;
            }

            if (delta > jumpDelta && !isInvunerable)
            {
                bossHealth--;
                if (bossHealth <= 0)
                {
                    isDead = true;
                }
                StartCoroutine(invicibility());
            }
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}

