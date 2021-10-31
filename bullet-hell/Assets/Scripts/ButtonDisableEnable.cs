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

        transform.Find("Picture").gameObject.GetComponent<Image>().sprite = lockedImage;
    }

    public void EnableButton()
    {
        GetComponent<Button>().enabled = true;
        transform.Find("Picture").gameObject.GetComponent<Image>().sprite = unlockedImage;
    }
}
