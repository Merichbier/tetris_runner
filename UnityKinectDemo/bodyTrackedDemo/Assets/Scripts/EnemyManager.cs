using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public GameObject[] enemyPrefabs;
    private Transform characterTransform;
    private float maxZ;
    private float gl;
    private float gns;

    // waiting time
    private float sleepTime = 100;
    private float sleepTimeLeft;
    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        characterTransform = GameObject.FindGameObjectWithTag("Character").transform;
        var gm = GameObject.FindGameObjectWithTag("PlaneManager").GetComponent<PlaneManager>();
        maxZ = gm.spawnZ;
        gl = gm.groundLength;
        gns = gm.groundNumScreen;
        sleepTime = 10000;

        spawnEnemy();
        sleepTimeLeft = sleepTime;
        //Debug.Log("maxZ: " + maxZ);

    }

    // Update is called once per frame
    void Update()
    {
        characterTransform = GameObject.FindGameObjectWithTag("Character").transform;
        if (sleepTimeLeft > 0) sleepTimeLeft -= 1.0f;
        else
        {
            spawnEnemy();
            sleepTimeLeft = sleepTime;
        }
    }

    void spawnEnemy()
    {
        GameObject go;
        go = Instantiate(enemyPrefabs[0]) as GameObject;
        go.transform.SetParent(transform);
        go.transform.position = new Vector3(Random.Range(-5, 5), Random.Range(0, 20), characterTransform.position.z + 10);
        rb = go.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 0)));
        Debug.Log("snowball position: " + go.transform.position.ToString());
    }
}
