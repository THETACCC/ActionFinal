using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Destoryovertime : MonoBehaviour
{
    private float mytime = 0f;
    private float lifetime = 3f;

    private void Start()
    {
        mytime = lifetime;
    }

    void Update()
    {
        mytime -= Time.deltaTime;
        if (mytime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
