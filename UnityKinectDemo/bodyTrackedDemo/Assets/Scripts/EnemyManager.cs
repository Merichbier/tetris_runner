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
    List<GameObject> enemies;

    // waiting time
    private float sleepTime = 3;
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
        characterTransform = GameObject.FindGameObjectWithTag("Character").transform;
        enemies = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {        
        if (sleepTimeLeft > 0) sleepTimeLeft -= Time.deltaTime;
        else
        {
            spawnEnemy();
            sleepTimeLeft = sleepTime;
        }
        checkEnemies();
    }

    void spawnEnemy()
    {
        GameObject go;
        go = Instantiate(enemyPrefabs[0]) as GameObject;
        go.transform.SetParent(transform);
        go.transform.position = new Vector3(Random.Range(-5, 5), Random.Range(5, 10), characterTransform.position.z + 20);
        rb = go.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 0)));
        enemies.Add(go);
        //Debug.Log("snowball position: " + go.transform.position.ToString());
    }

    void checkEnemies()
    {
        if (enemies.Count != 0)
        {
            if (enemies[0] == null)
            {
                enemies.RemoveAt(0);
            }
            else if (enemies[0].transform.position.z < characterTransform.position.z - 5)
            {
                Debug.Log("removed enemy");
                Destroy(enemies[0]);
                enemies.RemoveAt(0);
            }
        }
    }
}
