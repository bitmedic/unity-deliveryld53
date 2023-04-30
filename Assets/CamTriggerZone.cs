using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamTriggerZone : MonoBehaviour
{

    public CinemachineVirtualCamera InsideCamera;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enter");
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Enter2");
            InsideCamera.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exit");
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Exit2");
            InsideCamera.enabled = false;
        }
    }
}
