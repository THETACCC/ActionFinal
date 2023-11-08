using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Luanch_drones : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float bulletLifeTime = 2f;
    private SpriteRenderer spriteRenderer;
    public float recoilAmount = 0.2f;
    public float recoilSpeed = 5f;
    public Transform Pivot;
    private Vector3 originalPosition;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;
    public Weaponmanage weaponmanage;
    public string weaponName;
    public TextMeshProUGUI cooldownText;

    //Cinemachine 
    private CinemachineImpulseSource impluseSrouce;

    //squash when fire
    private bool starteffect;
    private float effect_speed = 20f;
    public Transform pivot;

    void Start()
    {
        impluseSrouce = GetComponent<CinemachineImpulseSource>();
        originalPosition = Pivot.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction;

        if (mousePosition.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Weapon myWeapon = weaponmanage.GetWeaponByName(weaponName);
            if (Time.time > myWeapon.nextFireTime)
            {
                CameraShakeManager.instance.CameraShake(impluseSrouce);
                myWeapon.nextFireTime = Time.time + myWeapon.fireRate;
                starteffect = true;
                Shoot();
            }
        }

        if (starteffect)
        {
            Vector3 newScale = pivot.localScale;
            newScale.x = Mathf.Lerp(newScale.x, 1.25f, Time.deltaTime * effect_speed);
            newScale.y = Mathf.Lerp(newScale.y, 0.6f, Time.deltaTime * effect_speed);
            pivot.localScale = newScale;
            pivot.rotation = Quaternion.Lerp(pivot.rotation, Quaternion.Euler(new Vector3(0, 0, -10)), Time.deltaTime * effect_speed);

            Debug.Log(newScale.x);
            if (newScale.y <= 0.64f)
            {
                starteffect = false;
            }
        }
        else if (!starteffect)
        {
            Vector3 newScale = pivot.localScale;
            newScale.x = Mathf.Lerp(newScale.x, 1f, Time.deltaTime * effect_speed);
            newScale.y = Mathf.Lerp(newScale.y, 1f, Time.deltaTime * effect_speed);
            pivot.localScale = newScale;
            pivot.rotation = Quaternion.Lerp(pivot.rotation, Quaternion.Euler(new Vector3(0, 0, 0)), Time.deltaTime * effect_speed);
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        //Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        //rb.velocity = firePoint.up * bulletSpeed;
        Destroy(bullet, bulletLifeTime);
    }

    System.Collections.IEnumerator RecoilEffect()
    {
        Vector3 recoilPosition = originalPosition + new Vector3(-recoilAmount, 0, 0);
        while (Vector3.Distance(Pivot.localPosition, recoilPosition) > 0.01f)  // Use Pivot.localPosition
        {
            Pivot.localPosition = Vector3.Lerp(Pivot.localPosition, recoilPosition, recoilSpeed * Time.deltaTime);  // Use Pivot.localPosition
            yield return null;
        }

        while (Vector3.Distance(Pivot.localPosition, originalPosition) > 0.01f)  // Use Pivot.localPosition
        {
            Pivot.localPosition = Vector3.Lerp(Pivot.localPosition, originalPosition, recoilSpeed * Time.deltaTime);  // Use Pivot.localPosition
            yield return null;
        }
    }



}
