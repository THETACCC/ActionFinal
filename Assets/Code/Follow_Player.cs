using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Player : MonoBehaviour
{
    public Transform target;

    public Vector3 offset;

    private Vector3 _velocity = Vector3.zero;
    //public PlayerController player;






    void FixedUpdate()
    {
        Vector3 movePosition = target.position + offset;
        transform.position = movePosition;
    }
}
