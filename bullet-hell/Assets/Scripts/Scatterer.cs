using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

        var localScale = spawnPlane.transform.localScale;
        float scaleX = localScale.x*5;
        float scaleZ = localScale.z*5;

        spawnPlane.transform.position = new Vector3(0, 0, 0);
        spawnPlane.transform.rotation = Quaternion.identity;
        
        int totalWeight = weights.Sum();
        int cumWeight;
        int rndWeight;

        GameObject[] spawnedObjects = new GameObject[numObjects];

        for (int i = 0; i < numObjects; i++)
        {
            rndWeight = Random.Range(1, totalWeight+1);
            cumWeight = 0;
            for (int j = 0; j < weights.Length; j++)
            {
                cumWeight += weights[j];
                if (cumWeight >= rndWeight)
                {
                    spawnedObjects[i] = Instantiate(sourceObjects[j],
                        new Vector3(Random.Range(-scaleX, scaleX), 0, Random.Range(-scaleZ, scaleZ)),
                        Quaternion.identity);

                    if (rotationType == RotationType.Square)
                    {
                        spawnedObjects[i].transform.rotation = Quaternion.Euler(0, Random.Range(0, 3) * 90f, 0);
                    }
                    else if (rotationType == RotationType.All)
                    {
                        spawnedObjects[i].transform.rotation = Quaternion.Euler(0, Random.value * 360f, 0);
                    }

                    spawnedObjects[i].transform.parent = spawnPlane.transform;
                    break;
                }
            }
        }
        
        foreach (var spawnedObject in spawnedObjects)
        {
            if (spawnedObject.IsDestroyed())
            {
                continue;
            }
            var spawnedObjectBounds = spawnedObject.GetComponent<Collider>().bounds;
            foreach (var collidingObject in spawnedObjects)
            {
                if (spawnedObject.Equals(collidingObject))
                {
                    continue;
                }

                if (spawnedObject.IsDestroyed())
                {
                    break;
                }
        
                if (collidingObject.IsDestroyed())
                {
                    continue;
                }
        
        
                var collidingObjectBounds = collidingObject.GetComponent<Collider>().bounds;
                if (spawnedObjectBounds.Intersects(collidingObjectBounds))
                {
                    if (spawnedObjectBounds.size.sqrMagnitude>collidingObjectBounds.size.sqrMagnitude)
                    {
                        Destroy(collidingObject);
                    }
                    else
                    {
                        Destroy(spawnedObject);
                    }
                }
            }
        }

        spawnPlane.transform.position = planePosOriginal;
        spawnPlane.transform.rotation = planeRotOriginal;
    }
}