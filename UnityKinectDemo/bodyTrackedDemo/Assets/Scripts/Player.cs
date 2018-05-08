using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

   
    float health;
    float maxHealth=10;

    float energy;
    float maxEnergy=10;
    float energyGainAmount = 0.1f; //0.1 energy gained per second

    Image healthBarFill;
    Image energyBarFill;

    float speed = 20;
    float maxSpeed = 7;

    float score;

    Rigidbody r;

    public bool canMove;
    public bool demo;
    float hitDistance = 10;
    float jumpForce = 150;
    Vector3 startPosition;

    KinectManager kinectManager;
    SimpleGestureListener sgl;

    float wallPoints = 10;

    bool inFuryMode;
    
    // Use this for initialization
    void Start()
    {
        r = GetComponent<Rigidbody>();
        r.AddForce(new Vector3(0, 0, speed));
        sgl = GameObject.Find("Main Camera").GetComponent<SimpleGestureListener>();
        UI.UpdateText(0, "Score: " + score);
       // UI.UpdateText(1, "Lives: " + lives);
        startPosition = transform.position;
        kinectManager = GameObject.Find("Main Camera").GetComponent<KinectManager>();
        health = maxHealth;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(collision.gameObject);
            RemoveLife();
        }
        /*
        if (collision.gameObject.tag == "Wall_Hole")
        {
            UpdateScore(wallPoints, true);
            Destroy(collision.gameObject);
        }
        */
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall_Hole")
        {
            UpdateScore(wallPoints, true);
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Coin")
        {
            UpdateScore(other.gameObject.GetComponent<Coin>().GetPoints(), true);
            Destroy(other.gameObject);
        }
    }

    void RemoveLife()
    {
        health -= 1;
    }

    void UpdateScore(float f, bool increment)
    {
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
    void Update()
    {
        /*
         float index = -2;
         for (int i = 0; i < 8; i++)
         {
             Vector3 offset = new Vector3(index, 1.4f, 0);
             Vector3 origin = transform.position + offset;
             Vector3 direction = transform.TransformDirection(Vector3.forward*hitDistance);
             Debug.DrawRay(origin, direction,Color.red);
             index += 0.5f;
         }
         */
        if (health > 0 && canMove)// && kinectManager.IsUserDetected())
        {

            if (r.velocity.z < maxSpeed)
            {
                r.AddRelativeForce(Vector3.forward * speed);
            }
            UpdateScore(Vector3.Distance(transform.position, startPosition), false);

        }
        else
        {
            speed = 0;
            UI.UpdateText(3, "GAME OVER !");
        }

        UI.UpdateText(2, "Speed: " + Mathf.Round(r.velocity.magnitude));
        UpdateUiBars();
        energy += Time.deltaTime * energyGainAmount;
    }


    void UpdateUiBars() {
        healthBarFill.fillAmount = health / maxHealth;
        energyBarFill.fillAmount = energy / maxEnergy;
    }

    public void Swipe()
    {


    }

    public void Jump()
    {
        r.AddForce(Vector3.up * jumpForce);
    }

    public void EnterFuryMode()
    {
        if (!inFuryMode)
        {
            inFuryMode = true;
        }
    }

    public void Punch()
    {
        Debug.Log("Punched");

        float index = -2;
        for (int i = 0; i < 8; i++)
        {
            RaycastHit hit;

            Vector3 offset = new Vector3(index, 1.4f, 0);
            Vector3 origin = transform.position + offset;
            Vector3 direction = transform.TransformDirection(Vector3.forward);
            index += 0.5f;

            if (Physics.Raycast(origin, direction, out hit, hitDistance))
            {
                if (hit.transform.tag == "Wall")
                {
                    hit.transform.gameObject.transform.root.GetComponent<WallBreak>().BreakWall();
                    Debug.Log("Punched wall");
                    break;
                }
            }
            else
            {
                Debug.Log("Missed");
            }
        }
    }

    //Enter fury mode
    public void Clap()
    {
        Debug.Log("Clapped");
        if (energy >= maxEnergy) {
            EnterFuryMode();
        }
    }

    //Enter the bonus scene
    internal void Circle()
    {
        Debug.Log("Circle detected !");
        SceneManager.LoadScene(2);
    }
}