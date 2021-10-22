using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfLevelScript : MonoBehaviour {
    public GameObject nextLevelMenuUI;

    public void Enable() {
        nextLevelMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void NextLevel() {
        nextLevelMenuUI.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() {
        SceneManager.LoadScene("MainMenuScene");
    }
}
