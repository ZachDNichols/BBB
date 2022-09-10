using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : MonoBehaviour
{
    Transform trans;
    Rigidbody2D rb;
    public Transform pTrans;

    float moveForce = 8f;

    //if the player is to the right
    bool right;

    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(track());
    }

    // Update is called once per frame
    void Update()
    {
        float playerPos = pTrans.position.x;
        float currentPos = trans.position.x;


        if(currentPos > playerPos)
        {
            right = false;
        }
        else if (currentPos < playerPos)
        {
            right = true;
        }
    }

    IEnumerator track()
    {
        while (true)
        {
            if (right)
            {
                rb.velocity = new Vector3(moveForce, 0);
                yield return null;
            }
            else
            {
                rb.velocity = new Vector3(-moveForce, 0);
                yield return null;
            }
        }
    }    

}
