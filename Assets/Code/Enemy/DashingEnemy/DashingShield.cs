using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingShield : MonoBehaviour
{

    public DashingEnemy dashingenemy;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "HandgunBullet")
        {
            dashingenemy.isshield = false;
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Drone")
        {
            Destroy(collision.gameObject);
        }
    }
}
