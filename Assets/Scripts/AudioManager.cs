using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource wall_hit;
    public AudioSource snowball_hit;
    public AudioSource wall_break;
    public AudioSource coin_pickup;
    public AudioSource bonus_stage;

    public void playWallHitSound()
    {
        wall_hit.Play();
    }

    public void playSnowballHitSound()
    {
        snowball_hit.Play();
    }

    public void playWallBreakSound()
    {
        wall_break.Play();
    }

    public void playCoinPickupSound()
    {
        coin_pickup.Play();
    }

    public void playBonusStage()
    {
        bonus_stage.Play();
    }

    public void stopBonusStage()
    {
        bonus_stage.Stop();
    }

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
