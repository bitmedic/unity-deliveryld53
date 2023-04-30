using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

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

    private NavMeshAgent agent;
    private Transform character;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        character = transform.Find("Character");
    }

    // Start is called before the first frame update
    void Start()
    {
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

    public void FindPlace()
    {
        var seats = GameObject.FindObjectsOfType<Seat>();
        var emptySeats = seats.Where(s => { return s.guest == null; });
        var seat = emptySeats.ToArray()[Random.Range(0, emptySeats.Count())];
        character.transform.localRotation = Quaternion.Euler(90, 0, 0);
        agent.SetDestination(seat.transform.position);
    }
}
