using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TippingAudioController : MonoBehaviour
{
    private AudioSource audio;
    public List<AudioClip> soundsTipping;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void PlayRandomSound()
    {
        audio.PlayOneShot(soundsTipping[Random.Range(0, soundsTipping.Count)]);
    }
}
