using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkPlayer : MonoBehaviour
{

    public float pegel = .5f;

    public static DrunkPlayer Instance { get; private set; }

    public float playerSpeed;
    public float drunknessAmount; // how strong the extend of the sway is
    public float drunknessSpeed; // how much/fast the movement direction changes 

    public bool moveInXZ;


    private Animator walkingAnimation;
    private Transform character;

    void Awake()
    {
        Instance = this;
        walkingAnimation = GetComponentInChildren<Animator>();
        character = transform.Find("Character");
    }

    void Update()
    {
        bool strafing = false; // don't rotate while moving

        if (Input.GetKey(KeyCode.LeftShift))
        {
            strafing = true;
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            pegel += .01f;
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            pegel -= .05f;
        }


        Vector3 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        float offsetX = drunknessAmount * (Mathf.PerlinNoise1D(Time.time * drunknessSpeed) - 0.5f); // Mathf.Sin(Time.time * drunknessSpeed);
        float offsetY = drunknessAmount * (Mathf.PerlinNoise1D(Time.time * drunknessSpeed) - 0.5f); // Mathf.Sin(Time.time * drunknessSpeed);
        Vector3 swerveTarget = input + new Vector3(offsetX, offsetY, 0) * pegel; // input + offset

        swerveTarget = swerveTarget.normalized;

        if (input.magnitude == 0) // only if player not moves at all
        {
            swerveTarget *= 0.1f; // swaying in place
            if (walkingAnimation != null) walkingAnimation.SetBool("isWalking", false);
        }
        else
        {
            //UpdateRotation(transform, Vector3.zero, input);
            if (!strafing) UpdateRotation(Vector3.zero, swerveTarget);
            if (walkingAnimation != null) walkingAnimation.SetBool("isWalking", true);
        }

        if (moveInXZ)
        {
            swerveTarget = new Vector3(swerveTarget.x, 0f, swerveTarget.y);
        }

        transform.position += swerveTarget * playerSpeed * Time.deltaTime;
    }

    void UpdateRotation(Vector3 pos1, Vector3 pos2)
    {
        float angle = Mathf.Atan2(pos1.y - pos2.y, pos1.x - pos2.x) * 180 / Mathf.PI;
        angle += 90;
        character.localRotation = Quaternion.Euler(0, 0, angle);
    }

}
