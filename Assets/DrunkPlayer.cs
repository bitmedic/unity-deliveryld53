using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkPlayer : MonoBehaviour
{

    public static DrunkPlayer Instance { get; private set; }

    public float playerSpeed;
    public float drunknessAmount; // how strong the extend of the sway is
    public float drunknessSpeed; // how much/fast the movement direction changes 

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        Vector3 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        float offsetX = drunknessAmount * (Mathf.PerlinNoise1D(Time.time * drunknessSpeed) - 0.5f); // Mathf.Sin(Time.time * drunknessSpeed);
        float offsetY = drunknessAmount * (Mathf.PerlinNoise1D(Time.time * drunknessSpeed) - 0.5f); // Mathf.Sin(Time.time * drunknessSpeed);
        Vector3 swerveTarget = input + new Vector3(offsetX, offsetY, 0); // input + offset

        swerveTarget = swerveTarget.normalized;
        
        if (input.magnitude == 0) // only if player not moves at all
        {
            swerveTarget *= 0.1f; // swaying in place
        }


        transform.position += swerveTarget * playerSpeed * Time.deltaTime;
    }
}
