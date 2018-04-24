using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    float rotationSpeed = 50;
    float points = 10;

	// Use this for initialization
	void Start () {
		
	}

    public float GetPoints()
    {
        return points;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 tmp = transform.eulerAngles;
        tmp.y += Time.deltaTime*rotationSpeed;
        transform.eulerAngles=tmp;
	}
}
