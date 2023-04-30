using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public partial class Guest : MonoBehaviour
{
    [Header("Settings")]
    public float guestSpeed;
    public float minOrderWaitTime = 15;
    public float maxOrderWaitTime = 60;

    [Header("References")]
    public GameObject actualOrderDisplay;
    public GameObject memorizedOrderDisplay;
    public GameObject annoyedDisplay;
    public ParticleSystem moneyParticles;
    private Animator walkingAnimation;

    [Header("Debug")]
    [SerializeField] private float nextOrder;
    [SerializeField] private Transform target;
    [SerializeField] internal OrderType actualOrder;
    [SerializeField] internal OrderType memorizedOrder;
    [SerializeField] internal GuestState state;

    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;
    private Transform character;
    private Seat seat;
    private CamTriggerZone barZone;
    private Coroutine waitCoroutine;
    private bool readyToOrder;

    public enum GuestState
    {
        Entering, Waiting, Ordering, Drinking, Leaving
    }

    private void Awake()
    {
        walkingAnimation = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();
        character = transform.Find("Character");
        barZone = GameObject.FindAnyObjectByType<CamTriggerZone>();
        annoyedDisplay.SetActive(false);
        state = GuestState.Entering;
    }

    private DrinkInHandView drinkInHandView;

    // Start is called before the first frame update
    void Start()
    {
        name = name + Time.time;
        drinkInHandView = GetComponentInChildren<DrinkInHandView>();
    }

    // Update is called once per frame
    void Update()
    {

        if (memorizedOrder != null)
        {
            memorizedOrderDisplay.SetActive(true);
            memorizedOrderDisplay.GetComponentsInChildren<SpriteRenderer>()[1].sprite = memorizedOrder.orderImageSide;
            memorizedOrderDisplay.transform.rotation = Quaternion.identity;
            memorizedOrderDisplay.transform.localScale = Vector3.Lerp(memorizedOrderDisplay.transform.localScale, barZone.active ? 2 * Vector3.one : Vector3.one, .5f * Time.deltaTime);
            actualOrderDisplay.SetActive(false);

        }
        else if (actualOrder != null)
        {
            memorizedOrderDisplay.SetActive(false);
            actualOrderDisplay.SetActive(true);
            actualOrderDisplay.GetComponentsInChildren<SpriteRenderer>()[1].sprite = actualOrder.orderImageSide;
            actualOrderDisplay.transform.rotation = Quaternion.identity;
            actualOrderDisplay.transform.localScale = Vector3.Lerp(actualOrderDisplay.transform.localScale, barZone.active ? 2 * Vector3.one : Vector3.one, .5f * Time.deltaTime);
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
        if (waitCoroutine != null)
        {
            StopCoroutine(waitCoroutine); // they dont leave after taking the order
        }
        memorizedOrder = actualOrder; // give a chance to misremember;
        return memorizedOrder;
    }

    public bool IsReadyToOrder()
    {
        return readyToOrder && actualOrder == null;
    }

    public bool Deliver(OrderType order)
    {
        if (order == actualOrder)
        {
            drinkInHandView.ShowDrink(actualOrder.enumDrink);
            moneyParticles.Play();
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
        if (IsReadyToOrder())
        {
            drinkInHandView.ShowDrink(EnumDrink.None); // clear previous drink in hand

            var possibleOrders = OrderAndDeliver.Instance.possibleOrders;
            actualOrder = possibleOrders[Random.Range(0, possibleOrders.Count)];
            state = GuestState.Ordering;

            float waitTime = Random.Range(minOrderWaitTime, maxOrderWaitTime);
            waitCoroutine = StartCoroutine(LeaveBarIn(waitTime));
        }
    }

    public IEnumerator LeaveBarIn(float seconds)
    {
        Debug.Log(name + " will leave in " + seconds + "seconds");
        yield return new WaitForSeconds(seconds);
        LeaveBar();
    }

    private void LeaveBar()
    {
        Debug.Log(name + " is now leaving");
        state = GuestState.Leaving;

        obstacle.enabled = false;
        agent.enabled = true;
        character.transform.localRotation = Quaternion.Euler(90, 0f, 0);
        agent.destination = BarManager.Instance.LeaveBarLocation.position;
        readyToOrder = false;
        actualOrder = null;
        seat.guest = null;

        annoyedDisplay.SetActive(true);
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

        seat = emptySeats.ToArray()[Random.Range(0, emptySeats.Count())];

        seat.guest = this; // reserve
        agent.SetDestination(seat.transform.position);
        if (walkingAnimation != null) walkingAnimation.SetBool("isWalking", true);
        character.transform.localRotation = Quaternion.Euler(90, 0, 0);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == GuestState.Entering && collision.gameObject == seat.gameObject)
        {
            if (walkingAnimation != null) walkingAnimation.SetBool("isWalking", false);
            agent.enabled = false;
            obstacle.enabled = true;
            transform.position = seat.transform.position;
            transform.rotation = seat.transform.rotation;
            character.transform.localRotation = Quaternion.identity;
            readyToOrder = true;
            Debug.Log(name + " is ready to order");
            state = GuestState.Waiting;
        }

        if (state == GuestState.Leaving && collision.CompareTag("Killzone"))
        {
            Debug.Log(name + ": Good Night!");
            Destroy(gameObject);
        }
    }
}
