using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmTrayController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform ArmRightEmpty;
    public Transform ArmRightTray;
    public bool carriesTray;

    // Update is called once per frame
    void Update()
    {
        if (carriesTray)
        {
            ArmRightEmpty.gameObject.SetActive(false);
            ArmRightTray.gameObject.SetActive(true);
        }
        else
        {
            ArmRightEmpty.gameObject.SetActive(true);
            ArmRightTray.gameObject.SetActive(false);
        }
    }
}
