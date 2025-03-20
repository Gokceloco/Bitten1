using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public GameDirector gameDirector;

    private bool _isTimerOn;
    private float _remainingTime;

    public void StartTimeManager()
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
    }

    private void Update()
    {
        if (_isTimerOn && _remainingTime > 0)
        {
            _remainingTime -= Time.deltaTime;
            gameDirector.timerUI.UpdateTimer(_remainingTime 
                / (gameDirector.levelManager.currentLevel.levelTimeInMin * 60), _remainingTime);
        }
        if (_isTimerOn && _remainingTime <= 0)
        {
            TimeIsUp();            
        }
    }

    private void TimeIsUp()
    {
        gameDirector.LevelFailed(true);
    }
}
