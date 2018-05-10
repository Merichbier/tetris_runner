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
    float energyGainAmount = 0.5f; //0.1 energy gained per second

    Image healthBarFill;
    Image energyBarFill;

    float speed = 20;
    float maxSpeed = 7;

    float score;

    Rigidbody r;

    public bool canMove;

    float hitDistance = 10;
    float jumpForce = 150;
    Vector3 startPosition;

    KinectManager kinectManager;
   // SimpleGestureListener sgl;
    BonusScene bonus;

    float wallPoints = 10;

    bool inFuryMode;

    string sceneName;

    bool canPunchWall;


    ParticleSystem dust;
    ParticleSystem beams;
    Behaviour halo;


    // Use this for initialization
    void Start()
    {
        r = GetComponent<Rigidbody>();
        r.AddForce(new Vector3(0, 0, speed));
        //sgl = GameObject.Find("Main Camera").GetComponent<SimpleGestureListener>();
        bonus = GameObject.Find("GameHandler").GetComponent<BonusScene>();

       // UI.UpdateText(1, "Lives: " + lives);
        startPosition = transform.position;
        kinectManager = GameObject.Find("Main Camera").GetComponent<KinectManager>();
        health = maxHealth;

        sceneName = SceneManager.GetActiveScene().name;
        if (sceneName != "Start") { 
            healthBarFill = GameObject.Find("HealthBarFill").GetComponent<Image>();
            energyBarFill = GameObject.Find("EnergyBarFill").GetComponent<Image>();
        }

        halo = (Behaviour)GameObject.Find("Halo").GetComponent("Halo");



        dust = GameObject.Find("FuryAura").GetComponentsInChildren<ParticleSystem>()[0];
        beams = GameObject.Find("FuryAura").GetComponentsInChildren<ParticleSystem>()[1];

        dust.Pause();
        beams.Pause();

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(collision.gameObject);
            if (!inFuryMode) { 
                RemoveLife();
            }
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
            Coin coin = other.gameObject.GetComponent<Coin>();
            UpdateScore(coin.GetPoints(), true);
            coin.HideCoin();
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
    }

    /*
        Vector3 offset = new Vector3(0, 1.4f, 0);
        Vector3 origin = transform.position + offset;
        Vector3 direction = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(origin, direction * hitDistance, Color.yellow);
        */


    void DetectWall()
    {
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
                    canPunchWall = true;
                    break;
                }
                else
                {
                    canPunchWall = false;
                }                
            } else
            {
                canPunchWall = false;
            }
        }
        UI.SetPunchIcon(canPunchWall);
    }


    // Update is called once per frame
    void Update()
    {
        DetectWall();
        if (sceneName == "Start") {
            return;
        }
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
        }
        
        UpdateUiBars();
        energy += Time.deltaTime * energyGainAmount;
    }


    void UpdateUiBars() {
        healthBarFill.fillAmount = health / maxHealth;
        energyBarFill.fillAmount = energy / maxEnergy;
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
            halo.enabled = true;
            dust.Play();
            beams.Play();
        }
    }

    public void ExitFuryMode()
    {
        if (inFuryMode)
        {
            inFuryMode = false;
            halo.enabled = false;
            dust.Clear();
            beams.Clear();

            dust.Pause();
            beams.Pause();
        }
    }

    void HandleFury() {
        if (inFuryMode)
        {
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
                //Debug.Log("Missed");
            }
        }
    }

    //Enter fury mode
    public void Clap()
    {
        Debug.Log("Clapped");
        if (energy >= maxEnergy || SceneManager.GetActiveScene().name=="Start") {
            EnterFuryMode();
        } 
    }

    //Enter the bonus scene
    internal void Circle()
    {
        Debug.Log("Circle detected !");
        bonus.TurnOnBonus();        
    }
}