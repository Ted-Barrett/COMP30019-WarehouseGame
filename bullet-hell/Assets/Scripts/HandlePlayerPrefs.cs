using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum PlayerPrefsKeys
{
    MusicVolume,
    SFXVolume,
    Diagonals,
    Uphill,
    Highrise
};



public class HandlePlayerPrefs : MonoBehaviour
{
    public const int LEVEL_START_INDEX = ((int)PlayerPrefsKeys.Diagonals);
    private const float DEFAULT_VAL = 1.0f;
    void Awake() 
    {
        foreach(PlayerPrefsKeys key in Enum.GetValues(typeof(PlayerPrefsKeys)))
        {
            if(!PlayerPrefs.HasKey(key.ToString()))
            {
                PlayerPrefs.SetFloat(key.ToString(), DEFAULT_VAL);
            }
        }
    }
}
