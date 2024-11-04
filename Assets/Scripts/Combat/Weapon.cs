using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //  da bi u inspektoru mogli da dajemo razliciti Damage
    public int WeaponDMG;
    //  jer pucanje idalje radi dok nismo pokupili oruzije, to se mora iskljuciti zato kreiramo:
    public bool isActiveWeapon;
    //  provera da li puca
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 0.15f;

    //  burst
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft; 

    //  prefab za metkove:
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 500;
    public float bulletPrefabLifeTime = 3f;

    //  fles za pucanje:
    public GameObject muzzleEffect;
    //  referenca za animator:
    //  internal znaci da ga nema u inspektoru, ali ga ima u svim skriptama public
    internal Animator animator;

    //  loading
    public float reloadTime;
    public int magazinSize, bulletsLeft;
    public bool isReloading;

    //  cuvanje pozicije oruzija, da ne izgleda cudno kada pokupimo:
    public Vector3 spawnWeaponPosition;
    public Vector3 spawnWeaponRotation;

    //  lista pusaka u ponudi
    public enum WeaponModel
    {
        Pistol1911,
        M4,
        M107,
        Uzi,
        AK47,
        Rifle
    }

    public WeaponModel thisWeaponModel;

    //  modovi za pucanje:
    public enum shootMode
    {
        Single,
        Burst,
        Auto
    }
    public shootMode currentShootingMode;
     
    private void Awake()
    {
        //  pozivamo readyToShoot na true da moze da puca
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();

        bulletsLeft = magazinSize;
    }
    // Update is called once per frame
    void Update()
    {
        if (isActiveWeapon)
        {
            GetComponent<Outline>().enabled = false;
            if (bulletsLeft == 0 && isShooting && isReloading == true)
            {
                SoundManager.Instance.empyMagazineSound1911.Play();
            }

            if (currentShootingMode == shootMode.Auto)
            {
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == shootMode.Single ||
                currentShootingMode == shootMode.Burst)
            {
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }
            //  getkeydown je za jedan klika a getkey je za drzanje klika

            //  reload:
            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazinSize
                && isReloading == false 
                && WeaponManager.Instance.CheckAmmoLeft(thisWeaponModel) > 0)
            {
                //  pozivamo metodu Reload();
                Reload();
            }

            if (readyToShoot && isShooting && bulletsLeft > 0 && isReloading == false)
            {
                burstBulletsLeft = bulletsPerBurst;
                FireWeapon();
            }

            //  referenciramo iz singletona AmmoManager
            
        }
    }

    //  funkcija za ispaljivanje iz puske
    private void FireWeapon()
    {
        muzzleEffect.GetComponent<ParticleSystem>().Play();
        //  za svaki put kada pucamo smanjujemo metkove:
        //  umesto hardkodovanja pusta se oruzije koje je izabrano
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);
        bulletsLeft--;
        animator.SetTrigger("RECOIL");
        //  pozivanje zvuka iz klase SoundManager
        //  moramo pozvati shootChannel jer u soundManageru pucnjava je single zvuk
        //  i stavljamo ga u audiosource shootchannel
        SoundManager.Instance.shootChannel.Play();
        //  jer je na startu readyToShoot= true sada stavljamo na false, jer ne zelimo da se preklapa pucanje
        readyToShoot = false;
        //  ne mozemo pucati dok prvo pucanje se ne zavrsi

        //  instanciranje metkova
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        //  Quaternion.identity rotacija je na 0

        //  postavljanje damage na metak
        Bullet bull = bullet.GetComponent<Bullet>();
        bull.bulletDMG = WeaponDMG;
        
        Vector3 shootDirect = shootingDirection().normalized;
        bullet.transform.forward = shootDirect;
        bullet.GetComponent<Rigidbody>().AddForce(shootDirect * bulletVelocity, ForceMode.Impulse);

        //  ispaljivanje metkova napred
        //bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletVelocity, ForceMode.Impulse);
        //  unistavanje metkova
        StartCoroutine(DestroyBulletAfterTime(bullet,bulletPrefabLifeTime));

        //  provera da li je gotovo pucanje:
        if (allowReset)
        {
            //  zatim pozivamo reset i allow reset stavljamo na false
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        //  burst pucnjava(3 meta po kliku): 
        if(currentShootingMode == shootMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void Reload()
    {
        SoundManager.Instance.PlayReloadingSound(thisWeaponModel);
        animator.SetTrigger("RELOAD");
        
        isReloading = true;
        //  pauza dok se desava reload
        Invoke("ReloadCompleted", reloadTime);
    } 
    private void ReloadCompleted()
    {
        //  ako ima vise metkova nego sarzer, punimo inventory:
        if (WeaponManager.Instance.CheckAmmoLeft(thisWeaponModel) > magazinSize)
        {
            //  bulletsleft uzima magazine size i puni se
            bulletsLeft = magazinSize;
            //  zatim smanjujemo odredjeni ammo iz weapon managera, sto je sarzer
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        }
        else
        {
            bulletsLeft = WeaponManager.Instance.CheckAmmoLeft(thisWeaponModel);
            WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
        }

        isReloading = false;
    }
    private void ResetShot()
    {
        //  invokujemo metodu iz gore if(allowReset)
        //  i stavljamo ready to shoot i allow reset na true da mozemo da pucamo i da resetujemo
        readyToShoot = true;
        allowReset = true;
        //  ovo postoji da ne bi resetovali pucnjavu vise puta, tj da se ne preklapa
    } 
    /*
     Ovo sve koristimo ray da znamo tacno mesto gde izlazi metak*/
    public Vector3 shootingDirection()
    {
        //  automatski dobijamo referencu na glavnu kameru putem:
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit)) 
        {
            //  pogodak u telo
            targetPoint = hit.point;
        }
        else
        {
            //  pucanje u vazduh
            targetPoint = ray.GetPoint(100); 
        }
        Vector3 direction = targetPoint - bulletSpawn.position;
        //  da puca samo pravo:
        return direction + new Vector3(0,0,0);
    }
    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float bulletPrefabLifeTime)
    {
        yield return new WaitForSeconds(bulletPrefabLifeTime);
        Destroy(bullet);
    }
}
