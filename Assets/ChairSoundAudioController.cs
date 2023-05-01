using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairSoundAudioController : MonoBehaviour
{
    private AudioSource source;
    public List<AudioClip> sounds;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        source.PlayOneShot(sounds[Random.Range(0, sounds.Count)]);
    }
}
