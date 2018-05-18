using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdversarySpawner : MonoBehaviour
{
    private static float SPAWN_TIME = 3f;
    public static float SPAWN_OFFSET = 20f;
    public static float EPSILON_SPAWN = 0.5f;
    private static System.Random rng = new System.Random();

    // Use this for initialization
    void Start()
    {
        StartCoroutine("showAdversary");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator showAdversary()
    {
        var player = GameObject.Find("Player").GetComponent<Player>();
        yield return new WaitForSeconds(10f);
        while (!player.gameOver)
        {
            yield return new WaitForSeconds(SPAWN_TIME);
            if (rng.NextDouble() < 0.5f)
            {
                // Show a wall
                var wallManager = GameObject.Find("WallManager").GetComponent<WallSpawn>();
                //Debug.Log("Spawn a wall");
                wallManager.showWall();
            }
            else
            {
                // Show a snowball
                var enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
                //Debug.Log("Spawn a snowball");
                enemyManager.spawnEnemy();
            }
        }
    }
}
