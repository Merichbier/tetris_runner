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
    float maxEnergy=100;
    float energyGainAmount = 1f; //energy gained per second
    float energyLossAmount = 15;

    Image healthBarFill;
    Image energyBarFill;

    float speed = 40;
    float maxSpeed = 7;

    float score;
    int distScore;

    float coinCooldown;
    float coinCooldownMax = 0.05f;

    Rigidbody r;

    public bool canMove;
    public bool gameOver = false;

    float hitDistance = 10;
    float jumpForce = 150;
    Vector3 startPosition;

    KinectManager kinectManager;
    BonusScene bonus;
    AudioManager audioManager;

    float wallPoints = 10;

    float punchEnergy = 20;

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
    
    AudioSource audioSource;
    float pitchStart = 1f;
    float pitchEnd = 1.5f;
    float pitchIncrement = 0.01f;
 
    Image punchIconBack;
    Image punchIcon;
    
    TextMeshProUGUI gameOverText;

    ParticleSystem snowballExplode;

    // Use this for initialization
    void Start()
    {
        r = GetComponent<Rigidbody>();
        r.AddForce(new Vector3(0, 0, speed));
        bonus = GameObject.Find("GameHandler").GetComponent<BonusScene>();
        UI.UpdateScoreText("Score: " + score);
        startPosition = transform.position;
        kinectManager = GameObject.Find("Main Camera").GetComponent<KinectManager>();
        audioManager = GetComponentInChildren<AudioManager>();
        health = maxHealth;
        snowballExplode = GameObject.Find("SnowballExplode").GetComponent<ParticleSystem>();

        sceneName = SceneManager.GetActiveScene().name;
        if (sceneName != "Start") { 
            healthBarFill = GameObject.Find("HealthBarFill").GetComponent<Image>();
            energyBarFill = GameObject.Find("EnergyBarFill").GetComponent<Image>();
            
            bonus = GameObject.Find("GameHandler").GetComponent<BonusScene>();
            UI.UpdateScoreText("Score: " + 0);

            punchIcon = GameObject.Find("PunchIcon_Back").GetComponent<Image>();
            punchIconBack = GameObject.Find("PunchIcon").GetComponent<Image>();

            halo = (Behaviour)GameObject.Find("Halo").GetComponent("Halo");
            dust = GameObject.Find("FuryAura").GetComponentsInChildren<ParticleSystem>()[0];
            beams = GameObject.Find("FuryAura").GetComponentsInChildren<ParticleSystem>()[1];

            dust.Pause();
            beams.Pause();

            audioSource = GetComponentInChildren<AudioSource>();

            audioSource.pitch = pitchStart;
        }

        gameOverText = GameObject.Find("Text_GameOver").GetComponent<TextMeshProUGUI>();
   }

    void TestInput()
    {
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
        if (Input.GetKeyDown(KeyCode.I))
        {
            health = 0;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall" && healthLossCooldown <= 0)
        {
            WallSpawn ws = GameObject.Find("WallManager").GetComponent<WallSpawn>();
            ws.RemoveCollidedWall(collision.gameObject);

            if (!inFuryMode && collision.gameObject.name != "HoledWall9")
            {
                RemoveLife();
                audioManager.playWallHitSound();
                Debug.Log(collision.gameObject.name);
                healthLossCooldown = healthLossCooldownMax;
            }
        }
    }

    //arsalan branch
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall_Hole")
        {
            UpdateScore(wallPoints, true);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Coin")
        {
            if (coinCooldown <= 0)
            {
                audioManager.playCoinPickupSound();
                /*
                if (true || !audioSource.isPlaying)
                {
                    audioSource.Play();
                    audioSource.pitch += pitchIncrement;
                    if (audioSource.pitch >= pitchEnd)
                    {
                        audioSource.pitch = pitchStart;
                    }
                }
                */
                coinCooldown = coinCooldownMax;
                Coin coin = other.gameObject.GetComponent<Coin>();
                UpdateScore(coin.GetPoints());

                coin.Move();
            }
        }
        else if (healthLossCooldown <= 0 && other.gameObject.tag == "Enemy")
        {
            snowballExplode.Play();
            StartCoroutine(StopExplode());
            EnemyManager em = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
            em.RemoveCollidedEnemy(other.gameObject);

            if (!inFuryMode)
            {
                RemoveLife();
                audioManager.playSnowballHitSound();
                healthLossCooldown = healthLossCooldownMax;
            }

        }
    }

    IEnumerator StopExplode()
    {
        yield return new WaitForSeconds(1);
        snowballExplode.Pause();
        snowballExplode.Clear();
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

    void PlayerAlive()
    {
        if (health > 0 && canMove)// && kinectManager.IsUserDetected())
        {

            if (r.velocity.z < maxSpeed)
            {
                var force = Vector3.forward * speed;
                force.y = 1;
                r.AddRelativeForce(force);
            }
            distScore = (int)Math.Floor(Vector3.Distance(transform.position, startPosition));
        }
        else
        {
            Die();
        }
    }

    void UpdateScore(float f)
    {
        score += f;
        UI.UpdateScoreText("Score: " + Mathf.Round(distScore + score));
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneName == "Start") {
            return;
        }

        EnergyBarAnimation();
        TestInput();
        
        PlayerAlive();
        HandleEnergy();
        HandleEnergy();
        UpdateScore(0);
        UpdateUiBars();
        
        energy += Time.deltaTime * energyGainAmount;
        coinCooldown -= Time.deltaTime;
        healthLossCooldown -= Time.deltaTime;
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
        punchIconBack.enabled = energy >= punchEnergy;
        punchIcon.enabled = energy >= punchEnergy;
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
            audioManager.playWallBreakSound();
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

        if (energy >= maxEnergy)
        {
            energy = 0;
            bonus.TurnOnBonus();

            Color c = energyBarFill.color;
            c.a = 1;
            energyBarFill.color = c;
        }

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
        gameOver = true;
        gameOverText.enabled = true;
        GameObject.Find("RunSound").GetComponent<AudioSource>().Stop();
    }

}