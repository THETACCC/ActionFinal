using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drones : MonoBehaviour
{
    public float initialSpeed = 20f;
    public float decelerationRate = 0.5f;
    public float speedBoost = 10f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * initialSpeed;
    }

    void Update()
    {
        if (rb.velocity.magnitude > 0)
        {
            rb.velocity = rb.velocity.normalized * (rb.velocity.magnitude - (decelerationRate * Time.deltaTime));
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "HandgunBullet")
        {
            rb.velocity = rb.velocity.normalized * (rb.velocity.magnitude + speedBoost);
        }
        if (collision.gameObject.tag == "Dummy")
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "explosion_alone" || collision.gameObject.tag == "explosion")
        {
            Destroy(gameObject);
        }
    }
}
