using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerDraftAnimationController : MonoBehaviour
{
    public Transform LiquidMask;
    public Transform Head;

    public void SetPercentage(float percent)
    {
        LiquidMask.localPosition = new Vector3(0, (3.3f * percent/100), 0f);

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
    }
}
