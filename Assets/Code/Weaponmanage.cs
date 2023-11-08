using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weaponmanage : MonoBehaviour
{
    public Weapon[] weapons;
    public TextMeshProUGUI[] cooldownTexts;

    void Update()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            float remainingCooldown = weapons[i].nextFireTime - Time.time;
            if (remainingCooldown > 0)
            {
                cooldownTexts[i].text = remainingCooldown.ToString("F1") + "s";
            }
            else
            {
                weapons[i].nextFireTime = 0;
                cooldownTexts[i].text = "Ready";
            }
        }
    }

    public Weapon GetWeaponByName(string name)
    {
        foreach (Weapon weapon in weapons)
        {
            if (weapon.name == name)
            {
                return weapon;
            }
        }
        return null;
    }

}

[System.Serializable]
public class Weapon
{
    public string name;
    public float fireRate = 0.5f;
    public float nextFireTime = 0f;
}

