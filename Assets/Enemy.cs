using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float enemySpeed;
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = Player.Instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, enemySpeed * Time.deltaTime);
    }
}
