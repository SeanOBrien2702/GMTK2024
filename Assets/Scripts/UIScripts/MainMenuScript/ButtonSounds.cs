using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSounds : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip onClicksound;
    public AudioClip onHoverSound;

    public void OnHoverSound()
    {
        audioSource.PlayOneShot(onHoverSound);
    }

    public void OnClickSound()
    {
        audioSource.PlayOneShot(onClicksound);
    }
}
