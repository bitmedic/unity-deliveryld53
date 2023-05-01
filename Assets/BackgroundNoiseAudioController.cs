using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundNoiseAudioController : MonoBehaviour
{
    public List<AudioClip> sounds;
    private AudioSource source;

    public bool isWalking;
    public float minDelay;
    public float maxDelay;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponentInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWalking)
        {
            if (timer <= 0)
            {
                int randomStep = Random.Range(0, sounds.Count);
                source.PlayOneShot(sounds[randomStep]);

                timer = Random.Range(minDelay, maxDelay);
            }

            timer -= Time.deltaTime;
        }
        else
        {
            timer = 0;
        }
    }
}
