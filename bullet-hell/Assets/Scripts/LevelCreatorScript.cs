using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreatorScript : MonoBehaviour {
    [SerializeField]
    private float levelWidth;
    [SerializeField]
    private float levelHeight;
    [SerializeField]
    private Material floorMaterial;

    private void Start() {
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = floorMaterial;

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();

        Mesh mesh = new Mesh();

        float halfLevelWidth = levelWidth / 2;
        float halfLevelHeight = levelHeight / 2;

        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(halfLevelWidth, 0, halfLevelHeight),
            new Vector3(-halfLevelWidth, 0, halfLevelHeight),
            new Vector3(halfLevelWidth, 0, -halfLevelHeight),
            new Vector3(-halfLevelWidth, 0, -halfLevelHeight),
        };
        mesh.vertices = vertices;

        int[] triangles = new int[6]
        {
            // triangle 1
            0, 2, 1,
            // triangle 2
            1, 2, 3
        };
        mesh.triangles = triangles;

        Vector3[] normals = new Vector3[4]
        {
            Vector3.up,
            Vector3.up,
            Vector3.up,
            Vector3.up
        };
        mesh.normals = normals;

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        mesh.uv = uv;

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

        // Start the drone spawning
    }
}
