using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{

    // Use this for initialization
    int mode;
    string[] dsText = { "Try Punch", "Try Splash", "Try draw a circle" };
    void Start()
    {
        mode = 0; // tryPunch
        var ds = GameObject.FindGameObjectWithTag("Description").GetComponent<TextMeshPro>();
        ds.SetText(dsText[mode]);
    }

    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
            case 0:
                {
                    detectPunch();
                    break;
                }
            case 1:
                {
                    detectSplash();
                    break;
                }
            case 2:
                {
                    detectCircle();
                    break;
                }
        }

        if (Input.GetKey("down"))
        {
            SceneManager.LoadScene(1); // 0 (start) -> 1(main)
        }
    }

    void detectPunch()
    {
        if (false)
        {
            mode = 1;
            var ds = GameObject.FindGameObjectWithTag("Description").GetComponent<TextMeshPro>();
            ds.SetText(dsText[mode]);
        }
    }
    void detectSplash()
    {
        if (false)
        {
            mode = 2;
            var ds = GameObject.FindGameObjectWithTag("Description").GetComponent<TextMeshPro>();
            ds.SetText(dsText[mode]);
        }
    }

    void detectCircle()
    {
        if (false)
        {
            SceneManager.LoadScene(1);
        }
    }
}
