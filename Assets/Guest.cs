using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guest : MonoBehaviour
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

    public enum OrderType
    {
        None, Beer, Whisky, Prosecco
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        nextOrder = Time.time + Random.Range(orderDelayMin, orderDelayMax);
    }

    // Update is called once per frame
    void Update()
    {
        if (actualOrder == OrderType.None && Time.time > nextOrder)
        {
            nextOrder = Time.time + Random.Range(orderDelayMin, orderDelayMax);
            actualOrder = OrderType.Beer; // random this
        }

        if (memorizedOrder != OrderType.None)
        {
            memorizedOrderDisplay.SetActive(true);
            actualOrderDisplay.SetActive(false);
        }
        else if (actualOrder != OrderType.None)
        {
            memorizedOrderDisplay.SetActive(false);
            actualOrderDisplay.SetActive(true);
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

    public bool HasOrder()
    {
        return actualOrder != OrderType.None;
    }
}
