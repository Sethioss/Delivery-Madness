using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryWindow : CollisionObjects
{
    public GameObject unbrokenGlass;
    public GameObject glassToBreak;
    public override void GetHit(Collision collisionObject)
    {
        unbrokenGlass.SetActive(false);
        glassToBreak.SetActive(true);
        gameManager.ChangeScore(scoreGiven);
        gameManager._UIManager.SetTriggerAnim("playFeedback", scoreGiven);

        Rigidbody[] glassRigidbodies;
        glassRigidbodies = glassToBreak.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in glassRigidbodies)
        {
            rb.isKinematic = false;
        }

        gameManager.levelManager.destroyedWindows++;
        gameManager._UIManager.SetWindowTxt(gameManager.levelManager.destroyedWindows);
        string[] soundsToPlayBetween = { "glassBreak1", "glassBreak2", "glassBreak3" };
        gameManager.audioManager.PlayRandomBetween(soundsToPlayBetween);
    }

    public override void GetHit(Collider collisionObject)
    {
        Vector3 back = -transform.right * 200;

        unbrokenGlass.SetActive(false);
        glassToBreak.SetActive(true);
        gameManager.ChangeScore(scoreGiven);
        gameManager._UIManager.SetTriggerAnim("playFeedback", scoreGiven);

        Rigidbody[] glassRigidbodies;
        glassRigidbodies = glassToBreak.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in glassRigidbodies)
        {
            rb.isKinematic = false;
            rb.AddForce(back);
        }

        gameManager.levelManager.destroyedWindows++;
        gameManager._UIManager.SetWindowTxt(gameManager.levelManager.destroyedWindows);
        string[] soundsToPlayBetween = { "glassBreak1", "glassBreak2", "glassBreak3" };
        gameManager.audioManager.PlayRandomBetween(soundsToPlayBetween);
    }
}
