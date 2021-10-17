using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTruck : MonoBehaviour, IBoxContainer
{

    [SerializeField]
    private BoxType boxType;

    [SerializeField]
    private Vector3[] stackedBoxTranslations;

    [SerializeField]
    private Vector3 stackedBoxRotation;

    private List<Box> boxes = new List<Box>();

    private bool active;

    public bool Active
    {
        set => active = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!active && boxes.Count > 0)
        {
            UnloadBoxes(boxType);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Box box = other.gameObject.GetComponent<Box>();
        if (box != null && active)
        {
            LoadBox(box);
        }
    }

    private void LoadBox(Box item)
    {
        if (item.Type == boxType && boxes.Count < stackedBoxTranslations.Length && !boxes.Contains(item))
        {
            boxes.Add(item);
            item.transform.SetParent(transform);
            item.RigidBody.isKinematic = true;
            item.transform.localPosition = stackedBoxTranslations[boxes.Count - 1];
            item.transform.localRotation = Quaternion.Euler(stackedBoxRotation);
        }
    }

    public void Hit()
    {
        List<Box> boxes = UnloadBoxes(boxType);
        boxes.ForEach(b =>
        {
            b.Explode();
            Destroy(b.gameObject);
        });
    }

    public List<Box> UnloadBoxes(BoxType type)
    {
        if (type != boxType)
        {
            return new List<Box>();
        }

        boxes.ForEach(b =>
        {
            b.RigidBody.isKinematic = false;
            b.transform.SetParent(null);
            b.RigidBody.AddForce(b.transform.forward * 2, ForceMode.VelocityChange);
        });
        List<Box> toReturn = boxes;
        boxes = new List<Box>();
        return toReturn;
    }
}
