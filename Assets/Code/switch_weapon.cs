using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switch_weapon : MonoBehaviour
{
    public int selectedWeapon = 0;
    //sounds
    private AudioSource audioSource;

    public AudioClip handgunSound;
    public AudioClip shotgunSound;
    public AudioClip RocketSound;
    public AudioClip mortalSound;
    public AudioClip droneSound;


    //ready sounds
    public AudioClip handgunready;
    public AudioClip shotgunready;
    public AudioClip rocketready;
    public AudioClip mortalready;
    public AudioClip droneready;

    private bool handgunplayed = false;
    private bool shotgunplayed = false;
    private bool rocketplayed = false;
    private bool mortalplayed = false;
    private bool droneplayed = false;
    //getmanager
    public Weaponmanage manager;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SelectWeapon();
    }

    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeapon = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedWeapon = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedWeapon = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selectedWeapon = 4;
        }
        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }

        if (selectedWeapon == 0)
        {
            if (Input.GetButtonDown("Fire1") && manager.weapons[0].nextFireTime == 0)
            {
                audioSource.PlayOneShot(handgunSound);
                handgunplayed = false;
            }
        }
        else if (selectedWeapon == 1)
        {
            if (Input.GetButtonDown("Fire1") && manager.weapons[1].nextFireTime == 0)
            {
                audioSource.PlayOneShot(shotgunSound);
                shotgunplayed = false;
            }
        }
        else if (selectedWeapon == 2)
        {
            if (Input.GetButtonDown("Fire1") && manager.weapons[2].nextFireTime == 0)
            {
                audioSource.PlayOneShot(RocketSound);
                rocketplayed = false;
            }
        }
        else if (selectedWeapon == 3)
        {
            if (Input.GetButtonDown("Fire1") && manager.weapons[3].nextFireTime == 0)
            {
                audioSource.PlayOneShot(mortalSound);
                mortalplayed = false;
            }
        }
        else if (selectedWeapon == 4)
        {
            if (Input.GetButtonDown("Fire1") && manager.weapons[4].nextFireTime == 0)
            {
                audioSource.PlayOneShot(droneSound);
                droneplayed = false;
            }
        }

        if (manager.weapons[0].nextFireTime == 0  && !handgunplayed && !Input.GetButtonDown("Fire1"))
        {
            audioSource.PlayOneShot(handgunready);
            handgunplayed = true;
        }
        else if (manager.weapons[1].nextFireTime == 0  && !shotgunplayed && !Input.GetButtonDown("Fire1"))
        {
            audioSource.PlayOneShot(shotgunready);
            shotgunplayed = true;
        }
        else if (manager.weapons[2].nextFireTime == 0  && !rocketplayed && !Input.GetButtonDown("Fire1"))
        {
            audioSource.PlayOneShot(rocketready);
            rocketplayed = true;
        }
        else if (manager.weapons[3].nextFireTime == 0  && !mortalplayed && !Input.GetButtonDown("Fire1"))
        {
            audioSource.PlayOneShot(mortalready);
            mortalplayed = true;
        }
        else if (manager.weapons[4].nextFireTime == 0 && !droneplayed && !Input.GetButtonDown("Fire1"))
        {
            audioSource.PlayOneShot(droneready);
            droneplayed = true;
        }

    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
