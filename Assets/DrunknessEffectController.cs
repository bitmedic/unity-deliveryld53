using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DrunknessEffectController : MonoBehaviour
{
    public DrunkPlayer player;
    public Volume volume;

    public float VignetteDefaultIntensity;
    public float VignetteDefaultSmoothness;
    public float MaxLensDistortion;

    private LensDistortion lensDistortion = null;
    private Vignette vignette = null;


    public float pegel;
    float lensDist;

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
        timerToBlink = Random.Range(10, 30) * (0.3f / pegel);
    }

    private void Update()
    {
        if (this.pegel != player.pegel)
        {
            timerToBlink = Random.Range(10, 30) * (0.3f / player.pegel);
        }

        this.pegel = player.pegel;

        if (pegel <= 0.3f)
        {
            this.lensDist = 0;
        }
        if (pegel <= 1)
        {
            this.lensDist = (pegel / 2.5f) * MaxLensDistortion;
        }
        else
        {
            this.lensDist = (Mathf.Min(2, pegel) / 2) * MaxLensDistortion;
        }

        float sinValue = Mathf.Sin(Time.timeSinceLevelLoad);
        float lerpValue = Mathf.InverseLerp(-1, 1, sinValue);
        float lensDistortionValue = Mathf.Lerp(-1, 1, lerpValue) * lensDist;
                
        lensDistortion.intensity.value = lensDistortionValue;

        if (pegel >= 0.3f)
        {
            vignette.smoothness.value = VignetteDefaultSmoothness + (VignetteDefaultSmoothness * pegel / 4);
            vignette.intensity.value = VignetteDefaultIntensity + (VignetteDefaultSmoothness * pegel / 4);
            //if (timerToBlink < -0.3f)
            //{
            //    timerToBlink = Random.Range(10, 30) * (0.3f / pegel);
            //}
            //else if (timerToBlink <= -0.1f)
            //{
            //    vignette.smoothness.value = Mathf.Lerp(1, VignetteDefaultSmoothness, -5 * (timerToBlink + 0.1f));
            //    vignette.intensity.value = Mathf.Lerp(1, VignetteDefaultIntensity, -5 * (timerToBlink + 0.1f));
            //}
            //else if (timerToBlink <= 0)
            //{
            //    vignette.smoothness.value = Mathf.Lerp(VignetteDefaultSmoothness, 1, -10 * timerToBlink);
            //    vignette.intensity.value = Mathf.Lerp(VignetteDefaultIntensity, 1, -10 * timerToBlink);
            //}
            //timerToBlink -= Time.deltaTime;
        }
        else
        {
            vignette.smoothness.value = VignetteDefaultSmoothness / 2;
            vignette.intensity.value = VignetteDefaultIntensity / 2;
        }
    }
}
