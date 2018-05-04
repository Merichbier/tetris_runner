using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fading : MonoBehaviour
{
    public Texture2D fadingTexture;
    public float fadingSpeed;

    private int drawDepth = -10000; // low means top layer
    private float alpha = 1.0f;
    private int fadingDir = -1; // -1 means fade in, +1 means fade Out

    void OnGUI()
    {
        alpha += fadingDir * fadingSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        //GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadingTexture);

    }

    public float BeginFade(int direction)
    {
        fadingDir = direction;
        return fadingSpeed;
    }

    void ScreenLoaded()
    {
        BeginFade(-1);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("up"))
        {
            BeginFade(1);
            if (Application.loadedLevel == 0) Application.LoadLevel(1);
            else if (Application.loadedLevel == 1) Application.LoadLevel(0);
        }
    }
}
