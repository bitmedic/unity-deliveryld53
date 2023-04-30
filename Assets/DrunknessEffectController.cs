using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DrunknessEffectController : MonoBehaviour
{
    public Volume volume;

    public float VignetteDefaultIntensity;
    public float VignetteDefaultSmoothness;

    private LensDistortion lensDistortion = null;
    private Vignette vignette = null;


    public float pegel;
    float maxLensDist;

    float timerToBlink;

    private void Start()
    {
        volume.profile.TryGet<LensDistortion>(out lensDistortion);
        volume.profile.TryGet<Vignette>(out vignette);
        timerToBlink = Random.Range(10, 30) * (1 / (Mathf.Max(1, pegel)));
    }

    public void SetNewPegel(float pegel)
    {
        this.pegel = pegel;
        timerToBlink = Random.Range(10, 30) * (1 / pegel);
    }

    private void Update()
    {
        this.maxLensDist = (Mathf.Min(5, pegel) - 1) / 5;

        float sinValue = Mathf.Sin(Time.timeSinceLevelLoad);
        float lerpValue = Mathf.InverseLerp(-1, 1, sinValue);
        float lensDistortionValue = Mathf.Lerp(-1, 1, lerpValue) * maxLensDist;
                
        lensDistortion.intensity.value = lensDistortionValue;

        if (pegel >= 1.5f)
        {
            if (timerToBlink < -1)
            {
                timerToBlink = Random.Range(10, 30) * (1 / pegel);
            }
            else if (timerToBlink <= -0.5)
            {
                vignette.smoothness.value = Mathf.Lerp(1, VignetteDefaultSmoothness, -2 * (timerToBlink + 0.5f));
                vignette.intensity.value = Mathf.Lerp(1, VignetteDefaultIntensity, -2 * (timerToBlink + 0.5f));
            }
            else if (timerToBlink <= 0)
            {
                vignette.smoothness.value = Mathf.Lerp(VignetteDefaultSmoothness, 1, -2 * timerToBlink);
                vignette.intensity.value = Mathf.Lerp(VignetteDefaultIntensity, 1, -2 * timerToBlink);
            }
            timerToBlink -= Time.deltaTime;
        }
        else
        {
            vignette.smoothness.value = VignetteDefaultSmoothness / 2;
            vignette.intensity.value = VignetteDefaultIntensity / 2;
        }
    }
}
