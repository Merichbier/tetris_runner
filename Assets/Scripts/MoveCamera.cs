using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private static int HEAD = 3;
    private static float OFFSET_LOOK_AT = 10f;
    private static float OFFSET_X = 2.5f;
    private static float OFFSET_Z = 2f;
    // private instance of the KinectManager
    protected KinectManager kinectManager;

    // Use this for initialization
    void Start()
    {
        var rot = transform.rotation;
        rot.x = (float) Math.PI/12f;
        transform.rotation = rot;
    }

    // Update is called once per frame
    void Update()
    {
        var target = GameObject.FindGameObjectWithTag("Character").transform;
        FollowCharacter(target);
        MatchParallax(target);
    }

    private void FollowCharacter(Transform target)
    {
        var start = transform;

        var newPos = new Vector3(start.position.x, target.position.y + OFFSET_X, target.position.z - OFFSET_Z);
        //Debug.Log(string.Format("Char : ({0},{1},{2})", target.position.x, target.position.y, target.position.z));
        //Debug.Log(string.Format("New Pos : ({0},{1},{2})", newPos.x, newPos.y, newPos.z));
        transform.position = newPos;
    }

    private void MatchParallax(Transform target)
    {
        // Get the KinectManager instance
        if (kinectManager == null)
        {
            kinectManager = KinectManager.Instance;
        }
        if (kinectManager != null)
        {
            // Get the position of the body and store it.
            Vector3 trans = kinectManager.GetUserPosition(kinectManager.Player1);

            // Change camera position according to 
            //Debug.Log(string.Format("From : {0} -> {1})", transform.position.x, trans.x));
            transform.position = new Vector3(trans.x, transform.position.y, transform.position.z);
        }

        transform.LookAt(new Vector3(target.position.x, target.position.y, target.position.z + OFFSET_LOOK_AT));
    }
}