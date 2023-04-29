using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guest : MonoBehaviour
{

    public float guestSpeed;
    public Transform target;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position = Vector3.MoveTowards(transform.position, target.position, guestSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            rb.velocity = (target.position - transform.position).normalized * guestSpeed; // maybe slower by distance?
        }
    }
}
