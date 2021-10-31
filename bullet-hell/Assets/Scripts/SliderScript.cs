using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    [SerializeField]
    private PlayerPrefsKeys sliderProperty;

    public void SetSliderValue()
    {
        GetComponent<Slider>().value = PlayerPrefs.GetFloat(sliderProperty.ToString());
    }

    public void OnSliderChange()
    {
        PlayerPrefs.SetFloat(sliderProperty.ToString(), GetComponent<Slider>().value);
    }
}
