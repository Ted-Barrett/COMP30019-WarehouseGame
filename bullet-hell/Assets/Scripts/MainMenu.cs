using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : BaseMenuScript {
    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
