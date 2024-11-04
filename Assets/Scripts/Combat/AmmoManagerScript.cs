
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoManagerScript : MonoBehaviour
{
    public static AmmoManagerScript Instance { get; private set; }

    [Header("Ammo")]
    public TextMeshProUGUI magAmmo;
    public TextMeshProUGUI totalAmmo;

    private void Awake()
    {
        //  ovo je pravljenje singletona
        if (Instance != null && Instance != this)
        {
            //  ako postoji instanca unistavamo je, treba nam samo jedna
            //  ovo postoji da ne bi duplirali instance klase vec da samo postoji jedna
            //  ukoliko postoji vise:
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Update()
    {
        Weapon activeWeapon = WeaponManager.Instance.
            activeWeaponSlot.GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            magAmmo.text = $"{activeWeapon.bulletsLeft}";
            totalAmmo.text = $"{WeaponManager.Instance.CheckAmmoLeft(activeWeapon.thisWeaponModel)}";
        }
        else
        {
            magAmmo.text = "";
            totalAmmo.text = "";
        }
    }
}

