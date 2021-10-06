using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCollector : MonoBehaviour
{
    
    [SerializeField]
    public BoxType boxeTypesToCollect;

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
        // handle
        IBoxContainer boxContainer = other.gameObject.GetComponent<IBoxContainer>();
        if (boxContainer != null) 
        {
            List<Box> boxes = boxContainer.UnloadBoxes(boxeTypesToCollect);
            boxes.ForEach(b => Destroy(b.gameObject));
        }
    }
}
