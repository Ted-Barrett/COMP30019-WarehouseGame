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
///
/// This script should be placed on an empty object which has a plane as its child. The empty can be moved and rotated,
/// and the plane can be scaled. The plane should be positioned at (0,0,0) relative to the parent.
/// </summary>
public class Scatterer : MonoBehaviour
{
    [SerializeField] private GameObject[] sourceObjects; // objects to spawn

    [SerializeField] private int[] _weights; // likelihood that a given object will be chosen to spawn

    [SerializeField] private GameObject spawnPlane; // plane on which the objects spawn

    [SerializeField]
    private int _maxObjects; // maximum number of objects that will spawn (will be less if overlap disabled)

    [SerializeField] private bool allowOverlap = false; // allows overlapping object colliders

    private enum RotationType
    {
        None = 0,
        Square = 1,
        All = 2
    }

    [SerializeField] private RotationType rotationType = RotationType.None;

    private void Start()
    {
        // store original position and rotation
        Vector3 posOriginal = transform.position;
        Quaternion rotationOriginal = transform.rotation;

        // get scale of plane so that the area across which the objects are distributed can be calculated
        var localScale = spawnPlane.transform.localScale;
        float scaleX = localScale.x * 5; // multiplied by 5 to correspond by the default plane mesh
        float scaleZ = localScale.z * 5;

        // set the empty's position and rotation to 0 (will be moved back later)
        transform.position = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.identity;

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
                    spawnedObjects[i].AddComponent<Scatterable>(); // used to keep track of if the object should be destroyed

                    // rotates if necessary
                    if (rotationType == RotationType.Square)
                    {
                        spawnedObjects[i].transform.rotation = Quaternion.Euler(0, Random.Range(0, 3) * 90f, 0);
                    }
                    else if (rotationType == RotationType.All)
                    {
                        spawnedObjects[i].transform.rotation = Quaternion.Euler(0, Random.value * 360f, 0);
                    }

                    // sets object parent to the empty
                    spawnedObjects[i].transform.parent = gameObject.transform;

                    // sets MeshCollider to convex for more functional collision.
                    if (spawnedObjects[i].GetComponent<Collider>().GetType() == typeof(MeshCollider))
                    {
                        spawnedObjects[i].GetComponent<MeshCollider>().convex = true;
                    }

                    break;
                }
            }
        }

        // loops through all pairs of objects, and marks one object in a colliding pair for removal.
        if (!allowOverlap)
        {
            for (int i = 0; i < spawnedObjects.Length; i++)
            {
                var iScatterable = spawnedObjects[i].GetComponent<Scatterable>();

                if (iScatterable.setForRemoval)
                {
                    continue;
                }

                var iBounds = spawnedObjects[i].GetComponent<Collider>().bounds;

                for (int j = i + 1; j < spawnedObjects.Length; j++)
                {
                    if (iScatterable.setForRemoval)
                    {
                        break;
                    }

                    var jScatterable = spawnedObjects[j].GetComponent<Scatterable>();

                    if (jScatterable.setForRemoval)
                    {
                        continue;
                    }

                    var jBounds = spawnedObjects[j].GetComponent<Collider>().bounds;

                    if (iBounds.Intersects(jBounds))
                    {
                        iScatterable.setForRemoval = true;
                    }
                }
            }
        }

        // gets all the objects marked for removal
        var removeObjects = spawnedObjects.Where(o => o.GetComponent<Scatterable>().setForRemoval).ToArray();

        // removes marked objects in reverse list order
        for (int i = removeObjects.Length - 1; i >= 0; i--)
        {
            Destroy(removeObjects[i].gameObject);
        }

        // restores the empty to it's original position and rotation
        transform.position = posOriginal;
        transform.rotation = rotationOriginal;
    }
}