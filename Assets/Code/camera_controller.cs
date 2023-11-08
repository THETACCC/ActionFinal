using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_controller : MonoBehaviour
{
    public player_controller controller;
    public Transform target;
    public float smooth_speed = 0.125f;
    public Vector3 offest;

    private Vector3 _velocity = Vector3.zero;
    //public PlayerController player;

    public float limitLeft, limitRight, limitTop, limitBottom;


    //camera_zoom
    private float zoom;
    private float zoomMultiplier = 4f;
    private float MinZoom = 14f;
    private float MaxZoom = 16f;
    private float velocity = 1f;
    private float smoothTime = 0.75f;
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    public Transform camera_pos;

    [SerializeField] private Camera cam;

    private void Start()
    {
        zoom = cam.orthographicSize;
    }

    public void TriggerShake()
    {
        StartCoroutine(Shake());
    }


    // Start is called before the first frame update
    private void LateUpdate()
    {
       
        zoom = Mathf.Clamp(zoom, MinZoom, MaxZoom);

        if (controller.current_running_speed > 1f)
        {
            zoom = MinZoom;
           
        }
        else
        {
            zoom = MaxZoom;
        }
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, zoom, ref velocity, smoothTime);


        Transform currentTarget = target;

        Vector3 desiredPosition = currentTarget.position + offest;

        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, smooth_speed);
        transform.position = smoothedPosition;



    }

    IEnumerator Shake()
    {
        float elapsed = 0.0f;
        while (elapsed < shakeDuration)
        {
            Vector3 desiredPosition = target.position + offest;  // Calculate the desired position based on the target
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.position = desiredPosition + new Vector3(x, y, 0);  // Add the shake offset to the desired position

            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
