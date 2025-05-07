using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using System;
using TMPro;

public class Player : MonoBehaviour
{

    //Other Variables
    [SerializeField]
    private float moveForce = 10f;
    [SerializeField]
    private float jumpForce = 11f;
    private float movementX;
    private bool isGrounded;
    public bool IsGroundedA
    {
        get => isGrounded;
    }
    private float jumpTimeCounter;
    [SerializeField]
    private float jumpTime = 1;
    private static bool isJumping;
    private bool isFalling;
    private bool dead;
    public static bool canMove = true;
    private bool isInvunerable = false;
    private static int currentLevel;
    private int bananaNum;

    //Tags
    const string GROUND = "Ground";
    const string SLOPE = "Slope";
    const string PLATFORM = "Platform";
    const string ENEMY = "Enemy";
    const string BANANA = "Banana";
    const string BOSS = "Boss";
    const string TCAN = "Trashcan Monster";
    const string UNBLOCK = "Unbreakable Block";
    const string FISH = "Fish";
    const string SAM = "Jetpack";
    const string TONGUE = "Tongue";
    const string CRAB = "Crab";

    //Animation Bools
    private string jump = "Jump";
    private string walk = "Walk";

    //Unity Objects
    private Rigidbody2D body;
    private Animator anim;
    private Transform trans;
    private PolygonCollider2D plBody;
    private SpriteRenderer sr;
    private GameObject cameraController;
    [SerializeField]
    GameObject breaker;
    public TMP_Text bananaText;
    public AudioClip[] sfx;
    public AudioClip[] hurtSounds;
    public AudioClip[] jumpSounds;
    public AudioClip[] hitSounds;
    public AudioClip[] bananaSounds;
    public AudioClip dies;
    AudioSource audioSource;


    //Getters and Setters
    public static bool _isJumping
    {
        get { return isJumping; }
    }

    
    // Start is called before the first frame update
    void Awake()
    {
        canMove = true;
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        plBody = GetComponent<PolygonCollider2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        cameraController = GameObject.Find("Level Cam");
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("Current Level", currentLevel);
        bananaNum = 0;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
            return;
        playerMove();
        playerJump();
        fallCheck();
        isDead();
        AnimatePlayer();

        //Health.health = 3;
    }

    //Moves the player
    private void playerMove()
    {
        if (canMove)
        {
            movementX = Input.GetAxisRaw("Horizontal");
            if (movementX != 0)
            {
                body.bodyType = RigidbodyType2D.Dynamic;
            }
            
            body.linearVelocity = new Vector3(movementX * moveForce, body.linearVelocity.y);
        }
    }

