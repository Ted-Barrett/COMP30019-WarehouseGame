using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabScript : MonoBehaviour
{

    private PickableItem pickedItem;

    private List<PickableItem> pickableItems = new List<PickableItem>();

    public PickableItem GetPickedItem()
    {
        return pickedItem;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Action"))
        {

            PickableItem currentItem = pickedItem;
            if (currentItem != null)
            {
                DropItem(currentItem);
                RemoveItem(currentItem); // shuffle to end of queue
                AddItem(currentItem);
            }

            if (pickableItems.Count >= 1)
            {
                PickableItem nextPickup = pickableItems[0];
                if (!ReferenceEquals(nextPickup, currentItem))
                {
                    PickItem(nextPickup);
                }
            }
        }

        if (Input.GetButtonDown("Throw"))
        {
            PickableItem currentItem = pickedItem;
            if (currentItem != null)
            {
                ThrowItem(currentItem);
                RemoveItem(currentItem);
            }
        }
    }

    public void AddItem(PickableItem item)
    {
        foreach (PickableItem pickable in pickableItems)
        {
            if (ReferenceEquals(pickable, item))
            {
                return;
            }
        }
        pickableItems.Add(item);
    }

    public void RemoveItem(PickableItem item)
    {
        List<PickableItem> filtered = new List<PickableItem>();
        foreach (PickableItem pickable in pickableItems)
        {
            if (!ReferenceEquals(pickable, item))
            {
                filtered.Add(pickable);
            }
        }
        pickableItems = filtered;
    } 

    private void PickItem(PickableItem item)
    {
        pickedItem = item;
        item.GetRigidBody().isKinematic = true;
        item.transform.parent = transform;
        item.transform.localPosition = item.GetPickedUpTranslation();
        item.transform.localRotation = item.GetPickedUpRotation();
    }

    public void DropItem(PickableItem item)
    {
        pickedItem = null;
        item.transform.SetParent(null);
        item.GetRigidBody().isKinematic = false;
        item.GetRigidBody().AddForce(item.transform.forward * 2, ForceMode.VelocityChange);
    }

    public void ThrowItem(PickableItem item)
    {
        pickedItem = null;
        item.transform.SetParent(null);
        item.GetRigidBody().isKinematic = false;
        item.GetRigidBody().AddForce(item.transform.forward * 10, ForceMode.VelocityChange);
    }
}
