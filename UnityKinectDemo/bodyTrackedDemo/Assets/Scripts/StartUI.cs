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
        if (Input.GetKey("down"))
        {
            SceneManager.LoadScene(1); // 0 (start) -> 1(main)
        }
    }

    public void detectPunch()
    {
        if (mode == 0) { 
            mode = 1;
            ds.SetText(dsText[mode]);
        }
    }

    public void detectClap()
    {
        if (mode == 1) { 
            mode = 2;
            ds.SetText(dsText[mode]);
        }
    }

    public void detectCircle()
    {
        if (mode == 2) { 
            SceneManager.LoadScene(1);
        }
    }
}
