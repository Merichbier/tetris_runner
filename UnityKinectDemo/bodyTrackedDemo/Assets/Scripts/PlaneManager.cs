using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneManager : MonoBehaviour
{
    public GameObject[] groundPrefabs;
    public float spawnZ;
    public float groundLength = 20.0f;
    public int groundNumScreen = 8;

    private Transform characterTransform;

    private List<GameObject> activeGrounds;

    // Use this for initialization
    void Start()
    {
        activeGrounds = new List<GameObject>();
        characterTransform = GameObject.FindGameObjectWithTag("Character").transform;
        groundLength = 20;
        spawnZ = characterTransform.position.z - groundLength / 2;
        for (int i = 0; i < groundNumScreen; i++)
        {
            spawnGround();
        }

    }

    // Update is called once per frame
    void Update()
    {
        characterTransform = GameObject.FindGameObjectWithTag("Character").transform;
        if (spawnZ + groundLength / 2 - characterTransform.position.z < groundNumScreen * groundLength)
        {
            spawnGround();
            deleteGround();
        }
    }

    void spawnGround(int prefabIndex = -1)
    {
        GameObject go;
        go = Instantiate(groundPrefabs[0]) as GameObject;
        go.transform.SetParent(transform);
        go.transform.position = characterTransform.forward * spawnZ;
        spawnZ += groundLength;
        activeGrounds.Add(go);
    }

    void deleteGround()
    {
        //Debug.Log("DELETE---  position: " + characterTransform.position.z + " / spawnZ: " + spawnZ);
        Destroy(activeGrounds[0]);
        activeGrounds.RemoveAt(0);
    }
}
