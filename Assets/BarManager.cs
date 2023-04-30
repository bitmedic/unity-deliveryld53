using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarManager : MonoBehaviour
{
    public static BarManager Instance { get; private set; }

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

        var randomGuest = guests[Random.Range(0, guests.Length)];
        if (!randomGuest.HasNewOrder())
        {
            randomGuest.DecideOrder();
        }

    }
}
