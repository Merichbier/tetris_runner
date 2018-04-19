using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	int lives=1;
	float speed=200;
	Rigidbody r;

	// Use this for initialization
	void Start () {		
		r = GetComponent<Rigidbody> ();
		r.AddForce(new Vector3(0,0,speed));
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Wall") {
			Destroy (collision.gameObject);
			lives-=1;
		}
	}
		
	// Update is called once per frame
	void Update () {	
		if (lives <= 0) {
			r.velocity=new Vector3(0,0,0);
		}
	}




}
