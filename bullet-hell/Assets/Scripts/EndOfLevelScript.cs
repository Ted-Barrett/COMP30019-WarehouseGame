using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;


public class EndOfLevelScript : MonoBehaviour {
    public GameObject nextLevelMenuUI;
    private const int STARS_REQ_FOR_NEXT_LVL = 1;
    private const float GREYED_OUT_FACTOR = 0.33f;

    public void Enable(int score) {
        nextLevelMenuUI.SetActive(true);
        nextLevelMenuUI.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = "You scored " + score + " points!";
        StarControllerScript starController = nextLevelMenuUI.GetComponentInChildren<StarControllerScript>();
        starController.UpdateStarSprites(score);

        if(starController.GetNumFilledStars() < STARS_REQ_FOR_NEXT_LVL)
        {
            GameObject nextLevelButton = nextLevelMenuUI.transform.Find("NextLevelButton").gameObject;
            TextMeshProUGUI textMeshPro = nextLevelButton.GetComponentInChildren<TextMeshProUGUI>();
            textMeshPro.color = new Color(textMeshPro.color.r * GREYED_OUT_FACTOR,
                                            textMeshPro.color.g * GREYED_OUT_FACTOR,
                                            textMeshPro.color.b * GREYED_OUT_FACTOR);

            nextLevelButton.GetComponent<Image>().enabled = false;
            nextLevelButton.GetComponent<Button>().enabled = false;
        }
        
        int nextLevelBuildIndex = SceneManager.GetActiveScene().buildIndex + 1;
            
        if(nextLevelBuildIndex <= LevelSelectMenu.MAX_BUILD_INDEX)
        {
            PlayerPrefs.SetFloat(((PlayerPrefsKeys)nextLevelBuildIndex).ToString(),
                                 HandlePlayerPrefs.UNLOCKED_VAL);
        }

        Time.timeScale = 0f;
    }

    public void NextLevel() {
        Time.timeScale = 1f;
        nextLevelMenuUI.SetActive(false);
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelIndex = currentLevelIndex < 4 ? currentLevelIndex + 1 : 0;
        SceneManager.LoadScene(nextLevelIndex);
    }

    public void RetryLevel() {
        Time.timeScale = 1f;
        nextLevelMenuUI.SetActive(false);
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevelIndex);
    }

    public void QuitGame() {
        SceneManager.LoadScene("MainMenuScene");
    }
}
