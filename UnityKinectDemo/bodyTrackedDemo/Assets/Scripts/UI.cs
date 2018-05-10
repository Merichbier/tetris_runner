using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    static Text playerScore;
    static Text playerLives;
    static Text playerSpeed;
    static Text gameOver;
    static Text[] texts;

    static Image punchIcon;

    // Use this for initialization
    void Start()
    {

        punchIcon = GameObject.Find("Punch_Icon").GetComponent<Image>();
    }

    public static void SetPunchIcon(bool b) {
        punchIcon.enabled = b;
    }
}
