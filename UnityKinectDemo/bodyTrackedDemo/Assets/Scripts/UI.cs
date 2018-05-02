using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    static Text playerScore;
    static Text playerLives;
    static Text playerSpeed;
    static Text gameOver;
    static Text[] texts;

	// Use this for initialization
	void Start () {
        playerScore = GameObject.Find("Text_PlayerScore").GetComponent<Text>();
        playerLives = GameObject.Find("Text_PlayerLives").GetComponent<Text>();
        playerSpeed= GameObject.Find("Text_PlayerSpeed").GetComponent<Text>();
        gameOver = GameObject.Find("Text_GameOver").GetComponent<Text>();
        texts = new Text[4];
        texts[0] = playerScore;
        texts[1] = playerLives;
        texts[2] = playerSpeed;
        texts[3] = gameOver;
    }

    public static void UpdateText(int i, string s) {
        if (texts!=null) { 
            Text t = texts[i];
            t.text = s;
        }
    }

}
