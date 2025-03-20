using System;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public LevelManager levelManager;
    public FXManager fXManager;
    public AudioManager audioManager;
    public Player player;
    public TimeManager timeManager;

    [Header("UI")]
    public PlayerGotHitUI playerGotHitUI;
    public MainMenu mainMenu;
    public FailUI failUI;
    public VictoryUI victoryUI;
    public LevelUI levelUI;
    public TimerUI timerUI;
    public MessageUI messageUI;

    public GameState gameState;

    private void Start()
    {
        player.RestartPlayer(Vector3.zero);
        mainMenu.Show();
        timerUI.Hide();
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
            RestartLevel(Vector3.zero);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerPrefs.SetInt("LastReachedLevel", PlayerPrefs.GetInt("LastReachedLevel") + 1);
            RestartLevel(Vector3.zero);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerPrefs.SetInt("LastReachedLevel", PlayerPrefs.GetInt("LastReachedLevel") - 1);
            RestartLevel(Vector3.zero);
        }
    }

    public void RestartLevel(Vector3 startPos)
    {
        levelManager.RestartLevel();
        player.RestartPlayer(startPos);
        Invoke(nameof(ChangeGameStateToGamePlay), .2f);
        levelUI.SetLevelText(PlayerPrefs.GetInt("LastReachedLevel"));
        timeManager.StartTimeManager();
        if (levelManager.currentLevel.levelTimeInMin != 0)
        {
            var minText = "MINS";
            if (levelManager.currentLevel.levelTimeInMin == 1)
            {
                minText = "MIN";
            }

            messageUI.Show("YOU HAVE " + levelManager.currentLevel.levelTimeInMin + " " + minText + " TO FIND THE SERUM!", 4);
        }
    }

    void ChangeGameStateToGamePlay()
    {
        gameState = GameState.GamePlay;
    }

    public void LevelFailed(bool isTimeUp)
    {
        timerUI.Hide();
        failUI.Show(isTimeUp);
        levelManager.currentLevel.StopAllEnemies();
        gameState = GameState.FailScreen;
    }

    public void LevelCompleted()
    {
        timerUI.Hide();
        if (PlayerPrefs.GetInt("LastReachedLevel") < 5)
        {
            PlayerPrefs.SetInt("LastReachedLevel", PlayerPrefs.GetInt("LastReachedLevel") + 1);
            victoryUI.Show(false);
        }
        else
        {
            victoryUI.Show(true);
        }
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