using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvManager : MonoBehaviour
{

    float sleepTime;
    float sleepTimeLeft;
    private Transform characterTransform;
    PlaneManager pm; // planeManager;
    EnemyManager em; //enemyManager;
    WallManager wm; // wallManager;

    void Start()
    {
        pm = GameObject.FindGameObjectWithTag("PlaneManager").GetComponent<PlaneManager>();
        em = GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>();
        wm = GameObject.FindGameObjectWithTag("WallManager").GetComponent<WallManager>();

        sleepTime = 100;
        sleepTimeLeft = sleepTime;
    }

    // Update is called once per frame
    void Update()
    {
        characterTransform = GameObject.FindGameObjectWithTag("Character").transform;
        Debug.Log("sleepTimeLeft = " + sleepTimeLeft);

        if (sleepTimeLeft > 0) sleepTimeLeft -= 1.0f;
        else
        {
            Debug.Log("em, wm call");
            wm.spawnWall();
            em.spawnEnemy();
            sleepTimeLeft = sleepTime;
        }
        wm.checkWall();
        em.checkEnemy();
    }
}
