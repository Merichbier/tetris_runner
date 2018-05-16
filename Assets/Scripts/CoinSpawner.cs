using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour {

    int numCoins = 30;
    public static float coinSpacing = 2;
    public static float Y_OFFSET = 0.3f;
    public static float X_OFFSET;

    float spawnDelay = 0.4f;

    List<GameObject> coins;
    public bool sinusoidal;

    Transform player;

    //Sinusoidal pattern
    float amplitude = 1;
    float period = 0.5f;

    int coinZCounter;

    public Material coinMaterial;
    int coinScore;

    GameObject coinParent;

    // Use this for initialization
    void Start()
    {

        coinParent = new GameObject("Coins");
        player = GameObject.Find("Player").transform;
        coins = new List<GameObject>();
        coinZCounter = numCoins;

        for (int i = 0; i < numCoins; i++)
        {
            SpawnCoin(i);
        }
        StartCoroutine(ContinueSpawning());
    }

    void SpawnCoin(float i)
    {
        float x = 0;
        if (sinusoidal)
        {
            x = amplitude * Mathf.Sin(period * i);
        }
        GameObject coin = GameObject.Instantiate(Resources.Load("Coin")) as GameObject;
        coin.transform.position = new Vector3(x + X_OFFSET, Y_OFFSET, 3 + i * coinSpacing);
        coin.transform.parent = coinParent.transform;
        coins.Add(coin);
    }

    IEnumerator ContinueSpawning() {
        yield return new WaitForSeconds(spawnDelay);
        SpawnCoin(coinZCounter);
        coinZCounter++;
        if (coins.Count > 0) { 
            RemoveCoins();
        }
        StartCoroutine(ContinueSpawning());
    }

    void RemoveCoins()
    {
        coins.RemoveAll(item => item == null);
        for (int i = 0; i < coins.Count;i++) {
            GameObject g = coins[i];
            if (g.transform.position.z <= player.position.z-5)
            {
                Destroy(g);
            }
        }
    }
    
    void ChangeColor(Material m) {
        coinMaterial = m;
    }

    void ChangeScore(int s){
        coinScore = s;
    }

    private void Update()
    {
        foreach (GameObject g in coins)
        {
            if (g != null)
            {
                g.GetComponent<MeshRenderer>().material = coinMaterial;
                g.GetComponent<Coin>().SetPoints(coinScore);
            }
        }
    }

    public void ChangeCoinProperties(Material m, int score) {
        ChangeColor(m);
        ChangeScore(score);
    }

}
