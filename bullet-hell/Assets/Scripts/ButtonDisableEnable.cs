using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonDisableEnable : MonoBehaviour
{
    [SerializeField]
    private Sprite lockedImage;

    [SerializeField]
    private Sprite unlockedImage;

    public void DisableButton()
    {
        GetComponent<Button>().enabled = false;
        GetComponentInChildren<Image>().sprite = lockedImage;
    }

    public void EnableButton()
    {
        GetComponent<Button>().enabled = true;
        GetComponentInChildren<Image>().sprite = unlockedImage;
    }
}
