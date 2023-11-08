using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class create_objects : MonoBehaviour
{

    public GameObject spawnobject;
    public Vector3 newPosition;
    public Quaternion newRotation;



    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Vector3 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse_position.z = 0;
            Instantiate(spawnobject, mouse_position, Quaternion.identity);

        }




    }


    public void instantiatNewObject()
    {

    }
}
