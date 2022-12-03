using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { GS_PAUSEMENU, GS_GAME, GS_LEVELCOMPLETED, GS_GAME_OVER }

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState currentGameState = GameState.GS_PAUSEMENU;

    private void Awake()
    {
        instance = this;
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
    }

    private void SetGameState(GameState gameState)
    {
        currentGameState = gameState;
    }

    public void PauseMenu()
    {
        SetGameState(GameState.GS_PAUSEMENU);
    }

    public void InGame()
    {
        SetGameState(GameState.GS_GAME);
    }

    public void LevelCompleted()
    {
        SetGameState(GameState.GS_LEVELCOMPLETED);
    }

    public void GameOver()
    {
        SetGameState(GameState.GS_GAME_OVER);
    }
}
