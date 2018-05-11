using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneManager : MonoBehaviour
{
    public GameObject[] groundPrefabs;
    public float spawnZ = 0;
    public float groundLength = 100.0f;
    public int groundNumScreen = 8;

    private Transform characterTransform;

    private List<GameObject> activeGrounds;
    private int nextToSpawn = 0;

    public static float TERRAIN_Y_OFFSET = -8.75f;
    public static int THRESHOLD = 5;

    // Use this for initialization
    void Start()
    {
        activeGrounds = new List<GameObject>();
        characterTransform = GameObject.FindGameObjectWithTag("Character").transform;
        for (int i = 0; i < groundNumScreen; i++)
        {
            spawnGround();
        }

    }

    // Update is called once per frame
    void Update()
    {
        characterTransform = GameObject.FindGameObjectWithTag("Character").transform;
        if (characterTransform.position.z > spawnZ - (groundNumScreen - 1) * groundLength + THRESHOLD)
        {
            //Debug.Log(characterTransform.position.z + " > " + (spawnZ - groundNumScreen * groundLength + THRESHOLD));
            spawnGround();
            deleteGround();
        }
    }

    void spawnGround(int prefabIndex = -1)
    {
        GameObject go;
        go = Instantiate(groundPrefabs[nextToSpawn]) as GameObject;
        nextToSpawn = (nextToSpawn + 1) % groundPrefabs.Length;
        go.transform.SetParent(transform);
        go.transform.position = new Vector3(-groundLength / 2, TERRAIN_Y_OFFSET, spawnZ);//characterTransform.forward * spawnZ;

        spawnZ += groundLength;
        activeGrounds.Add(go);
    }

    void deleteGround()
    {
        //Debug.Log("DELETE---  position: " + characterTransform.position.z + " / spawnZ: " + spawnZ);
        Destroy(activeGrounds[0]);
        activeGrounds.RemoveAt(0);
    }

    public void ChangeMaterial(Material m) {
        foreach(GameObject g in activeGrounds)
        {
            MeshRenderer mr = g.GetComponent<MeshRenderer>();
            mr.material = m;
        }
    }
}
