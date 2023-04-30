using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderAndDeliver : MonoBehaviour
{

    public List<Guest.OrderType> rememberedOrders;
    public GameObject rememberedOrdersDisplay;

    public List<Guest.OrderType> carryingOrders;
    public GameObject carryingOrdersDisplay;

    // Start is called before the first frame update
    void Start()
    {
        rememberedOrders = new List<Guest.OrderType>();
        GetComponent<PlayerArmTrayController>().carriedItems = carryingOrders;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Guest")
        {
            Guest guest = collision.collider.GetComponent<Guest>();

            if (guest.HasNewOrder() && rememberedOrders.Count < 5 && carryingOrders.Count == 0) // somehow display full brain and not taking an order
            {
                Guest.OrderType order = guest.TakeOrder(GetComponent<DrunkPlayer>());
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

            if (carryingOrders.Count > 0)
            {
                if (guest.actualOrder != Guest.OrderType.None && guest.memorizedOrder != Guest.OrderType.None && carryingOrders.Contains(guest.memorizedOrder))
                {
                    carryingOrders.Remove(guest.memorizedOrder);
                    guest.Deliver(guest.memorizedOrder);
                    if (carryingOrders.Count == 1)
                    {
                        Debug.Log("The last is for the waiter.");
                        DrunkPlayer.Instance.GetDrunk(.1f);
                        carryingOrders.Clear();
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

                // on extra
                carryingOrders.Add(Guest.OrderType.Beer);

                rememberedOrders.Clear();
                rememberedOrdersDisplay.SetActive(false);
            }
        }
    }
}
