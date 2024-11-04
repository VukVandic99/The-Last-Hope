using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerScript : MonoBehaviour
{
    public int HP = 100;
    //  referenca na ekran da bude krvav pri udarcu
    public GameObject bloodyScreen;
    //  referemca ma health igraca
    public TextMeshProUGUI playerHealthUI;
    
    //  GAMEOVER tekst:
    public GameObject gameOverUI;

    //  boolean vrednost da li je igrac mrtav;
    public bool isDead;
    //private Animator playerAnimator;
    private void Start()
    {
        //playerAnimator = transform.GetComponentInChildren<Animator>();
        //  iz nekog razloga ne radi ovde 
        //playerHealthUI.text = $"Health: {HP}";
        SoundManager.Instance.actionBackgroundMusic.PlayOneShot
            (SoundManager.Instance.actionMusic);
    }

    public void TakeDMG(int dmgAmount)
    {
        HP-=dmgAmount;
        if (HP <= 0)
        {
            print("Player dead");
            PlayerDead();
            isDead = true;
        }
        else
        {
            print("Player hit");
            //  posle svakog hita dobijamo bloody screan
            StartCoroutine(BloodyScreenEffect());
            playerHealthUI.text = $"Health: {HP}";

            //  pozivanje zvuka na udarac
            SoundManager.Instance.playeChannel.PlayOneShot
                (SoundManager.Instance.playerHit);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieAttackingHand"))
        {
            if (isDead == false)
            {
                //  necemo da hardkodujemo dmg vec kreiracemo novu skriptu
                //  zombie i zombieHand
                TakeDMG(other.gameObject.GetComponent<ZombieHand>().damage);
                //TakeDMG(25);
            }
            
        }
    }
    private IEnumerator BloodyScreenEffect()
    {
        //  provera:
        if(bloodyScreen.activeInHierarchy == false)
        {
            bloodyScreen.SetActive(true);  
        }
        //  delay za animaciju, da nakon npr 4 sec se ugasi:
        yield return new WaitForSeconds(2.5f);

        if (bloodyScreen.activeInHierarchy)
        {
            bloodyScreen.SetActive(false);
        }
    }
    private void PlayerDead()
    {
        //  pozivanje zvuka na smrt
        SoundManager.Instance.playeChannel.PlayOneShot
            (SoundManager.Instance.playerDeath);

        SoundManager.Instance.actionBackgroundMusic.Stop();
        //  pozivanje muzike kad umre igrac, sa delay nakon par sekundi
        SoundManager.Instance.playeChannel.clip =
            SoundManager.Instance.playerDeadMusic;
        SoundManager.Instance.playeChannel.PlayDelayed(1f);
        //  gasimo skriptu playerMovement i cameraMovenet
        GetComponent<PlayerMovement>().enabled = false;

        //  animacija za umiranje
        GetComponentInChildren<Animator>().enabled = true;
        playerHealthUI.gameObject.SetActive(false);
        //  pokrecemo fade screen
        GetComponent<FadeDeathScreen>().StartFade();
        //  pokrecemo nov koruntin za prikas Dead na ekranu
        StartCoroutine(GameOverUI());
    }

    private IEnumerator GameOverUI()
    {
        yield return new WaitForSeconds(1f);
        gameOverUI.gameObject.SetActive(true);

        //  na kraju igrice ti pokazuje tvoj skor:
        int waveSurvived = GlobalReferenca.Instance.waveNumberForHighScore;
        //  uzimamo poslednji talas ne talas na kojem smo
        if(waveSurvived-1 > SaveLoadManager.Instance.LoadHighScore())
        {
            SaveLoadManager.Instance.SaveHighScore(waveSurvived - 1);
        }

        //  povratak u mainmenu sa caroutine:
        StartCoroutine(ReturnToMainMenu());
    }
    private IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(6f);

        SceneManager.LoadScene("MainMenu");
    }
}
