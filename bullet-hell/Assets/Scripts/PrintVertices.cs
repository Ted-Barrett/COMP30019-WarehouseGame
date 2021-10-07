using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintVertices : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var vertices = this.GetComponent<MeshFilter>().mesh.vertices;

        foreach(Vector3 vec in vertices)
        {
            Debug.Log(vec.ToString("F7"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
