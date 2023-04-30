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
    public ParticleSystem moneyParticles;

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

    private DrinkInHandView drinkInHandView;

    // Start is called before the first frame update
    void Start()
    {
        nextOrder = Time.time + Random.Range(orderDelayMin, orderDelayMax);

        drinkInHandView = GetComponentInChildren<DrinkInHandView>();
    }

    // Update is called once per frame
    void Update()
    {

        if (memorizedOrder != null)
        {
            memorizedOrderDisplay.SetActive(true);
            memorizedOrderDisplay.GetComponentsInChildren<SpriteRenderer>()[1].sprite = memorizedOrder.orderImageSide;
            actualOrderDisplay.SetActive(false);
        }
        else if (actualOrder != null)
        {
            memorizedOrderDisplay.SetActive(false);
            actualOrderDisplay.SetActive(true);
            actualOrderDisplay.GetComponentsInChildren<SpriteRenderer>()[1].sprite = actualOrder.orderImageSide;
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
            drinkInHandView.ShowDrink(actualOrder.enumDrink);
            moneyParticles.Play();
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
            drinkInHandView.ShowDrink(EnumDrink.None); // clear previous drink in handw

            var possibleOrders = OrderAndDeliver.Instance.possibleOrders;
            actualOrder = possibleOrders[Random.Range(0, possibleOrders.Count)];
        }
    }

    public void FindPlace()
    {
        var seats = GameObject.FindObjectsOfType<Seat>();

        var emptySeats = seats.Where(s => { return s.guest == null; });
        if (emptySeats.Count() == 0)
        {
            Debug.Log("No more free seats.");
            Destroy(this.gameObject);
            return;
        }

        var seat = emptySeats.ToArray()[Random.Range(0, emptySeats.Count())];
        
        seat.guest = this; // reserve
        agent.SetDestination(seat.transform.position);
        character.transform.localRotation = Quaternion.Euler(90, 0, 0);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TRIGGER " + collision.name);
    }
}
