using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing_Rocket : MonoBehaviour
{
    public float explosionRadius = 5f;
    public int explosionDamage = 10;
    public LayerMask targetLayer;

    public Transform target;
    public float speed = 5f;
    private Rigidbody2D rb;
    public targetDummy Dummy;
    public GameObject explosionRadiusIndicator;

    //particles
    [SerializeField] private ParticleSystem explosion = default;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Dummy").transform;
    }

    void Update()
    {
        Vector2 direction = (Vector2)target.position - rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = -rotateAmount * 200f;
        rb.velocity = transform.up * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        explosion.Play();
        if (collision.gameObject.tag == "HandgunBullet" || collision.gameObject.tag == "ShotgunBullet" || collision.gameObject.tag == "explosion" || collision.gameObject.tag == "Dummy" || collision.gameObject.tag == "TNT")
        {
            explosion.Play();
            Explode();
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "explosion_alone")
        {
            Explode();
 
            //GameObject indicator = Instantiate(explosionRadiusIndicator_alone, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void Explode()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, explosionRadius, targetLayer);
        ShowExplosionRadius();
    }
    void ShowExplosionRadius()
    {
 
        GameObject indicator = Instantiate(explosionRadiusIndicator, transform.position, Quaternion.identity);
    }





}
