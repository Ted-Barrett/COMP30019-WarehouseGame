using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCollector : MonoBehaviour
{
    
    [SerializeField]
    public GameObject boxesToCollect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
       GrabScript grabScript = other.gameObject.GetComponent<GrabScript>();
        if (grabScript != null && grabScript.GetPickedItem() != null) 
        {
            PickableItem item = grabScript.GetPickedItem();
            if (item.gameObject.name.Contains(boxesToCollect.gameObject.name))
            {
                grabScript.DropItem(item);
                grabScript.RemoveItem(item);
                Destroy(item.gameObject);
            }
        }
    }
}
