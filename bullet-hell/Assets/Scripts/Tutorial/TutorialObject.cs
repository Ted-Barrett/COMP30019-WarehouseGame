using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TutorialObject
{
    public bool isComplete();

    public String getHintText();

    public GameObject getHoverPoint();
}
