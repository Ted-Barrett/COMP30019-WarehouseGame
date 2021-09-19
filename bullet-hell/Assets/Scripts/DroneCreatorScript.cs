using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCreatorScript : MonoBehaviour {
    // The mesh renderer of spawning area, so we know where to drop the crates
    [SerializeField] private MeshRenderer spawnAreaMeshRenderer;
    private Bounds spawnBounds;

    [SerializeField] private GameObject droneWithCratePrefab;

    // Delay between crate spawns
    [SerializeField] private float spawnRate;

    void Start() {
        InvokeRepeating("DropCrate", 0.0f, spawnRate);
        spawnBounds = spawnAreaMeshRenderer.bounds;
    }

    void DropCrate() {
        float spawnHeight = 2;
        Vector3 spawnHeightOffset = Vector3.up * spawnHeight;
        // Spawn a drone holding a box
        GameObject newDrone = Instantiate(droneWithCratePrefab, spawnBounds.center + spawnHeightOffset, Quaternion.identity, this.transform);
        
        Vector3 target = new Vector3(
        Random.Range(spawnBounds.min.x, spawnBounds.max.x),
        spawnHeight,
        Random.Range(spawnBounds.min.z, spawnBounds.max.z));

        DroneHandlerScript droneHandlerScript = newDrone.GetComponent<DroneHandlerScript>();
        droneHandlerScript.target = target;
        
    }
}
