using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEditor;

public class CreateBoxOutlineMesh : MonoBehaviour
{
    struct CornerInfo
    {
        public CornerInfo(int i, bool r, bool t, bool f) : this()
        {
            index = i;
            top = t;
            right = r;
            front = f;
        }

        public int index;
        public bool top;
        public bool right;
        public bool front;
        public Vector3 pos;
        public Vector3 outlineNorm;
    }

    private Vector3 centre = new Vector3(0.0f, 0.001135f, 0.0f);

    void Start()
    {
        CornerInfo[] corners = GetCornerInfo();
        Mesh mesh = this.GetComponent<MeshFilter>().mesh;
        SetPositions(corners, mesh);
        SetOutlineNormals(corners, mesh);

        Vector3[] vertices = GetNewVertices(corners);
        Vector3[] outlineNormals = GetOutlineNormals(corners);
        int[] indices = GetNewIndices(corners);
        Vector2[] cull = new Vector2[vertices.Length];


        Mesh newMesh = new Mesh();
        newMesh.vertices = vertices;
        newMesh.SetIndices(indices, MeshTopology.Lines, 0);
        newMesh.SetUVs(3, outlineNormals);
        newMesh.SetUVs(1, cull);

        AssetDatabase.CreateAsset(newMesh, "Assets/Fbx/boxOutline.asset");
        AssetDatabase.SaveAssets();
    }

    private void SetOutlineNormals(CornerInfo[] corners, Mesh mesh) 
    {
        Vector3 outlineNorm;
        for (int i = 0; i < corners.Length; i++)
        {
            outlineNorm = Vector3.zero;

            if(corners[i].top) outlineNorm += Vector3.up;
            else outlineNorm += Vector3.down;

            if(corners[i].front) outlineNorm += Vector3.forward;
            else outlineNorm += Vector3.back;

            if(corners[i].right) outlineNorm += Vector3.right;
            else outlineNorm += Vector3.left;

            corners[i].outlineNorm = outlineNorm.normalized;
        }
    }

    private CornerInfo[] GetCornerInfo() {

        return new CornerInfo[] 
                    {
                        new CornerInfo(1, true, true, true),
                        new CornerInfo(4, true, false, true),
                        new CornerInfo(8, false, false, true),
                        new CornerInfo(10, false, true, true),
                        new CornerInfo(13, false, true, false),
                        new CornerInfo(16, false, false, false),
                        new CornerInfo(20, true, false, false),
                        new CornerInfo(22, true, true, false)
                    };
        
    }

    private void SetPositions(CornerInfo[] corners, Mesh mesh)
    {
        var vertices = mesh.vertices;

        for(int i = 0; i < vertices.Length; i++)
        {
            for(int j = 0; j < corners.Length; j++)
            {
                if(corners[j].index == i)
                {
                    corners[j].pos = vertices[i];
                }
            }
        }
    }

    private Vector3[] GetNewVertices(CornerInfo[] corners)
    {
        Vector3[] result = new Vector3[corners.Length];

        for(int i = 0; i < corners.Length; i++)
        {
            result[i] = corners[i].pos;
        }

        return result;
    }

    private Vector3[] GetOutlineNormals(CornerInfo[] corners)
    {
        Vector3[] result = new Vector3[corners.Length];

        for(int i = 0; i < corners.Length; i++)
        {
            result[i] = corners[i].outlineNorm
;
        }

        return result;
    }

    private int[] GetNewIndices(CornerInfo[] corners)
    {
        int[] indices = new int[24];
        int index = 0;

        for(int i = 0; i < corners.Length - 1; i++)
        {
            for(int j = i + 1; j < corners.Length; j++)
            {
                int count = 0;
                if(corners[i].top == corners[j].top) count++;
                if(corners[i].right == corners[j].right) count++;
                if(corners[i].front == corners[j].front) count++;

                if(count == 2) 
                {
                    indices[index++] = i;
                    indices[index++] = j;
                }
            }
        }

        return indices;
    }
}
