using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateAudio : MonoBehaviour
{
    [SerializeField]
    private PlayerPrefsKeys audioProperty;
    void Update()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat(audioProperty.ToString());
    }
}
