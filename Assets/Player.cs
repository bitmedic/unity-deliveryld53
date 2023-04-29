using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player Instance { get; private set; }

    public float playerSpeed;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        Vector3 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        transform.position += input * playerSpeed * Time.deltaTime;
    }
}
