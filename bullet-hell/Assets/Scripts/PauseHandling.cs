using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseHandling : MonoBehaviour {
    public static bool GameIsPaused = false;
    private GameObject pauseMenuUI;
    private GameObject HUD;

    void Start() {
        pauseMenuUI = transform.Find("PauseMenu").gameObject;
        HUD = GameObject.Find("HUD").gameObject;
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
        
        HUD.SetActive(true);

        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause() {
        pauseMenuUI.SetActive(true);
        HUD.SetActive(false);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}
