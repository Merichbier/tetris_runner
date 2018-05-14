using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{

    // Use this for initialization
    int mode;
    string[] dsText = { "Waiting for player","Try Punching", "Try Clapping", "Try to draw a circle" };
    TextMeshProUGUI ds;

    float counter;
    float counterMax = 1;
    KinectManager km;

    void Start()
    {
        ds = GameObject.FindGameObjectWithTag("Description").GetComponent<TextMeshProUGUI>();
        ChangeMode(0);
        km = GameObject.Find("Player").GetComponentInChildren<KinectManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (km.IsUserDetected())
        {
            counter += Time.deltaTime;
        }
        else {
            counter = 0;
        }

        if (counter >= counterMax && mode<1)
        {
            ChangeMode(1);
        }
        
        if (Input.GetKey("down"))
        {
            SceneManager.LoadScene(1); // 0 (start) -> 1(main)
        }
    }

    void ChangeMode(int newMode) {
        mode = newMode;
        ds.SetText(dsText[mode]+" "+mode.ToString());
        Debug.Log("changed to " + newMode);
    }

    public void detectPunch()
    {
        if (mode == 1) {
            ChangeMode(2);
        }
    }

    public void detectClap()
    {
        if (mode == 2) {
            ChangeMode(3);
        }
    }

    public void detectCircle()
    {
        if (mode == 3) { 
            SceneManager.LoadScene(1);
        }
    }
}
