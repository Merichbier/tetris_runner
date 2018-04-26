using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	int lives=3;
    float score;
	float speed=20;
	Rigidbody r;
    float maxSpeed = 10;
    public bool canMove;
    SimpleGestureListener sgl;
    float hitDistance = 2;
    float jumpForce = 150;
    Vector3 startPosition;
    KinectManager kinectManager;
    float wallPoints = 10;

    bool furyModeReady;
    bool inFuryMode;

    // Use this for initialization
    void Start () {		
		r = GetComponent<Rigidbody> ();
		r.AddForce(new Vector3(0,0,speed));
        sgl = GameObject.Find("Main Camera").GetComponent<SimpleGestureListener>();
        UI.UpdateText(0, "Score: " + score);
        UI.UpdateText(1, "Lives: " + lives);
        startPosition = transform.position;
        kinectManager = GameObject.Find("Main Camera").GetComponent<KinectManager>();
    }

    void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Wall") {
			Destroy (collision.gameObject);
            RemoveLife();
        }
        if (collision.gameObject.tag == "Coin")
        {
            UpdateScore(collision.gameObject.GetComponent<Coin>().GetPoints(),true);
            Destroy(collision.gameObject);            
        }
        if (collision.gameObject.tag == "Wall_Hole")
        {
            UpdateScore(wallPoints, true);
            Destroy(collision.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall_Hole")
        {
            UpdateScore(wallPoints, true);
            Destroy(other.gameObject);
        }
    }

    void RemoveLife()
    {
        lives -= 1;
        UI.UpdateText(1, "Lives: " + lives);
    }

    void UpdateScore(float f,bool increment) {
        if (increment)
        {
            score += f;
        }
        else
        {
            score = f;
        }        
        UI.UpdateText(0, "Score: " + Mathf.Round(score));
    }

    /*
        Vector3 offset = new Vector3(0, 1.4f, 0);
        Vector3 origin = transform.position + offset;
        Vector3 direction = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(origin, direction * hitDistance, Color.yellow);
        */

    // Update is called once per frame
    void Update () {
        if (lives > 0 && canMove)// && kinectManager.IsUserDetected())
        {
            
            if (r.velocity.z < maxSpeed)
            {
                r.AddRelativeForce(Vector3.forward * speed);
            }
            UpdateScore(Vector3.Distance(transform.position,startPosition),false);
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

    public void EnterFuryMode()
    {
        if (furyModeReady) { 
            inFuryMode = true;
            furyModeReady = false;
        }
    }

    public void Punch()
    {
        Debug.Log("Punched");
    }

}
