using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Guest spawnedObject;
    public float spawnDelayInitial;
    public float spawnDelayBetween;
    public float secondsUntilMaxSpawnSpeed = 5 * 60;
    public float maxSpawnSpeedMultiplier;
    public float currentSpawnSpeedMultiplier;


    private float startTime;

    void Start()
    {
        StartCoroutine(DoSpawn());
        currentSpawnSpeedMultiplier = 1;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float elapsedTime = (Time.time - startTime);
        float elapsedTimePercent = Mathf.InverseLerp(0, secondsUntilMaxSpawnSpeed, elapsedTime);
        currentSpawnSpeedMultiplier = Mathf.Lerp(1, maxSpawnSpeedMultiplier, elapsedTimePercent);
    }

    IEnumerator DoSpawn()
    {
        yield return new WaitForSeconds(spawnDelayInitial);
        while (true)
        {
            Guest guest = Instantiate(spawnedObject, transform.position, Quaternion.identity);
            guest.FindPlace();
            float seconds = spawnDelayBetween / currentSpawnSpeedMultiplier;
            Debug.Log("Next guest in " + seconds + " seconds.");
            yield return new WaitForSeconds(seconds);
        }
    }
}
