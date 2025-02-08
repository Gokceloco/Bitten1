using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int currentLevelNo;
    public List<Level> levelPrebs;

    public Level currentLevel;
    public void RestartLevel()
    {
        DeleteCurrentLevel();
        CreateNewLevel(currentLevelNo - 1);
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
        currentLevel = Instantiate(levelPrebs[levelNo]);
        currentLevel.transform.position = Vector3.zero;
    }    
}
