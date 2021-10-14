using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCollector : MonoBehaviour
{
    
    [SerializeField]
    public GameObject boxesToCollect;

    private GrabScript _grabScript;
    // Start is called before the first frame update
    void Start()
    {
        _grabScript = GameObject.FindWithTag("Player").GetComponent<GrabScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && _grabScript.GetPickedItem() != null) 
        {
            PickableItem item = _grabScript.GetPickedItem();
            if (item.gameObject.name.Contains(boxesToCollect.gameObject.name))
            {
                _grabScript.DropItem(item);
                _grabScript.RemoveItem(item);
                Destroy(item.gameObject);
            }
        }
    }
}
