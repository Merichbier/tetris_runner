using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    static TextMeshProUGUI playerScore;

    // Use this for initialization
    void Start()
    {
        playerScore = GameObject.Find("Text_Score").GetComponent<TextMeshProUGUI>();
    }

    public static void UpdateScoreText(string s) {
        playerScore.text = s;
    }
}
