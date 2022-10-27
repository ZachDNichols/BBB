using System.Collections;
using UnityEngine;
using UnityEngine.Playables;


public class BossTrigger : MonoBehaviour
{
    public GameObject bossCutscene;
    public PlayableDirector btimeline;
    public GameObject player;
    private Rigidbody2D rb;
    public Transform bTrans;
    public Rigidbody2D bBody;
    public GameObject hearts;

    private AudioSource Baudio;

    public AudioClip bossSting;
    public GameObject backgroundMusic;
    public GameObject bossMusic;
    public GameObject boss;
    public Transform cameraPos;
    public GameObject cinemaCam;

    bool started = false;
    bool triggered = false;
    bool playerGrounded;

    Keyframe[] keyframesX;
    AnimationCurve animX;
    AnimationCurve animY;
    Keyframe[] keyframesY;
    float toCameraX = 707.55f;
    float toCameraY = 41.14f;

    bool cameraMoveStarted = false;
    bool cameraMoved = false;


    // Start is called before the first frame update
    void Start()
    {
        btimeline = bossCutscene.GetComponent<PlayableDirector>();
        rb = player.GetComponent<Rigidbody2D>();
        Baudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        playerGrounded = player.GetComponent<Player>().IsGroundedA;

        if (triggered && playerGrounded && !started)
        {
            boss.SetActive(true);
            started = true;
            backgroundMusic.SetActive(false);
            Baudio.PlayOneShot(bossSting);
            hearts.SetActive(false);
            btimeline.Play();
            StartCoroutine(stopCutscene());
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            triggered = true;
            cinemaCam.SetActive(false);
        }
    }

    /*
    IEnumerator moveCamera()
    {
        keyframesX = new Keyframe[60];
        keyframesY = new Keyframe[60];

        float cameraX = toCameraX - cameraPos.position.x;
        float cameraY = toCameraY - cameraPos.position.y;

        cameraX = cameraX / 59;
        cameraY = cameraY / 59;

        //Creates keyframes for the X axis of Camera
        for (var i = 0; i < keyframesX.Length; i++)
        {
            keyframesX[i] = new Keyframe(i, cameraPos.position.x + (cameraX * i));
        }
        animX = new AnimationCurve(keyframesX);

        for (var i = 0; i < keyframesY.Length; i++)
        {
            keyframesY[i] = new Keyframe(i, cameraPos.position.y + (cameraY * i));
        }
        animY = new AnimationCurve(keyframesY);
        
        while(cameraPos.position.x != toCameraX && cameraPos.position.y != toCameraY)
        {
            Debug.Log("Moving");
            cameraPos.position = new Vector2(animX.Evaluate(Time.time), animY.Evaluate(Time.time));
            yield return null;
        }

        cameraMoved = true;

    }
    */


    IEnumerator stopCutscene()
    {
        Player.canMove = false;
        yield return new WaitForSeconds(8.28f);
        btimeline.Stop();
        hearts.SetActive(true);
        rb.velocity = Vector3.zero;
        bBody.velocity = Vector3.zero;
        Player.canMove = true;
        yield return new WaitForSeconds(1.3f);
        bossMusic.SetActive(true);
        TrumpBoss.start = true;
    }



}
