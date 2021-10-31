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
        transform.parent.Find("Settings").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
