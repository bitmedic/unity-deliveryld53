using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicEffect : MonoBehaviour
{
    DrunkPlayer player;
    AudioSource source;
    AudioDistortionFilter distortion;

    private float smoothTime = 2f;
    private float v1, v2, v3;

    void Start()
    {
        player = DrunkPlayer.Instance;
        source = GetComponent<AudioSource>();
        distortion = GetComponent<AudioDistortionFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        float pegel = player.pegel;

        float pct = Mathf.InverseLerp(0, 2f, pegel);

        source.pitch = Mathf.SmoothDamp(source.pitch, Mathf.Lerp(1, 2, pct), ref v1, smoothTime);
        source.volume = Mathf.SmoothDamp(source.volume, Mathf.Lerp(.01f, .002f, pct), ref v2, smoothTime);
        distortion.distortionLevel = Mathf.SmoothDamp(distortion.distortionLevel, Mathf.Lerp(.5f, .95f, pct), ref v3, smoothTime);
    }
}
