using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterractionWeaponManager : MonoBehaviour
{
    public static InterractionWeaponManager Instance { get; private set; }

    public Weapon hoveredWeapon = null;
    public AmmoBox hoveredAmmo = null;
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

    private void Update()
    {
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hitLook;

            if(Physics.Raycast(ray, out hitLook) )
            {
                GameObject objectHit = hitLook.transform.gameObject;
                //  ukoliko nije aktivno oruzije i jeste oruzije onda radi outline
                //  u suprotnom animacija muzzle ne radi kako treba i crta se outline beli na njoj
                //  jer je hover preko oruzija
                if (objectHit.GetComponent<Weapon>() && 
                    objectHit.GetComponent<Weapon>().isActiveWeapon == false)
                {
                    hoveredWeapon = objectHit.gameObject.GetComponent<Weapon>();
                    hoveredWeapon.GetComponent<Outline>().enabled = true;

                    //  u metodi pickedUpWEapon() argument za oruzije u ovoj klasi je objectHit
                    //  npr ukoliko hoverujemo preko pistolja objectHit.gameObject(pistolj)
                    //  vrednost se prosledjuje u PickUpWeapon() instancu iz klase WeaponManager
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        WeaponManager.Instance.PickUpWeapon(objectHit.gameObject); 
                    }
                }
                else
                {
                    if (hoveredWeapon)
                    {
                        //  ovo sada gasi outline na trenutnom oruziju koje se prethodno hoverovalo
                        hoveredWeapon.GetComponent<Outline>().enabled = false;
                    }
                }

                //  AmmoBox
                if (objectHit.GetComponent<AmmoBox>())
                {
                    hoveredAmmo = objectHit.gameObject.GetComponent<AmmoBox>();
                    //hoveredAmmo.GetComponent<Outline>().enabled = true;

                    //  u metodi pickedUpWEapon() argument za oruzije u ovoj klasi je objectHit
                    //  npr ukoliko hoverujemo preko pistolja objectHit.gameObject(pistolj)
                    //  vrednost se prosledjuje u PickUpWeapon() instancu iz klase WeaponManager
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        WeaponManager.Instance.PickUpAmmo(hoveredAmmo);
                        Destroy(objectHit.gameObject);
                    }
                }
                else
                {
                    if (hoveredAmmo)
                    {
                        //  ovo sada gasi outline na trenutnom oruziju koje se prethodno hoverovalo
                        //hoveredAmmo.GetComponent<Outline>().enabled = false;
                    }
                }
            }
        }
    }
}
