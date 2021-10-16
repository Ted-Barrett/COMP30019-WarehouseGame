using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Sets certain vertices to not contribute to the box outline
// Any edge containing at least one of these vertices will not produce an outline
public class BoxDoubleOutlineCulling : MonoBehaviour
{
    [SerializeField]
    Mesh originalMesh;
    private Camera mainCam;
    Vector3[] verts;
    List<float>[] edgeAngles;
    List<Edge> edges;

    const float EPSILON = 0.0000001f;

    struct Edge
    {
        public Edge(int start, int end)
        {
            this.start = start;
            this.end = end;
        }
        public int start;
        public int end;
    }

    Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        mesh = this.GetComponent<MeshFilter>().mesh;
        verts = mesh.vertices;
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Get edges as list of ints
        int[] indices = originalMesh.GetIndices(0);

        edges = ConvertToEdgeList(indices);
        UpdateEdgeAngles();
        CullEdges();

        indices = ConvertEdgesToIndices();
        mesh.SetIndices(indices, MeshTopology.Lines, 0);
    }

    private int[] ConvertEdgesToIndices()
    {
        int[] indices = new int[edges.Count * 2];
        int index = 0;

        foreach(Edge edge in edges)
        {
            indices[index++] = edge.start;
            indices[index++] = edge.end;
        }

        return indices;
    }

    private List<Edge> ConvertToEdgeList(int[] indices)
    {
        List<Edge> returnList = new List<Edge>();
        for(int i = 0; i < indices.Length; i += 2)
        {
            returnList.Add(new Edge(indices[i], indices[i + 1]));
        }

        return returnList;
    }

    // If 2 edges from a vertex intersect with other edges set that vertex to be culled
    private void CullEdges()
    {
        List<int> culledVertices = new List<int>();

        for(int i = 0; i < verts.Length; i++)
        {
            if(GetMaxConsecutiveAngle(edgeAngles[i]) < Mathf.PI)
            {
                culledVertices.Add(i);
            }
        }

        if(culledVertices.Count == 2
            && VerticesAdjacent(culledVertices[0], culledVertices[1]))
        {
            int a = FindOppositeVertex(culledVertices[0]);
            int b = FindOppositeVertex(culledVertices[1]);
            
            edges.RemoveAll(element => (element.start == a && element.end == b)
                                        || (element.end == a && element.start == b));
        }

        edges.RemoveAll(element0 => culledVertices.Exists(element1 => element1 == element0.start
                                                                    || element1 == element0.end));
    }

    private int FindOppositeVertex(int a)
    {
        for(int i = 0; i < verts.Length; i++)
        {
            if(Mathf.Abs(verts[i].x - verts[a].x) > EPSILON
                && Mathf.Abs(verts[i].y - verts[a].y) > EPSILON
                && Mathf.Abs(verts[i].z - verts[a].z) > EPSILON)
            {
                return i;
            }
        }

        return 0;
    }

    private bool VerticesAdjacent(int a, int b)
    {
        return edges.Exists(element => (element.start == a && element.end == b)
                                        || (element.end == a && element.start == b));
    }

    private float GetMaxConsecutiveAngle(List<float> list)
    {
        float maxAngle = 0f;
        float currAngle;

        for(int i = 0; i < list.Count - 1; i++)
        {
            currAngle = list[i + 1] - list[i];

            if(currAngle > maxAngle) 
            {
                maxAngle = currAngle;
            }
        }

        if(list.Count > 0)
        {
            currAngle = 2f * Mathf.PI - list[list.Count - 1] + list[0];
            if(currAngle > maxAngle) maxAngle = currAngle;
        }

        return maxAngle;
    }

    // Update the positions of the edges
    private void UpdateEdgeAngles()
    {
        int[] indices = originalMesh.GetIndices(0);
        edgeAngles = new List<float>[verts.Length];

        for(int i = 0; i < verts.Length; i++)
        {
            edgeAngles[i] = new List<float>();
        }

        int vert1, vert2;
        Vector2 p1, p2;

        for(int i = 0; i < indices.Length; i += 2)
        {
            vert1 = indices[i];
            vert2 = indices[i + 1];

            p1 = mainCam.WorldToScreenPoint(transform.TransformPoint(verts[vert1]));
            p2 = mainCam.WorldToScreenPoint(transform.TransformPoint(verts[vert2]));

            edgeAngles[vert1].Add(GetPhase(p2 - p1));
            edgeAngles[vert2].Add(GetPhase(p1 - p2));
        }

        foreach(List<float> list in edgeAngles)
        {
            list.Sort();
        }
    }

    private float GetPhase(Vector2 point)
    {
        if(point.x == 0.0)
        {
            if(point.y >= 0) return Mathf.PI / 2.0f;
            else return 3f * Mathf.PI / 2f;
        }

        float angle = Mathf.Atan(point.y / point.x);
        
        if(point.x < 0) angle += Mathf.PI;

        if(angle < 0) angle += 2f * Mathf.PI;

        return angle;
    }
}
