using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepsAudioController : MonoBehaviour
{
    public List<AudioClip> soundsSteps;
    private AudioSource source;

    public bool isWalking;
    public float stepTimeInterval;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponentInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking)
        {
            if (timer <= 0)
            {
                int randomStep = Random.Range(0, soundsSteps.Count);
                source.PlayOneShot(soundsSteps[randomStep]);
                timer = stepTimeInterval;
            }

            timer -= Time.deltaTime;
        }
        else
        {
            timer = 0;
        }
    }
}
