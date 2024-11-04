using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; private set; }

    //  string u koji cuvamo neki highscore
    private string highScoreKey = "HighScore";
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
    //  metoda za save
    public void SaveHighScore(int score)
    {
        //  koristimo playerpref za cuvanje podataka, ugradjena u unity
        PlayerPrefs.SetInt(highScoreKey, score);

    }
    //  loading metoda koja sluzi za vracanje int vrednost highscora
    public int LoadHighScore()
    {
        //  kada zelimo load moramo naci highScoreKey
        if (PlayerPrefs.HasKey(highScoreKey))
        {
            return PlayerPrefs.GetInt(highScoreKey);
        }
        else
        {
            return 0;
            //  vracamo 0 jer nema highscore i nije ni bitan
        }

    }
}
