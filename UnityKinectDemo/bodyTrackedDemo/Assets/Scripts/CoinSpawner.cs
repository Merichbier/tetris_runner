﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour {

    int numCoins = 30;
    float coinSpacing = 1;
    public float yOffset;
    public float xOffset;

    List<GameObject> coins;
    public bool sinusoidal;

    //Sinusoidal pattern
    float amplitude = 1;
    float period = 0.5f;

    // Use this for initialization
    void Start()
    {
        GameObject coinParent = new GameObject("Coins");
        coins = new List<GameObject>();
        for (int i = 0; i < numCoins; i++)
        {
            float x = 0;
            if (sinusoidal)
            {
                x = amplitude * Mathf.Sin(period * i);
            }
            GameObject coin = GameObject.Instantiate(Resources.Load("Coin")) as GameObject;
            coin.transform.position = new Vector3(x + xOffset, yOffset, i * coinSpacing);
            coin.transform.parent = coinParent.transform;
            coin.GetComponent<Coin>().SetTotalCoins(numCoins);
            coins.Add(coin);
        }
    }

    public void ChangeColor(Material m) {
        foreach(GameObject g in coins)
        {
            g.GetComponent<MeshRenderer>().material = m;
        }
    }

}
