using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SFX
{
    public string name;
    public AudioClip clip;
    [Range(0, 1)]
    public float volume = 1;
}

public class AudioManager : MonoBehaviour
{
    public AudioSource source;
    public bool sfxActivated = true;
    public SFX[] SFXList;

    public void PlaySound(string name)
    {
        for(int i = 0; i < SFXList.Length; i++)
        {
            if(SFXList[i].name == name)
            {
                source.volume = SFXList[i].volume;
                source.clip = SFXList[i].clip;
                source.PlayOneShot(SFXList[i].clip);
                break;
            }
        }
    }

    public void PlayRandomBetween(string[] selection)
    {
        int random = Random.Range(0, selection.Length);
        string selected = selection[random];

        for(int i = 0; i < SFXList.Length; i++)
        {
            if(SFXList[i].name == selected)
            {
                source.volume = SFXList[i].volume;
                source.clip = SFXList[i].clip;
                source.PlayOneShot(SFXList[i].clip);
                break;
            }
        }
    }

    public void ToggleSFX()
    {
        sfxActivated = !sfxActivated;
    }
}
