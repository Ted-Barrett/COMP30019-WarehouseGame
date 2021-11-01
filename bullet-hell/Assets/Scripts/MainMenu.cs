using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : BaseMenuScript {
    public void PlayGame() {
        GameObject levelSelectMenu = transform.parent.Find("LevelSelect").gameObject;
        levelSelectMenu.SetActive(true);
        levelSelectMenu.GetComponent<LevelSelectMenu>().LoadMenu();
        gameObject.SetActive(false);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void Settings()
    {
        GameObject settingsMenu = transform.parent.Find("Settings").gameObject;
        settingsMenu.SetActive(true);
        settingsMenu.GetComponent<SettingsMenu>().SetAllSliderValues();
        gameObject.SetActive(false);
    }
}
