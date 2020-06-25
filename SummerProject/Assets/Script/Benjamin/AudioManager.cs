using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource myAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(AudioClip myClip, float volume)
    {
        if (myAudioSource.pitch != 1)
        {
            myAudioSource.pitch = 1;
        }
        myAudioSource.PlayOneShot(myClip, volume);
    }
    public void PlaySoundRDMPitch(AudioClip myClip, float volume, float minPitch, float maxPitch)
    {
        myAudioSource.pitch = Random.Range(minPitch, maxPitch);
        myAudioSource.PlayOneShot(myClip, volume);
    }
}
