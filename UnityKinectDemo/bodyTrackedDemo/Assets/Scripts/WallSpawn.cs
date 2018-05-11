using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parabox.CSG;
using System.Threading;
using System;

public class WallSpawn : MonoBehaviour
{
    public List<GameObject> wallsPrefabs = new List<GameObject>();
    private int indexPrefab = 0;

    private static float MinScaleX = 1f;
    private static float MaxScaleX = 2f;

    private static float MinScaleY = 0.25f;
    private static float MaxScaleY = 2f;

    private static float MinScaleZ = 0.2f;
    private static float MaxScaleZ = 1f;

    private static float SPAWN_TIME = 3f;
    private static float APPEARING_SPEED = 0.05f;
    private float elapsedTime = 0f;

    private static float SPAWN_OFFSET = 15f;
    private GameObject appearingWall;
    private Vector3 finalPosition;
    private Mesh m;
    private bool done = false;

    private List<GameObject> walls = new List<GameObject>();


    int rows = 10;
    int columns = 10;
    Vector3 unitVector = new Vector3(1, 1, 1);
    float blockScale = 0.1f;
    float yOffset = 0.05f;
    float xOffset = 0;

    // Use this for initialization
    void Start()
    {
        //StartCoroutine(TurnOnGravity());
        StartCoroutine("showWall");

    }
    IEnumerator TurnOnGravity()
    {
        yield return new WaitForSeconds(3);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject g = GameObject.Find("WallPiece" + i + "_" + j);
                Rigidbody r = g.GetComponent<Rigidbody>();
                r.useGravity = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (appearingWall != null)
            appearingWall.transform.position = Vector3.Slerp(appearingWall.transform.position, finalPosition, APPEARING_SPEED);
        CleanWalls();
    }

    private void CleanWalls()
    {
        if (walls.Count < 1)
            return;

        var player = GameObject.FindGameObjectWithTag("Character").transform;
        if (walls.Count == 0)
            return;

        GameObject firstWall = walls[0];
        if (player.position.z > firstWall.transform.position.z + PlaneManager.THRESHOLD)
        {
            walls.Remove(firstWall);
            Destroy(firstWall);
        }
    }

    public void RemoveCollidedWall(GameObject wall)
    {
        walls.Remove(wall);
        Destroy(wall);
    }

    private void ComputeSubtract(GameObject wall, GameObject hole)
    {

        m = CSG.Subtract(wall, hole);
        done = true;
    }

    // Not used, can't have a smooth game with it...
    private IEnumerator createWall()
    {
        while (true)
        {
            yield return new WaitForSeconds(SPAWN_TIME);

            Debug.Log("Start createWall");
            System.Random rng = new System.Random();
            GameObject wall = GameObject.Instantiate(Resources.Load("Rock5A")) as GameObject;
            GameObject hole = GameObject.Instantiate(Resources.Load("Hole0")) as GameObject;
            if (wall == null)
            {
                Debug.Log("Wall is null");
            }
            if (hole == null)
            {
                Debug.Log("Hole is null");
            }
            // Random Scale of the wall
            float sx = (float)rng.NextDouble() * (MaxScaleX - MinScaleX);
            float sy = (float)rng.NextDouble() * (MaxScaleY - MinScaleY);
            float sz = (float)rng.NextDouble() * (MaxScaleZ - MinScaleZ);
            wall.transform.localScale = new Vector3(sx, sy, sz);
            // Find correct y position for the hole
            // Find right terrain
            Terrain[] terrains = Terrain.activeTerrains;
            Debug.Log("There is " + terrains.Length + " Terrains");

            // Subtract objects
            done = false;
            ThreadPool.QueueUserWorkItem(delegate { ComputeSubtract(wall, hole); });
            yield return new WaitUntil(() => { return done; });

            GameObject composite = new GameObject();
            composite.AddComponent<MeshFilter>().sharedMesh = m;
            composite.AddComponent<MeshRenderer>().sharedMaterial = wall.GetComponent<MeshRenderer>().sharedMaterial;


            // Position it ahead of the player
            var player = GameObject.FindGameObjectWithTag("Character").transform;
            composite.transform.position = new Vector3(0f, -5f, player.transform.position.z + SPAWN_OFFSET);
            // Set texture
            //Renderer renderer = GetComponent<Renderer>();
            //renderer.material.settexture("rock5_snow", )
        }
    }

    private IEnumerator showWall()
    {
        while (true)
        {
            yield return new WaitForSeconds(SPAWN_TIME);
            GameObject wall = GameObject.Instantiate(wallsPrefabs[indexPrefab]) as GameObject;

            indexPrefab = indexPrefab++ % wallsPrefabs.Count;

            appearingWall = wall;
            PositionWall(wall);

            // Keep track of walls
            walls.Add(wall);
        }

    }

    private void PositionWall(GameObject wall)
    {
        // Position it ahead of the player
        var player = GameObject.FindGameObjectWithTag("Character").transform;
        Vector3 position = new Vector3(0f, -5f, player.transform.position.z + SPAWN_OFFSET);
        wall.transform.position = position;

        // Find right terrain height
        float height = PlaneManager.getHeight(position);
        if (height == 0f)
            Debug.Log("Got default value for walls..");

        position.y = height + 0.5f;

        finalPosition = position;
    }
}
