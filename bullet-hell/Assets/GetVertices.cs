using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetVertices : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var vertices = this.GetComponent<MeshFilter>().mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            Debug.Log("Vertex #"+i+" = " + vertices[i].ToString("F7"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
