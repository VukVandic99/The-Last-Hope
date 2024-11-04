using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReferenca : MonoBehaviour
{
    public static GlobalReferenca Instance { get; private set; }

    //  
    public GameObject bulletImpactEffectPrefab;
    public int waveNumberForHighScore;
    private void Awake()
    {
        //  ovo je pravljenje singletona
        if(Instance != null && Instance != this)
        {
            //  ako postoji instanca unistavamo je, treba nam samo jedna
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}

//Singleton je dizajn šablon koji se koristi da bi se obezbedilo da samo jedna instanca određene
//klase postoji u aplikaciji tokom njenog životnog ciklusa.
//Koristimo Singleton kada želimo da pristupimo globalnoj instanci te klase iz bilo kog mesta u kodu,
//bez potrebe da stalno pravimo nove objekte ili prosleđujemo referencu