    //Determins if the player is dead
    void isDead()
    {
        if (Health.health <= 0)
        {
            audioSource.PlayOneShot(dies);
            canMove = false;
            body.linearVelocity = Vector2.zero;
            dead = true;
            plBody.enabled = false;
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce * 1.5f);
            sr.flipY = true;
            Destroy(cameraController);
        }
    }

    //Lets the player jump
    private void playerJump()
    {
        if(!isFalling & isGrounded && Input.GetKeyDown(KeyCode.Space) && canMove)
        {
            int hunjo = UnityEngine.Random.Range(0, 101);

            if(hunjo == 69)
            {
                audioSource.PlayOneShot(jumpSounds[jumpSounds.Length - 1]);
            }
            else
            {
                audioSource.PlayOneShot(jumpSounds[UnityEngine.Random.Range(0, jumpSounds.Length - 2)]);
            }

            body.bodyType = RigidbodyType2D.Dynamic;
            anim.SetBool(jump, true);
            isJumping = true;
            jumpTimeCounter = jumpTime;
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
    }

    private void AnimatePlayer()
    {
        if (dead)
        {
            anim.SetBool(jump, false);
            anim.SetBool(jump, false);
            return;
        }

        if (!isJumping && !isFalling && isGrounded)
        {
            anim.SetBool(jump, false);
            //Right
            if (movementX > 0)
            {
                anim.SetBool(walk, true);
                trans.transform.localScale = new Vector2(-1, 1);
                body.bodyType = RigidbodyType2D.Dynamic;
            }
            //Left
            else if (movementX < 0)
            {
                anim.SetBool(walk, true);
                trans.transform.localScale = new Vector2(1, 1);
                body.bodyType = RigidbodyType2D.Dynamic;
            }
            else
            {
                anim.SetBool(walk, false);
            }
        }

        if (isJumping || isFalling)
        {
            anim.SetBool(jump, true);
        }
    }
    private void fallCheck()
    {
        if (body.linearVelocity.y < -8f)
        {
            isFalling = true;
        }
        else
        {
            isFalling = false;
        }

    }

    void KnockBack(GameObject enemy, float kbTime, float kbForceX, float kbForceY)
    {
        Transform enemyTrans = enemy.GetComponent<Transform>();
        Vector2 direction = enemyTrans.position - trans.position;
        canMove = false;
        //If hit from the left
        if (direction.x > 0)
        {
            kbForceX = kbForceX * -1f;
        }

        if (Health.health > 0)
        {
            StartCoroutine(moveBack(kbForceX, kbForceY, kbTime));
        }
        canMove = true;
    }

    IEnumerator moveBack(float kbForceX, float kbForceY, float kbTime)
    {
        float time = kbTime;
        while (time > 0)
        {
            body.linearVelocity = new Vector3(kbForceX, kbForceY);
            yield return null;
            time = time - Time.deltaTime;
        }
    }

    IEnumerator invicibility()
    {
        if (Health.health > 0)
        {
            int invincibleLayer = 7;
            int playerLayer = 3;

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
        }
    }
        
        void hurtPlayer(GameObject enemy)
        {
            int h = Health.health;
            h--;
            Health.health = h;
            audioSource.PlayOneShot(hurtSounds[UnityEngine.Random.Range(0, hurtSounds.Length - 1)]);
            KnockBack(enemy, 0.1f, 20f, 0.5f);
            StartCoroutine(invicibility());
        }

        IEnumerator enemyJump()
        {

            audioSource.PlayOneShot(hitSounds[UnityEngine.Random.Range(0, hitSounds.Length - 1)]);

            isGrounded = false;
                anim.SetBool(jump, true);
                float time = 0.2f;
                while (time > 0)
                {
                    body.linearVelocity = new Vector3(body.linearVelocity.x, jumpForce);
                    yield return null;
                    time = time - Time.deltaTime;

                }
        }

        bool determineAngle(Vector3 hit)
        {
            bool above = false;
            float angle = Vector3.Angle(hit, Vector3.up);
            
            if (Mathf.Approximately(angle, 0))
            {
            above = true;
            }

            return above;
        }

        bool isBelow(Vector3 hit)
        {
            bool below = false;
            float angle = Vector3.Angle(hit, Vector3.up);

            if (Mathf.Approximately(angle, 180))
                {
                    below = true;
                }
            return below;
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {

        GameObject collisionObject = collision.gameObject;
        Vector3 hit = collision.contacts[0].normal;
        bool above = determineAngle(hit);

        switch (collision.gameObject.tag)
        {
            case ENEMY:
                if (!above && !isInvunerable)
                {
                    hurtPlayer(collisionObject);
                }
                else if (isJumping)
                {
                    hurtPlayer(collisionObject);
                }
                else
                {
                    StartCoroutine(enemyJump());
                }
                break;
            case BOSS:
                if (!above && !isInvunerable)
                {
                    hurtPlayer(collisionObject);
                }
                else if (isJumping)
                {
                    hurtPlayer(collisionObject);
                }
                else
                {
                    StartCoroutine(enemyJump());
                }
                break;
            case GROUND:
                if (above)
                {
                    isGrounded = true;
                }
                break;
            case SLOPE:
                if (!isJumping)
                    isGrounded = true;
                if (isGrounded && movementX == 0 && !isJumping)
                {
                    body.bodyType = RigidbodyType2D.Kinematic;
                    body.linearVelocity = new Vector2(0f, 0f);
                }
                break;
            case PLATFORM:
                if (above && !isJumping)
                {
                    isGrounded = true;
                }
                else if(isBelow(hit))
                {
                    BreakBlock(collision);
                }
                break;
            case TCAN:
                    hurtPlayer(collisionObject);
                break;
            case UNBLOCK:
                if (above && !isJumping)
                {
                    isGrounded = true;
                }
                break;
            case SAM:
                if (!above && !isInvunerable)
                {
                    hurtPlayer(collisionObject);
                }
                else if (isJumping)
                {
                    hurtPlayer(collisionObject);
                }
                else
                {
                    StartCoroutine(enemyJump());
                }
                break;
            case CRAB:
                hurtPlayer(collisionObject);
                break;

        }
        }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionObject = collision.gameObject;

        switch (collision.gameObject.tag)
        {
            case BANANA:
                collectBanana(collision);
                break;
            case FISH:
                hurtPlayer(collisionObject);
                bananaNum = bananaNum / 2;
                bananaText.text = Convert.ToString(bananaNum);
                break;
            case TONGUE:
                hurtPlayer(collisionObject);
                break;
        }
    }

    void collectBanana(Collider2D collision)
    {

        bananaNum++;
        audioSource.PlayOneShot(sfx[0]);

        if (bananaNum >= 50)
        {
            bananaNum = 0;
            audioSource.PlayOneShot(bananaSounds[UnityEngine.Random.Range(0, bananaSounds.Length - 1)]);
            int h = 3;
            Health.health = h;
        }
        bananaText.text = Convert.ToString(bananaNum);
        Destroy(collision.gameObject);
    }

    void BreakBlock(Collision2D collision)
    {
        StartCoroutine(breakBlock(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y));
        Destroy(collision.gameObject);
    }

    IEnumerator breakBlock(float x, float y)
    {
            yield return null;
            audioSource.PlayOneShot(sfx[1]);
            GameObject temp = Instantiate(breaker, new Vector3(x + 2.26f, y - 0.1f), Quaternion.identity);
            isJumping = false;
            body.linearVelocity = new Vector3(body.linearVelocity.x, 0f);
            yield return new WaitForSeconds(1f);
            Destroy(temp);
    }

}

