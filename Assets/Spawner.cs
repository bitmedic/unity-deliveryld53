using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnedObject;
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
        Instantiate(spawnedObject, transform.position, Quaternion.identity);
    }
}
