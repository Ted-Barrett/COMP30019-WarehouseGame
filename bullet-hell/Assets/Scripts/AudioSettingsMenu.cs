using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AudioSettingsMenu : BaseMenuScript
{
    public void SetAllSliderValues()
    {
        Slider[] sliders = GetComponentsInChildren<Slider>();
        
        foreach(Slider slider in sliders)
        {
            slider.gameObject.GetComponent<SliderScript>().SetSliderValue();
        }
    }
}
