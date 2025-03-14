using System;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public LevelManager levelManager;
    public FXManager fXManager;
    public AudioManager audioManager;
    public Player player;

    [Header("UI")]
    public PlayerGotHitUI playerGotHitUI;
    public MainMenu mainMenu;

    public GameState gameState;

    private void Start()
    {
        //RestartLevel();
        player.RestartPlayer();
        mainMenu.Show();
        gameState = GameState.MainMenu;
        player.FreezeRigidBody();
        audioManager.StopAmbientSound();
        if (UnityEngine.Random.value < .5f)
        {
            audioManager.PlayAmbiantSound();
        }
        else
        {
            audioManager.PlayAmbiantSound2();
        }
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

    public void RestartLevel()
    {
        levelManager.RestartLevel();
        player.RestartPlayer();
    }
}

public enum GameState
{
    GamePlay,
    FailScreen,
    VictoryScreen,
    MainMenu,
}