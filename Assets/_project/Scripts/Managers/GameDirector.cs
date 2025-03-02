using System;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public LevelManager levelManager;
    public FXManager fXManager;
    public Player player;

    public GameState gameState;

    private void Start()
    {
        RestartLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            levelManager.currentLevelNo += 1;
            levelManager.currentLevelNo 
                = Mathf.Min(levelManager.currentLevelNo, levelManager.levelPrebs.Count);
            RestartLevel();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            levelManager.currentLevelNo -= 1;
            levelManager.currentLevelNo = Mathf.Max(1, levelManager.currentLevelNo);
            RestartLevel();
        }
    }

    private void RestartLevel()
    {
        levelManager.RestartLevel();
        player.RestartPlayer();
        gameState = GameState.GamePlay;
    }
}

public enum GameState
{
    GamePlay,
    FailScreen,
    VictoryScreen,
    MainMenu,
}