using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hand_gun_bullet : MonoBehaviour
{


    public float accelerationRate = 0.7f;
    public Rigidbody2D rb;
    //effects
    private float speed = 10f;
    void Update()
    {
        Vector3 newScale = transform.localScale;
        newScale.x = Mathf.Lerp(newScale.x, 0.01f, Time.deltaTime * speed);
        newScale.y = Mathf.Lerp(newScale.y, 1.5f, Time.deltaTime * speed);
        transform.localScale = newScale;
        rb.velocity = rb.velocity.normalized * (rb.velocity.magnitude + accelerationRate * Time.deltaTime);
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Dummy")
        {

            bool isCriticalHit = false;
            DamagePopup.Create(gameObject.transform.position, 25, isCriticalHit);
        }

        if (!(collision.gameObject.tag == "InvisibleWall"))
        {
            Destroy(gameObject);
        }

    }
}
