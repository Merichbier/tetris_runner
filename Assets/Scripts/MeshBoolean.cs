using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parabox.CSG;
using UnityEditor;

public class MeshBoolean : MonoBehaviour {

    GameObject g1;
    GameObject g2;

	// Use this for initialization
	void Start () {
        Transform[] obj = GetComponentsInChildren<Transform>();
        g1 = obj[1].gameObject;
        g2 = obj[2].gameObject;

        Boolean();
	}

    void Boolean()
    {
        Mesh m = CSG.Subtract(g1,g2);
        GameObject composite = new GameObject();

        composite.AddComponent<MeshFilter>().sharedMesh = m;
        composite.AddComponent<MeshRenderer>().sharedMaterial = g2.GetComponent<MeshRenderer>().sharedMaterial;

        GenerateBarycentric(composite);

        GameObject.Destroy(g1);
        GameObject.Destroy(g2);

        
        string saveName = "SavedMesh";
        var mf = composite.GetComponent<MeshFilter>();
        if (mf)
        {
            var savePath = "Assets/" + saveName + ".asset";
            Debug.Log("Saved Mesh to:" + savePath);
            AssetDatabase.CreateAsset(mf.mesh, savePath);
        }




    //        Object prefab = PrefabUtility.CreatePrefab("Assets/Resources/o1.prefab", composite);

    // AssetDatabase.CreateAsset(m,"Assets/Resources/Mesh1.fbx");
    //AssetDatabase.SaveAssets();
}

    void GenerateBarycentric(GameObject go)
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
