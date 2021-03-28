using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : CollisionObjects
{
    public float speed;
    Vector3 rotation;

    private GameObject coin;
    private GameObject vfx;

    private void Start()
    {
        coin = transform.GetChild(0).gameObject;
        vfx = transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        if(!GameManager.instance.gameOver)
        {
            rotation = new Vector3(transform.rotation.x, speed, transform.rotation.z);
            transform.Rotate(rotation);
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.score += 20;
            GameManager.instance.overallScore += 20;
            GameManager.instance._UIManager.UpdateScoreUI();
            GameManager.instance.audioManager.PlaySound("coinSFX");
            coin.gameObject.SetActive(false);
            GameManager.instance.player.GetComponent<PlayerController>().PlayCoinVfx();
        }
    }*/

    public override void GetHit(Collider collider)
    {
        GameManager.instance.ChangeScore(scoreGiven);
        GameManager.instance._UIManager.UpdateScoreUI();
        GameManager.instance.audioManager.PlaySound("coinSFX");
        coin.gameObject.SetActive(false);
        GameManager.instance.player.GetComponent<PlayerController>().PlayCoinVfx();
    }
}
