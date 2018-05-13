using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomGestureHandler : MonoBehaviour
{

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

    Text custom;
    Text custom2;

    bool clapReady;
    bool clapTouch;

    private float clapCount;

    //Max amount of time it should take for clap to complete after initialising
    float clapCountMax = 0.7f;

    //Minimum distance hands need to be to start clap
    float clapInitDistance = 0.3f;

    //Detect a clap if hands are closer than this distance
    float clapDetectDistance = 0.1f;

    //Wont recognize clap gesture if hands have too much height between them
    float clap_yLimit = 0.15f;

    float clap_zLimit = 0.15f;

    Player player;


    bool punchReady;
    bool punched;

    bool slapRightReady;
    bool slapLeftReady;

    int circleStateRight = 0;
    int circleStateLeft = 0;

    private float punchCounter;
    private float slapCounterRight;
    private float slapCounterLeft;
    private float circleCounterRight;
    private float circleCounterLeft;
    private float circleApproximationThreshold = 0.01f;
    private float circleArmThreshold = 0.1f;

    //Max time to take when executing a punch
    private float punchCounterMax = 2f;
    private float slapCounterMax = 2f;
    private float circleCounterMax = 8f;

    public bool debug;

    StartUI startUI;

    // Use this for initialization
    void Start()
    {
        km = GameObject.Find("Main Camera").GetComponent<KinectManager>();
        player = GetComponent<Player>();
        startUI = GameObject.Find("GameHandler").GetComponent<StartUI>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleClap();
        HandlePunch();
        HandleSlaps();
        HandleCircles();
    }

    private void HandleSlaps()
    {
        Vector3[] jointsPos = km.GetPlayer1_Pos();
        Vector3 rightHand = jointsPos[rightHandIndex];
        Vector3 leftHand = jointsPos[leftHandIndex];
        Vector3 hips = jointsPos[hipCenterIndex];

        //Debug.Log(String.Format("Left : {0}, Right {1}", slapLeftReady, slapRightReady));
        HandleRightSlap(rightHand, hips);
        //HandleLeftSlap(leftHand, hips);
    }

    private void HandleCircles()
    {
        Vector3[] jointsPos = km.GetPlayer1_Pos();
        Vector3 rightHand = jointsPos[rightHandIndex];
        Vector3 leftHand = jointsPos[leftHandIndex];
        Vector3 rightElbow = jointsPos[rightElbowIndex];
        Vector3 leftElbow = jointsPos[leftElbowIndex];
        Vector3 righShoulder = jointsPos[rightShoulderIndex];
        Vector3 leftShoulder = jointsPos[leftShoulderIndex];

        HandleCircle(rightHand, rightElbow, righShoulder, true);
        //HandleCircle(leftHand, leftElbow, leftShoulder, false);
    }

    void SetText(Text t, string s)
    {
        t.text = s;
    }

    void SetText(Text t, Vector3 v)
    {
        t.text = "" + Math.Round(v.z, 3);// +"\n, "+Math.Round(v.y, 3) +"\n, "+Math.Round(v.z,3);
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

            if (startUI != null)
            {
                startUI.detectClap();
            }
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
                player.Clap();
                if (debug)
                {
                    custom.text = "Did Clap ? Clapped";
                }
            }
        }

        //failed to clap fast enough
        if (clapCount > clapCountMax)
        {
            clapTouch = false;
            clapReady = false;
            clapCount = 0;
            if (debug)
            {
                custom.text = "Did Clap ? ";
            }
        }

    }



    void HandlePunch()
    {
        Vector3[] jointsPos = km.GetPlayer1_Pos();
        Vector3 diffPos = jointsPos[rightHandIndex] - jointsPos[rightShoulderIndex];

        if (!punchReady && Mathf.Abs(diffPos.z) < 0.33f)
        {
            punchReady = true;
            punched = false;
            punchCounter = 0;
            if (debug)
            {
                custom2.text = "Did Punch ? ";
            }
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

                if (startUI != null)
                {
                    startUI.detectPunch();
                }
            }
            if (punchCounter > punchCounterMax)
            {
                punched = false;
                punchReady = false;
                punchCounter = 0;
                if (debug)
                {
                    custom2.text = "Did Punch ? ";
                }
            }
        }
    }

    void HandleRightSlap(Vector3 hand, Vector3 hips)
    {

        bool isRight = hand.x > hips.x;
        bool isLeft = hand.x < hips.x;
        // Reset every variable if right hand on the right
        if (isRight)
        {
            resetRightSlap();
            return;
        }
        slapCounterRight += Time.deltaTime;

        //Debug.Log(String.Format("Left : {0}, Right {1}", isLeft, isRight));
        if (slapCounterRight < slapCounterMax)
        {
            if (!slapRightReady && isLeft)
            {
                // State 1 of slaping with right hand
                Debug.Log("Right Slap ready");
                slapRightReady = true;
                return;
            }
            if (slapRightReady && isRight)
            {
                Debug.Log("RightSlap");
                resetRightSlap();
                player.Slap();
                return;
            }
        }
        else
        {
            resetRightSlap();
        }

    }

    private void resetRightSlap()
    {
        slapRightReady = false;
        slapCounterRight = 0;
    }

    void HandleLeftSlap(Vector3 hand, Vector3 hips)
    {
        bool isRight = hand.x > hips.x;
        bool isLeft = hand.x < hips.x;
        // Reset every variable if right hand on the right
        if (isLeft)
        {
            resetLeftSlap();
            return;
        }
        slapCounterLeft += Time.deltaTime;
        if (slapCounterLeft < slapCounterMax)
        {
            if (!slapLeftReady && isRight)
            {
                // State 1 of slaping with left hand
                Debug.Log("Left Slap Ready");
                slapLeftReady = true;
            }
            else if (slapLeftReady && isLeft)
            {
                Debug.Log("LeftSlap");
                resetLeftSlap();
                player.Slap();

            }
        }else
        {
            resetLeftSlap();
        }
    }

    private void resetLeftSlap()
    {
        slapLeftReady = false;
        slapCounterLeft = 0;
    }

    void HandleCircle(Vector3 hand, Vector3 elbow, Vector3 shoulder, bool forRightHand)
    {
        // We want the player to fully extend the arm when doing a circle
        float straighDist = Vector3.Magnitude(hand - shoulder);
        float joinDist = Vector3.Magnitude(hand - elbow) + Vector3.Magnitude(elbow - shoulder);
        bool isArmExtended = ApproxmateEqual(joinDist, straighDist, circleArmThreshold);

        bool isNorth = ApproxmateEqual(hand.x, shoulder.x, circleApproximationThreshold)
            && hand.y > shoulder.y;

        bool isNorthEast = hand.x > shoulder.x
            && hand.y > shoulder.y;

        bool isEast = hand.x > shoulder.x
            && ApproxmateEqual(hand.y, shoulder.y, circleApproximationThreshold);

        bool isSouthEast = hand.x > shoulder.x
            && hand.y < shoulder.y;

        bool isSouth = ApproxmateEqual(hand.x, shoulder.x, circleApproximationThreshold)
            && hand.y < shoulder.y;

        bool isSouthWest = hand.x < shoulder.x
            && hand.y < shoulder.y;

        bool isWest = hand.x < shoulder.x
            && ApproxmateEqual(hand.y, shoulder.y, circleApproximationThreshold);

        bool isNorthWest = hand.x < shoulder.x
            && hand.y > shoulder.y;


        // Reset counter if state is 0
        circleCounterRight = circleStateRight == 0 ? 0 : circleStateRight;
        circleCounterLeft = circleStateLeft == 0 ? 0 : circleStateLeft;

        // Increase Counter
        circleCounterRight += forRightHand ? Time.deltaTime : 0;
        circleCounterLeft += forRightHand ? 0 : Time.deltaTime;
        //Debug.Log("Counter is : " + circleCounterRight);


        float circleCounter = forRightHand ? circleCounterRight : circleCounterLeft;
        int circleState = forRightHand ? circleStateRight : circleStateLeft;
        //Debug.Log(String.Format("N : {0}, NE : {1}, E : {2}, SE : {3}, S : {4}, SW : {5}, W : {6}, NW : {7}, state : {8}", isNorth, isNorthEast, isEast, isSouthEast, isSouth, isSouthWest, isWest, isNorthWest, circleStateRight));
        if (circleCounter < circleCounterMax && isArmExtended)
        {
            if ((circleState == 0 && isNorth)
                || (circleState == 1 && isNorthEast)
                || (circleState == 2 && isEast)
                || (circleState == 3 && isSouthEast)
                || (circleState == 4 && isSouth)
                || (circleState == 5 && isSouthWest)
                || (circleState == 6 && isWest)
                || (circleState == 7 && isNorthWest))
            {
                Debug.Log("CircleState" + (forRightHand ? "Right" : "Left") + " is now : " + (++circleState));

                if (forRightHand)
                {
                    circleStateRight += 1;
                    circleCounterRight = 0;
                }
                else
                {
                    circleStateLeft += 1;
                    circleCounterLeft = 0;
                }

            }

            if (circleState == 8)
            {
                player.Circle();
                if (startUI != null)
                {
                    startUI.detectCircle();
                }
                resetCirleSettings("", forRightHand);
            }

        }
        else if (!(circleCounter < circleCounterMax) && circleState > 0)
        {
            resetCirleSettings("too slow", forRightHand);
        }
        else if (!isArmExtended && circleState > 0)
        {
            resetCirleSettings("arm not exteded", forRightHand);
        }
    }

    private void resetCirleSettings(string msg, bool forRightHand)
    {
        Debug.Log("Circle aborted : " + msg);
        if (forRightHand)
        {
            circleStateRight = 0;
            circleCounterRight = 0;
        }
        else
        {
            circleStateLeft = 0;
            circleCounterLeft = 0;
        }
    }

    private bool ApproxmateEqual(float a, float b, float threshold)
    {
        return Math.Abs(b - a) < threshold;
    }
}
