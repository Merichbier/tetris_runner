﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{
    float health;
    float maxHealth=3;

    float energy;
    float maxEnergy=10;
    float energyGainAmount = 0.5f; 
    float energyLossAmount = 2.5f; 

    Image healthBarFill;
    Image energyBarFill;

    float speed = 20;
    float maxSpeed = 7;

    float score;

    TextMeshProUGUI gameOverText;
    TextMeshProUGUI scoreText;

    Rigidbody r;

    public bool canMove;

    float hitDistance = 10;
    float jumpForce = 150;
    Vector3 startPosition;

    KinectManager kinectManager;
    BonusScene bonus;

    float wallPoints = 10;

    bool inFuryMode;

    string sceneName;

    bool canPunchWall;

    ParticleSystem dust;
    ParticleSystem beams;
    Behaviour halo;

    bool increaseBarAlpha;
    bool furyModeReady;

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

        gameOverText = GameObject.Find("GameOver").GetComponent<TextMeshProUGUI>();
        scoreText = GameObject.Find("TotalScore").GetComponent<TextMeshProUGUI>();

    }

    float healthLossCooldown; //prevent wall collision removing more than 1 health
    float healthLossCooldownMax = 1;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Enemy" && healthLossCooldown <= 0)
        {
            Destroy(collision.gameObject);
            if (!inFuryMode)
            {
                RemoveLife();
                healthLossCooldown = healthLossCooldownMax;
            }
        }
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


    


    void EnergyBarAnimation() {
        furyModeReady = energy >= maxEnergy;

        if (furyModeReady || inFuryMode)
        {

            if (!increaseBarAlpha && energyBarFill.color.a >= 0)
            {
                Color c = energyBarFill.color;
                c.a -= Time.deltaTime;
                energyBarFill.color = c;
            }
            else if (increaseBarAlpha && energyBarFill.color.a <= 1)
            {
                Color c = energyBarFill.color;
                c.a += Time.deltaTime;
                energyBarFill.color = c;
            }

            if (energyBarFill.color.a <= 0)
            {
                increaseBarAlpha = true;
                Color c = energyBarFill.color;
                c.a = 0;
                energyBarFill.color = c;
            }
            else if (energyBarFill.color.a >= 1)
            {
                increaseBarAlpha = false;
                Color c = energyBarFill.color;
                c.a = 1;
                energyBarFill.color = c;
            }
        }

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

    void HealthLossCooldownPeriod()
    {
        if (healthLossCooldown > 0)
        {
            healthLossCooldown -= Time.deltaTime;
            if (healthLossCooldown <= 0)
            {
                healthLossCooldown = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        EnergyBarAnimation();
        DetectWall();
        
        if (sceneName == "Start") {
            return;
        }

        if (health > 0 && canMove)// && kinectManager.IsUserDetected())
        {
            HealthLossCooldownPeriod();
            if (r.velocity.z < maxSpeed)
            {
                r.AddRelativeForce(Vector3.forward * speed);
            }
            UpdateScore(Vector3.Distance(transform.position, startPosition), false);
        }
        else
        {
            Die();
        }
        
        UpdateUiBars();
        HandleEnergy();
    }

    void HandleEnergy() {
        energy += Time.deltaTime * energyGainAmount;
        if (inFuryMode && energy > 0)
        {
            energy -= Time.deltaTime * energyLossAmount;
            if (energy <= 0)
            {
                energy = 0;
                ExitFuryMode();
            }
        }
    }

    void Die()
    {
        speed = 0;
        gameOverText.enabled = true;
        scoreText.text = "You scored " + Math.Round(score) + " points";
        scoreText.enabled = true;
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

            Color c = energyBarFill.color;
            c.a = 1;
            energyBarFill.color = c;
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