using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BarManager : MonoBehaviour
{
    public static BarManager Instance { get; private set; }
    public Transform LeaveBarLocation;
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(NewOrder), 5f, 10f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void NewOrder()
    {
        Guest[] guests = (Guest[])FindObjectsOfType(typeof(Guest));

        var waitingGuests = guests.Where(g => { return g.state == Guest.GuestState.WaitingToOrder; }).ToList();

        if (waitingGuests.Count > 0)
        {
            var randomGuest = waitingGuests[Random.Range(0, waitingGuests.Count)];
            if (randomGuest.CanOrder())
            {
                Debug.Log(randomGuest.name + " is ordering");
                randomGuest.DecideOrder();
            }
            else
            {
                Debug.Log(randomGuest.name + " is not ready " + randomGuest.state);
            }
        }
    }
}
