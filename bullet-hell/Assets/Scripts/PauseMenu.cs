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

    public void AudioSettings()
    {
        transform.parent.Find("AudioSettings").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
