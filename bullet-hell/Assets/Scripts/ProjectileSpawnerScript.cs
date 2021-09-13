using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawnerScript : MonoBehaviour {
    [SerializeField]
    private ProjectileScript projectileObject;

    [SerializeField]
    private float spawnRate;

    [SerializeField]
    private float projectileSpeed;
    [SerializeField]
    private float projectileRotationSpeed;

    void Start() {
        InvokeRepeating("CreateProjectile", 2.0f, 0.3f);
    }

    void CreateProjectile() {
        ProjectileScript newProjectile = Instantiate(projectileObject, this.transform.position + Vector3.up + Vector3.right, Quaternion.identity, this.transform);
        Vector3 projectileDirection = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
        newProjectile.Initialise(projectileSpeed, projectileRotationSpeed, projectileDirection);
    }
}
