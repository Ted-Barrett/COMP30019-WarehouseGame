using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabScript : MonoBehaviour, IBoxContainer
{

    private PickableItem pickedItem;

    private List<PickableItem> pickableItems = new List<PickableItem>();

    public PickableItem PickedItem { get => pickedItem; }

    // Update is called once per frame
    void Update()
    {
        // hack for now to remove deleted objects from list
        filterPickable();
        if (Input.GetButtonDown("Action"))
        {

            PickableItem currentItem = pickedItem;
            if (currentItem != null)
            {
                DropItem(currentItem);
                RemoveItem(currentItem); // shuffle to end of queue
                AddItem(currentItem);
                return;
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

    private void filterPickable()
    {
        List<PickableItem> filtered = new List<PickableItem>();
        foreach (PickableItem pickable in pickableItems)
        {
            if (pickable)
            {
                filtered.Add(pickable);
            }
        }
        pickableItems = filtered;
    }

    public void Hit()
    {
        if (pickedItem != null)
        {
            Box box = pickedItem.GetComponent<Box>();
            if (box != null)
            {
                box.Explode();
                Destroy(pickedItem.gameObject);
                return;
            }


            HandTruck truck = pickedItem.GetComponent<HandTruck>();
            if (truck != null)
            {
                truck.Hit();
                return;
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
        item.RigidBody.isKinematic = true;
        item.transform.SetParent(transform);
        item.transform.localPosition = item.PickedUpTranslation;
        item.transform.localRotation = Quaternion.Euler(item.PickedUpRotation);

        HandTruck handTruck = item.GetComponent<HandTruck>();
        if (handTruck != null)
        {
            handTruck.Active = true;
        }
    }

    public void Drop()
    {
        if (pickedItem != null)
        {
            DropItem(pickedItem);
        }
    }

    public void DropItem(PickableItem item)
    {
        pickedItem = null;
        item.transform.SetParent(null);
        item.RigidBody.isKinematic = false;
        item.RigidBody.AddForce(item.transform.forward * 2, ForceMode.VelocityChange);

        HandTruck handTruck = item.GetComponent<HandTruck>();
        if (handTruck != null)
        {
            handTruck.Active = false;
        }
    }

    public List<Box> UnloadBoxes(BoxType type)
    {
        List<Box> unloaded = new List<Box>();
        if (pickedItem != null)
        {
            Box pickedBox = pickedItem.gameObject.GetComponent<Box>();
            if (pickedBox != null && pickedBox.Type == type)
            {
                RemoveItem(pickedItem);
                DropItem(pickedItem);
                unloaded.Add(pickedBox);
            }
        }
        return unloaded;
    }

    public void ThrowItem(PickableItem item)
    {
        pickedItem = null;
        item.transform.SetParent(null);
        item.RigidBody.isKinematic = false;
        item.RigidBody.AddForce(item.transform.forward * 10, ForceMode.VelocityChange);

        HandTruck handTruck = item.GetComponent<HandTruck>();
        if (handTruck != null)
        {
            handTruck.Active = false;
        }
    }
}
