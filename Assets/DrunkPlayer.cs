using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkPlayer : MonoBehaviour
{

    public float pegel = 0f;

    public static DrunkPlayer Instance { get; private set; }

    public float playerSpeed;
    public float playerSpeedBase = 3;
    public float playerSpeedPegelMulti = 2;
    public float drunknessAmount; // how strong the extend of the sway is
    public float drunknessSpeed; // how much/fast the movement direction changes 

    private PlayerArmTrayController playerTray;
    public List<AudioClip> traySounds;
    public AudioSource audioSource;
    private float timerSound = 0;

    private Animator walkingAnimation;
    private Transform character;
    private OrderAndDeliver orders;
    private StepsAudioController walkingAudio;
    public SpriteRenderer starRing;

    private bool controlsEnabled;
    private bool isWalking;

    void Awake()
    {
        Instance = this;
        walkingAnimation = GetComponentInChildren<Animator>();
        orders = GetComponent<OrderAndDeliver>();
        playerTray = GetComponent<PlayerArmTrayController>();
        walkingAudio = GetComponentInChildren<StepsAudioController>();
        character = transform.Find("Character");
        controlsEnabled = true;
        playerSpeed = playerSpeedBase;
        isWalking = false;
    }

    void Update()
    {
        bool strafing = false; // don't rotate while moving
        Vector3 input = Vector2.zero;

        if (controlsEnabled)
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (orders.StealRandomDrink())
                {
                    pegel += .1f;
                }
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                strafing = true;
            }
            input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            playerSpeed = playerSpeedBase + pegel * playerSpeedPegelMulti * playerSpeedBase;
            float offsetX = drunknessAmount * (Mathf.PerlinNoise1D(Time.time * drunknessSpeed) - 0.5f); // Mathf.Sin(Time.time * drunknessSpeed);
            float offsetY = drunknessAmount * (Mathf.PerlinNoise1D(Time.time * drunknessSpeed) - 0.5f); // Mathf.Sin(Time.time * drunknessSpeed);
            Vector3 swerveTarget = input + new Vector3(offsetX, offsetY, 0) * pegel; // input + offset

            swerveTarget = swerveTarget.normalized;

            if (input.magnitude == 0) // only if player not moves at all
            {
                if (pegel <= 0.6f)
                {
                    swerveTarget *= 0.01f; // swaying in place
                }
                else if (pegel <= 1.2f)
                {
                    swerveTarget *= 0.05f; // swaying in place
                }
                else
                {
                    swerveTarget *= 0.1f; // swaying in place
                }

                isWalking = false;
                if (walkingAnimation != null) walkingAnimation.SetBool("isWalking", false);
            }
            else
            {
                //UpdateRotation(transform, Vector3.zero, input);
                if (!strafing) UpdateRotation(Vector3.zero, swerveTarget);
                isWalking = true;
                if (walkingAnimation != null) walkingAnimation.SetBool("isWalking", true);
            }

            transform.position += swerveTarget * playerSpeed * Time.deltaTime;
        }

        if (timerSound <= 0)
        {
            if (isWalking && playerTray.carriedItems.Count > 0)
            {
                int randomSound = Random.Range(0, traySounds.Count);
                audioSource.PlayOneShot(traySounds[randomSound]);
            }

            timerSound = Random.Range(1f, 10f);
        }
        timerSound -= Time.deltaTime;

        walkingAudio.isWalking = isWalking;

        starRing.color = new Color(1f, 1f, 1f, pegel);
        starRing.GetComponent<Animator>().speed = pegel;
        starRing.transform.localScale = Vector3.one * pegel;
    }
    

    void UpdateRotation(Vector3 pos1, Vector3 pos2)
    {
        float angle = Mathf.Atan2(pos1.y - pos2.y, pos1.x - pos2.x) * 180 / Mathf.PI;
        angle += 90;
        character.localRotation = Quaternion.Euler(0, 0, angle);
    }

    public void GetDrunk(float amount)
    {
        pegel = Mathf.Clamp01(pegel + amount);
    }

    public void EnableControls(bool enable)
    {
        controlsEnabled = enable;
    }

    public void LookAt(Vector3 target)
    {
        UpdateRotation(Vector3.zero, target);
    }
}
