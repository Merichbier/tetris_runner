using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parabox.CSG;

public class WallBreak : MonoBehaviour {

    int numBreaks = 5;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Shatter();
        }
    }

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
        float z = 0;

        float spread = 0.5f;

        float y = Random.Range(-spread,spread);
        float x = Random.Range(-spread,spread);

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

    int rows = 10;
    int columns = 6;
    Vector3 unitVector = new Vector3(1, 1, 1);
    float blockScale = 1f;
    float yOffset = 0.05f;
    float xOffset = 0;

    // Use this for initialization
    void Shatter()
    {
        Debug.Log("Shatter");
        float orgZ = transform.position.z;
        Destroy(gameObject);
        GameObject parentObj = new GameObject("WallShatter");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject g = GameObject.Instantiate(Resources.Load("WallPiece")) as GameObject;
                g.transform.position = transform.position;
                g.transform.parent = parentObj.transform;
                g.transform.position = new Vector3(i * blockScale + xOffset, j * blockScale + yOffset, 0);
                g.transform.localScale = unitVector * blockScale;
                g.name = "WallPiece" + i + "_" + j;
            }
        }

        GameObject gSphere = GameObject.Instantiate(Resources.Load("InvisibleSphere")) as GameObject;
        gSphere.transform.parent = parentObj.transform;
        gSphere.transform.position = Vector3.zero;
        /*
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject g = GameObject.Find("WallPiece" + i + "_" + j);
                Rigidbody r = g.GetComponent<Rigidbody>();
                r.constraints = RigidbodyConstraints.None;
            }
        }
        */
        parentObj.transform.position += new Vector3(-4.4f, -3f, orgZ);

        
        //StartCoroutine(TurnOnGravity());

    }

    IEnumerator TurnOnGravity()
    {
        yield return new WaitForSeconds(3);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject g = GameObject.Find("WallPiece" + i + "_" + j);
                Rigidbody r = g.GetComponent<Rigidbody>();
                r.useGravity = true;
            }
        }
    }

}
