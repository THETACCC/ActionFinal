using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIEffect : MonoBehaviour
{
    //get the RectTransform and the TMPRO
    private TextMeshPro textmesh;
    private RectTransform m_transform;

    //get the points on dummy
    public GameObject m_dummy;
    public targetDummy dummy;
    private SpriteRenderer dummy_renderer;

    //effects
    private bool starteffect;
    private float speed = 20f;
    private void Awake()
    {
        dummy_renderer = m_dummy.GetComponentInChildren<SpriteRenderer>();
        m_transform = gameObject.GetComponent<RectTransform>();
        textmesh = transform.GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        if (dummy.colorIndex == 1)
        {
            starteffect= true;
        }


        if (starteffect)
        {
            Vector3 newScale = m_transform.localScale;
            newScale.x = Mathf.Lerp(newScale.x, 1.3f, Time.deltaTime * speed);
            newScale.y = Mathf.Lerp(newScale.y, 0.7f, Time.deltaTime * speed);
            m_transform.localScale = newScale;


            if (newScale.x >= 1.26f)
            {
                starteffect = false;
            }
        }
        else if (!starteffect)
        {
            Vector3 newScale = m_transform.localScale;
            newScale.x = Mathf.Lerp(newScale.x, 1f, Time.deltaTime * speed);
            newScale.y = Mathf.Lerp(newScale.y, 1f, Time.deltaTime * speed);
            m_transform.localScale = newScale;
        }




    }



}
