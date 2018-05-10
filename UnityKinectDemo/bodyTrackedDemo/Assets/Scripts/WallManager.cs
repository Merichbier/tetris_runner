using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{

    public GameObject[] WallPrefabs;
    private const float sleepTime=5;
    private float sleepTimeLeft;

    private Transform characterTransform;
    private List<GameObject> activeWalls;


    // Use this for initialization
    void Start()
    {
        Debug.Log("walltest");
        //sleepTimeLeft = sleepTime;
        //Debug.Log("start/ sleepTimeLeft = " + sleepTimeLeft);
        activeWalls = new List<GameObject>();
        characterTransform = GameObject.FindGameObjectWithTag("Character").transform;
    }

    // Update is called once per frame
    void Update()
    {        
        if (sleepTimeLeft > 0) sleepTimeLeft -= Time.deltaTime;
        else
        {
            spawnWall();
        }
        checkWall(); 
    }

    public void spawnWall(int prefabIndex = -1)
    {
      //  Debug.Log("Spawn Wall");
        GameObject go;
        go = Instantiate(WallPrefabs[0]) as GameObject;
        go.transform.SetParent(transform);
        go.transform.position = characterTransform.forward * characterTransform.position.z;
        go.transform.position += new Vector3(0, WallPrefabs[0].transform.localScale.y / 2, 40);
        activeWalls.Add(go);
        sleepTimeLeft = sleepTime;
//        Debug.Log("Reset = " + sleepTimeLeft);
    }

    void checkWall()
    {
        if (activeWalls.Count != 0)
        {            
            if (activeWalls[0] != null) {
                if (activeWalls[0].transform.position.z < characterTransform.position.z - 10)
                {
                    Debug.Log("DELETE---  character: " + characterTransform.position.z + " / wall: " + activeWalls[0].transform.position.z);
                    Destroy(activeWalls[0]);
                    activeWalls.RemoveAt(0);
                }
            }
        }
    }
}