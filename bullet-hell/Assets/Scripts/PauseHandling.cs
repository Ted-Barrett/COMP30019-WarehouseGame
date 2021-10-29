using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseHandling : MonoBehaviour {
    public static bool GameIsPaused = false;
    private GameObject pauseMenuUI;

    void Start() {
        pauseMenuUI = transform.Find("PauseMenu").gameObject;
        Resume();
    }
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameIsPaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void Resume() {
        foreach(BaseMenuScript menu in GetComponentsInChildren<BaseMenuScript>())
        {
            menu.gameObject.SetActive(false);
        }

        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}
