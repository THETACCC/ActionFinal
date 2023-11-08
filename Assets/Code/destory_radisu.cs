using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class destory_radisu : MonoBehaviour
{

    public float timeLeft = 1f;

    private GameObject dummy;


    private void Start()
    {
        dummy = GameObject.FindGameObjectWithTag("Dummy");
    }
    void Update()
    {
        timeLeft -= Time.deltaTime;


        if (timeLeft <= 0)
        {
            Destroy(gameObject);
        }
    }

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "SeekerEnemy")
        {
            Destroy(collision.gameObject);
        }
    }
    
}