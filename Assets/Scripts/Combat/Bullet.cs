using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDMG;
    //  metoda za pogodak na target tag
    //  OnCol... je metoda koja se poziva automatski kada dodje do sudara izmedju dva kolajder objekta u igrici
    //  zato se koristi za metak impakt
    private void OnCollisionEnter(Collision objectHit)
    {
        if (objectHit.gameObject.CompareTag("Target"))
        {
            print("Hit " + objectHit.gameObject.name + " !");
            //CreateBulleImpactEffect(objectHit);
            //  ako pogodi target onda da se unitsti:
            Destroy(gameObject);
        }

        if (objectHit.gameObject.CompareTag("Wall"))
        {
            print("Hit a wall");
            //CreateBulleImpactEffect(objectHit);
            //  ako pogodi target onda da se unitsti:
            Destroy(gameObject);
        }

        if (objectHit.gameObject.CompareTag("Bottle"))
        {
            print("Hit a bottle");
            objectHit.gameObject.GetComponent<Bottle>().Crash();
            //  ne unistavamo metak!!!
            Destroy(gameObject);
        }
        //  za collision sa zombijem:
        if (objectHit.gameObject.CompareTag("Enemy"))
        {
            print("Hit a zombie");

            //  provera da li je zombie mrtav, ako jeste ne deluje dmg
            if (objectHit.gameObject.GetComponent<Enemy>().isDead == false)
            {
                objectHit.gameObject.GetComponent<Enemy>().TakeDMG(bulletDMG);
                //  animacija se gasi jer se ona desava jedino kada delujemo damage
                //  ovo je popravka jer zombie konstantno je dobijao dmg i konstantno umirao
            }
            Destroy(gameObject);
        }
    }
    //void CreateBulleImpactEffect(Collision objectHit)
    //{
    //    instanciramo efekat na prvo mesto gde metak udari:
    //    ContactPoint kontakt = objectHit.contacts[0];
    //    objectHit je kolisn objekat i daje info o koliziji izmedju metka i objekta drugog
    //      objectHit.contacts[0] ovo je niz objekata tipa contactPoint gde[0] uzima prvu tacku kolizije

    //    GameObject hole = Instantiate(
    //          treba nam efekat prefab, a ne smemo da koristimo referencu na metak jer se metak sam po sebi instancira
    //          zato imamo globalnu referencu na koje zelimo da referenciramo
    //        GlobalReferenca.Instance.bulletImpactEffectPrefab, kontakt.point,
    //        Quaternion.LookRotation(kontakt.normal)
    //          ovo kreira rotaciju usperem prema kontakt.normal, kventerion roatcije gleda u odredjenom smeru kada mu se prosledi vektor
    //          kontakt.normal je normala(vektor pravilan) na povrsini objekta na mestu kontakta
    //        );
    //    rupa postaje dete objekta koji smo pogodili metkom
    //    hole.transform.SetParent(objectHit.gameObject.transform);
    //}
}
