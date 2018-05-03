using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveHistogram : MonoBehaviour {

    KinectManager km;
    Text debug;

	// Use this for initialization
	void Start () {
        km = GameObject.Find("Main Camera").GetComponent<KinectManager>();
        debug= GameObject.Find("p_custom").GetComponent<Text>();
        StartCoroutine(Snapshot());
	}

    IEnumerator Snapshot()
    {
        debug.text = "Waiting";
        yield return new WaitUntil(km.IsUserDetected);
        debug.text = "User found, get ready";
        yield return new WaitForSeconds(5);
        debug.text = "Taking snapshot";
        Texture2D img = km.GetUsersLblTex();
        debug.text="Saving img";
        SaveTextureToFile(img, "Assets/1.png");
        debug.text = "Done";
    }

    void SaveTextureToFile(Texture2D texture, string filename)
    {
        System.IO.File.WriteAllBytes(filename, texture.EncodeToPNG());
    }

}
