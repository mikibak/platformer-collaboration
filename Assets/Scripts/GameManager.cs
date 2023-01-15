using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState { GS_PAUSEMENU, GS_GAME, GS_LEVELCOMPLETED, GS_GAME_OVER, GS_OPTIONS }

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState currentGameState = GameState.GS_PAUSEMENU;

    public Canvas InGameCanvas;
    public Canvas PauseMenuCanvas;
    public Canvas GameOverCanvas;
    public Canvas LevelCompletedCanvas;
    public Canvas OptionsCanvas;

    public GameObject controlsText;
    private bool controlsShown = false;

    public GameObject player;
    private ScoreManager playerScoreManager;
    private PlayerController playerController;
    public Text qualityText;
    public Slider volumeSlider;

    private int score;
    public int highScore;
    public static string keyHighScore = "HighscoreLevel1"; //key for player prefs saving
    public Text scoreText;
    public Text highScoreText;
    private string qualityName;

    public GameObject checkpointText;

    //tutorial
    public GameObject TutorialCanvas;
    public GameObject TutText1;
    public GameObject TutText2;


    private void Start()
    {
        playerScoreManager = player.GetComponent<ScoreManager>();
        playerController = player.GetComponent<PlayerController>();
    }

    private void Awake()
    {
        if (PlayerPrefs.HasKey(keyHighScore) == false) {
            PlayerPrefs.SetInt(keyHighScore, 0);
        }
        instance = this;
        DisableAllCanvases();
        //PauseMenuCanvas.enabled = true;
        ChangeQualityName();
        Tutorial();
        //volumeSlider.onValueChanged = SetVolume(volumeSlider.value);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentGameState == GameState.GS_PAUSEMENU)
            {
                InGame();
            }
            else if (currentGameState == GameState.GS_GAME)
            {
                PauseMenu();
            }
            else if (currentGameState == GameState.GS_LEVELCOMPLETED)
            {
                LevelCompleted();
            }
        }
    }

    private void SetGameState(GameState gameState)
    {
        //LevelCompletedCanvas.enabled = (currentGameState == GameState.GS_LEVELCOMPLETED)
        currentGameState = gameState;
    }

    private void DisableAllCanvases()
    {
        InGameCanvas.enabled = false;
        PauseMenuCanvas.enabled = false;
        GameOverCanvas.enabled = false;
        LevelCompletedCanvas.enabled = false;
        OptionsCanvas.enabled = false;
    }

    public void PauseMenu()
    {
        SetGameState(GameState.GS_PAUSEMENU);
        DisableAllCanvases();
        PauseMenuCanvas.enabled = true;
        Time.timeScale = 0;
    }

    public void InGame()
    {
        SetGameState(GameState.GS_GAME);
        DisableAllCanvases();
        InGameCanvas.enabled = true;
        Time.timeScale = 1;
    }

    public void LevelCompleted()
    {
        score = playerScoreManager.score + playerController.health;
        Scene currentScene;
        currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Level1") {
            highScore = PlayerPrefs.GetInt(keyHighScore);
            if (highScore < score) {
                Debug.Log("Setting Highscore");
                PlayerPrefs.SetInt(keyHighScore, score);
                highScore = score;
            }
        }
        scoreText.text = "SCORE: " + score.ToString();
        highScoreText.text = "HIGH SCORE: " + highScore.ToString();

        SetGameState(GameState.GS_LEVELCOMPLETED);
        DisableAllCanvases();
        LevelCompletedCanvas.enabled = true;
    }

    public void GameOver()
    {
        SetGameState(GameState.GS_GAME_OVER);
        DisableAllCanvases();
        GameOverCanvas.enabled = true;
        AudioSource audio = GameOverCanvas.GetComponent<AudioSource>();
        audio.Play();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Controls()
    {
        controlsShown = !controlsShown;
        controlsText.SetActive(controlsShown);
    }

    public void Options()
    {
        SetGameState(GameState.GS_OPTIONS);
        DisableAllCanvases();
        OptionsCanvas.enabled = true;
        Time.timeScale = 0;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void SetHigherGraphics()
    {
        QualitySettings.IncreaseLevel();
        ChangeQualityName();
    }

    public void SetLowerGraphics()
    {
        QualitySettings.DecreaseLevel();
        ChangeQualityName();
    }

    public void ChangeQualityName()
    {
        qualityName = QualitySettings.names[QualitySettings.GetQualityLevel()];
        qualityText.text = "QUALITY: " + qualityName;
    }

    public void SetVolume()
    {
        AudioListener.volume = volumeSlider.value;
    }

    public void ShowCheckpointText()
    {
        checkpointText.SetActive(true);
        Invoke("HideCheckpointText", 3);
    }

    private void HideCheckpointText()
    {
        checkpointText.SetActive(false);
    }

    public void Tutorial()
    {
        Time.timeScale = 0;
        TutorialCanvas.SetActive(true);
        TutText1.SetActive(true);
        TutText2.SetActive(false);
    }

    public void nextTut()
    {
        TutText2.SetActive(true);
        TutText1.SetActive(false);
    }

    public void EndTut()
    {
        TutorialCanvas.SetActive(false);
        TutText2.SetActive(false);
        InGame();
    }
}
