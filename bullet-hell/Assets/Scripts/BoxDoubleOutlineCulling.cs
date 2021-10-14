using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sets certain vertices to not contribute to the box outline
// All edges with these vertices will not produce an outline
public class BoxDoubleOutlineCulling : MonoBehaviour
{
    private Camera mainCam;
    Vector3[] verts;
    Mesh mesh;

    // Structure to hold screen space edge
    struct Edge
    {
        public Edge(int startIndex, Vector2 start, int endIndex, Vector2 end)
        {
            this.startIndex = startIndex;
            this.start = start;
            this.endIndex = endIndex;
            this.end = end;
        }
        public Vector2 start;
        public int startIndex;
        public Vector2 end;
        public int endIndex;
    }
    

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
        Edge[] edges = UpdateEdges();
        int[] intersectionCount = CountIntersections(edges);
        Vector2[] toCull = GetVerticesToCull(intersectionCount);

        mesh.SetUVs(1, toCull);
    }

    // If 2 edges from a vertex intersect with other edges set that vertex to be culled
    private Vector2[] GetVerticesToCull(int[] intersectionCount)
    {
        Vector2[] toCull = new Vector2[intersectionCount.Length];

        for(int i = 0; i < intersectionCount.Length; i++)
        {
            if(intersectionCount[i] == 2) toCull[i].x = 1;
            else toCull[i].x = 0;
        }

        return toCull;
    }

    // Count the number of intersections between edges (per vertex)
    private int[] CountIntersections(Edge[] edges)
    {
        int[] intersectionCount = new int[verts.Length];

        for(int i = 0; i < edges.Length - 1; i++)
        {
            for(int j = i + 1; j < edges.Length; j++)
            {
                if(edges[i].startIndex == edges[j].startIndex
                    || edges[i].startIndex == edges[j].endIndex
                    || edges[i].endIndex == edges[j].startIndex
                    || edges[i].endIndex == edges[j].endIndex) 
                {
                    continue;
                }

                if(EdgeIntersection(edges[i], edges[j]))
                {
                    intersectionCount[edges[i].startIndex]++;
                    intersectionCount[edges[i].endIndex]++;
                    intersectionCount[edges[j].startIndex]++;
                    intersectionCount[edges[j].endIndex]++;
                }
            }
        }

        return intersectionCount;
    }

    // Update the positions of the edges
    private Edge[] UpdateEdges()
    {
        int[] indices = mesh.GetIndices(0);
        Edge[] edges = new Edge[indices.Length / 2];
        Edge toAdd;
        Vector2 p1, p2;

        for(int i = 0; i < indices.Length; i += 2)
        {
            p1 = mainCam.WorldToScreenPoint(transform.TransformPoint(verts[indices[i]]));
            p2 = mainCam.WorldToScreenPoint(transform.TransformPoint(verts[indices[i + 1]]));

            if(p1.x < p2.x) toAdd = new Edge(indices[i], p1, indices[i + 1], p2);
            else toAdd = new Edge(indices[i + 1], p2, indices[i], p1);

            edges[i / 2] = toAdd;
        }

        return edges;
    }


    // Check intersection btwn 2 screen space edges
    private bool EdgeIntersection(Edge e1, Edge e2)
    {
        float m1, m2, div1, div2;
        bool vertical1, vertical2;
        vertical1 = vertical2 = false;

        div1 = e1.end.x - e1.start.x;
        div2 = e2.end.x - e2.start.x;

        if(div1 != 0) m1 = (e1.end.y - e1.start.y) / div1;
        else 
        {
            vertical1 = true;
            m1 = 0;
        }
        
        if(div2 != 0) m2 = (e2.end.y - e2.start.y) / div2;
        else
        {
            vertical2 = true;
            m2 = 0;
        }
        
        float x, mDiv;


        if(!vertical1 && !vertical2)
        {
            mDiv = m1 - m2;
            if(mDiv == 0) return false;

            float c1 = e1.start.y - m1 * e1.start.x;
            float c2 = e2.start.y - m2 * e2.start.x;

            x = (c2 - c1) / mDiv;

            if(x > e1.start.x && x < e1.end.x && x > e2.start.x && x < e2.end.x)
            {
                return true;
            }
        }
        else if(vertical1 && !vertical2)
        {
            float c2 = e2.start.y - m2 * e2.start.x;
            float y = m2 * e1.start.x  + c2;
            if(((y > e1.start.y && y < e1.end.y) || (y < e1.start.y && y > e1.end.y))
                && ((y > e2.start.y && y < e2.end.y) || (y < e2.start.y && y > e2.end.y))) return true;

            return false;
        }
        else if(!vertical1 && vertical2)
        {
            float c1 = e1.start.y - m1 * e1.start.x;
            float y = m1 * e1.start.x  + c1;
            if(((y > e1.start.y && y < e1.end.y) || (y < e1.start.y && y > e1.end.y))
                && ((y > e2.start.y && y < e2.end.y) || (y < e2.start.y && y > e2.end.y))) return true;

            return false;
        }
        
        return false;        
    }
}
