using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static float MAX_X = 5f;
    private static float SPEED_SNOWBALL = 15f;
    private static float DESTROY_DISTANCE = 5f;

    public GameObject[] enemyPrefabs;
    private GameObject currentEnemy;
    private List<GameObject> enemies = new List<GameObject>();
    private float LAUCH_ANGLE = 45f;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CleanSnowballs();
        //Move snowball in direction of the player
        if (currentEnemy == null)
            return;
        var player = GameObject.FindGameObjectWithTag("Character").transform.position;
        // Launch Enemy
        var rb = currentEnemy.GetComponent<Rigidbody>();
        var horizontalSpeed = rb.velocity;
        horizontalSpeed.y = 0;
        var speed = Vector3.Magnitude(horizontalSpeed);
        var direction = player - currentEnemy.transform.position;
        direction.y = 0;
        direction = Vector3.Normalize(direction) * speed;
        direction.y = rb.velocity.y;
        rb.velocity = direction;

    }

    private void CleanSnowballs()
    {
        if (enemies.Count == 0)
            return;
        var player = GameObject.FindGameObjectWithTag("Character").transform.position;
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            GameObject enemy = enemies[i];
            if (player.z > enemy.transform.position.z + PlaneManager.THRESHOLD || enemy.transform.position.y < 0)
            {
                enemies.Remove(enemy);
                Destroy(enemy);
            }
        }

    }
    public void RemoveCollidedEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
        Destroy(enemy);
    }
    public void TryDestroyEnemy(Vector3 position)
    {
        if (enemies.Count == 0)
        {
            Debug.Log("Not enemy");
            return;
        }

        GameObject firstEnemy = enemies[0];
        if (Vector3.Distance(firstEnemy.transform.position, position) < DESTROY_DISTANCE)
        {
            enemies.Remove(firstEnemy);
            currentEnemy = null;
            // Put it away
            var rb = firstEnemy.GetComponent<Rigidbody>();
            rb.velocity = InverseHorizontalDirection(rb.velocity);
            Debug.Log("Enemy pushed away !");
        }
        else
        {
            Debug.Log("Distance was : " + Vector3.Distance(firstEnemy.transform.position, position));
        }
    }

    public void spawnEnemy()
    {
        GameObject enemy;
        enemy = Instantiate(enemyPrefabs[0]) as GameObject;
        enemy.transform.SetParent(transform);

        // Show it ahead of the player
        var player = GameObject.Find("joint_Pelvis").transform.position;

        // Random start along X axis
        float xPosition = (float)(new System.Random()).NextDouble() * MAX_X * 2f - MAX_X;
        Vector3 position = new Vector3(xPosition, -5f, player.z + AdversarySpawner.SPAWN_OFFSET * 2f);
        position.y = PlaneManager.getHeight(position) + enemy.GetComponent<SphereCollider>().radius + AdversarySpawner.EPSILON_SPAWN;

        enemy.transform.position = position;
        currentEnemy = enemy;
        enemies.Add(enemy);

        // Launch Enemy
        Vector3 playerXZPos = new Vector3(player.x, enemy.transform.position.y, player.z);
        enemy.transform.LookAt(playerXZPos);

        var target = player;
        target.z += 13f;
        // Kinematic formula
        float R = Vector3.Distance(enemy.transform.position, target);
        float G = Physics.gravity.y;
        float tanAlpha = (float)Math.Tan(LAUCH_ANGLE * Mathf.Deg2Rad);
        float H = player.y - enemy.transform.position.y;

        // Compute velocity

        float Vz = Mathf.Sqrt(G * R * R / (2f * (H - R * tanAlpha)));
        float Vy = tanAlpha * Vz;

        var localVelocity = new Vector3(0f, Vy, Vz);
        var globalVelocity = enemy.transform.TransformDirection(localVelocity);



        var rb = enemy.GetComponent<Rigidbody>();
        rb.velocity = globalVelocity;
    }

    private Vector3 InverseHorizontalDirection(Vector3 direction)
    {
        var newDirection = new Vector3(-direction.x, 0, -direction.z);
        return newDirection;
    }
}
