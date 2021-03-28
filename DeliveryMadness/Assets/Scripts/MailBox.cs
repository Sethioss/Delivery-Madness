using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailBox : MonoBehaviour
{
    public int scoreGiven = 75;
    private bool hit = false;

    private void OnCollisionEnter(Collision collision)
    {
        if(!hit)
        {
            hit = true;

            if (collision.gameObject.CompareTag("Letter"))
            {
                GetComponent<Animator>().SetTrigger("LetterIn");
                collision.gameObject.SetActive(false);
                GameManager.instance.levelManager.mailbox++;
                GameManager.instance._UIManager.SetMailboxTxt(GameManager.instance.levelManager.mailbox);
                GameManager.instance.ChangeScore(scoreGiven);
                GameManager.instance._UIManager.SetTriggerAnim("playFeedback", scoreGiven);
                GameManager.instance.audioManager.PlaySound("mailBoxClose");
            }
        }

    }
}
