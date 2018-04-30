using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    static Text playerScore;
    static Text playerLives;
    static Text[] texts;

    // Use this for initialization
    void Start()
    {
        playerScore = GameObject.Find("Text_PlayerScore").GetComponent<Text>();
        playerLives = GameObject.Find("Text_PlayerLives").GetComponent<Text>();
        texts = new Text[3];
        texts[0] = playerScore;
        texts[1] = playerLives;
        texts[2].text = Application.loadedLevel.ToString();
    }

    public static void UpdateText(int i, string s)
    {
        Text t = texts[i];
        t.text = s;
    }

}
