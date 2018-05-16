using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour {

    int numCoins = 30;
    public static float coinSpacing = 2;
    public static float Y_OFFSET = 0.3f;
    public static float X_OFFSET;

    float spawnDelay = 0.2f;

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
    }

    void initCoins()
    {
        for (int i = 0; i < numCoins; i++)
        {
            //Debug.Log("Spawn coin " + i + "th");
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
        var position = new Vector3(x + X_OFFSET, Y_OFFSET, 3 + i * coinSpacing);
        position.y = PlaneManager.getHeight(position) + CoinSpawner.Y_OFFSET;
        coin.transform.position = position;
        coin.transform.parent = coinParent.transform;
        coins.Add(coin);
    }

    IEnumerator ContinueSpawning() {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            if(coins.Count < numCoins)
            {
                SpawnCoin(coinZCounter);
                coinZCounter++;
            }
        }
    }

    void CleanCoins()
    {
        coins.RemoveAll(item => item == null);
        for (int i = 0; i < coins.Count;i++) {
            GameObject coin = coins[i];
            if (coin.transform.position.z <= player.position.z-5)
            {
                Destroy(coin);
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
        if(coins.Count == 0)
        {
            initCoins();
        }
        foreach (GameObject coin in coins)
        {
            if (coin != null)
            {
                coin.GetComponent<MeshRenderer>().material = coinMaterial;
                coin.GetComponent<Coin>().SetPoints(coinScore);
            }
        }
        CleanCoins();
    }

    public void ChangeCoinProperties(Material m, int score) {
        ChangeColor(m);
        ChangeScore(score);
    }

}
