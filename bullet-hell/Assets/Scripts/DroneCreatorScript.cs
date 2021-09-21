using UnityEngine;

public class DroneCreatorScript : MonoBehaviour {
    // The mesh renderer of spawning area, so we know where to drop the crates
    [SerializeField] private MeshRenderer crateSpawnArea;
    private Bounds crateSpawnBounds;

    [SerializeField] private GameObject droneWithCratePrefab;

    // Delay between crate spawns
    [SerializeField] private float spawnRate;

    [SerializeField] private MeshRenderer droneSpawnMeshRenderer;
    [SerializeField] private MeshRenderer droneSpawnExclusionMeshRenderer;

    [SerializeField] private float spawnHeight;

    void Start() {
        InvokeRepeating("DropCrate", 0.0f, spawnRate);
        crateSpawnBounds = crateSpawnArea.bounds;
    }

    void DropCrate() {
        // Spawn a drone holding a box
        GameObject newDrone = Instantiate(droneWithCratePrefab, GetNewLocationWithinBounds(), Quaternion.identity, this.transform);
        
        Vector3 target = new Vector3(
                Random.Range(crateSpawnBounds.min.x, crateSpawnBounds.max.x),
                spawnHeight,
                Random.Range(crateSpawnBounds.min.z, crateSpawnBounds.max.z));

        DroneHandlerScript droneHandlerScript = newDrone.GetComponent<DroneHandlerScript>();
        droneHandlerScript.target = target;
        droneHandlerScript.exitTarget = GetNewLocationWithinBounds();
    }

    Vector3 GetNewLocationWithinBounds() {
        // Bit of a hack, but should work...
        Bounds droneSpawnBounds = droneSpawnMeshRenderer.bounds;
        Bounds droneSpawnExclusionBounds = droneSpawnExclusionMeshRenderer.bounds;

        int loopCounter = 0;
        while (loopCounter < 100) {
            Vector3 newSpawn = new Vector3(
                Random.Range(droneSpawnBounds.min.x, droneSpawnBounds.max.x),
                droneSpawnExclusionBounds.min.y,
                Random.Range(droneSpawnBounds.min.z, droneSpawnBounds.max.z));

            if (!droneSpawnExclusionBounds.Contains(newSpawn)) {
                return new Vector3(newSpawn.x, spawnHeight, newSpawn.z);
            }

            loopCounter++;
        }

        print("Loop limit reached, you probably need to assign things in the editor.");
        return Vector3.up;
    }
}
