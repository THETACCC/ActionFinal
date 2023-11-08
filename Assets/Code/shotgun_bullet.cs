using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shotgun_bullet : MonoBehaviour
{
    public float speed = 5f;
    private Transform rocket;
    public Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rocket = GameObject.FindGameObjectWithTag("Rocket").transform;
    }

    void Update()
    {
        if (rocket != null)
        {
            Vector2 direction = (Vector2)rocket.position - rb.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = -rotateAmount * 200f;
            rb.velocity = transform.up * speed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Dummy")
        {
            bool isCriticalHit = false;
            DamagePopup.Create(gameObject.transform.position, 200, isCriticalHit);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "ShotgunBullet")
        {

        }
        else
        {
            Destroy(gameObject);
        }
    }
}
