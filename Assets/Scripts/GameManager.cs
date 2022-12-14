using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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


    private void Awake()
    {
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
        }
        controlsText.SetActive(controlsShown);
    }

    private void SetGameState(GameState gameState)
    {
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
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("Level1");
    }
}
