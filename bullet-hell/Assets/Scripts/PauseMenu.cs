using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : BaseMenuScript
{
    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame() {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void Settings()
    {
        GameObject settingsMenu = transform.parent.Find("Settings").gameObject;
        settingsMenu.SetActive(true);
        settingsMenu.GetComponent<SettingsMenu>().SetAllSliderValues();
        gameObject.SetActive(false);
    }
}
