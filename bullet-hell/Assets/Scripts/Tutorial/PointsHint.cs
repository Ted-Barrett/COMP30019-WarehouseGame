using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsHint : MonoBehaviour, TutorialObject
{
    public bool isComplete()
    {
        return false;
    }

    public string getHintText()
    {
        return "Get as many points as you can before time runs out!";
    }

    public GameObject getHoverPoint()
    {
        return this.gameObject;
    }
}
