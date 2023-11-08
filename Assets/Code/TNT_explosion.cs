using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNT_explosion : MonoBehaviour
{
    public float explosionRadius = 5f;
    public int explosionDamage = 10;
    public LayerMask targetLayer;
    public targetDummy Dummy;
    public GameObject explosionRadiusIndicator;
    public GameObject explosionRadiusIndicator_alone;
    public GameObject explosionRadiusIndicator_super;
    public float timeToExplode = 5f;
    public float flashInterval = 0.5f;
    public Color flashColor = Color.red;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    //particles
     private ParticleSystem explosionsmall = default;
     private ParticleSystem explosionbig = default;
     private GameObject smallexplosion;
     private GameObject bigexplosion;
    void Start()
    {
        smallexplosion = GameObject.FindGameObjectWithTag("particles");
        bigexplosion = GameObject.FindGameObjectWithTag("particlesbig");
        explosionsmall = smallexplosion.GetComponent<ParticleSystem>();
        explosionbig = bigexplosion.GetComponent<ParticleSystem>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        StartCoroutine(FlashAndExplode());
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "HandgunBullet" || collision.gameObject.tag == "ShotgunBullet" || collision.gameObject.tag == "explosion" || collision.gameObject.tag == "explosion_rocket")
        {
            smallexplosion.transform.position = this.transform.position;
            explosionsmall.Play();
            Explode();
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "explosion_alone")
        {
            smallexplosion.transform.position = this.transform.position;
            explosionsmall.Play();
            GameObject indicator = Instantiate(explosionRadiusIndicator_alone, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "Rocket")
        {
            bigexplosion.transform.position = this.transform.position;
            explosionbig.Play();

            GameObject indicator = Instantiate(explosionRadiusIndicator_super, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "SeekerEnemy")
        {
            smallexplosion.transform.position = this.transform.position;
            explosionsmall.Play();
            GameObject indicator = Instantiate(explosionRadiusIndicator_alone, transform.position, Quaternion.identity);
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
        StartCoroutine(DestroyIndicator(indicator, 3f));
    }

    IEnumerator DestroyIndicator(GameObject indicator, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(indicator);
    }


    IEnumerator FlashAndExplode()
    {
        float elapsed = 0f;
        while (elapsed < timeToExplode)
        {
            spriteRenderer.color = (spriteRenderer.color == originalColor) ? flashColor : originalColor;
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }

        GameObject indicator = Instantiate(explosionRadiusIndicator_alone, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
