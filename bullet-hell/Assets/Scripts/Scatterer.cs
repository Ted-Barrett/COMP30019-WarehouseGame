using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Scatterer : MonoBehaviour
{
    [SerializeField] private GameObject[] sourceObjects;

    [SerializeField] private int[] weights;

    [SerializeField] private GameObject spawnPlane;

    [SerializeField] private int numObjects;

    private enum RotationType
    {
        None = 0,
        Square = 1,
        All = 2
    }

    [SerializeField] private RotationType rotationType = RotationType.None;

    private void Start()
    {
        Vector3 planePosOriginal = spawnPlane.transform.position;
        Quaternion planeRotOriginal = spawnPlane.transform.rotation;

        spawnPlane.transform.position = new Vector3(0, 0, 0);
        spawnPlane.transform.rotation = Quaternion.identity;
        
        int totalWeight = weights.Sum();
        int cumWeight;
        int rndWeight = Random.Range(1, totalWeight);

        GameObject[] spawnedObjects = new GameObject[numObjects];

        for (int i = 0; i < numObjects; i++)
        {
            cumWeight = 0;
            for (int j = 0; j < weights.Length; j++)
            {
                cumWeight += weights[j];
                if (cumWeight >= rndWeight)
                {
                    spawnedObjects[i] = Instantiate(sourceObjects[j],
                        new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f)),
                        Quaternion.identity);
                    spawnedObjects[i].transform.parent = spawnPlane.transform;
                    
                    if (rotationType == RotationType.Square)
                    {
                        spawnedObjects[i].transform.rotation=Quaternion.Euler(0,Random.Range(0,3)*90f,0);
                    } else if (rotationType == RotationType.All)
                    {
                        spawnedObjects[i].transform.rotation=Quaternion.Euler(0,Random.value*360f,0);
                    }
                }
            }
        }

        spawnPlane.transform.position = planePosOriginal;
        spawnPlane.transform.rotation = planeRotOriginal;
    }
}