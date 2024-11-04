using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZombieSpawnController : MonoBehaviour
{
    public PlayerScript playerScript;
    //  za pamcenje da li je igrica predjena da se vrati na 0
    private bool completedAllWaves = false;

    public int zombiesPerWave = 5;
    public int currentZombiesPerWave;

    //  vreme razmliske izmedju spawnovanje zombija u talasu da idu jedan po jedan
    //  a ne odjednom svi jer mi se desava bug u unitiju
    public float spawnDelay = 0.5f;

    public int currentWave = 0;
    //  vreme izmedju talasa spawnovanja
    public float waveCooldown = 15.0f;

    //  boolean vrednost da li smo u cooldown ili ne
    public bool inCooldown;
    //  za UI da pise kuldaun:
    public float cooldownCounter = 0f;

    public GameObject zombiePrefab;

    //  pravimo listu zombija da broji zombije i da znamo kada je gotov wave:
    public List<Enemy> currentZombiesAlive;


    //  reference na text izmedju talasa
    public TextMeshProUGUI waveOverTextUI;
    public TextMeshProUGUI coolDownCounterUI;
    public TextMeshProUGUI currentWaveUI;

    // reference za UI
    public TextMeshProUGUI playerHealthUI;
    public TextMeshProUGUI ammoUI;

    //  za kraj igrice
    public FadeVictoryScreen fadeVictoryScreen;

    //  GAMEOVER tekst
    public GameObject gameOverUI;

    //  waveova za kraj igrice:
    private int wavesForGameOver = 15;

    //  provera da li je gameover:
    private bool gameOver = false;

    //  stop spawnovanja zombija
    private bool stopSpawning = false;

    //  boss
    public GameObject bossPrefab;
    private bool bossSpawned = false;
    private bool bossAlive = false;

    private void Start()
    {
        //  trenutni zombie uzima zombies per wave
        currentZombiesPerWave = zombiesPerWave;

        //  zatim skor postavljamo na nulu:
        GlobalReferenca.Instance.waveNumberForHighScore = currentWave;
        playerScript = FindObjectOfType<PlayerScript>();
        //  zapocni igru odmah
        StartNextWave();
    }
    private void Update()
    {
        //  uzimanje mrtvih zombija u novu praznu listu zombieToRemove
        List<Enemy> zombieToRemove = new List<Enemy>();
        //  prolazimo kroz zive zombije:
        foreach (Enemy zombieList in currentZombiesAlive)
        {
            if (zombieList.isDead)
            {
                //  dodajemo zombije ako su mrtvi u listu toRemove
                zombieToRemove.Add(zombieList);
            }
        }

        //  uklanjanje zombija:
        foreach (Enemy zombie in zombieToRemove)
        {
            //  uzimamo trenutno zivog zombija i pozivamo funckiju remove(zombija(enemy))
            currentZombiesAlive.Remove(zombie);
        }
        //  praznimo listu to reamove kada ih sve uklonimo
        zombieToRemove.Clear();


        //  zapocinjemo cooldown ako su svi mrtvi
        //  ako su svi mrtvi ==0 i ako nismo u cooldown onda zapocinjemo novi wave
        if (currentZombiesAlive.Count == 0 && inCooldown == false)
        {
            //  zapocinjemo novi cooldown za sledeci wave
            StartCoroutine(WaveCooldown());
        }

        //  pokretanje cooldowna
        if (inCooldown)
        {
            //  umanjivanje sekunde
            cooldownCounter -= Time.deltaTime;
        }
        else
        {
            //  resetovanje brojaca
            cooldownCounter = waveCooldown;
        }

        //  postavljamo da text cooldown bude isti kao i cooldownCounter
        coolDownCounterUI.text = cooldownCounter.ToString("F0");
        //  chatGPT hvala ti na F0
        //  ovde ne mora da se aktivira jer je dete textaUI

        if (currentWave == wavesForGameOver && currentZombiesAlive.Count == 0)
        {
            GameOver();
        }
    }
    private IEnumerator WaveCooldown()
    {
        //  postavljamo da jesmo u cooldown
        inCooldown = true;
        //  ovde ukljucujemo text za cooldown
        waveOverTextUI.gameObject.SetActive(true);
        //waveOverTextUI.text = $"The wave starts in:";
        if (playerScript != null && currentWave %2 ==0)
        {
            playerScript.HP = 100; // Postavljanje HP na maksimalnu vrednost
            playerScript.playerHealthUI.text = $"Health: {playerScript.HP}"; // azuriranje UI-a
        }
        //  ovo je delay za wave cooldown
        yield return new WaitForSeconds(waveCooldown);
        //  nakon waveCoolDown = 10 sekundi prodje posle ovog deleya gore
        //  isCoodown stvljamo na false
        inCooldown = false;
        //  zatim brisemo text
        waveOverTextUI.gameObject.SetActive(false);

        //  zatim da povecavamo zombie po talasu nakon cooldowna, posle svakog talasa 1 vise
        currentZombiesPerWave += 3;  // 5*2 itd itd
        //  zatim zapocinjemo metodu start next wave
        StartNextWave();
    }
    private void StartNextWave()
    {
        if (gameOver)
        {
            Debug.Log("The game is over");
            return;
        }
        else
        {
            /*ovde treba da ocistimo zive zombije, povecavamo broj talasa i 
         zapocinjemo karountin koji ce biti spawn*/
            currentZombiesAlive.Clear();
            currentWave++;

            //  zatim ovde updejutjemo highscore u globalnu referencu time da bi imali highscore
            //  wave number uzima currentWave, ispod je ++ da bi highsocre bio 1 a ne 0
            GlobalReferenca.Instance.waveNumberForHighScore = currentWave;

            // Ako je broj talasa veci od 2, zaustavi spawnovanje zombija
            if (currentWave > 2)
            {
                stopSpawning = true;
                Debug.Log("Stopping spawning after wave 8");
                if (!bossSpawned)
                {
                    StartCoroutine(SpawnBoss());
                    bossSpawned = true; // Ensure only one boss is spawned
                    waveOverTextUI.gameObject.SetActive(false);
                }
                return;
            }

            coolDownCounterUI.gameObject.SetActive(true);

            //  povecavamo textUI
            currentWaveUI.text = $"Wave: {currentWave}";
            StartCoroutine(SpawnWave());
        }

    }
    private IEnumerator SpawnWave()
    {
        if (stopSpawning)
        {
            //  gasenje counter ui
            coolDownCounterUI.gameObject.SetActive(false);
            // Ako je stopSpawning true, ne spawnuj zombije
            yield break;
        }
        //  spawnovanje zombija
        for (int i = 0; i < currentZombiesPerWave; i++)
        {
            Vector3 spawnPosition = transform.position;

            //  instanciranje zombija
            var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
            //  uzimanje skripte enemy od zombija, da se svakom zombiju nalepe informacije
            Enemy enemyScript = zombie.GetComponent<Enemy>();
            //  pracenje da li je zombie ziv
            currentZombiesAlive.Add(enemyScript);

            //  delay za spawnovanje da ne bude u isto vreme
            yield return new WaitForSeconds(spawnDelay);
        }
    }
    private IEnumerator SpawnBoss()
    {
        coolDownCounterUI.gameObject.SetActive(false);
        waveOverTextUI.gameObject.SetActive(false);

        yield return new WaitForSeconds(spawnDelay);

        Vector3 spawnPosition = transform.position;
        var boss = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
        Enemy bossScript = boss.GetComponent<Enemy>();
        currentZombiesAlive.Add(bossScript);

        bossAlive = true;

        // Boss will be the only enemy spawning now
        while (bossAlive)
        {
            // Check if the boss is dead
            if (bossScript.isDead)
            {
                bossAlive = false;
            }
            yield return null; // Wait until boss is defeated
        }

        GameOver();
    }
    private void GameOver()
    {
        if (gameOver)
        {
            return; // Game is already over
        }

        gameOver = true;
        completedAllWaves = currentWave == wavesForGameOver;

        // Start fade for victory screen
        fadeVictoryScreen.StartFade();

        // Stop background music and play victory sound
        SoundManager.Instance.actionBackgroundMusic.Stop();
        SoundManager.Instance.playeChannel.clip = SoundManager.Instance.playerVictoryMusic;
        SoundManager.Instance.playeChannel.PlayDelayed(1f);

        // Hide UI elements
        playerHealthUI.gameObject.SetActive(false);
        ammoUI.gameObject.SetActive(false);
        currentWaveUI.gameObject.SetActive(false);
        coolDownCounterUI.gameObject.SetActive(false);
        waveOverTextUI.gameObject.SetActive(false);

        Debug.Log("Victory! You have completed all waves!");
        StartCoroutine(GameOverUI());
    }
    private IEnumerator GameOverUI()
    {
        yield return new WaitForSeconds(1f);
        gameOverUI.gameObject.SetActive(true);

        //  na kraju igrice ti pokazuje tvoj skor:
        int waveSurvived = GlobalReferenca.Instance.waveNumberForHighScore;

        //  provera da li smo presli sve vejvove, ako jesmo skor je 0
        if (completedAllWaves)
        {
            // Ako je igras zavrsio sve talase, resetuj high score na nulu
            SaveLoadManager.Instance.SaveHighScore(0);
        }
        else
        {
            // Ako nije zavrsio sve talase, updejtuj high score ako je bolji
            if (waveSurvived - 1 > SaveLoadManager.Instance.LoadHighScore())
            {
                SaveLoadManager.Instance.SaveHighScore(waveSurvived - 1);
            }
        }
        //  povratak u mainmenu sa caroutine:
        StartCoroutine(ReturnToMainMenu());
    }
    private IEnumerator ReturnToMainMenu()
    {
        // Wait for 5 seconds to show the victory screen
        yield return new WaitForSeconds(8.0f);

        // Load the Main Menu scene
        SceneManager.LoadScene("MainMenu"); // Make sure to replace "MainMenu" with the actual name of your menu scene
    }
}