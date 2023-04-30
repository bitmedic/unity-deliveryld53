using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmTrayController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform ArmRightEmpty;
    public Transform ArmRightTray;
    public List<OrderType> carriedItems;

    // Update is called once per frame
    void Update()
    {
        if (carriedItems.Count > 0)
        {
            ArmRightEmpty.gameObject.SetActive(false);
            ArmRightTray.gameObject.SetActive(true);

            var orderRenderers = ArmRightTray.GetComponentsInChildren<SpriteRenderer>(true);
            for (int i = 1; i < orderRenderers.Length; i++)
            {
                orderRenderers[i].gameObject.SetActive(i - 1 < carriedItems.Count);
            }

            for (int i = 0; i < carriedItems.Count; i++)
            {
                orderRenderers[i + 1].sprite = carriedItems[i].orderImage;
            }
        }
        else
        {
            ArmRightEmpty.gameObject.SetActive(true);
            ArmRightTray.gameObject.SetActive(false);
        }
    }
}
