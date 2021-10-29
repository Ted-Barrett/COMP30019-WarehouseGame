using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HandlePlayerPrefs : MonoBehaviour
{
    private const float DEAULT_AUDIO_VAL = 1.0f;
    void Awake() 
    {
        foreach(PlayerPrefsKeys key in Enum.GetValues(typeof(PlayerPrefsKeys)))
        {
            if(!PlayerPrefs.HasKey(key.ToString()))
            {
                PlayerPrefs.SetFloat(key.ToString(), DEAULT_AUDIO_VAL);
            }
        }
    }
}
