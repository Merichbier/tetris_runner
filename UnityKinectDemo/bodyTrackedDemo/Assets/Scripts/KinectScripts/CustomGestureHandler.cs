using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGestureHandler : MonoBehaviour {


    KinectManager km;

    private const int leftHandIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.HandLeft;
    private const int rightHandIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.HandRight;

    private const int leftElbowIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.ElbowLeft;
    private const int rightElbowIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.ElbowRight;

    private const int leftShoulderIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.ShoulderLeft;
    private const int rightShoulderIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.ShoulderRight;

    private const int hipCenterIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.HipCenter;
    private const int shoulderCenterIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.ShoulderCenter;
    private const int leftHipIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.HipLeft;
    private const int rightHipIndex = (int)KinectWrapper.NuiSkeletonPositionIndex.HipRight;


    bool clapReady;

    private float clapCount;
    float clapCountMax = 0.7f;

    bool clapTouch;

    float clapInitDistance = 0.4f;
    float clapDetectDistance = 0.1f;

    //Wont recognize clap gesture if hands have too much height between them
    float clap_yLimit = 0.15f;

    float clap_zLimit = 0.15f;

    Player player;

    // Use this for initialization
    void Start()
    {
        km = GameObject.Find("Main Camera").GetComponent<KinectManager>();
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleClap();
    }

    void HandleClap()
    {
        Vector3[] jointsPos = km.GetPlayer1_Pos();
        Vector3 handDistance = jointsPos[rightHandIndex] - jointsPos[leftHandIndex];

        //Initiate the clap
        if (!clapReady && handDistance.x > clapInitDistance && handDistance.y < clap_yLimit && handDistance.z < clap_zLimit)
        {
            clapCount = 0;
            clapReady = true;
            clapTouch = false;
        }

        //make sure clap happens fast enough
        if (!clapTouch && clapReady && handDistance.x < clapInitDistance && handDistance.y < clap_yLimit && handDistance.z < clap_zLimit)
        {
            clapCount += Time.deltaTime;
            if (handDistance.x < clapDetectDistance && clapCount < clapCountMax)
            {
                //DO SOMETHING HERE
                clapTouch = true;
                clapReady = false;
                clapCount = 0;
                player.EnterFuryMode();
                Debug.Log("clapped");
            }

            //failed to clap fast enough
            if (clapCount > clapCountMax)
            {
                clapTouch = false;
                clapReady = false;
                clapCount = 0;
            }
        }
    }



    bool punchReady;
    bool punched;

    private float punchCounter;
    float punchCounterMax = 0.6f;

    void HandlePunch()
    {
        Vector3[] jointsPos = km.GetPlayer1_Pos();
        Vector3 diffPos = jointsPos[rightHandIndex] - jointsPos[rightShoulderIndex];

        if (!punchReady && Mathf.Abs(diffPos.z) < 0.33f)
        {
            punchReady = true;
            punched = false;
            punchCounter = 0;
        }

        if (punchReady && !punched && Mathf.Abs(diffPos.z) > 0.33f)
        {
            punchCounter += Time.deltaTime;
            if (Mathf.Abs(diffPos.z) > 0.65f && punchCounter < punchCounterMax)
            {
                //Do something
                punched = true;
                punchReady = false;
                punchCounter = 0;
                player.Punch();
            }
            if (punchCounter > punchCounterMax)
            {
                punched = false;
                punchReady = false;
                punchCounter = 0;
            }
        }
    }


}
