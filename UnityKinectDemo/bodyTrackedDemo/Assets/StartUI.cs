using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{

    // Use this for initialization
    int mode;
    string[] dsText = { "Try Punching", "Try Clapping", "Try to draw a circle" };
    TextMeshProUGUI ds;

    void Start()
    {
        mode = 0; // tryPunch
        ds = GameObject.FindGameObjectWithTag("Description").GetComponent<TextMeshProUGUI>();
        ds.SetText(dsText[mode]);
    }

    // Update is called once per frame
    void Update()
    {
        /*
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
        */
        if (Input.GetKey("down"))
        {
            SceneManager.LoadScene(1); // 0 (start) -> 1(main)
        }
    }

    public void detectPunch()
    {
        mode = 1;
        ds.SetText(dsText[mode]);
    }

    public void detectClap()
    {
        mode = 2;
        ds.SetText(dsText[mode]);
    }

    public void detectCircle()
    {
        SceneManager.LoadScene(1);
    }
}
