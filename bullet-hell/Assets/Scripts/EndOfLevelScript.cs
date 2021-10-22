using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class EndOfLevelScript : MonoBehaviour {
    public GameObject nextLevelMenuUI;

    public void Enable(int score) {
        nextLevelMenuUI.SetActive(true);
        nextLevelMenuUI.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = "You scored " + score + " points!";
        Time.timeScale = 0f;
    }

    public void NextLevel() {
        Time.timeScale = 1f;
        nextLevelMenuUI.SetActive(false);
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelIndex = currentLevelIndex < 5 ? currentLevelIndex + 1 : 0;
        SceneManager.LoadScene(nextLevelIndex);
    }

    public void QuitGame() {
        SceneManager.LoadScene("MainMenuScene");
    }
}
