using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionObject : MonoBehaviour
{
    public bool finishLine = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(!finishLine)
            {
                GameManager.instance.StopGame(false);
            }
            else
            {
                GameManager.instance.StopGame(true);
            }

        }
    }
}
