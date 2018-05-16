using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource hit;


    public void playHitSound()
    {
        hit.Play();
    }
    
    public AudioClip clip;
    AudioSource source;

    int startingPitch = 0;
    int timeToDecrease = 5;
    AudioSource audioSource;

    void Start()
    {
        /*
        //Fetch the AudioSource from the GameObject
        audioSource = GetComponent<AudioSource>();
        //Initialize the pitch
        audioSource.pitch = startingPitch;
        */
    }
    /*
    void Update()
    {
        //Debug.Log(audioSource.pitch);
        //While the pitch is over 0, decrease it as time passes.
        if (audioSource.pitch < 7)
            audioSource.pitch += Time.deltaTime;// * startingPitch / timeToDecrease;
        
    }
    */
}
