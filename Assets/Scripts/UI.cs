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

    Image GetImage(string s)
    {
        return GameObject.Find(s).GetComponent<Image>();
    }

    public static void UpdateScoreText(string s) {
        playerScore.text = s;
    }

    public static void SetBonusFill(float f)
    {
        bonusFill.fillAmount = f;
    }

    public static void SetHealthFill(float f)
    {
        healthFill.fillAmount = f;
    }

    public static void SetEnergyFill(float f)
    {
        energyFill.fillAmount = f;
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
