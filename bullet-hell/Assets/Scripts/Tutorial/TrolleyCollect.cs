using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrolleyCollect : MonoBehaviour, TutorialObject
{

    [SerializeField] private GameObject hoverPoint;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
    }

    public bool isComplete()
    {
        return _rigidbody.isKinematic;
    }

    public string getHintText()
    {
        return "Coloured trolleys can collect up to 3 boxes at once!";
    }

    public GameObject getHoverPoint()
    {
        return hoverPoint;
    }
}
