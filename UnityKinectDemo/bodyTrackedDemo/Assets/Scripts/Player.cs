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

    float hitDistance = 10;
    float jumpForce = 150;
    Vector3 startPosition;

    KinectManager kinectManager;
   // SimpleGestureListener sgl;
    BonusScene bonus;

    float wallPoints = 10;

    bool inFuryMode;

    string sceneName;
    
    // Use this for initialization
    void Start()
    {
        r = GetComponent<Rigidbody>();
        r.AddForce(new Vector3(0, 0, speed));
        //sgl = GameObject.Find("Main Camera").GetComponent<SimpleGestureListener>();
        bonus = GameObject.Find("GameHandler").GetComponent<BonusScene>();
        UI.UpdateText(0, "Score: " + score);
       // UI.UpdateText(1, "Lives: " + lives);
        startPosition = transform.position;
        kinectManager = GameObject.Find("Main Camera").GetComponent<KinectManager>();
        health = maxHealth;

        sceneName = SceneManager.GetActiveScene().name;
        if (sceneName != "Start") { 
            healthBarFill = GameObject.Find("HealthBarFill").GetComponent<Image>();
            energyBarFill = GameObject.Find("EnergyBarFill").GetComponent<Image>();
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            WallSpawn ws = GameObject.Find("WallManager").GetComponent<WallSpawn>();
            ws.RemoveCollidedWall(collision.gameObject);
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
        //Debug.Log("Collision with " + other.gameObject.tag);
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
        if (other.gameObject.tag == "Enemy")
        {
            EnemyManager em = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
            em.RemoveCollidedEnemy(other.gameObject);
            RemoveLife();
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

        //float index = -2;
        //for (int i = 0; i < 8; i++)
        //{
        //    RaycastHit hit;

        //    Vector3 offset = new Vector3(index, 1.4f, 0);
        //    Vector3 origin = transform.position + offset;
        //    Vector3 direction = transform.TransformDirection(Vector3.forward);
        //    index += 0.5f;

        //    if (Physics.Raycast(origin, direction, out hit, hitDistance))
        //    {
        //        if (hit.transform.tag == "Wall")
        //        {
        //            hit.transform.gameObject.transform.root.GetComponent<WallBreak>().BreakWall();
        //            Debug.Log("Punched wall");
        //            break;
        //        }
        //    }
        //    else
        //    {
        //        //Debug.Log("Missed");
        //    }
        //}
        var wallManager = GameObject.Find("WallManager").GetComponent<WallSpawn>();
        wallManager.TryDestroyWall(transform.position);
    }

    //Enter fury mode
    public void Clap()
    {
        Debug.Log("Clapped");
        if (energy >= maxEnergy) {
            EnterFuryMode();
        }
    }

    public void Slap()
    {
        var enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        enemyManager.TryDestroyEnemy(transform.position);
    }

    //Enter the bonus scene
    internal void Circle()
    {
        Debug.Log("Circle detected !");
        bonus.TurnOnBonus();        
    }
}