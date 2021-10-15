using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreScript : MonoBehaviour {
    private TextMeshProUGUI ScoreText;
    private int score = 0;

    void Start() {
        ScoreText = this.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        UpdateScoreDisplay();
    }

    public void AddScore(int scoreToAdd) {
        score += scoreToAdd;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay() {
        ScoreText.text = score.ToString();
    }
}
