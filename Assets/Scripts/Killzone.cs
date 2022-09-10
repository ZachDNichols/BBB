using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    const string PLAYER = "Player";
    const string FISH = "Fish";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case PLAYER:
                int h = 0;
                Health.health = h;
                break;
            case FISH:
                break;
            default:
                Destroy(collision.gameObject);
                break;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        switch (collision.gameObject.tag)
        {
            case PLAYER:
                int h = 0;
                Health.health = h;
                break;
            case FISH:
                break;
            default:
                Destroy(collision.gameObject);
                break;

        }
    }
}
