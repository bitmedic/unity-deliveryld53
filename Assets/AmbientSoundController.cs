using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundController : MonoBehaviour
{
    public float ambientSoundLevel; // 0=leer to 1=voll
    public List<AudioClip> soundsGlassEmpty;
    public List<AudioClip> soundsGlassFull;

    private AudioSource audioSource;

    private float timerGlassSound = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.audioSource = GetComponent<AudioSource>();

        float secondsToWait = (-1 * ambientSoundLevel + 2.5f) * 5;
        float randomSeconds = Random.Range(secondsToWait * 0.7f, secondsToWait * 1.3f);
        timerGlassSound = Mathf.Max(0.1f, randomSeconds);
    }

    // Update is called once per frame
    void Update()
    {
        if (timerGlassSound <= 0)
        {
            int randomSound = Random.Range(0, soundsGlassEmpty.Count);
            audioSource.PlayOneShot(soundsGlassEmpty[randomSound]);


            // time to next glass sound
            // bei leerer bar vllt alle 5? sekunden, wenn voll alle 1 sekunde?
            float secondsToWait = (-1 * ambientSoundLevel + 2.5f) * 5;
            float randomSeconds = Random.Range(secondsToWait * 0.7f, secondsToWait * 1.3f);
            timerGlassSound = Mathf.Max(0.1f, randomSeconds);
        }

        timerGlassSound -= Time.deltaTime;
    }
}
