using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    float rotationSpeed = 50;
    float points = 10;
    int totalCoins;
    Transform player;
    MeshRenderer mr;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").transform;
        
	}

    public float GetPoints()
    {
        return points;
    }

    public void SetPoints(int i) {
        points = i;
    }

    IEnumerator Hide() {
        mr = GetComponent<MeshRenderer>();
        mr.enabled = false;
        yield return new WaitForSeconds(0.5f);
        mr.enabled = true;
    }

    public void HideCoin() {
        StartCoroutine(Hide());
    }

    void Move()
    {
        Vector3 currPos = transform.position;
        currPos.z += totalCoins;
        transform.position = currPos;
    }

    public void SetTotalCoins(int t) {
        totalCoins = t;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 tmp = transform.eulerAngles;
        tmp.y += Time.deltaTime*rotationSpeed;
        transform.eulerAngles=tmp;
        if (player.transform.position.z >= transform.position.z)
        {
            Move();
        }
	}
}
