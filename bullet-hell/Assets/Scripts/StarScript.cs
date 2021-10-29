using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StarScript : MonoBehaviour
{
    [SerializeField]
    private int reqScore;

    [SerializeField]
    private Sprite filledStar;

    void Awake() 
    {
        GetComponentInChildren<TextMeshProUGUI>().text = reqScore.ToString();
    }

    public int UpdateSprite(int score)
    {
        if(score >= reqScore)
        {
            gameObject.GetComponent<Image>().sprite = filledStar;
            return 1;
        }

        return 0;
    }
}
