using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameDirector gameDirector;
    public List<Level> levelPrebs;

    public Level currentLevel;
    public void RestartLevel()
    {
        DeleteCurrentLevel();
        CreateNewLevel(PlayerPrefs.GetInt("LastReachedLevel"));
    }

    private void DeleteCurrentLevel()
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }
    }

    private void CreateNewLevel(int levelNo)
    {
        currentLevel = Instantiate(levelPrebs[levelNo - 1]);
        currentLevel.transform.position = Vector3.zero;
        currentLevel.StartLevel(gameDirector.player);
    }    
}
