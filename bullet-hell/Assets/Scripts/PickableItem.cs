using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour
{

    [SerializeField]
    private Vector3 pickedUpTranslation;

    [SerializeField]
    private Quaternion pickedUpRotation;

    private Rigidbody rigidBody;

    public Vector3 GetPickedUpTranslation()
    {
        return pickedUpTranslation;
    }

    public Quaternion GetPickedUpRotation()
    {
        return pickedUpRotation;
    }

    public Rigidbody GetRigidBody()
    {
        return rigidBody;
    }

    // Start is called before the first frame update
    void Start()
    {  
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        GrabScript grabber = other.gameObject.GetComponent<GrabScript>();
        if (grabber != null)
        {
            grabber.AddItem(this);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        GrabScript grabber = other.gameObject.GetComponent<GrabScript>();
        if (grabber != null)
        {
            grabber.RemoveItem(this);
        }
    }
}
