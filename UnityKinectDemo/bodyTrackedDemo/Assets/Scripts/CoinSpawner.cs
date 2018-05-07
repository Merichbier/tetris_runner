using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{

    int numCoins = 30;
    float coinSpacing = 1;
    public float yOffset;
    public float xOffset;
    public float zOffset;
    public bool sinusoidal;

    //Sinusoidal pattern
    float amplitude = 1;
    float period = 0.5f;

    GameObject coinParent;

    // Use this for initialization
    void Start()
    {
        coinParent = new GameObject("Coins");
        zOffset = 0;
        CoinSpawn();

    }

    // Update is called once per frame
    void Update()
    {

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
    }
    void CoinSpawn()
    {
        SinusoidalSpawn();
    }

    void changeSpacing(float newSpacing)
    {
        coinSpacing = newSpacing;
    }
}
