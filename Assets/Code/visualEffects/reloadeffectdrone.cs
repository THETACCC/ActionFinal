using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class reloadeffectdrone : MonoBehaviour
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

    //get the weapon
    public int theweapon;
    public switch_weapon weaponswitch;

    //timer
    private float time = 0f;
    private float the_time = 0.1f;
    private bool countdown = false;
    private void Awake()
    {
        //manage = GetComponent<Weaponmanage>();
        m_transform = gameObject.GetComponent<RectTransform>();
        textmesh = transform.GetComponent<TextMeshPro>();
    }

    private void Update()
    {
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
            countdown = false;
        }

        if (starteffect)
        {
            Vector3 newScale = m_transform.localScale;
            newScale.x = Mathf.Lerp(newScale.x, 0.06f, Time.deltaTime * speed);
            newScale.y = Mathf.Lerp(newScale.y, 0.09f, Time.deltaTime * speed);
            m_transform.localScale = newScale;

            //if (newScale.x >= 1.1f)
            //{
            //   starteffect = false;
            //}
        }
        else if (!starteffect)
        {
            Vector3 newScale = m_transform.localScale;
            newScale.x = Mathf.Lerp(newScale.x, 0.12f, Time.deltaTime * speed);
            newScale.y = Mathf.Lerp(newScale.y, 0.12f, Time.deltaTime * speed);
            m_transform.localScale = newScale;
        }




    }



}
