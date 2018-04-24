using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {
    KinectManager kinectManager;

    void Start()
    {
    }


    // Update is called once per frame
    void Update () {
        KinectManager kinectManager = KinectManager.Instance;
        if (!kinectManager || !kinectManager.IsInitialized() || !kinectManager.IsUserDetected())
            return;
        

        /*
        if (gestureListener.IsSwipeLeft())
        {
            GestureInfo.text = "Swiped left";
        }
        else if (gestureListener.IsSwipeRight())
        {
            GestureInfo.text = "Swiped right";
        }
        else {
            GestureInfo.text = "DO gesture";
        }
        */
    }
}