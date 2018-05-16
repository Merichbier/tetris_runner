using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BonusScene : MonoBehaviour {

    //Time the scene lasts in seconds
    float bonusTime = 10f;
    float counter;

    Image barBorder;
    Image barFill;

    public Material bonusMaterial;
    public Material normalMaterial;

    public Material bonusCoin;
    public Material normalCoin;

    PlaneManager pm;
    CoinSpawner cs;

    int bonusPoints = 20;
    int normalPoints = 10;

    bool bonusEnabled;

	// Use this for initialization
	void Start () {
        barFill = GameObject.Find("BonusBar_Fill").GetComponent<Image>();
        barBorder = GameObject.Find("BonusBar_Border").GetComponent<Image>();

        pm = GameObject.Find("PlaneManager").GetComponent<PlaneManager>();
        cs = GetComponent<CoinSpawner>();


        barFill.enabled = false;
        barBorder.enabled = false;
    }

    public void TurnOnBonus() {
        bonusEnabled = true;
       // pm.ChangeMaterial(bonusMaterial);
        barFill.enabled = true;
        barBorder.enabled = true;
        cs.ChangeCoinProperties(bonusCoin,bonusPoints);
    }

    void TurnOffBonus() {
        bonusEnabled = false;
        //pm.ChangeMaterial(normalMaterial);
        barFill.enabled = false;
        barBorder.enabled = false;        
        cs.ChangeCoinProperties(normalCoin,normalPoints);
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.K)) {
            TurnOnBonus();
        }
        if (bonusEnabled) { 
            counter += Time.deltaTime;
            barFill.fillAmount = (bonusTime - counter) / bonusTime;
            if (counter > bonusTime) {
                TurnOffBonus();
                counter = 0;
            }
        }
    }
}
