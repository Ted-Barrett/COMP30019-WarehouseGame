using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarControllerScript : MonoBehaviour
{
    private int numFilledStars = 0;

    public void UpdateStarSprites(int score)
    {
        Image[] starImages = GetComponentsInChildren<Image>();
        numFilledStars = 0;

        foreach(Image starImage in starImages)
        {
            numFilledStars += starImage.gameObject.GetComponent<StarScript>().UpdateSprite(score);
        }
    }

    public int GetNumFilledStars() { return numFilledStars; }
    
}
