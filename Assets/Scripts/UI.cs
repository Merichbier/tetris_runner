using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{


    static Image bonusFill;
    static Image healthFill;
    static Image energyFill;
    static Image punchIcon;
    static Image furyIcon;
    static TextMeshProUGUI playerScore;
    static TextMeshProUGUI gameOver;

    static float lerpSpeed = 4;
    static float healthAmount = 1;
    static float energyAmount;
    static float bonusAmount;

    // Use this for initialization
    void Start()
    {
        playerScore = GameObject.Find("Text_Score").GetComponent<TextMeshProUGUI>();
        bonusFill = GetImage("Bonus_Fill");
        healthFill = GetImage("Health_Fill");
        energyFill = GetImage("Energy_Fill");
        punchIcon = GetImage("PunchIcon");
        furyIcon = GetImage("FuryIcon");
        gameOver = GameObject.Find("Text_GameOver").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        healthFill.fillAmount = Mathf.Lerp(healthFill.fillAmount, healthAmount, Time.deltaTime * lerpSpeed);
        energyFill.fillAmount = Mathf.Lerp(energyFill.fillAmount, energyAmount, Time.deltaTime * lerpSpeed);
        bonusFill.fillAmount = Mathf.Lerp(bonusFill.fillAmount, bonusAmount, Time.deltaTime * lerpSpeed);
        BarAnimations();
    }

    float alpha;

    void BarAnimations()
    {
        Color c = energyFill.color;
        Color c2 = bonusFill.color;

        alpha = Mathf.PingPong(Time.time, 1);

        c.a = alpha;
        c2.a = alpha;


        if (energyFill.fillAmount >= 1) {
            energyFill.color = c;
        }
        if (bonusFill.fillAmount >= 1)
        {
            bonusFill.color = c2;
        }
    }

    /*
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
           */

    Image GetImage(string s)
    {
        return GameObject.Find(s).GetComponent<Image>();
    }

    public static void UpdateScoreText(string s) {
        playerScore.text = s;
    }

    public static void SetBonusFill(float f)
    {
        bonusAmount = f;
    }

    public static void SetHealthFill(float f)
    {
        healthAmount = f;
    }

    public static void SetEnergyFill(float f)
    {
        energyAmount = f;
    }

    public static void SetPunchIcon(bool b)
    {
        punchIcon.enabled = b;
    }

    public static void SetFuryIcon(bool b)
    {
        furyIcon.enabled = b;
    }

    public static void SetGameOver(bool b)
    {
        gameOver.enabled = b;
    }
}
