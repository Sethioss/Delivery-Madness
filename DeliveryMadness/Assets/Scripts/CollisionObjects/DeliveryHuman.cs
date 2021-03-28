using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryHuman : CollisionObjects
{
    public override void GetHit(Collision collisionObject)
    {
        gameManager.ChangeScore(scoreGiven);
        gameManager._UIManager.SetTriggerAnim("playFeedback", scoreGiven);
        gameManager.levelManager.human++;
        gameManager._UIManager.SetHumanTxt(gameManager.levelManager.human);

        GetComponent<Animator>().SetTrigger("hit");

        gameManager.audioManager.PlaySound("thud");

        string[] soundsToPlayBetween = { "pain1", "pain2", "pain3" };
        gameManager.audioManager.PlayRandomBetween(soundsToPlayBetween);
    }

    public override void GetHit(Collider collisionObject)
    {
        gameManager.ChangeScore(scoreGiven);
        gameManager._UIManager.SetTriggerAnim("playFeedback", scoreGiven);
        gameManager.levelManager.human++;
        gameManager._UIManager.SetHumanTxt(gameManager.levelManager.human);

        GetComponent<Animator>().SetTrigger("hit");

        gameManager.audioManager.PlaySound("thud");

        string[] soundsToPlayBetween = { "pain1", "pain2", "pain3" };
        gameManager.audioManager.PlayRandomBetween(soundsToPlayBetween);
    }
}
