using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    float rotationSpeed = 50;
    float points = 10;
    int totalCoins;
    bool pickedUp;
    //Transform player;
    Vector3 orgPos;
    MeshRenderer mr;

	// Use this for initialization
	void Start () {
        // player = GameObject.Find("Player").transform;
        orgPos = transform.position;
        // uiPos = GameObject.Find("CoinImage").transform.position+new Vector3(0,0,5);
        meter = GameObject.Find("I");
    }

    public float GetPoints()
    {
        return points;
    }

    public void SetPoints(int i) {
        points = i;
    }

    public void Move()
    {
        pickedUp = true;
    }

    void Rotate() {
        Vector3 tmp = transform.eulerAngles;
        tmp.y += Time.deltaTime * rotationSpeed;
        transform.eulerAngles = tmp;
    }

    float speed = 10;
    Vector3 uiPos;

    GameObject meter;

    // Update is called once per frame
    void FixedUpdate () {
        Rotate();
        if (pickedUp)
        {
            transform.position = Vector3.Lerp(transform.position, meter.transform.position, 1.6f * Time.deltaTime);
            //transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 1f * Time.deltaTime);
            //Debug.Log(Vector3.Magnitude(transform.position - meter.transform.position));
            if (Vector3.Magnitude(transform.position - meter.transform.position) < 5f)
            {
                Destroy(gameObject);
            }
        } 
    }
}
