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

    private const bool DISABLE_LEVEL_ACCESS_WHEN_LOCKED = false;

    public void DisableButton()
    {
        if(DISABLE_LEVEL_ACCESS_WHEN_LOCKED)
        {
            GetComponent<Button>().enabled = false;
        }
        else
        {
            GetComponent<Button>().targetGraphic = null;
        }

        transform.Find("Picture").gameObject.GetComponent<Image>().sprite = lockedImage;
    }

    public void EnableButton()
    {
        GetComponent<Button>().enabled = true;
        transform.Find("Picture").gameObject.GetComponent<Image>().sprite = unlockedImage;
    }
}
