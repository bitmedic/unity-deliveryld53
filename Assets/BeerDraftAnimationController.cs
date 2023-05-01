using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerDraftAnimationController : MonoBehaviour
{
    public Transform LiquidMask;
    public Transform Head;

    public List<AudioClip> soundsPouring;
    private AudioSource audio;

    private void Start()
    {
        audio = GetComponentInChildren<AudioSource>();
    }

    public void SetPercentage(float percent)
    {
        LiquidMask.localPosition = new Vector3(0, (3.3f * percent/80), 0f);

        if (percent >= 80)
        {
            Head.gameObject.SetActive(true);
        }
        else
        {
            Head.gameObject.SetActive(false);
        }
    }

    public void Visible(bool value)
    {
        this.gameObject.SetActive(value);

        if (audio == null)
        {
            audio = GetComponentInChildren<AudioSource>();
        }

        if (value)
        {
            audio.PlayOneShot(soundsPouring[Random.Range(0, soundsPouring.Count)]);
        }
    }
}
