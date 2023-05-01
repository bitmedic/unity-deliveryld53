using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public partial class Guest : MonoBehaviour
{
    [Header("Settings")]
    public float guestSpeed;
    public float minWaitTimeAfterOrder = 20;
    public float maxWaitTimeAfterOrder = 45;
    public float minWaitTimeBeforeOrder = 10;
    public float maxWaitTimeBeforeOrder = 30;
    public float chanceToOrderAnotherDrink = 50;

    [Header("References")]
    public GameObject actualOrderDisplay;
    public GameObject memorizedOrderDisplay;
    public GameObject annoyedDisplay;
    public ParticleSystem moneyParticles;
    private Animator walkingAnimation;
    private StepsAudioController walkingAudio;

    [Header("Debug")]
    [SerializeField] private float nextOrder;
    [SerializeField] private Transform target;
    [SerializeField] internal OrderType wantedOrder;
    [SerializeField] internal OrderType orderedOrder;
    [SerializeField] internal GuestState state;

    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;
    private Transform character;
    private Seat seat;
    private CamTriggerZone barZone;
    private Coroutine waitCoroutine;

    public enum GuestState
    {
        Entering, WaitingToOrder, Ordering, HasOrdered, Drinking, Leaving
    }

    private void Awake()
    {
        walkingAnimation = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();
        walkingAudio = GetComponentInChildren<StepsAudioController>();
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

        if (orderedOrder != null)
        {
            memorizedOrderDisplay.SetActive(true);
            memorizedOrderDisplay.GetComponentsInChildren<SpriteRenderer>()[1].sprite = orderedOrder.orderImageSide;
            memorizedOrderDisplay.transform.rotation = Quaternion.identity;
            memorizedOrderDisplay.transform.localScale = Vector3.Lerp(memorizedOrderDisplay.transform.localScale, barZone.active ? 2 * Vector3.one : Vector3.one, .5f * Time.deltaTime);
            actualOrderDisplay.SetActive(false);

        }
        else if (wantedOrder != null)
        {
            memorizedOrderDisplay.SetActive(false);
            actualOrderDisplay.SetActive(true);
            actualOrderDisplay.GetComponentsInChildren<SpriteRenderer>()[1].sprite = wantedOrder.orderImageSide;
            actualOrderDisplay.transform.rotation = Quaternion.identity;
            actualOrderDisplay.transform.localScale = Vector3.Lerp(actualOrderDisplay.transform.localScale, barZone.active ? 2 * Vector3.one : Vector3.one, .5f * Time.deltaTime);
        }
        else
        {
            memorizedOrderDisplay.SetActive(false);
            actualOrderDisplay.SetActive(false);
        }

        if (walkingAnimation != null) walkingAnimation.SetBool("isWalking", state == GuestState.Entering || state == GuestState.Leaving);
        walkingAudio.isWalking = state == GuestState.Entering || state == GuestState.Leaving;
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
        orderedOrder = wantedOrder;
        state = GuestState.HasOrdered;
        return orderedOrder;
    }

    public bool IsReadyToOrder()
    {
        return state == GuestState.Ordering;
    }

    public bool CanOrder()
    {
        return state == GuestState.WaitingToOrder;
    }

    public void Deliver(OrderType order)
    {
        if (order == wantedOrder)
        {
            drinkInHandView.ShowDrink(wantedOrder.enumDrink);
            moneyParticles.Play();
            BarManager.Instance.money += Random.Range(5, 10);
            wantedOrder = null;
            orderedOrder = null;
            state = GuestState.Drinking;
            float drinkTime = Random.Range(15, 45);
            StartCoroutine(FinishDrink(drinkTime));
        }
    }

    private IEnumerator FinishDrink(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        drinkInHandView.ShowDrink(EnumDrink.None);

        if (Random.Range(0, 100) <= chanceToOrderAnotherDrink && ! BarManager.Instance.lastCall)
        {
            state = GuestState.WaitingToOrder;
            StartCoroutine(OrderDrinkIn(Random.Range(minWaitTimeBeforeOrder, maxWaitTimeBeforeOrder)));
        }
        else
        {
            LeaveBar();
        }
    }

    public void DecideOrder()
    {
        if (state == GuestState.WaitingToOrder || state == GuestState.Entering && BarManager.Instance.lastCall)
        {
            Debug.Log(name + " is ordering");

            var possibleOrders = OrderAndDeliver.Instance.possibleOrders;
            wantedOrder = possibleOrders[Random.Range(0, possibleOrders.Count)];
            state = GuestState.Ordering;

            float waitTime = Random.Range(minWaitTimeAfterOrder, maxWaitTimeAfterOrder);
            waitCoroutine = StartCoroutine(LeaveBarIn(waitTime));
        }
    }

    public IEnumerator OrderDrinkIn(float seconds)
    {
        Debug.Log(name + " will order in " + seconds + "seconds");
        yield return new WaitForSeconds(seconds);
        DecideOrder();
    }

    public IEnumerator LeaveBarIn(float seconds)
    {
        Debug.Log(name + " will leave in " + seconds + "seconds");
        yield return new WaitForSeconds(seconds);
        LeaveBar();
    }

    private void LeaveBar()
    {
        if (!BarManager.Instance.closingBar && BarManager.Instance.lastCall)
        {
            return; // do not leave during last call
        }

        Debug.Log(name + " is now leaving");
        state = GuestState.Leaving;

        obstacle.enabled = false;
        agent.enabled = true;
        character.transform.localRotation = Quaternion.Euler(90, 0f, 0);
        agent.destination = BarManager.Instance.LeaveBarLocation.position;
        wantedOrder = null;
        orderedOrder = null;
        seat.guest = null;
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
        character.transform.localRotation = Quaternion.Euler(90, 0, 0);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == GuestState.Entering && collision.gameObject == seat.gameObject)
        {
            agent.enabled = false;
            obstacle.enabled = true;
            transform.position = seat.transform.position;
            transform.rotation = seat.transform.rotation;
            character.transform.localRotation = Quaternion.identity;
            Debug.Log(name + " is ready to order");
            state = GuestState.WaitingToOrder;
            StartCoroutine(OrderDrinkIn(Random.Range(minWaitTimeBeforeOrder, maxWaitTimeBeforeOrder)));
        }

        if (state == GuestState.Leaving && collision.CompareTag("Killzone"))
        {
            Debug.Log(name + ": Good Night!");
            Destroy(gameObject);
        }
    }
}
