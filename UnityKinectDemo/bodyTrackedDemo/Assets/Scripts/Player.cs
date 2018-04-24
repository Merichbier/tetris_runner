using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	int lives=3;
    float score;
	float speed=8;
	Rigidbody r;
    float maxSpeed = 10;
    public bool canMove;
    SimpleGestureListener sgl;
    float hitDistance = 2;
    float jumpForce = 150;
    Vector3 startPosition;
    
    // Use this for initialization
    void Start () {		
		r = GetComponent<Rigidbody> ();
		r.AddForce(new Vector3(0,0,speed));
        sgl = GameObject.Find("Main Camera").GetComponent<SimpleGestureListener>();
        UI.UpdateText(0, "Score: " + score);
        UI.UpdateText(1, "Lives: " + lives);
        startPosition = transform.position;
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
        score = f;
        UI.UpdateText(0, "Score: " + Mathf.Round(score));
    }
		
	// Update is called once per frame
	void Update () {
        /*
        Vector3 offset = new Vector3(0, 1.4f, 0);
        Vector3 origin = transform.position + offset;
        Vector3 direction = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(origin, direction * hitDistance, Color.yellow);
        */
        if (lives > 0 && canMove)
        {
            
            if (r.velocity.z < maxSpeed)
            {
                r.AddRelativeForce(Vector3.forward * speed);
            }
            IncrementScore(Vector3.Distance(transform.position,startPosition));
        }
        else {
            speed = 0;
        }
	}

     public void Swipe()
    {
        
        RaycastHit hit;
        Vector3 offset = new Vector3(0, 1.4f, 0);
        Vector3 origin = transform.position + offset;
        Vector3 direction = transform.TransformDirection(Vector3.forward);
        
        if (Physics.Raycast(origin, direction, out hit, hitDistance))
        {
            if (hit.transform.tag == "Enemy")
            {
                Destroy(hit.transform.gameObject);
            }
        }
    }

    public void Jump()
    {
        r.AddForce(Vector3.up * jumpForce);
    }

}
