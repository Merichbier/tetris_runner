﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parabox.CSG;
using System.Threading;
using System;

public class WallSpawn : MonoBehaviour
{
    public List<GameObject> wallsPrefabs = new List<GameObject>();
    private int indexPrefab = 7;

    private static float MinScaleX = 1f;
    private static float MaxScaleX = 2f;

    private static float MinScaleY = 0.25f;
    private static float MaxScaleY = 2f;

    private static float MinScaleZ = 0.2f;
    private static float MaxScaleZ = 1f;

    private static float APPEARING_SPEED = 0.15f;
    private float elapsedTime = 0f;

    private GameObject appearingWall;
    private Vector3 finalPosition;
    private Mesh m;
    private bool done = false;
    private static float DESTROY_DISTANCE = 5f;

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
        indexPrefab = (new System.Random()).Next(0, wallsPrefabs.Count);
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
        if (walls.Count == 0)
            return;
        var player = GameObject.FindGameObjectWithTag("Character").transform;

        for (int i = walls.Count - 1; i >= 0; i--)
        {
            GameObject wall = walls[i];
            if (player.position.z > wall.transform.position.z + PlaneManager.THRESHOLD)
            {
                walls.Remove(wall);
                Destroy(wall);
            }
        }

    }

    public void RemoveCollidedWall(GameObject wall)
    {
        walls.Remove(wall);
        Destroy(wall);
    }

    public void TryDestroyWall(Vector3 position)
    {
        if (walls.Count == 0)
            return;

        GameObject firstWall = walls[0];
        if (Vector3.Distance(firstWall.transform.position, position) < DESTROY_DISTANCE)
        {
            walls.Remove(firstWall);
            Destroy(firstWall);
        }
    }

    private void ComputeSubtract(GameObject wall, GameObject hole)
    {

        m = CSG.Subtract(wall, hole);
        done = true;
    }

    // Not used, can't have a smooth game with it...
    private IEnumerator createWall()
    {
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
        composite.transform.position = new Vector3(0f, -5f, player.transform.position.z + AdversarySpawner.SPAWN_OFFSET);
        // Set texture
        //Renderer renderer = GetComponent<Renderer>();
        //renderer.material.settexture("rock5_snow", )
    }

    public void showWall()
    {
        var rng = new System.Random();
        GameObject wall = GameObject.Instantiate(wallsPrefabs[indexPrefab]) as GameObject;
        wall.transform.SetParent(transform);
        var rotation = wall.transform.rotation;
        rotation.y += (float)(180 * Math.Round(rng.NextDouble()));
        indexPrefab = rng.Next(0, wallsPrefabs.Count);
        //indexPrefab = testIndex;

        wall.transform.rotation = rotation;

        appearingWall = wall;
        PositionWall(wall);

        // Keep track of walls
        walls.Add(wall);
    }

    private void PositionWall(GameObject wall)
    {
        // Position it ahead of the player
        var player = GameObject.FindGameObjectWithTag("Character").transform;
        Vector3 position = new Vector3(0f, -15f, player.transform.position.z + AdversarySpawner.SPAWN_OFFSET);
        wall.transform.position = position;
        var zSize = wall.GetComponentInChildren<MeshCollider>().bounds.extents.z;
        //Debug.Log("Size is :" + zSize);
        // Find right terrain height
        float heightStart = PlaneManager.getHeight(position - new Vector3(0, 0, zSize));
        float heightEnd = PlaneManager.getHeight(position + new Vector3(0, 0, zSize));

        position.y = (heightStart + heightEnd) / 2f;

        finalPosition = position;
    }
}
