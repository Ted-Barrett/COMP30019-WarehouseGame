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
        nextLevelMenuUI.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() {
        SceneManager.LoadScene("MainMenuScene");
    }
}
