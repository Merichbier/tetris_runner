using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Parabox.CSG;

public class WallBreak : MonoBehaviour {

    int numBreaks = 5;


	// Use this for initialization
	public void BreakWall () {
        if (numBreaks <= 0)
        {
            return;
        }
        numBreaks--;
        GameObject hole = GameObject.Instantiate(Resources.Load("Hole")) as GameObject;
        hole.transform.parent = transform;

        //if this doesnt work, might depend on orientation (replace x with z)
        float x = 0;

        float spread = 0.5f;

        float y = Random.Range(-spread,spread);
        float z = Random.Range(-spread,spread);

        Vector3 offset = new Vector3(x, y, z);

        hole.transform.localPosition = Vector3.zero+offset;
        
        Transform[] obj = transform.GetComponentsInChildren<Transform>();

        GameObject g1 = obj[1].gameObject;
        GameObject g2 = obj[2].gameObject;

        Mesh m = CSG.Subtract(g1, g2);

        GameObject composite = new GameObject();

        
        composite.AddComponent<MeshFilter>().sharedMesh = m;
        composite.AddComponent<MeshRenderer>().sharedMaterial = g2.GetComponent<MeshRenderer>().sharedMaterial;
        composite.AddComponent<MeshCollider>();
        composite.tag = "Wall";
        GenerateBarycentric(composite);
        
        GameObject.Destroy(g1);
        GameObject.Destroy(g2);
        
        composite.transform.parent = transform;
    }

    static void GenerateBarycentric(GameObject go)
    {
        Mesh m = go.GetComponent<MeshFilter>().sharedMesh;

        if (m == null) return;

        int[] tris = m.triangles;
        int triangleCount = tris.Length;

        Vector3[] mesh_vertices = m.vertices;
        Vector3[] mesh_normals = m.normals;
        Vector2[] mesh_uv = m.uv;

        Vector3[] vertices = new Vector3[triangleCount];
        Vector3[] normals = new Vector3[triangleCount];
        Vector2[] uv = new Vector2[triangleCount];
        Color[] colors = new Color[triangleCount];

        for (int i = 0; i < triangleCount; i++)
        {
            vertices[i] = mesh_vertices[tris[i]];
            normals[i] = mesh_normals[tris[i]];
            uv[i] = mesh_uv[tris[i]];

            colors[i] = i % 3 == 0 ? new Color(1, 0, 0, 0) : (i % 3) == 1 ? new Color(0, 1, 0, 0) : new Color(0, 0, 1, 0);

            tris[i] = i;
        }

        Mesh wireframeMesh = new Mesh();

        wireframeMesh.Clear();
        wireframeMesh.vertices = vertices;
        wireframeMesh.triangles = tris;
        wireframeMesh.normals = normals;
        wireframeMesh.colors = colors;
        wireframeMesh.uv = uv;

        go.GetComponent<MeshFilter>().sharedMesh = wireframeMesh;
    }

}
