using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownScript : MonoBehaviour {
    private TextMeshProUGUI TimerText;
    private Image ForegroundImage;
    [SerializeField] private int countdownDuration;
    private float timeLeft;

    [SerializeField] private EndOfLevelScript endOfLevelScript;
    [SerializeField] private ScoreScript scoreScript;


    void Start() {
        TimerText = this.transform.Find("TimerText").GetComponent<TextMeshProUGUI>();
        ForegroundImage = this.transform.Find("Wheel").GetComponent<Image>();
        timeLeft = countdownDuration;
    }

    void Update() {
        if (timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            DisplayTime(Mathf.CeilToInt(timeLeft));
        } else {
            endOfLevelScript.Enable(scoreScript.score);
        }
    }

    void DisplayTime(int time) {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        TimerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        ForegroundImage.fillAmount = (float)time / (float)this.countdownDuration;
    }
}
