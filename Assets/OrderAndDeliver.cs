using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderAndDeliver : MonoBehaviour
{

    public List<OrderType> rememberedOrders;
    public GameObject rememberedOrdersDisplay;

    public List<OrderType> carryingOrders;
    public GameObject carryingOrdersDisplay;

    public GameObject draftTimerDisplay;
    public BeerDraftAnimationController beerDraftAnimationController;
    public List<OrderType> possibleOrders; 

    public static OrderAndDeliver Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        rememberedOrders = new List<OrderType>();
        rememberedOrdersDisplay.SetActive(false);
        GetComponent<PlayerArmTrayController>().carriedItems = carryingOrders;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(name + " :: " + collision.collider.name);

        if (collision.collider.tag == "Guest")
        {
            Guest guest = collision.collider.GetComponent<Guest>();

            if (guest.IsReadyToOrder() && rememberedOrders.Count < 5 && carryingOrders.Count == 0)
            {
                OrderType order = guest.TakeOrder(GetComponent<DrunkPlayer>());
                Debug.Log("Touched guest wants: " + guest.wantedOrder + ". I will bring " + order);
                rememberedOrders.Add(order);
                if (rememberedOrders.Count > 0)
                {
                    rememberedOrdersDisplay.SetActive(true);
                    var orderRenderers = rememberedOrdersDisplay.GetComponentsInChildren<SpriteRenderer>(true);

                    for (int i = 1; i < orderRenderers.Length; i++)
                    {
                        orderRenderers[i].gameObject.SetActive(i - 1 < rememberedOrders.Count);
                    }

                    for (int i = 0; i < rememberedOrders.Count; i++)
                    {
                        orderRenderers[i + 1].sprite = rememberedOrders[i].orderImageSide;
                    }
                }
            }

            if (carryingOrders.Count > 0)
            {
                if (guest.wantedOrder != null && guest.orderedOrder != null && carryingOrders.Contains(guest.orderedOrder))
                {
                    carryingOrders.Remove(guest.orderedOrder);
                    guest.Deliver(guest.orderedOrder);
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
                var player = DrunkPlayer.Instance;
                player.transform.position = collision.transform.position;
                player.LookAt(Vector2.down);
                StartCoroutine(nameof(DraftDrinks));
            }
        }
    }

    private IEnumerator DraftDrinks()
    {
        DrunkPlayer.Instance.EnableControls(false);


        foreach (var order in rememberedOrders)
        {
            yield return AnimateDraftTimer();
            carryingOrders.Add(order);
        }

        rememberedOrders.Clear();
        rememberedOrdersDisplay.SetActive(false);

        DrunkPlayer.Instance.EnableControls(true);
    }

    private IEnumerator AnimateDraftTimer()
    {
        draftTimerDisplay.SetActive(false);
        beerDraftAnimationController.Visible(true);
        float waitTime = 0;
        float draftTime = 1f;
        
        SpriteRenderer renderer = draftTimerDisplay.GetComponent<SpriteRenderer>();
        renderer.material.SetFloat("_Arc2", 360f);
        
        while (waitTime < draftTime)
        {
            yield return new WaitForSeconds(.1f);
            waitTime += .1f;
            renderer.material.SetFloat("_Arc2", 360f / draftTime * (draftTime - waitTime));

            beerDraftAnimationController.SetPercentage((100 * waitTime) / draftTime);
        }
        draftTimerDisplay.SetActive(false);
        beerDraftAnimationController.Visible(false);
    }
}
