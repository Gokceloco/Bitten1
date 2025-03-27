using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public GameDirector gameDirector;

    private bool _isTimerOn;
    private bool _isTimerRedUIShowing;
    private float _remainingTime;

    public void RestartTimeManager()
    {
        var levelTime = gameDirector.levelManager.currentLevel.levelTimeInMin;
        if (levelTime != 0)
        {
            gameDirector.timerUI.Show();
            _isTimerOn = true;
            _remainingTime = levelTime * 60;
        }
        else
        {
            gameDirector.timerUI.Hide();
            _isTimerOn = false;
        }
        _isTimerRedUIShowing = false;
    }

    private void Update()
    {
        if (gameDirector.gameState != GameState.GamePlay)
        {
            return;
        }
        if (_isTimerOn && _remainingTime > 0)
        {
            _remainingTime -= Time.deltaTime;
            gameDirector.timerUI.UpdateTimer(_remainingTime 
                / (gameDirector.levelManager.currentLevel.levelTimeInMin * 60), _remainingTime);
        }
        if (!_isTimerRedUIShowing && _remainingTime <= 5 
            && gameDirector.levelManager.currentLevel.levelTimeInMin != 0)
        {
            gameDirector.timerRedUI.Show();
            _isTimerRedUIShowing = true;
        }
        if (_isTimerOn && _remainingTime <= 0)
        {
            TimeIsUp();
            _isTimerOn = false;
        }
    }

    private void TimeIsUp()
    {
        gameDirector.LevelFailed(true, 2);
    }
}
