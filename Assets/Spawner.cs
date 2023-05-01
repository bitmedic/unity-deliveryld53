using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Guest spawnedObject;
    public float minSpawnDelay;

    void Start()
    {
        StartCoroutine(DoSpawn());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator DoSpawn()
    {
        Debug.Log("Waiting " + minSpawnDelay + " seconds for first guest");
        yield return new WaitForSeconds(minSpawnDelay);
        while (!BarManager.Instance.lastCall) // no spawns after last call
        {
            Guest guest = Instantiate(spawnedObject, transform.position, Quaternion.identity);
            guest.FindPlace();
            float delay = minSpawnDelay / BarManager.Instance.GetCurrentSpawnRate();
            Debug.Log("Next guest in " + delay + " seconds.");
            yield return new WaitForSeconds(delay);
        }
    }
}
