using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Guest spawnedObject;
    public float minSpawnDelay;

    void Start()
    {
        SpawnGuest();
        SpawnGuest();
        SpawnGuest();
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
            SpawnGuest();
            float delay = minSpawnDelay / BarManager.Instance.GetCurrentSpawnRate();
            Debug.Log("Next guest in " + delay + " seconds.");
            yield return new WaitForSeconds(delay);
        }
    }

    private void SpawnGuest()
    {
        Guest guest = Instantiate(spawnedObject, transform.position, Quaternion.identity);
        Seat[] linkedFreeSeats = guest.FindPlace(null);
        if (linkedFreeSeats != null)
        {
            Debug.Log("linked free seats: " + linkedFreeSeats);
            foreach (var s in linkedFreeSeats)
            {
                Guest g = Instantiate(spawnedObject, transform.position, Quaternion.identity);
                g.FindPlace(s);
            }
        }
    }
}
