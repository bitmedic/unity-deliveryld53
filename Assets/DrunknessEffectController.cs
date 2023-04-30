using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DrunknessEffectController : MonoBehaviour
{
    public Volume volume;
    private LensDistortion lensDistortion = null;

    public float pegel;
    float maxLensDist;

    private void Start()
    {
        volume.profile.TryGet<LensDistortion>(out lensDistortion);
    }

    public void SetNewPegel(float pegel)
    {
        this.pegel = pegel;
    }

    private void Update()
    {
        this.maxLensDist = Mathf.Min(4, pegel) / 5;

        float sinValue = Mathf.Sin(Time.timeSinceLevelLoad);
        float lerpValue = Mathf.InverseLerp(-1, 1, sinValue);
        float lensDistortionValue = Mathf.Lerp(-1, 1, lerpValue) * maxLensDist;

        Debug.Log(lerpValue);
        
        lensDistortion.intensity.value = lensDistortionValue;
    }
}
