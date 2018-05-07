using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private List<GameObject> activeEnemies;

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
        activeEnemies = new List<GameObject>();
        characterTransform = GameObject.FindGameObjectWithTag("Character").transform;
        var gm = GameObject.FindGameObjectWithTag("PlaneManager").GetComponent<PlaneManager>();
        maxZ = gm.spawnZ;
        gl = gm.groundLength;
        gns = gm.groundNumScreen;

        /* sleepTime = 1000;

         spawnEnemy();

         sleepTimeLeft = sleepTime;
         */

    }

    // Update is called once per frame
    void Update()
    {
        characterTransform = GameObject.FindGameObjectWithTag("Character").transform;

    }

    public void spawnEnemy()
    {
        GameObject go;
        go = Instantiate(enemyPrefabs[0]) as GameObject;
        go.transform.SetParent(transform);
        go.transform.position = new Vector3(Random.Range(-5, 5), Random.Range(0, 20), characterTransform.position.z + 10);
        rb = go.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 0)));
        }
        activeEnemies.Add(go);

    }

    public void checkEnemy()
    {
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            if (activeEnemies[0].transform.position.y < -5)
            {
                Destroy(activeEnemies[0]);
                activeEnemies.RemoveAt(0);
            }
        }
    }
}
