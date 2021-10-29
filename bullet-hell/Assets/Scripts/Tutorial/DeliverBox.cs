using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverBox : MonoBehaviour, TutorialObject
{
    private Boolean completed = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isComplete()
    {
        return completed;
    }

    public string getHintText()
    {
        return "Deliver boxes to the lorry of the same colour.";
    }

    public GameObject getHoverPoint()
    {
        return this.gameObject;
    }

    public bool Completed
    {
        get => completed;
        set => completed = value;
    }
}
