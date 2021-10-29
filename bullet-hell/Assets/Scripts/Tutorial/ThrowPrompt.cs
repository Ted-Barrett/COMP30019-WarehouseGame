using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowPrompt : MonoBehaviour, TutorialObject
{
    private bool complete = false;

    private void Update()
    {
        if (Input.GetAxisRaw("Throw") > 0.5)
        {
            complete = true;
        }
    }

    public bool isComplete()
    {
        return complete;
    }

    public string getHintText()
    {
        return "Press F to throw boxes or trolleys";
    }

    public GameObject getHoverPoint()
    {
        return this.gameObject;
    }
}
