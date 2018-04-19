using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawn : MonoBehaviour {

    int rows=10;
    int columns=10;
    Vector3 unitVector=new Vector3(1,1,1);
    float blockScale = 0.1f;
    float yOffset = 0.05f;
    float xOffset = 0;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < columns; j++) {
                GameObject g = GameObject.Instantiate(Resources.Load("WallPiece")) as GameObject;
                g.transform.position = new Vector3(i*blockScale+xOffset,j*blockScale+yOffset,0);
                g.transform.localScale = unitVector*blockScale;
                g.name = "WallPiece" + i + "_" + j;
            }
        }

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject g = GameObject.Find("WallPiece"+i+"_"+j);
                Rigidbody r = g.GetComponent<Rigidbody>();
                r.constraints = RigidbodyConstraints.None;
            }
        }
        //StartCoroutine(TurnOnGravity());

    }
    IEnumerator TurnOnGravity() {
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
	void Update () {
		
	}
}
