using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BonusScene : MonoBehaviour {

    //Time the scene lasts in seconds
    float bonusTime = 20f;
    float counter;
    Image barFill;

	// Use this for initialization
	void Start () {
        barFill = GameObject.Find("BonusBar_Fill").GetComponent<Image>();
	}

    //Need to transfer player data between scenes
    void Update () {
        counter += Time.deltaTime;
        barFill.fillAmount = (bonusTime - counter) / bonusTime;
        if (counter > bonusTime) {
            SceneManager.LoadScene(1);
        }
	}
}
