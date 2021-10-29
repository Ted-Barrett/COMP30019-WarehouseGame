using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBox : MonoBehaviour, TutorialObject
{
    private Rigidbody _rigidbody;
    [SerializeField] private GameObject hoverPoint;
    [SerializeField] private DeliverBox _deliverBox;
    private void Start()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
    }

    public bool isComplete()
    {
        return (_rigidbody.isKinematic);
    }

    public string getHintText()
    {
        return "Pick up and drop boxes with spacebar. Throw with F.";
    }

    public GameObject getHoverPoint()
    {
        return hoverPoint;
    }

    private void OnDestroy()
    {
        _deliverBox.Completed = true;
    }
}
