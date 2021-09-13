using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Places objects from sourceObjects on to spawnPlane randomly. A higher weight will cause that object to be
/// more likely to spawn.
///
/// A given index in _weights corresponds to the same index in sourceObjects
/// </summary>
public class Scatterer : MonoBehaviour
{
    [SerializeField] private GameObject[] sourceObjects;

    [SerializeField] private int[] _weights;

    [SerializeField] private GameObject spawnPlane;

    [SerializeField] private int _maxObjects;

    [SerializeField] private bool allowOverlap = false;

    private enum RotationType
    {
        None = 0,
        Square = 1,
        All = 2
    }

    [SerializeField] private RotationType rotationType = RotationType.None;

    private void Start()
    {
        // store plane original position and rotation
        Vector3 planePosOriginal = spawnPlane.transform.position;
        Quaternion planeRotOriginal = spawnPlane.transform.rotation;

        // get scale of plane so that the area across which the objects are distributed can be calculated
        var localScale = spawnPlane.transform.localScale;
        float scaleX = localScale.x * 5; // multiplied by 5 to correspond by the default plane mesh
        float scaleZ = localScale.z * 5;

        // set the plane's position and rotation to 0 (will be moved back later)
        spawnPlane.transform.position = new Vector3(0, 0, 0);
        spawnPlane.transform.rotation = Quaternion.identity;

        int totalWeight = _weights.Sum(); // total probabilistic weight of objects
        int cumWeight; // cumulative probabilistic weight of objects 
        int rndWeight; // a random weight which will allow a certain object to be chosen

        GameObject[] spawnedObjects = new GameObject[_maxObjects]; // stores objects which have been spawned

        // spawns _maxObjects objects
        for (int i = 0; i < _maxObjects; i++)
        {
            rndWeight = Random.Range(1, totalWeight + 1);
            cumWeight = 0;
            for (int j = 0; j < _weights.Length; j++)
            {
                // keeps adding weights until cumWeight exceeds rndWeight, at which point the
                // object which caused rndWeight to be exceeded will be spawned.
                cumWeight += _weights[j];
                if (cumWeight >= rndWeight)
                {
                    // spawns object
                    spawnedObjects[i] = Instantiate(sourceObjects[j],
                        new Vector3(Random.Range(-scaleX, scaleX), 0, Random.Range(-scaleZ, scaleZ)),
                        Quaternion.identity);

                    // rotates if necessary
                    if (rotationType == RotationType.Square)
                    {
                        spawnedObjects[i].transform.rotation = Quaternion.Euler(0, Random.Range(0, 3) * 90f, 0);
                    }
                    else if (rotationType == RotationType.All)
                    {
                        spawnedObjects[i].transform.rotation = Quaternion.Euler(0, Random.value * 360f, 0);
                    }

                    // sets object parent to plane
                    spawnedObjects[i].transform.parent = spawnPlane.transform;

                    // sets MeshCollider to convex for more functional collision.
                    if (spawnedObjects[i].GetComponent<Collider>().GetType() == typeof(MeshCollider))
                    {
                        spawnedObjects[i].GetComponent<MeshCollider>().convex = true;
                    }

                    break;
                }
            }
        }

        if (!allowOverlap)
        {
            // finds collisions between objects and removes them
            foreach (var spawnedObject in spawnedObjects)
            {
                if (spawnedObject.IsDestroyed())
                {
                    continue;
                }

                var spawnedObjectBounds = spawnedObject.GetComponent<Collider>().bounds;
                foreach (var collidingObject in spawnedObjects)
                {
                    if (spawnedObject.IsDestroyed())
                    {
                        break;
                    }

                    if (collidingObject.IsDestroyed() || spawnedObject.Equals(collidingObject))
                    {
                        continue;
                    }

                    var collidingObjectBounds = collidingObject.GetComponent<Collider>().bounds;
                    if (spawnedObjectBounds.Intersects(collidingObjectBounds))
                    {
                        // destroys the smaller object
                        if (spawnedObjectBounds.size.sqrMagnitude > collidingObjectBounds.size.sqrMagnitude)
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
        }

        // returns the plane to it's original position
        spawnPlane.transform.position = planePosOriginal;
        spawnPlane.transform.rotation = planeRotOriginal;
    }
}