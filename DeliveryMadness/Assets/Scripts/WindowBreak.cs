using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowBreak : MonoBehaviour
{
    public GameObject unbrokenGlass;
    public GameObject glassToBreak;
    public int scoreGiven = 50;
    public bool broken;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Letter"))
        {
            if(!broken)
            {
                broken = true;
                unbrokenGlass.SetActive(false);
                glassToBreak.SetActive(true);
                GameManager.instance.ChangeScore(scoreGiven);
                GameManager.instance._UIManager.SetTriggerAnim("playFeedback", scoreGiven);

                Rigidbody[] glassRigidbodies;
                glassRigidbodies = glassToBreak.GetComponentsInChildren<Rigidbody>();
                foreach (Rigidbody rb in glassRigidbodies)
                {
                    rb.isKinematic = false;
                }

                GameManager.instance.levelManager.destroyedWindows++;
                GameManager.instance._UIManager.SetWindowTxt(GameManager.instance.levelManager.destroyedWindows);
                string[] soundsToPlayBetween = { "glassBreak1", "glassBreak2", "glassBreak3" };
                GameManager.instance.audioManager.PlayRandomBetween(soundsToPlayBetween);
            }
        }
    }
}
