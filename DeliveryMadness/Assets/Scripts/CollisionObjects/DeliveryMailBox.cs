using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryMailBox : CollisionObjects
{
        public override void GetHit(Collision collisionObject)
        {
                GetComponent<Animator>().SetTrigger("LetterIn");
                collisionObject.gameObject.SetActive(false);
                gameManager.levelManager.mailbox++;
                gameManager._UIManager.SetMailboxTxt(GameManager.instance.levelManager.mailbox);
                gameManager.ChangeScore(scoreGiven);
                gameManager._UIManager.SetTriggerAnim("playFeedback", scoreGiven);
                gameManager.audioManager.PlaySound("mailBoxClose");
        }
}