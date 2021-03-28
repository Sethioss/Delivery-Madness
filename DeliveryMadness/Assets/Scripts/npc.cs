using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc : MonoBehaviour
{
    public bool down = false;
    public int scoreGiven = 100;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Letter"))
        {
            if (!down)
            {
                down = true;
                GameManager.instance.ChangeScore(scoreGiven);
                GameManager.instance._UIManager.SetTriggerAnim("playFeedback", scoreGiven);
                GameManager.instance.levelManager.human++;
                GameManager.instance._UIManager.SetHumanTxt(GameManager.instance.levelManager.human);

                GetComponent<Animator>().SetTrigger("hit");

                GameManager.instance.audioManager.PlaySound("thud");

                string[] soundsToPlayBetween = { "pain1", "pain2", "pain3" };
                GameManager.instance.audioManager.PlayRandomBetween(soundsToPlayBetween);
            }
        }
    }
}
