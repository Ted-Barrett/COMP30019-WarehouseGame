using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMenuScript : MonoBehaviour
{
    [SerializeField]
    private BaseMenuScript parentMenuScript;

    public void Back()
    {
        parentMenuScript.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
