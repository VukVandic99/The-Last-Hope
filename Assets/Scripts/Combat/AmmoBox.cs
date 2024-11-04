using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public int ammoAmount;
    public AmmoType ammoType;

    public enum AmmoType
    {
        M4_8Ammo,
        sniperAmmo,
        uziAmmo,
        AKAmmo,
        M1911Ammo,
        M107Ammo
    }

}
