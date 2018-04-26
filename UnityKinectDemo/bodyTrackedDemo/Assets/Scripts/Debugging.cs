using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debugging : MonoBehaviour {

    KinectManager km;

    private const int leftHandIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.HandLeft;
    private const int rightHandIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight;

    private const int leftElbowIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.ElbowLeft;
    private const int rightElbowIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.ElbowRight;


    Image r_hand;
    Image l_hand;
    Image r_elbow;
    Image l_elbow;

    Text p_r_hand;
    Text p_l_hand;

    Text custom;
    Text custom2;

    bool clapReady;
    


    // Use this for initialization
    void Start () {
        km = GameObject.Find("Main Camera").GetComponent<KinectManager>();
        /*
        r_hand = GameObject.Find("JT_rHand").GetComponent<Image>();
        l_hand = GameObject.Find("JT_lHand").GetComponent<Image>();
        r_elbow = GameObject.Find("JT_rElbow").GetComponent<Image>();
        l_elbow = GameObject.Find("JT_lElbow").GetComponent<Image>();

        p_r_hand= GameObject.Find("p_rHand").GetComponent<Text>();
        p_l_hand = GameObject.Find("p_lHand").GetComponent<Text>();
        */
        custom = GameObject.Find("p_custom").GetComponent<Text>();
        custom2 = GameObject.Find("p_custom2").GetComponent<Text>();

    }
    /*
    IEnumerator Clap() {
        yield return new WaitForSeconds(1f);
    }
    */

    float clapCount;
    float clapCountMax = 1;
    bool clapTouch;

    float clapInitDistance = 0.4f;
    float clapDetectDistance = 0.1f;

    //Wont recognize clap gesture if hands have too much height between them
    float clap_yLimit = 0.15f;

    float clap_zLimit = 0.15f;

    // Update is called once per frame
    void Update () {
        /*
        bool[] jointsTracked = km.GetPlayer1_JointsTracked();
        SetColor(r_hand,jointsTracked[rightHandIndex]);
        SetColor(l_hand, jointsTracked[leftHandIndex]);
        SetColor(r_elbow, jointsTracked[rightElbowIndex]);
        SetColor(l_elbow, jointsTracked[leftElbowIndex]);
        */
        Vector3[] jointsPos = km.GetPlayer1_Pos();
        // SetText(p_r_hand, jointsPos[rightHandIndex]);
        //SetText(p_l_hand, jointsPos[leftHandIndex]);
        Vector3 handDistance = jointsPos[rightHandIndex] - jointsPos[leftHandIndex];

        if (!clapReady && handDistance.x > clapInitDistance && handDistance.y < clap_yLimit && handDistance.z < clap_zLimit)
        {
            clapCount = 0;
            clapReady = true;
            clapTouch = false;
        }

        if (!clapTouch && clapReady && handDistance.x < clapInitDistance && handDistance.y < clap_yLimit && handDistance.z < clap_zLimit)
        {
            clapCount += Time.deltaTime;
            if(handDistance.x<clapDetectDistance && clapCount < clapCountMax)
            {
                //DO SOMETHING HERE
                clapTouch = true;
                clapReady = false;
                clapCount = 0;
            }
            //failed to clap fast enough
            if (clapCount > clapCountMax) {
                clapTouch = false;
                clapReady = false;
                clapCount = 0;
            }
        }
        /*
        if (clapTouch)
        {
            clapCountReturn += Time.deltaTime;

        }
        */

        SetText(custom2, "ClapReady: " + clapReady + ", ClapTouch: " + clapTouch+", ClapCounter"+clapCount);
        SetText(custom, jointsPos[rightHandIndex] - jointsPos[leftHandIndex]);

    }

    void SetColor(Image img,bool tracked)
    {
        if (tracked)
        {
            img.color = Color.green;
        }
        else
        {
            img.color = Color.red;
        }

    }

    void SetText(Text t,string s)
    {
        t.text = s;
    }

    void SetText(Text t,Vector3 v)
    {
        t.text = ""+Math.Round(v.z, 3);// +"\n, "+Math.Round(v.y, 3) +"\n, "+Math.Round(v.z,3);
    }

}
