using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Guest : MonoBehaviour
{
    [Header("Settings")]
    public float guestSpeed;
    public float orderDelayMin = 15;
    public float orderDelayMax = 60;

    [Header("References")]
    public GameObject actualOrderDisplay;
    public GameObject memorizedOrderDisplay;

    [Header("Debug")]
    public float nextOrder;
    public Transform target;
    public OrderType actualOrder;
    public OrderType memorizedOrder;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        nextOrder = Time.time + Random.Range(orderDelayMin, orderDelayMax);
    }

    // Update is called once per frame
    void Update()
    {

        if (memorizedOrder != null)
        {
            memorizedOrderDisplay.SetActive(true);
            memorizedOrderDisplay.GetComponentsInChildren<SpriteRenderer>()[1].sprite = memorizedOrder.orderImage;
            actualOrderDisplay.SetActive(false);
        }
        else if (actualOrder != null)
        {
            memorizedOrderDisplay.SetActive(false);
            actualOrderDisplay.SetActive(true);
            actualOrderDisplay.GetComponentsInChildren<SpriteRenderer>()[1].sprite = actualOrder.orderImage;
        }
        else
        {
            memorizedOrderDisplay.SetActive(false);
            actualOrderDisplay.SetActive(false);
        }

    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            rb.velocity = (target.position - transform.position).normalized * guestSpeed; // maybe slower by distance?
        }
    }

    public OrderType TakeOrder(DrunkPlayer player)
    {
        memorizedOrder = actualOrder; // give a chance to misremember;
        return memorizedOrder;
    }

    public bool HasNewOrder()
    {
        return actualOrder != null && memorizedOrder == null;
    }

    public bool Deliver(OrderType order)
    {
        if (order == actualOrder)
        {
            nextOrder = Time.time + Random.Range(orderDelayMin, orderDelayMax);
            actualOrder = null;
            memorizedOrder = null;
            return true;
        }
        else
        {
            return false; // what else should happen in case the wrong order was memorized? 
        }
    }

    public void DecideOrder()
    {
        if (!HasNewOrder())
        {
            var possibleOrders = OrderAndDeliver.Instance.possibleOrders;
            actualOrder = possibleOrders[Random.Range(0, possibleOrders.Count)];
        }
    }
}
