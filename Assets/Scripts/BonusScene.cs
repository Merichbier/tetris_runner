using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BonusScene : MonoBehaviour {

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
        pm = GameObject.Find("PlaneManager").GetComponent<PlaneManager>();
        cs = GetComponent<CoinSpawner>();
     }

    public void TurnOnBonus() {
        bonusEnabled = true;
        cs.ChangeCoinProperties(bonusCoin,bonusPoints);
    }

    public void TurnOffBonus() {
        bonusEnabled = false;
        cs.ChangeCoinProperties(normalCoin,normalPoints);
    }
}
