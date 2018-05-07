using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvManager : MonoBehaviour
{

    int sleepTime;
    int sleepTimeLeft;
    public bool bonus;
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

        // spawn wall and enemy alternately ( they should not appear simultaneously)
        if (!bonus)
        {
            if (sleepTimeLeft == sleepTime / 2)
            {
                wm.spawnWall();
            }
            if (sleepTimeLeft == 0)
            {
                em.spawnEnemy();
            }

            // sleepTIme deduction every moment
            if (sleepTimeLeft > 0) sleepTimeLeft -= 1;
            else
            {
                sleepTimeLeft = sleepTime;
            }

            // check if there is any inactive walls or enemies to destroy
            wm.checkWall();
            em.checkEnemy();

        }
    }

}
