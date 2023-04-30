using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Guest spawnedObject;
    public float spawnDelay;

    void Start()
    {
        InvokeRepeating(nameof(DoSpawn), 0f, spawnDelay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DoSpawn()
    {
        Guest guest = Instantiate(spawnedObject, transform.position, Quaternion.identity);
        guest.FindPlace();
    }
}
