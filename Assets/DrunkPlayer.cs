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

    public List<Guest.OrderType> rememberedOrders;
    public GameObject rememberedOrdersDisplay;

    public List<Guest.OrderType> carryingOrders;
    public GameObject carryingOrdersDisplay;
    private Animator walkingAnimation;
    private Transform character;

    void Awake()
    {
        Instance = this;
        rememberedOrders = new List<Guest.OrderType>();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Guest")
        {
            Guest guest = collision.collider.GetComponent<Guest>();
            if (guest.HasNewOrder() && rememberedOrders.Count < 5) // somehow display full brain and not taking an order
            {
                Guest.OrderType order = guest.TakeOrder(this);
                Debug.Log("Touched guest wants: " + guest.actualOrder + ". I will bring " + order);
                rememberedOrders.Add(order);
                if (rememberedOrders.Count > 0)
                {
                    rememberedOrdersDisplay.SetActive(true);
                    var orderRenderers = rememberedOrdersDisplay.GetComponentsInChildren<SpriteRenderer>(true);
                    for (int i = 1; i < orderRenderers.Length; i++)
                    {
                        orderRenderers[i].gameObject.SetActive(i - 1 < rememberedOrders.Count);
                        // TODO also update displayed order
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DraftZone")
        {
            if (carryingOrders.Count == 0 && rememberedOrders.Count > 0)
            {
                foreach (var order in rememberedOrders)
                {
                    carryingOrders.Add(order);
                }

                rememberedOrders.Clear();
                rememberedOrdersDisplay.SetActive(false);
                carryingOrdersDisplay.SetActive(true);

                var orderRenderers = carryingOrdersDisplay.GetComponentsInChildren<SpriteRenderer>(true);
                for (int i = 1; i < orderRenderers.Length; i++)
                {
                    orderRenderers[i].gameObject.SetActive(i - 1 < carryingOrders.Count);
                }
            }
        }
    }
}
