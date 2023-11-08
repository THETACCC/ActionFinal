using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{



    private void Start()
    {

        bool isCriticalHit = Random.Range(0, 100) < 30;
        DamagePopup.Create(Vector3.zero, 400, isCriticalHit);
    }
}
