using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionObjects : MonoBehaviour
{
    public int scoreGiven;
    protected bool hit = false;

    protected GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Letter") || collision.gameObject.CompareTag("Player"))
        {
            if (!hit)
            {
                hit = true;
                GetHit(collision);
                collision.gameObject.GetComponent<Journal>().duration = 0.2f;
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Letter"))
        {
            if (!hit)
            {
                hit = true;
                GetHit(collision);
                collision.GetComponent<Journal>().duration = 0.2f;
            }
        }

        else if (collision.gameObject.CompareTag("Player"))
        {
            if (!hit)
            {
                hit = true;
                GetHit(collision);
            }
        }
    }
    public virtual void GetHit(Collision collisionObject)
    {
    }

    public virtual void GetHit(Collider collisionObject)
    {

    }
}
