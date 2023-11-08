using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReloadEffect : MonoBehaviour
{
    //get the RectTransform and the TMPRO
    private TextMeshPro textmesh;
    private RectTransform m_transform;

    //get the points on weapon manager
    public GameObject weaponmanager;
    public Weaponmanage manage;
    private SpriteRenderer dummy_renderer;

    //effects
    private bool starteffect;
    private float speed = 20f;
    //the gun

    public int theweapon;
    public switch_weapon weaponswitch;

    //get the target position
    private RectTransform rectTransform;
    private float moveAmount = 150f;
    private float lerpSpeed = 20f;
    private Vector3 originalPosition;

    //alpha effect
    private TextMeshProUGUI textMesh;
    public float fadeSpeed = 5f;

    //timer
    private float time = 0f;
    private float the_time = 0.1f;
    private bool countdown = false;


    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        //manage = GetComponent<Weaponmanage>();
        time = the_time;
        textMesh = gameObject.GetComponent<TextMeshProUGUI>();
        originalPosition = rectTransform.localPosition;
    }

    private void Update()
    {
        if (textMesh == null)
        {
            return;
        }
        Color currentColor = textMesh.color;
        if ((weaponswitch.selectedWeapon == theweapon) && (Input.GetButtonDown("Fire1")) && (manage.weapons[theweapon].nextFireTime <= 0))
        {
            starteffect = true;
            countdown = true;
        }
        if (countdown)
        {
            time -= Time.deltaTime;
        }
        if (time < 0f)
        {
            time = the_time;
            starteffect = false;
            countdown= false;
        }
        //else
        //{
        //   starteffect = false;
        //}

        if (starteffect)
        {
            Vector3 newScale = rectTransform.localScale;
            newScale.x = Mathf.Lerp(newScale.x, 1.2f, Time.deltaTime * speed);
            newScale.y = Mathf.Lerp(newScale.y, 0.7f, Time.deltaTime * speed);
            rectTransform.localScale = newScale;

        }
        else if (!starteffect)
        {
            Vector3 newScale = rectTransform.localScale;
            newScale.x = Mathf.Lerp(newScale.x, 1f, Time.deltaTime * speed);
            newScale.y = Mathf.Lerp(newScale.y, 1f, Time.deltaTime * speed);
            rectTransform.localScale = newScale;
        }

        if (weaponswitch.selectedWeapon == theweapon)
        {



            // m_transform.position.x = Mathf.Lerp(gameObject.transform.position.x, -360, Time.deltaTime * speed);
            Vector3 targetPosition = originalPosition + new Vector3(moveAmount, moveAmount, 0);
            rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, targetPosition, Time.deltaTime * lerpSpeed);
            if (manage.weapons[theweapon].nextFireTime <= 0)
            {
                currentColor.a = Mathf.Lerp(currentColor.a, 1f, Time.deltaTime * fadeSpeed);
            }
            else
            {
                currentColor.a = Mathf.Lerp(currentColor.a, 0.05f, Time.deltaTime * fadeSpeed); // Alpha set to 0.05 when reloading
            }


        }
        else
        {
            rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, originalPosition, Time.deltaTime * lerpSpeed);
            if (manage.weapons[theweapon].nextFireTime <= 0)
            {
                currentColor.a = Mathf.Lerp(currentColor.a, 0.45f, Time.deltaTime * fadeSpeed);
            }
            else
            {
                currentColor.a = Mathf.Lerp(currentColor.a, 0.05f, Time.deltaTime * fadeSpeed); // Alpha set to 0.05 when reloading
            }
        }

        textMesh.color = currentColor;
    }



}
