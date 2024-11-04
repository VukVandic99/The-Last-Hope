using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
//  singleton je obrazac projektovanja koji se koristi da bi se osigurala samo jedna
//  instanca odredjene klase i pruzio globalan pristup njoj


public class WeaponManager : MonoBehaviour
{
    //  kreiramo listu pusaka koje mozemo pokupiti:
    public List<GameObject> weaponSlots;
    public GameObject activeWeaponSlot;

    [Header("Ammo")]
    public int totalM4_8Ammo = 0;
    public int totalSniperAmmo = 0;
    public int totalUziAmmo = 0;
    public int totalAKAmmo = 0;
    public int totalM1911Ammo = 0;
    public int totalM107Ammo = 0;


    private void Start()
    {
        //  ovaj kod radi kada zapocnemo igru active slot je prvi u listi
            activeWeaponSlot = weaponSlots[0];
    }
    private void Update()
    {
        foreach (GameObject weaponSlot in weaponSlots)
        {
            if (weaponSlot == activeWeaponSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //  alfa1 je broj 1 na tastaturi a alfa dva je broj 2
            //  uzimamo indeks 0 sto je prvi slot
            SwitchActiveSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //  i indeks 1 sto je drugi slot 
            SwitchActiveSlot(1);
        }
    }
    /*
     Ova skripta sluzi za puske, ali je menadzer ne osnovne funkcije kao pucaj i repetiraj
    u prevodu ovo sluzi za kontrolu svih pusaka i kako ih koristimo i menjamo sa jedne puske na drugu*/
    //  naravno ovo je singleton
    public static WeaponManager Instance { get; private set; }
    private void Awake()
    {
        //  ovo je pravljenje singletona
        if (Instance != null && Instance != this)
        {
            //  ako postoji instanca unistavamo je, treba nam samo jedna
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void PickUpWeapon(GameObject pickedWeapon)
    {
        //  ukoliko radi unistavamo oruzije: provera metodom Destroy();
        //Destroy(pickedWeapon);
        AddWeaponToActiveSlot(pickedWeapon);
    }

    private void AddWeaponToActiveSlot(GameObject pickedWeapon)
    {
        //  metoda za razmenu oruzija:
        DropCurrentWeapon(pickedWeapon);
        //  na pokupljenu pusku stavljamo da roditelj bude activeWeaponSlot
        pickedWeapon.transform.SetParent(activeWeaponSlot.transform, false);

        //  uzimamo weapon skriptu i postavljamo pozicije
        Weapon weapon = pickedWeapon.GetComponent<Weapon>();
        //  lokalne pozicije u spawnWeaponPosition i rotation
        pickedWeapon.transform.localPosition = new Vector3(weapon.spawnWeaponPosition.x
            , weapon.spawnWeaponPosition.y, weapon.spawnWeaponPosition.z);
        pickedWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnWeaponRotation.x,
            weapon.spawnWeaponRotation.y, weapon.spawnWeaponRotation.z);

        weapon.isActiveWeapon = true;

        //  animacija ne radi:
        weapon.animator.enabled = true;
    }
    private void DropCurrentWeapon(GameObject pickedWeapon)
    {
        //  ukoliko pokusamo da pokupimo jos jedno oruzije a imamo u ruci oruzije
        if (activeWeaponSlot.transform.childCount > 0)
        {
            //  onda ispustamo oruzije:
            //  ukoliko imamo oruzije(npr pistolj) onda weaponToDrop uzima referencu na trenutno oruzije
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;

            //  vise nije active weapon (referenca na pistolj):
            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false;

            //  gasimo animaciju za ispusteno oruzije:
            weaponToDrop.GetComponent<Weapon>().animator.enabled = false;

            //  postavljamo roditelja ovog oruzija da bude isti roditelj kao oruzije koje kupimo
            //  vracamo ga u poziciju gde se nalazilo oruzije koje smo pokupili, da ne nestane
            weaponToDrop.transform.SetParent(pickedWeapon.transform.parent);

            //  postavljamo poziciju i rotaciju oruzija koje kupimo da bude isto kao i oruzija koje smo ispustili
            weaponToDrop.transform.localPosition = pickedWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedWeapon.transform.localRotation;
            //  u prevodu zamenimo oruzije sa drugim oruzijem, sve
        }
    }

    public void SwitchActiveSlot(int slotNumber)
    {
        //  ukoliko imamo nesto u activeslot(npr oruzije)
        //  onda stavljamo false, jer to vise ne treba da bude activeWeapon
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
        }
        //  active weapon postaje novi slot sto je indeks koji je argument ove metode(0-prvi element 1- drugi)
        activeWeaponSlot = weaponSlots[slotNumber];
        
        if (activeWeaponSlot.transform.childCount > 0)
        {
            //  zatim pravimo novo oruzije:
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.isActiveWeapon = true;
        }
    }

    internal void DecreaseTotalAmmo(int bulletsToDecrease, Weapon.WeaponModel thisWeapon)
    {
        switch (thisWeapon)
        {
            case Weapon.WeaponModel.Pistol1911:
                totalM1911Ammo -= bulletsToDecrease;
                break;
            case Weapon.WeaponModel.Rifle:
                totalSniperAmmo -= bulletsToDecrease;
                break;
            case Weapon.WeaponModel.Uzi:
                totalUziAmmo -= bulletsToDecrease;
                break;
            case Weapon.WeaponModel.AK47:
                totalAKAmmo -= bulletsToDecrease;
                break;
            case Weapon.WeaponModel.M107:
                totalM107Ammo -= bulletsToDecrease;
                break;
            case Weapon.WeaponModel.M4:
                totalM4_8Ammo -= bulletsToDecrease;
                break;

        }
    }
    internal void PickUpAmmo(AmmoBox ammo)
    {
        switch (ammo.ammoType)
        {
            case AmmoBox.AmmoType.M4_8Ammo:
                totalM4_8Ammo += ammo.ammoAmount;
                break;
            case AmmoBox.AmmoType.sniperAmmo:
                totalSniperAmmo += ammo.ammoAmount;
                break;
            case AmmoBox.AmmoType.AKAmmo:
                totalAKAmmo += ammo.ammoAmount;
                break;
            case AmmoBox.AmmoType.M107Ammo:
                totalM107Ammo += ammo.ammoAmount;
                break;
            case AmmoBox.AmmoType.M1911Ammo:
                totalM1911Ammo += ammo.ammoAmount;
                break;
            case AmmoBox.AmmoType.uziAmmo:
                totalUziAmmo += ammo.ammoAmount;
                break;
        }
    }

    public int CheckAmmoLeft(Weapon.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {

            case Weapon.WeaponModel.Pistol1911:
                return totalM1911Ammo;
                break;
            case Weapon.WeaponModel.Uzi:
                return totalUziAmmo;
                break;
            case Weapon.WeaponModel.M107:
                return totalM107Ammo;
                break;
            case Weapon.WeaponModel.AK47:
                return totalAKAmmo;
                break;
            case Weapon.WeaponModel.M4:
                return totalM4_8Ammo;
                break;
            case Weapon.WeaponModel.Rifle:
                return totalSniperAmmo;
                break;
            default:
                return 0;
        }
    }
}
