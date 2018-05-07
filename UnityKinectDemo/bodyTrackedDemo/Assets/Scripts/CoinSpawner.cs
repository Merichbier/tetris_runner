using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{

    int numCoins = 30;
    public float coinSpacing = 1;
    public float yOffset;
    public float xOffset;
    public float zOffset;
    public bool sinusoidal;

    private int spawnTimes = 5;
    //Sinusoidal pattern
    float amplitude = 1;
    float period = 0.5f;

    GameObject coinParent;

    // Use this for initialization
    void Start()
    {
        coinParent = new GameObject("Coins");
        zOffset = 0;
        for (int i = 0; i < spawnTimes; i++)
        {
            SpawnCoin();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Transform characterTransform = GameObject.FindGameObjectWithTag("Character").transform;
        if (zOffset - characterTransform.position.z < spawnTimes * numCoins * coinSpacing)
        {
            SpawnCoin();
            CheckCoin();
        }
    }

    void SinusoidalSpawn()
    {
        //GameObject coinParent = new GameObject("Coins");
        for (int i = 0; i < numCoins; i++)
        {
            float x = 0;
            if (sinusoidal)
            {
                x = amplitude * Mathf.Sin(period * i);
            }
            GameObject coin = GameObject.Instantiate(Resources.Load("Coin")) as GameObject;
            coin.transform.position = new Vector3(x + xOffset, yOffset, zOffset + i * coinSpacing);
            coin.transform.parent = coinParent.transform;
        }
        zOffset = zOffset + numCoins * coinSpacing;
    }

    void StraightSpawn()
    {
        for (int i = 0; i < numCoins; i++)
        {
            GameObject coin = GameObject.Instantiate(Resources.Load("Coin")) as GameObject;
            coin.transform.position = new Vector3(xOffset, yOffset, zOffset + i * coinSpacing);
            coin.transform.parent = coinParent.transform;
        }
        zOffset = zOffset + numCoins * coinSpacing;
    }

    void SpawnCoin()
    {
        if (sinusoidal) SinusoidalSpawn();
        else StraightSpawn();
    }

    void changeSpacing(float newSpacing)
    {
        coinSpacing = newSpacing;
    }

    void CheckCoin()
    {

    }


}
