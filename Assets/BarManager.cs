using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BarManager : MonoBehaviour
{
    public static BarManager Instance { get; private set; }
    public Transform LeaveBarLocation;
    public float barCloseTime;
    public float lastCallTime;
    public float barOpenSince;
    private float barOpenAt;
    public bool lastCall;
    public AnimationCurve guestSpawnOverTime;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(NewOrder), 5f, 10f);
        barOpenAt = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        barOpenSince = Time.time - barOpenAt;


        if (!lastCall && barOpenSince > lastCallTime)
        {
            lastCall = true;
            var waitingGuests = FindWaitingGuests();
            foreach (var guest in waitingGuests)
            {
                guest.DecideOrder();
            }
        }

        if (barOpenSince > barCloseTime)
        {
            Debug.Log("Bar closing now");
            this.enabled = false;
            CancelInvoke();

            foreach (var guest in FindGuests())
            {
                StartCoroutine(guest.LeaveBarIn(Random.Range(1f, 5f)));
            }

        }
    }

    private List<Guest> FindWaitingGuests()
    {
        Guest[] guests = (Guest[])FindObjectsOfType(typeof(Guest));

        var waitingGuests = guests.Where(g => { return g.state == Guest.GuestState.WaitingToOrder; }).ToList();

        return waitingGuests;
    }

    private List<Guest> FindGuests()
    {
        Guest[] guests = (Guest[])FindObjectsOfType(typeof(Guest));
        return guests.ToList();
    }

    private void NewOrder()
    {
        var waitingGuests = FindWaitingGuests();

        if (waitingGuests.Count > 0)
        {
            var randomGuest = waitingGuests[Random.Range(0, waitingGuests.Count)];
            randomGuest.DecideOrder();
        }
    }

    public Guest FindGuestWithOpenOrder()
    {
        Guest[] guests = (Guest[])FindObjectsOfType(typeof(Guest));

        var waitingGuests = guests.Where(g => { return g.state == Guest.GuestState.HasOrdered; }).ToList();

        if (waitingGuests.Count > 0)
        {
            return waitingGuests[Random.Range(0, waitingGuests.Count)];
        }

        return null;
    }

    public float GetTimePercentage()
    {
        return barOpenSince / barCloseTime;
    }

    public float GetCurrentSpawnRate()
    {
        return guestSpawnOverTime.Evaluate(GetTimePercentage());
    }
}
