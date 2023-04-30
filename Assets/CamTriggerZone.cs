using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamTriggerZone : MonoBehaviour
{

    public CinemachineVirtualCamera InsideCamera;
    public bool active;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InsideCamera.enabled = true;
            active = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (InsideCamera != null)
            {
                InsideCamera.enabled = false;
                active = false;
            }
        }
    }
}
