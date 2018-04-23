using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var start = transform;
        var target = GameObject.FindGameObjectWithTag("Character").transform;
        var newPos = new Vector3(start.position.x, start.position.y, target.position.z-1.5f);
        Debug.Log(string.Format("Char : ({0},{1},{2})", target.position.x, target.position.y, target.position.z));
        Debug.Log(string.Format("New Pos : ({0},{1},{2})", newPos.x, newPos.y, newPos.z));
        transform.position = newPos;
    }
}
