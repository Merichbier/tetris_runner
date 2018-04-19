using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	int lives=3;
    float score;
	float speed=8;
	Rigidbody r;
    float maxSpeed = 10;

	// Use this for initialization
	void Start () {		
		r = GetComponent<Rigidbody> ();
		r.AddForce(new Vector3(0,0,speed));
        UI.UpdateText(0, "Score: " + score);
        UI.UpdateText(1, "Lives: " + lives);
        // r.freezeRotation = true;
        

    }

    void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Wall") {
			Destroy (collision.gameObject);
            RemoveLife();
        }
	}

    void RemoveLife()
    {
        lives -= 1;
        UI.UpdateText(1, "Lives: " + lives);
    }

    void IncrementScore(float f) {
        score += f;
        UI.UpdateText(0, "Score: " + Mathf.Round(score));
    }
		
	// Update is called once per frame
	void Update () {
        if (lives > 0)
        {
            Debug.Log(r.velocity.z);
            if (r.velocity.z < maxSpeed)
            {
                r.AddRelativeForce(Vector3.forward * speed);
            }
            IncrementScore(Time.deltaTime*10);
        }
        else {
            speed = 0;
        }
	}




}
