using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectMenu : BaseMenuScript
{
    private const int TUTORIAL_BUILD_INDEX = 1;
    private const int DIAGONALS_BUILD_INDEX = 2;
    private const int UPHILL_BUILD_INDEX = 3;
    private const int HIGHRISE_BUILD_INDEX = 4;

    public void LoadMenu()
    {
        for(int i = HandlePlayerPrefs.LEVEL_START_INDEX; 
            i < Enum.GetNames(typeof(PlayerPrefsKeys)).Length;
            i++)
        {
            string levelName = ((PlayerPrefsKeys)i).ToString();

            if(PlayerPrefs.GetFloat(levelName) == 1.0f)
            {
                transform.Find(levelName).GetComponent<ButtonDisableEnable>().DisableButton();
            }
            else
            {
                transform.Find(levelName).GetComponent<ButtonDisableEnable>().EnableButton();
            }
        }
    }

    public void Tutorial()
    {
        SceneManager.LoadScene(TUTORIAL_BUILD_INDEX);
    }

    public void Diagonals()
    {
        SceneManager.LoadScene(DIAGONALS_BUILD_INDEX);
    }

    public void Uphill()
    {
        SceneManager.LoadScene(UPHILL_BUILD_INDEX);
    }

    public void Highrise()
    {
        SceneManager.LoadScene(HIGHRISE_BUILD_INDEX);
    }
   
    private void DisableLevel(int levelIndex)
    {
        string levelName = ((PlayerPrefsKeys)levelIndex).ToString();
        GameObject buttonObject = transform.Find(levelName).gameObject;
        
    }
}
