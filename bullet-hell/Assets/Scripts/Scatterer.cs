using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scatterer : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;

    [SerializeField] private int[] weights;

    [SerializeField] private GameObject spawnPlane;

    [SerializeField] private int numObjects;

    private void Start()
    {
        var clone = Instantiate(objects[0],new Vector3(0,0,0),Quaternion.identity);
    }
}
