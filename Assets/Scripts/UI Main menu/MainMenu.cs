using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //  uzimamo scenu SampleScene
    string newGameScene = "SampleScene";
    //  treba nam za highscore
    public TMP_Text highScoreUI;
    //  ovo gore je za rendering u 3d prostoru a TextMeshProUGUI je za 2d prostor

    public AudioClip bacgroundMusic;
    public AudioSource MainmenuMusic;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //  pokretanje muzike u mainmenu
        MainmenuMusic.PlayOneShot(bacgroundMusic);

        int highScore = SaveLoadManager.Instance.LoadHighScore();
        highScoreUI.text = $"High score: {highScore}";
    }

    public void StartNewGame()
    {
        MainmenuMusic.Stop();
        
        SceneManager.LoadScene(newGameScene);
    }

    public void ExitGame()
    {
        //  iz nekog razloga ovo ne radi pa moram #if Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
