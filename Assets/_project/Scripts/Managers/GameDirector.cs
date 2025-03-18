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
    public FailUI failUI;
    public VictoryUI victoryUI;
    public LevelUI levelUI;

    public GameState gameState;

    private void Start()
    {
        player.RestartPlayer();
        mainMenu.Show();
        gameState = GameState.MainMenu;
        player.FreezeRigidBody();
        audioManager.StopAmbientSound();
        StartAmbientSound();
        PlayerPrefs.SetInt("LastReachedLevel", Math.Max(PlayerPrefs.GetInt("LastReachedLevel"), 1));
        levelUI.SetLevelText(PlayerPrefs.GetInt("LastReachedLevel"));
    }

    private void StartAmbientSound()
    {
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
            PlayerPrefs.SetInt("LastReachedLevel", PlayerPrefs.GetInt("LastReachedLevel") + 1);
            RestartLevel();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerPrefs.SetInt("LastReachedLevel", PlayerPrefs.GetInt("LastReachedLevel") - 1);
            RestartLevel();
        }
    }

    public void RestartLevel()
    {
        levelManager.RestartLevel();
        player.RestartPlayer();
        Invoke(nameof(ChangeGameStateToGamePlay), .2f);
        levelUI.SetLevelText(PlayerPrefs.GetInt("LastReachedLevel"));
    }

    void ChangeGameStateToGamePlay()
    {
        gameState = GameState.GamePlay;
    }

    public void PlayerFailed()
    {
        failUI.Show();
        levelManager.currentLevel.StopAllEnemies();
        gameState = GameState.FailScreen;
    }

    public void LevelCompleted()
    {
        var lastReachedLevel = PlayerPrefs.GetInt("LastReachedLevel");
        PlayerPrefs.SetInt("LastReachedLevel", lastReachedLevel + 1);
        victoryUI.Show(levelManager.levelPrebs.Count == PlayerPrefs.GetInt("LastReachedLevel") - 1);
        levelManager.currentLevel.StopAllEnemies();
        gameState = GameState.VictoryScreen;
    }
}

public enum GameState
{
    GamePlay,
    FailScreen,
    VictoryScreen,
    MainMenu,
}