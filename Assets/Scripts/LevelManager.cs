using System.Collections;
using System.Collections.Generic;
using Lucky.Kits.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    public int currentLevel = -1;
    public int totalLevels = 20;

    public void LoadLevel(int i)
    {
        currentLevel = i;
        SceneManager.LoadScene($"Level{i}");
    }

    public void LoadNextLevel()
    {
        if (currentLevel < totalLevels)
            LoadLevel(currentLevel + 1);
        else
        {
            SceneManager.LoadScene("End");
        }
    }
}