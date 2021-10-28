using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    private GameObject audioSettings;

    void Start() {
        audioSettings = transform.Find("AudioSettings").gameObject;
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
        pauseMenuUI.SetActive(false);
        audioSettings.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void QuitGame() {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void AudioSettings()
    {
        pauseMenuUI.SetActive(false);
        audioSettings.SetActive(true);
        Slider[] sliders = audioSettings.GetComponentsInChildren<Slider>();
        
        foreach(Slider slider in sliders)
        {
            slider.gameObject.GetComponent<SliderScript>().SetSliderValue();
        }
    }

    public void BackAudioSettings()
    {
        audioSettings.SetActive(false);
        pauseMenuUI.SetActive(true);
    }
}
