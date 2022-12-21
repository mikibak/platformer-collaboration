using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState { GS_PAUSEMENU, GS_GAME, GS_LEVELCOMPLETED, GS_GAME_OVER }

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState currentGameState = GameState.GS_PAUSEMENU;

    public Canvas InGameCanvas;
    public Canvas PauseMenuCanvas;
    public Canvas GameOverCanvas;
    public Canvas LevelCompletedCanvas;

    public GameObject controlsText;
    private bool controlsShown = false;
    
    public GameObject player;
    private ScoreManager playerScoreManager;
    private PlayerController playerController;

    private int score;
    public int highScore;
    public static string keyHighScore = "HighscoreLevel1"; //key for player prefs saving
    public Text scoreText;
    public Text highScoreText;


    private void Start()
    {
        playerScoreManager = player.GetComponent<ScoreManager>();
        playerController = player.GetComponent<PlayerController>();
    }

    private void Awake()
    {
        if(PlayerPrefs.HasKey(keyHighScore)==false) {
            PlayerPrefs.SetInt(keyHighScore, 0);
        }
        instance = this;
        DisableAllCanvases();
        //PauseMenuCanvas.enabled = true;
        InGame();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(currentGameState == GameState.GS_PAUSEMENU)
            {
                InGame();
            }
            else if(currentGameState == GameState.GS_GAME)
            {
                PauseMenu();
            }
            else if(currentGameState == GameState.GS_LEVELCOMPLETED)
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
    }

    public void PauseMenu()
    {
        SetGameState(GameState.GS_PAUSEMENU);
        DisableAllCanvases();
        PauseMenuCanvas.enabled = true;
    }

    public void InGame()
    {
        SetGameState(GameState.GS_GAME);
        DisableAllCanvases();
        InGameCanvas.enabled = true;
    }

    public void LevelCompleted()
    {
        score = playerScoreManager.score + playerController.health;
        Scene currentScene;
        currentScene = SceneManager.GetActiveScene();
        if(currentScene.name == "Level1") {
            highScore = PlayerPrefs.GetInt(keyHighScore);
            if(highScore < score) {
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

    public void ResetGame()
    {
        SceneManager.LoadScene("Level1");
    }
}
