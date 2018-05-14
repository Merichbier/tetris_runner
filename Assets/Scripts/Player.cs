using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{   
    float health;
    float maxHealth=10;

    float energy;
    float maxEnergy=10;
    float energyGainAmount = 0.1f; //0.1 energy gained per second
    private float energyLossAmount = 2;

    Image healthBarFill;
    Image energyBarFill;

    float speed = 40;
    float maxSpeed = 7;

    float score;

    Rigidbody r;

    public bool canMove;

    float hitDistance = 10;
    float jumpForce = 150;
    Vector3 startPosition;

    KinectManager kinectManager;
    BonusScene bonus;

    float wallPoints = 10;

    float punchEnergy = 2;

    string sceneName;

    bool increaseBarAlpha;
    bool inFuryMode;
    bool furyModeReady;
    bool canPunchWall;

    float healthLossCooldown; //prevent wall collision removing more than 1 health
    float healthLossCooldownMax = 1;
    



    //particle effects
    ParticleSystem dust;
    ParticleSystem beams;
    Behaviour halo;
  

    // Use this for initialization
    void Start()
    {
        r = GetComponent<Rigidbody>();
        r.AddForce(new Vector3(0, 0, speed));
        bonus = GameObject.Find("GameHandler").GetComponent<BonusScene>();
        UI.UpdateScoreText("Score: " + score);
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

        //gameOverText = GameObject.Find("GameOver").GetComponent<TextMeshProUGUI>();
        //scoreText = GameObject.Find("TotalScore").GetComponent<TextMeshProUGUI>();
    }



    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall" && healthLossCooldown <= 0)
        {
            WallSpawn ws = GameObject.Find("WallManager").GetComponent<WallSpawn>();
            ws.RemoveCollidedWall(collision.gameObject);

            if (!inFuryMode)
            {
                RemoveLife();
                healthLossCooldown = healthLossCooldownMax;
            }
        }
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
        if (healthLossCooldown <= 0 && other.gameObject.tag == "Enemy")
        {
            EnemyManager em = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
            em.RemoveCollidedEnemy(other.gameObject);

            if (!inFuryMode)
            {
                RemoveLife();
                healthLossCooldown = healthLossCooldownMax;
            }

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
        UI.UpdateScoreText("Score: " + Mathf.Round(score));
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneName == "Start") {
            return;
        }

        EnergyBarAnimation();

        if (Input.GetKeyDown(KeyCode.G))
        {
            energy = maxEnergy;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Circle();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            EnterFuryMode();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Punch();
        }


        healthLossCooldown -= Time.deltaTime;

        if (health > 0 && canMove)// && kinectManager.IsUserDetected())
        {

            if (r.velocity.z < maxSpeed)
            {
                var force = Vector3.forward * speed;
                force.y = 1;
                r.AddRelativeForce(force);
            }
            UpdateScore(Vector3.Distance(transform.position, startPosition), false);

        }
        else
        {
            Die();
        }

        UpdateUiBars();
        HandleEnergy();

        //UI.UpdateText(2, "Speed: " + Mathf.Round(r.velocity.magnitude));
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

    void HandleEnergy()
    {
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
        if (energy > punchEnergy) { 
            var wallManager = GameObject.Find("WallManager").GetComponent<WallSpawn>();
            wallManager.TryDestroyWall(transform.position);
            energy -= punchEnergy;
            Debug.Log("Energy used for punch");
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

    void EnergyBarAnimation()
    {
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

    void Die()
    {
        speed = 0;
        /*
        gameOverText.enabled = true;
        scoreText.text = "You scored " + Math.Round(score) + " points";
        scoreText.enabled = true;
        */
    }

}