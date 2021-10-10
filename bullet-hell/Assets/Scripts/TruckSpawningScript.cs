using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

/// <summary>
/// spawns and moves trucks from one road piece to another. Ignores orientation of road pieces, and just
/// points the truck from the start node to the end and translates it.
/// </summary>
public class TruckSpawningScript : MonoBehaviour
{
    // start and end GameObjects and associated positions
    [SerializeField] private Transform startRoad;
    private Vector3 _startPos;
    [SerializeField] private Transform endRoad;
    private Vector3 _endPos;
    // rotation of the truck
    private float _rotationY;
    
    [SerializeField] private float speed;

    // truck prefab
    [SerializeField] private GameObject truck;

    [SerializeField] [Tooltip("Time between spawns in seconds")] private float timeBetweenSpawns;

    private float _timeToNextSpawn;

    [SerializeField] [Tooltip("Between 0 and 1. 0 means no variation.")] private float spawnTimeVariation;

    private float _lastSpawn;

    private HashSet<GameObject> _spawnedTrucks;

    private Vector3 _translateDirection;

    private static readonly Vector3 RoadOffset = new Vector3(0f, -0.127868f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        // calculates movement vectors and truck rotation
        _startPos = startRoad.position+RoadOffset;
        _endPos = endRoad.position+RoadOffset;
        _translateDirection = (_endPos-_startPos).normalized;
        _rotationY = Quaternion.LookRotation(_translateDirection,Vector3.up).eulerAngles.y;
        
        // initialises truck hashset
        _spawnedTrucks = new HashSet<GameObject>();
        _spawnedTrucks.Add(Instantiate(truck, _startPos, Quaternion.Euler(0, _rotationY, 0)));
        _lastSpawn = Time.time;
        _timeToNextSpawn = timeBetweenSpawns * (1 + UnityEngine.Random.Range(-spawnTimeVariation,spawnTimeVariation));
    }

    // Update is called once per frame
    void Update()
    {
        _spawnedTrucks.RemoveWhere(objDestroyed); // removes destroyed trucks from hashset
        foreach (var spawnedTruck in _spawnedTrucks) // moves all trucks in relevant direction
        {
            spawnedTruck.transform.Translate(_translateDirection * (speed * Time.deltaTime),Space.World);
        }

        if (Time.time - _lastSpawn > _timeToNextSpawn) // spawns new truck
        {
            _lastSpawn = Time.time;
            _timeToNextSpawn = timeBetweenSpawns * (1 + UnityEngine.Random.Range(-spawnTimeVariation,spawnTimeVariation));
            _spawnedTrucks.Add(Instantiate(truck, _startPos, Quaternion.Euler(0, _rotationY, 0)));
        }
    }

    bool objDestroyed(GameObject i)
    {
        return i.IsDestroyed();
    }
}
